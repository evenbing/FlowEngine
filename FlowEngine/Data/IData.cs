using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Data
{
    interface IData
    {
        IDataCursor getCursor();
    }
    
}
