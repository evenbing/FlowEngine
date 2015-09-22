using FlowEngine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Flow
{
    class FlowSequence : FlowElement
    {
        private const int FAILURE = 0;
        private const int SUCCESS = 1;
        private const int DONE = 2;

        int exitOn;
        bool start, map;
        IData save_pipeline;
    
        public FlowSequence(IData value):base(value)
        {
            type = TYPE_SEQUENCE;
        }

        public new void init(FlowState flowState)
        {
            save_pipeline = IDataUtil.clone(flowState.pipeline);
            start = true;
        }

        public override void invoke(FlowState flowState)
        {
            bool isRoot = FlowState.isRoot(this);
            bool disabled = false;
            FlowElement last = flowState.last;
            if ((last != null) && (!last.enabled))
            {
                disabled = true;
            }
            if (flowState.shouldExit(this) || 
                (flowState.error == null && exitOn == SUCCESS && !start && !map && !disabled) || 
                (flowState.error != null && exitOn == FAILURE))
            {
                flowState.done = true;
                flowState.willExit();
                return;
            }
            if (flowState.error != null)
            {
                FlowExceptionHandler fex = flowState.flowExHandler;
                if (fex != null)
                {
                    fex.resetException();
                }
                flowState.error = null;
                IData pipe = flowState.pipeline;
                IDataCursor cursor = pipe.getCursor();
                while (cursor.first() && cursor.delete()){}
                cursor.destroy();
                IDataUtil.append(save_pipeline, pipe);
            }
            else
            {
                save_pipeline = IDataUtil.clone(flowState.pipeline);
            }
            if (flowState.pushNextNode() == null)
            {
                flowState.done = true;
                if (isRoot && flowState.incremental)
                {
                    if (flowState.shouldExit(this) ||
                        (flowState.error == null && exitOn == SUCCESS && !start && !map && !disabled) ||
                        (flowState.error != null && exitOn == FAILURE))
                    {
                        flowState.done = true;
                        flowState.willExit();
                    }
                }
            }
            else
            {
                map = flowState.current().type.Equals(TYPE_MAP);
            }
            if (start)
            {
                start = false;
            }
        }
    }
}
