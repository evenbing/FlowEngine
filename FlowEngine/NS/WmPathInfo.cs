using FlowEngine.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.NS
{
    class WmPathInfo
    {
        public static WmPathItem[] parsePath(String path)
        {
            if ((path == null) || (path.Length == 0))
            {
                return null;
            }
            List<WmPathItem> pathInfo = new List<WmPathItem>();

            StringParser st = new StringParser(path, "/", "\"", true);


            int type = -1;
            int javaType = 0;
            int dim = -1;
            while (st.moveNextToken())
            {
                String name = st.currentToken();

                type = -1;
                javaType = 0;
                dim = -1;
                String nsName = null;
                int position = -1;
                int arrayIndex = -1;
                int tableIndex = -1;
                String varArrayIndex = null;
                String varTableIndex = null;

                StringParser dt = new StringParser(name, ";", "\"", true);
                if (dt.moveNextToken())
                {
                    String s = dt.currentToken();


                    int quote = 0;
                    if (s.StartsWith("\""))
                    {
                        quote = s.LastIndexOf("\"");
                    }
                    int pos = s.IndexOf("(", quote);
                    int ind = s.IndexOf("[", quote);
                    if (quote > 0)
                    {
                        name = s.Substring(1, quote);
                    }
                    else if (pos != -1)
                    {
                        if ((ind != -1) && (pos > ind))
                        {
                            name = s.Substring(0, ind);
                        }
                        else
                        {
                            name = s.Substring(0, pos);
                        }
                    }
                    else if (ind != -1)
                    {
                        name = s.Substring(0, ind);
                    }
                    else
                    {
                        name = s;
                    }
                    if (pos != -1)
                    {
                        try
                        {
                            if ((ind != -1) && (pos > ind))
                            {
                                position = -1;
                            }
                            else
                            {
                                position = s.IndexOf(")", quote);
                                if ((position != -1) && (position > pos))
                                {
                                    position = Int32.Parse(s.Substring(pos + 1, position));
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            position = -1;
                        }
                    }
                    if (ind != -1)
                    {
                        pos = s.IndexOf("]", ind);
                        if ((pos != -1) && (pos > ind))
                        {
                            try
                            {
                                int tempVarind = s.IndexOf("%", ind);
                                if ((tempVarind != -1) && (tempVarind < pos))
                                {
                                    int tempVarind2 = s.IndexOf("%", tempVarind + 1);
                                    if (tempVarind2 > pos)
                                    {
                                        pos = s.IndexOf("]", tempVarind2);
                                    }
                                    arrayIndex = -2;
                                    varArrayIndex = s.Substring(ind + 1, pos);
                                }
                                else
                                {
                                    arrayIndex = Int32.Parse(s.Substring(ind + 1, pos));
                                }
                            }
                            catch (Exception e)
                            {
                                arrayIndex = -1;
                            }
                            ind = s.IndexOf("[", pos);
                            if (ind != -1)
                            {
                                pos = s.IndexOf("]", ind);
                                try
                                {
                                    if ((pos != -1) && (pos > ind))
                                    {
                                        int tempVarind = s.IndexOf("%", ind);
                                        if ((tempVarind != -1) && (tempVarind < pos))
                                        {
                                            tableIndex = -2;
                                            varTableIndex = s.Substring(ind + 1, pos);
                                        }
                                        else
                                        {
                                            tableIndex = Int32.Parse(s.Substring(ind + 1, pos));
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    tableIndex = -1;
                                }
                            }
                        }
                    }
                }
                if (dt.moveNextToken())
                {
                    try
                    {
                        String objType = dt.currentToken();
                        int index = objType.IndexOf(".");
                        if (index != -1)
                        {
                            javaType = Int32.Parse(objType.Substring(index + 1));
                            type = Int32.Parse(objType.Substring(0, index));
                        }
                        else
                        {
                            type = Int32.Parse(objType);
                        }
                    }
                    catch (Exception e)
                    {
                        type = -1;
                        javaType = 0;
                    }
                }
                if (dt.moveNextToken())
                {
                    try
                    {
                        dim = Int32.Parse(dt.currentToken());
                    }
                    catch (Exception e)
                    {
                        dim = -1;
                    }
                }
                if (dt.moveNextToken())
                {
                    nsName = dt.currentToken();
                }
                pathInfo.Add(new WmPathItem(name, nsName, position, arrayIndex, varArrayIndex, tableIndex, varTableIndex, type, javaType, dim));
            }
            return pathInfo.Count == 0 ? null : pathInfo.ToArray();
        }
    }
}
