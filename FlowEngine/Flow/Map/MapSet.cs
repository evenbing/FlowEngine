using FlowEngine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Flow.Map
{
    class MapSet
    {
        public static String substituteVars(Object[] list, IData pipeline)
        {
            StringBuilder sb = new StringBuilder();
            int length = list.Length / 2 * 2;
            for (int i = 0; i < length; i += 2)
            {
                sb.Append(list[i] as string);
                MapWmPathInfo pathInfo = list[(i + 1)] as MapWmPathInfo;
                Object ob = IDataWmPathProcessor.getNode(pipeline, pathInfo.getPathItems());
                if ((ob is String))
                {
                    sb.Append((String)ob);
                }
                else
                {
                    sb.Append("%" + pathInfo.path + "%");
                }
            }
            if (list.Length / 2 * 2 != list.Length)
            {
                sb.Append((String)list[(list.Length - 1)]);
            }
            return sb.ToString();
        }

        public static String[] parseVars(String str)
        {
            if ((str == null) || ((str.IndexOf('%') < 0) && (str.IndexOf((char)65285) < 0)))
            {
                return null;
            }
            List<string> tokens = new List<string>();

            int index = 0;
            int last = 0;
            int flag = 0;
            for (;;)
            {
                int begin = -1;
                while (begin < 0)
                {
                    int single_percent = str.IndexOf('%', index);
                    int double_percent = str.IndexOf((char)65285, index);
                    if (((single_percent < double_percent) && (single_percent != -1)) || (double_percent == -1))
                    {
                        begin = single_percent;
                    }
                    else if (((double_percent < single_percent) && (double_percent != -1)) || (single_percent == -1))
                    {
                        begin = double_percent;
                    }
                    if (begin < 0)
                    {
                        break;
                    }
                    if ((begin + 1 < str.Length) && ((str[begin + 1] == '%') || (str[begin + 1] == (char)65285)))
                    {
                        index = begin + 2;
                        begin = -1;
                    }
                }
                if (begin >= 0)
                {
                    if (begin + 1 < str.Length)
                    {
                        int end = -1;
                        index = begin + 1;
                        while (end < 0)
                        {
                            end = str.IndexOf('%', index);
                            if (end < 0)
                            {
                                end = str.IndexOf((char)65285, index);
                                if (end < 0)
                                {
                                    break;
                                }
                            }
                        }
                        if (end < 0)
                        {
                            break;
                        }
                        if (begin == last)
                        {
                            tokens.Add("");
                        }
                        else
                        {
                            tokens.Add(str.Substring(last, begin));
                        }
                        tokens.Add(str.Substring(begin + 1, end));

                        index = end + 1;
                        last = index;
                        flag = index;
                    }
                    else
                    {
                        last = str.Length;
                        break;
                    }

                    if (index >= str.Length)
                    {
                        break;
                    }
                }
            }
            if ((last < str.Length) && (tokens.Count > 1))
            {
                tokens.Add(str.Substring(last));
            }
            if ((last == str.Length) && (tokens.Count > 1))
            {
                tokens.Add(str.Substring(flag, last));
            }
            if (tokens.Count < 2)
            {
                return null;
            }
            return tokens.ToArray();
        }

        public static Object[] processPath(String[] list)
        {
            if ((list == null) || (list.Length < 1))
            {
                return null;
            }
            Object[] result = new Object[list.Length];
            int length = list.Length / 2 * 2;
            for (int i = 0; i < length; i += 2)
            {
                result[i] = list[i];
                result[(i + 1)] = MapWmPathInfo.create(list[(i + 1)]);
            }
            if (list.Length / 2 * 2 != list.Length)
            {
                result[(result.Length - 1)] = list[(list.Length - 1)];
            }
            return result;
        }
    }
}
