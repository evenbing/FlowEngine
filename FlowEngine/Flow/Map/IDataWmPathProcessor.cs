using FlowEngine.Data;
using FlowEngine.NS;
using FlowEngine.Util.Pluggable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Flow.Map
{
    class IDataWmPathProcessor
    {
        public static Object getNode(IData pipeline, WmPathItem[] items)
        {
            if ((pipeline == null) || (items == null) || (items.Length == 0))
            {
                return null;
            }
            if (items.Length == 1)
            {
                return get(pipeline, items[0], pipeline);
            }
            IData parent = pipeline;
            for (int i = 0; i < items.Length - 1; i++)
            {
                Object o = get(parent, items[i], pipeline);
                if ((o is IData))
                {
                    parent = (IData)o;
                }
                else
                {
                    return null;
                }
            }
            return get(parent, items[(items.Length - 1)], pipeline);
        }


        private static Object get(IData parent, WmPathItem pathItem, IData origPipe)
        {
            return get(parent, pathItem, true, origPipe);
        }

        public static object get(IData parent, WmPathItem pathItem, bool bestEffort, IData origPipe)
        {
            if ((parent == null) || (pathItem == null))
            {
                return null;
            }
            Object value = null;
            if (pathItem.pathType == 0)
            {
                IDataCursor cursor = parent.getCursor();
                String name = pathItem.name;
                if (cursor.first(name))
                {
                    value = cursor.getValue();
                }
                cursor.destroy();
            }
            else if (pathItem.pathType == 1)
            {
                if (pathItem.position < 0)
                {
                    return null;
                }
                IDataCursor cursor = parent.getCursor();
                if (seek(cursor, pathItem.position))
                {
                    value = cursor.getValue();
                }
                cursor.destroy();
            }
            else if (pathItem.pathType == 2)
            {
                if (pathItem.position < 0)
                {
                    return null;
                }
                IDataCursor cursor = parent.getCursor();
                String name = pathItem.name;
                int position = pathItem.position;


                bool find = true;
                for (int i = 0; i <= position; i++)
                {
                    if (!cursor.next(name))
                    {
                        find = false;
                        break;
                    }
                }
                if (find)
                {
                    value = cursor.getValue();
                }
                cursor.destroy();
            }
            else if (pathItem.pathType == 3)
            {
                return null;
            }
            if ((!bestEffort) &&
              (!dataCheck(value, pathItem)))
            {
                value = null;
            }
            if ((value is WmIDataList))
            {
                value = ((WmIDataList)value).getItems();
            }
            if (pathItem.arrayIndex == -1)
            {
                return value;
            }
            return getByArrayIndex(value, pathItem, origPipe);
        }

        private static bool seek(IDataCursor ic, int position)
        {
            int curPos = 0;
            if (ic.first())
            {
                while (curPos < position)
                {
                    if (!ic.next())
                    {
                        return false;
                    }
                    curPos++;
                }
                return true;
            }
            return false;
        }

        private static bool dataCheck(object data, WmPathItem pathItem)
        {
            if ((data is WmIDataList))
            {
                data = ((WmIDataList)data).getItems();
            }
            if (data == null)
            {
                return true;
            }
            int type = pathItem.type;
            int dim = pathItem.dim;
            if (type == -1)
            {
                if (dim == 2)
                {
                    return data is object[][];
                }
                if (dim == 1)
                {
                    return data is object[];
                }
                return true;
            }
            if (dim == -1)
            {
                if (type == 1)
                {
                    return ((data is string)) || ((data is string[])) || ((data is string[][]));
                }
                if (NSField.isRecordType(type))
                {
                    return data is IData;
                }
                return true;
            }
            if ((type == 3) && (dim == 0))
            {
                return true;
            }
            if (((data is string)) && (type == 1) && (dim == 0))
            {
                return true;
            }
            if (((data is string[])) && (type == 1) && (dim == 1))
            {
                return true;
            }
            if (((data is string[][])) && (type == 1) && (dim == 2))
            {
                return true;
            }
            if (((data is IData)) && (NSField.isRecordType(type)) && (dim == 0))
            {
                return true;
            }
            if (((data is IData[])) && (NSField.isRecordType(type)) && (dim == 1))
            {
                return true;
            }
            if (((data is object[])) && (type == 3) && (dim == 1))
            {
                return true;
            }
            return false;
        }

        static object getByArrayIndex(object value, WmPathItem pathItem, IData origPipe)
        {
            if (pathItem.arrayIndex == -1)
            {
                return value;
            }
            if ((pathItem.tableIndex != -1) && ((value is Object[][])))
            {
                Object[][] obj = (Object[][])value;
                int arrayIndex = pathItem.arrayIndex;
                if (arrayIndex == -2)
                {
                    arrayIndex = getIndexFromPipe(pathItem.varArrayIndex, origPipe);
                }
                if (arrayIndex < obj.Length)
                {
                    int tableIndex = pathItem.tableIndex;
                    if (tableIndex == -2)
                    {
                        tableIndex = getIndexFromPipe(pathItem.varTableIndex, origPipe);
                    }
                    if (tableIndex < obj[arrayIndex].Length)
                    {
                        return obj[arrayIndex][tableIndex];
                    }
                    return null;
                }
            }
            else if ((value is object[]))
            {
                object[] obj = (object[])value;
                int index = pathItem.arrayIndex;
                if (index == -2)
                {
                    index = getIndexFromPipe(pathItem.varArrayIndex, origPipe);
                }
                if (index < obj.Length)
                {
                    return obj[index];
                }
                return null;
            }
            return null;
        }
        public static int getIndexFromPipe(String expr, IData pipe)
        {
            String[] vars = MapSet.parseVars(expr);
            Object[] paths = MapSet.processPath(vars);
            if (paths == null)
            {
                return -1;
            }
            String val = MapSet.substituteVars(paths, pipe);
            try
            {
                return Int32.Parse(val);
            }
            catch (Exception e) { }
            return -1;
        }
    }
}