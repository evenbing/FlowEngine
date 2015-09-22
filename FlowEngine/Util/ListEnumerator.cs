using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Util
{
    class ListEnumerator : IEnumerator
    {
        List v;
        object current;
        int index = -1;

        public ListEnumerator(List elements)
        {
            v = elements;
        }

        object IEnumerator.Current
        {
            get
            {
                return current;
            }
        }

        bool IEnumerator.MoveNext()
        {
            if (index < v.capacity() - 1)
            {
                index += 1;
                current = v.elementAt(index);
                return true;
            }
            return false;
        }

        void IEnumerator.Reset()
        {
            index = -1;
        }
    }
}
