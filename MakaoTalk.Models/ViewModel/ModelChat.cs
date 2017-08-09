using MakaoTalk.Database;
using MakaoTalk.Database.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaoTalk.Models.ViewModel
{
    public class ModelChat : DomainModel
    {
        [SqlAttribute(Key = true)]
        public int ChatID { get; set; }

        public string UserID { get; set; }

        public string UserName { get; set; }

        public string Contents { get; set; }

        public DateTime SendDate { get; set; }
    }
}
