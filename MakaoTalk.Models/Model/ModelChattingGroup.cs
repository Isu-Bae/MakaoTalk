using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakoTalk.Models.Model
{
    public enum GroupTypes
    {
        Single,
        Multi
    }

    public class ModelChattingGroup
    {
        public GroupTypes Type { get; set; }

        public int GroupID { get; set; }

        public IEnumerable<ModelUser> Users { get; set; }

        public string LastChat { get; set; }

        public byte[] GroupImage { get; set; }


    }
}
