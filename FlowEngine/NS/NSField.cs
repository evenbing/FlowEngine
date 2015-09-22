using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.NS
{
    class NSField : NSNode
    {
        public const int FIELD_STRING = 1;
        public const int FIELD_RECORD = 2;
        public const int FIELD_OBJECT = 3;
        public const int FIELD_RECORDREF = 4;
        public const int FIELD_FIELDREF = 5;
        public const int FIELD_SUBTYPE_UNKNOWN = 0;

        public static bool isRecordType(int type)
        {
            return (type == FIELD_RECORD) || (type == FIELD_RECORDREF);
        }
    }
}
