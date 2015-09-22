using FlowEngine.Data;
using FlowEngine.Flow.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Flow
{
    class FlowExit : FlowElement
    {
        private const int SUCCESS = 0;
        private const int FAILURE = 1;

        public string from;
        string message, clonedMessage;
        int signal;
        bool done;
        private bool isTimeout = false;
        Object[] msgSubList;

        public FlowExit(IData value):base(value)
        {
            type = TYPE_EXIT;
        }

        public override void invoke(FlowState flowState)
        {
            if (msgSubList != null)
            {
                clonedMessage = MapSet.substituteVars(msgSubList, flowState.pipeline);
            }
            flowState.exit = this;
            flowState.done = true;
        }

        public bool isExitFlow()
        {
            return (from != null) && (from.Equals("$flow"));
        }

        public bool isExitParent()
        {
            return (from != null) && (from.Equals("$parent"));
        }

        public bool isExitLoop()
        {
            return (from != null) && (from.Equals("$loop"));
        }

        public bool isExitLabel()
        {
            return (!isExitFlow()) && (!isExitParent()) && (!isExitLoop());
        }

        public bool shouldExit(FlowElement current)
        {
            if (current == null)
            {
                return false;
            }
            if (isExitFlow())
            {
                return true;
            }
            bool loop = current.type.Equals(TYPE_LOOP) || current.type.Equals(TYPE_RETRY);
            if (isExitLoop() )
            {
                if (loop)
                {
                    done = true;
                }
            }else if (isExitParent())
            {
                done = true;
            }else if (current.name != null)
            {
                if (from.Equals(current.name))
                {
                    done = true;
                }
            }
            return true;
        }

        public bool isExitDone()
        {
            return done;
        }

        public Exception getFailure()
        {
            if (signal == FAILURE)
            {
                return new Exception(clonedMessage);
            }
            return null;
        }
    }
}
