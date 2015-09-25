using FlowEngine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Flow
{
    abstract class FlowElement : ICloneable
    {
        public const string TYPE_SEQUENCE = "SEQUENCE";
        public const string TYPE_ROOT = "ROOT";
        public const string TYPE_EXIT = "EXIT";
        public const string TYPE_LOOP = "LOOP";
        public const string TYPE_RETRY = "RETRY";
        public const string TYPE_MAP = "MAP";

        public string type, name, comment, scope;
        public FlowElement parent;
        public FlowElement[] children;
        public bool enabled, pipeDone;
        public int id, nodeIndex;
        public List<FlowElement> nodes;

        public FlowElement(IData value)
        {

        }

        public void init(FlowState state) { }

        public abstract void invoke(FlowState flowState);

        public object Clone()
        {
            return MemberwiseClone() as FlowElement;
        }

        public int getNodeCount()
        {
            return nodes != null ? nodes.Count : 0;
        }

        public FlowElement getNodeAt(int index)
        {
            if ((nodes == null) || (index < 0) || (index >= nodes.Count))
            {
                return null;
            }
            return nodes[index];
        }

        public void nextNodeIndex()
        {
            nodeIndex += 1;
        }
    }
}
