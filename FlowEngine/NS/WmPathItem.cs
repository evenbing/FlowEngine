using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.NS
{
    class WmPathItem
    {
        public int pathType, position, type, dim, arrayIndex, tableIndex;
        public string name = "", varArrayIndex, varTableIndex;
        private string nsName;
        private int subType;

        public WmPathItem(string name, string nsName, int position, int arrayIndex, int tableIndex, int type, int dim)
        {
            this.name = name;
            this.nsName = nsName;
            this.position = position;
            this.arrayIndex = arrayIndex;
            this.tableIndex = tableIndex;
            this.type = type;
            this.dim = dim;

            this.subType = 0;
            if ((name != null) && (position > -1))
            {
                pathType = 2;
            }
            else if (((name == null) || (name.Length == 0)) && (position > -1))
            {
                pathType = 1;
            }
            else
            {
                pathType = 0;
            }
        }

        public WmPathItem(string name, string nsName, int position, int arrayIndex, string varArrayIndex, int tableIndex, string varTableIndex, int type, int dim) : this(name, nsName, position, arrayIndex, tableIndex, type, dim)
        {
            this.varArrayIndex = varArrayIndex;
            this.varTableIndex = varTableIndex;
        }

        public WmPathItem(string name, string nsName, int position, int arrayIndex, string varArrayIndex, int tableIndex, string varTableIndex, int type, int subType, int dim) : this(name, nsName, position, arrayIndex, varArrayIndex, tableIndex, varTableIndex, type, dim)
        {
            this.subType = subType;
        }

        public int getDimension()
        {
            int dataDim = dim;
            if (arrayIndex != -1)
            {
                dataDim--;
            }
            if (tableIndex != -1)
            {
                dataDim--;
            }
            return dataDim;
        }
    }
}
