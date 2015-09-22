using FlowEngine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Flow
{
    class FlowState
    {
        public FlowElement last;
        public FlowExit exit;
        public Exception error;
        Stack<FlowElement> state = new Stack<FlowElement>();
        public FlowExceptionHandler flowExHandler;
        public IData pipeline;
        static int flowElementID = 0;
        public bool incremental = false;
        //bool ignoreTimeouts = false;

        public bool done
        {
            get
            {
                return state.Count == 0;
            }
            set
            {
                FlowElement flowElement = current();
                if (flowElement != null)
                {
                    flowElement.pipeDone = value;
                }
            }
        }

        public FlowElement current()
        {
            if (state.Count == 0)
            {
                return null;
            }
            return state.Peek();
        }

        public static bool isRoot(FlowElement flowElement)
        {
            return flowElement.type.Equals(FlowElement.TYPE_ROOT);
        }

        public bool shouldExit(FlowElement current)
        {
            if (exit != null)
            {
                return exit.shouldExit(current);
            }
            return false;
        }

        public void willExit()
        {
            if (exit != null)
            {
                if (exit.isExitDone())
                {
                    error = exit.getFailure();
                    exit = null;
                }else if (!exit.isExitDone() && current().parent == null)
                {
                    if (exit.isExitLabel())
                    {
                        error = new Exception("BAD LABEL - " + exit.name + " - " + exit.from);
                        exit = null;
                    }
                    else if (exit.isExitLoop())
                    {
                        string exitName = exit.name;
                        if (exitName == null)
                        {
                            exitName = "";
                        }
                        error = new Exception("NO ANCESTOR - " + exitName);
                        exit = null;
                    }
                    else
                    {
                        error = exit.getFailure();
                        exit = null;
                    }
                }
            }
        }

        public bool hasMoreNodes()
        {
            if (state.Count == 0)
            {
                return false;
            }
            return current().nodeIndex < current().getNodeCount();
        }

        public FlowElement pushNextNode()
        {
            FlowElement parent = current();
            FlowElement child = pushNode(parent.nodeIndex);
            parent.nextNodeIndex();
            return child;
        }

        public FlowElement push(FlowElement node)
        {
            if (node.id == 0)
            {
                node.id = newFlowElementID();
            }
            FlowElement next = node.Clone() as FlowElement;
            state.Push(next);
            return next;
        }

        private FlowElement pushNode(int index)
        {
            if (hasMoreNodes())
            {
                return push(current().getNodeAt(index));
            }
            return null;
        }

        static int newFlowElementID()
        {
            return flowElementID++;
        }
    }
}
