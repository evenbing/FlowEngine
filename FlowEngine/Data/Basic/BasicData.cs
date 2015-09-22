using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Data.Basic
{
    class BasicData : IData
    {
        public static IData create()
        {
            return new BasicData();
        }

        IDataCursor IData.getCursor()
        {
            throw new NotImplementedException();
        }
    }
}
