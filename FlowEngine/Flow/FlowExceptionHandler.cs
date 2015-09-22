using FlowEngine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Flow
{
    abstract class FlowExceptionHandler
    {
        public abstract IData getException();

        public abstract void resetException();

        public abstract void handleError(Exception paramException, IData paramValues);
    }
}
