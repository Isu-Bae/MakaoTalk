using MakaoTalk.Database;
using MakaoTalk.Models.Model;
using MakaoTalk.Models.ViewModel;
using MakaoTalk.Utilities.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaoTalk.Services.Message
{

    public interface IMessageService
    {
        void SaveMessage(ModelChat message);

        // TODO: 질문!! userID 는 내 id 를 넣는건가요?(즉.. 앱 실행시킨 폰 주인)
        IEnumerable<ModelUser> GetFriends(string userID);
    }

    public class MessageService : IMessageService
    {
        private readonly IDatabase _repository;

        public MessageService()
        {
            _repository = new DatabaseMSSQLMakaoTalk();
        }

        public void SaveMessage(ModelChat message)
        {
            var query = $@"
            INSERT INTO [dbo].[ChatingRecord]
                       ([UserID]
                       ,[UserName]
                       ,[Contents]
                       ,[SendDate])
                 VALUES
                       ('{message.UserID}'
                       ,'{message.UserName}'
                       ,'{message.Contents}'
                       ,'{message.SendDate.ToString24Hour()}')";

            _repository.Execute(query);
        }

        public IEnumerable<ModelUser> GetFriends(string userID)
        {
            var query = $@"
            select 
	            b.UserName
	            , a.FriendID
            from FriendsList a 
	            join UserList b on a.FriendID = b.UserID
            where a.UserID = '{userID}'";

            return _repository.SelectBySQL<ModelUser>(query);
        }
    }
}
