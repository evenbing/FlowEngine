using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Util
{
    class List
    {
        private object[] list;
        private int curPtr = 0;

        public List() : this(10)
        {
        }

        public List(int size)
        {
            list = new object[size];
        }

        public List(object[] o)
        {
            if (o != null)
            {
                for (int i = 0; i < o.Length; i++)
                {
                    addElement(o[i]);
                }
            }
        }

        public List(IEnumerator e):this()
        {
            if (e == null)
            {
                return;
            }
            while (e.MoveNext())
            {
                addElement(e.Current);
            }
        }

        public void addElement(object o)
        {
            list[(curPtr++)] = o;
            if (curPtr >= list.Length)
            {
                object[] newlist = new object[list.Length * 2];
                Array.Copy(list, 0, newlist, 0, list.Length);
                list = newlist;
            }
        }

        public bool contains(object o)
        {
            for (int i = 0; i < curPtr; i++)
            {
                if (list[i] == o)
                {
                    return true;
                }
            }
            return false;
        }

        public object elementAt(int index)
        {
            if ((index >= 0) && (index < list.Length))
            {
                return list[index];
            }
            throw new IndexOutOfRangeException(index.ToString());
        }

        public int indexOf(object o)
        {
            return indexOf(o, 0);
        }

        public int indexOf(object o, int start)
        {
            for (int i = start; i < this.curPtr; i++)
            {
                object c = this.list[i];
                if (c == o)
                {
                    return i;
                }
                if ((c != null) && (c.Equals(o)))
                {
                    return i;
                }
            }
            return -1;
        }

        public void removeElement(object o)
        {
            int index = indexOf(o);
            if (index >= 0)
            {
                removeElementAt(index);
            }
        }

        public object lastElement()
        {
            if (curPtr > 0)
            {
                return list[(curPtr - 1)];
            }
            throw new IndexOutOfRangeException();
        }

        public Object removeLastElement()
        {
            if (this.curPtr > 0)
            {
                return this.list[(--this.curPtr)];
            }
            throw new IndexOutOfRangeException();
        }

        public void setElementAt(Object o, int index)
        {
            if ((index >= 0) && (index < this.list.Length))
            {
                list[index] = o;
            }
            else
            {
                throw new IndexOutOfRangeException(index.ToString());
            }
        }

        public void removeElementAt(int index)
        {
            if ((curPtr > 0) && (index >= 0) && (index < list.Length))
            {
                if (index >= 0)
                {
                    for (int i = index; i < curPtr; i++)
                    {
                        list[i] = list[(i + 1)];
                    }
                }
                curPtr -= 1;
            }
            else
            {
                throw new IndexOutOfRangeException(index.ToString());
            }
        }

        public void copyInto(object[] o)
        {
            Array.Copy(list, 0, o, 0, curPtr);
        }

        public int capacity()
        {
            return list.Length;
        }

        public int size()
        {
            return curPtr;
        }

        public bool isEmpty()
        {
            return curPtr == 0;
        }

        public void reset()
        {
            curPtr = 0;
        }

        public void truncate(int newSize)
        {
            if ((newSize >= 0) && (newSize < curPtr))
            {
                curPtr = newSize;
            }
        }

        public IEnumerator elements()
        {
            return new ListEnumerator(this);
        }

        public string[] getStrings()
        {
            string[] s = new string[curPtr];
            copyInto(s);
            return s;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("{");
            for (int i = 0; i < this.curPtr; i++)
            {
                if (i < this.curPtr - 1)
                {
                    sb.Append(this.list[i]).Append(", ");
                }
                else
                {
                    sb.Append(this.list[i] == null ? "null" : this.list[i].ToString());
                }
            }
            sb.Append('}');
            return sb.ToString();
        }
    }
}
