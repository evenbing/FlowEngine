using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Data
{
    interface IDataCursor
    {
        string getKey();
        object getValue();
        bool first();
        bool first(string key);
        bool last();
        bool last(string key);
        bool previous();
        bool previous(string key);
        bool next();
        bool next(string key);
        bool delete();
        void destroy();
        void insertAfter(string paramString, object paramObject);
    }
}
