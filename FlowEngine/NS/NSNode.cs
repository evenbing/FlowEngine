using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.NS
{
    public abstract class NSNode
    {
        public static NSType NODE_UNKNOWN_TYPE = NSType.create("unknown");
        NSType nodeType = NODE_UNKNOWN_TYPE;
        public NSType getNodeTypeObj()
        {
            return this.nodeType;
        }
    }
}
