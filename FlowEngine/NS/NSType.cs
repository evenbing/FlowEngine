using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.NS
{
    public class NSType
    {
        public static String UNKNOWN = "unknown";
        String type = "unknown";
        static String KEY_TYPE_NAME = "type_name";
        public NSType() { }
        protected NSType(String type)
        {
            this.type = type;
        }

        public static NSType create(String type)
        {
            return new NSType(type);
        }
        public bool equals(NSType type)
        {
            if (type != null)
            {
                return equals(type.getValue());
            }
            return (this.type == null) || ("unknown".Equals(this.type));
        }
        public bool equals(String type)
        {
            String t = getValue();
            if (t == null)
            {
                t = "unknown";
            }
            if (type == null)
            {
                type = "unknown";
            }
            if (t.EndsWith("/unknown"))
            {
                t = t.Substring(0, t.LastIndexOf('/'));
            }
            if (type.EndsWith("/unknown"))
            {
                type = type.Substring(0, type.LastIndexOf('/'));
            }
            return type.Equals(t);
        }

        public String getType()
        {
            return this.type;
        }

        public void setType(String type)
        {
            this.type = type;
        }

        public void setValue(String value)
        {
            this.type = value;
        }
        public String getValue()
        {
            return this.type;
        }

        }
}
