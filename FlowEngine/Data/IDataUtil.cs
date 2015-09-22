using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Data
{
    class IDataUtil
    {
        public static void append(IData src, IData dst)
        {
            if ((src == null) || (dst == null) || (src == dst))
            {
                return;
            }
            IDataCursor srcC = src.getCursor();
            IDataCursor dstC = dst.getCursor();


            dstC.last();
            while (srcC.next())
            {
                dstC.insertAfter(srcC.getKey(), srcC.getValue());
            }
            srcC.destroy();
            dstC.destroy();
        }

        public static IData clone(IData src)
        {
            if (src is ICloneable) {
                try
                {
                    return (IData)((ICloneable)src).Clone();
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            IData clone = IDataFactory.create();
            
            append(src, clone);
            return clone;
        }
    }
}
