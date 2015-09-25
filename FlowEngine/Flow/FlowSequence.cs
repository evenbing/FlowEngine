using FlowEngine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowEngine.NS;

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

        private NSService getService(Namespace ns, NSName name)
        {
            NSNode svc = ns.getNode(name);
            if ((svc != null) && (svc.getNodeTypeObj().equals(NSService.TYPE)))
            {
                return (NSService)svc;
            }
            return null;
        }
        public new void init(FlowState flowState)
        {
            save_pipeline = IDataUtil.clone(flowState.pipeline);
            start = true;
        }
        /*protected bool validateMessage(FlowState state, IData pipeline, bool inOpt)
        {
            NSService svc = getService(state.getNamespace(), NSName.create(getNSName()));

            int vo = inOpt ? svc.getInputValidatorOptions() : svc.getOutputValidatorOptions();
            if (vo == 2)
            {
                IData val = null;

                NSRecord nsr = inOpt ? svc.getSignature().getInput() : svc.getSignature().getOutput();

                ValidatorOptions vtorOpts = Validator.getDefaultOptions();

                Validator vtor = Validator.create(pipeline, nsr, vtorOpts);
                boolean valid = false;
                Exception ex = null;
                try
                {
                    val = vtor.validate();
                    IDataCursor ic = val.getCursor();
                    if (ic.first("isValid"))
                    {
                        valid = IDataUtil.getBoolean(ic);
                    }
                    ic.destroy();
                }
                catch (Exception e)
                {
                    ex = e;
                }
                if ((!valid) || (ex != null))
                {
                    Object[] subs = { svc.getNSName().getFullName(), FlowInvoke.getValidationMsgs(val) };
                    if (ex == null)
                    {
                        ex = new FlowException(FlowExceptionBundle.class, inOpt? FlowExceptionBundle.FAILED_INPUT_VALIDATION : FlowExceptionBundle.FAILED_OUTPUT_VALIDATION, "", subs);
    }
        handleError(state, ex);
}
      return valid;
    }
    return true;
  }*/

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
