using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaoTalk.Database.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SqlAttribute : Attribute
    {
        public object DefaultValue { get; set; }
        public string FieldName { get; set; } = string.Empty;
        public bool Key { get; set; } = false;
        public bool ExceptSelect { get; set; } = false;
        public bool ExceptInsert { get; set; } = false;
        public bool ExceptUpdate { get; set; } = false;
    }
}
