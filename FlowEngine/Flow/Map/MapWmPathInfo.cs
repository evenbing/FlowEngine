using FlowEngine.NS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Flow.Map
{
    class MapWmPathInfo
    {
        WmPathItem[] pathInfo;
        public String path;
        string[] names;
        int[] dims, types;
        int nodePathDim, pathDim, pathChar;


        private MapWmPathInfo(String path)
        {
            this.path = path;
            init();
        }

        public WmPathItem[] getPathItems()
        {
            return pathInfo;
        }

        public static MapWmPathInfo create(String path)
        {
            if ((path == null) || (path.Length == 0))
            {
                return null;
            }
            try
            {
                MapWmPathInfo aPathInfo = new MapWmPathInfo(path);
                if (aPathInfo.getPathItems() == null)
                {
                    return null;
                }
                return aPathInfo;
            }
            catch (Exception e) { }
            return null;
        }

        private void init()
        {
            pathInfo = WmPathInfo.parsePath(path);
            if (pathInfo == null)
            {
                return;
            }
            names = new string[pathInfo.Length];
            for (int i = 0; i < pathInfo.Length; i++)
            {
                names[i] = pathInfo[i].name;
            }
            dims = new int[pathInfo.Length];
            for (int i = 0; i < pathInfo.Length; i++)
            {
                dims[i] = pathInfo[i].dim;
            }
            types = new int[pathInfo.Length];
            for (int i = 0; i < pathInfo.Length; i++)
            {
                types[i] = pathInfo[i].type;
            }
            pathDim = 0;
            for (int i = 0; i < pathInfo.Length; i++)
            {
                pathDim += pathInfo[i].getDimension();
            }
            this.nodePathDim = 0;
            for (int i = 0; i < pathInfo.Length; i++)
            {
                nodePathDim += pathInfo[i].dim;
            }
            if (pathInfo.Length == 1)
            {
                pathChar = 1;
            }
            else if (this.pathDim == this.pathInfo[(this.pathInfo.Length - 1)].getDimension())
            {
                pathChar = 2;
            }
            else
            {
                pathChar = 0;
            }
        }
    }
}
