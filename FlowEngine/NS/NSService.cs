using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.NS
{
    public abstract class NSService : NSNode
    {
        public static NSType TYPE = NSType.create("service");
    }
}
