using MakaoTalk.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakoTalk.Models.Model
{
    public class ModelUser : DomainModel
    {
        public string UserName { get; set; }

        public string FriendID { get; set; } 

    }
    
}
