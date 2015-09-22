using FlowEngine.Data.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Data
{
    class IDataFactory
    {
        public static IData create()
        {
            return BasicData.create();
        }
    }
}
