using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MakaoTalk.Database
{
    public class DatabaseMSSQLMakaoTalk : DatabaseMSSQL
    {
        public DatabaseMSSQLMakaoTalk() : base()
        {
            Open();
        }

        ~DatabaseMSSQLMakaoTalk()
        {
            Close();
        }

        protected override string GetConnectionString()
        {
            return $@"Data Source=localhost;Initial Catalog=MakaoTalk;Persist Security Info=True;User ID=sa;Password={ GetUserPW() };MultipleActiveResultSets=True;Connect Timeout=30;Application Name=MakaoTalk";
        }

        private string GetUserPW()
        {
            byte[] key = new ASCIIEncoding().GetBytes("abcdefghijklmnopqrstuvwx");
            byte[] IV = new byte[8];

            byte[] Data = { 174, 204, 227, 23, 14, 65, 141, 182, 55, 191, 83, 17, 242, 62, 185, 87 };

            var cStream = new CryptoStream(new MemoryStream(Data),
                new TripleDESCryptoServiceProvider().CreateDecryptor(key, IV),
                CryptoStreamMode.Read);

            byte[] result = new byte[Data.Length];

            cStream.Read(result, 0, result.Length);

            return new ASCIIEncoding().GetString(result, 0, result.Length - 1);
        }

        private string GetInformSecurityPW()
        {
            try
            {
                byte[] key = new ASCIIEncoding().GetBytes("abcdefghijklmnopqrstuvwx");
                byte[] IV = new byte[8];

                byte[] Data = { 127, 219, 83, 70, 130, 78, 116, 26, 100, 211, 66, 182, 189, 37, 202, 73 };

                var cStream = new CryptoStream(new MemoryStream(Data),
                    new TripleDESCryptoServiceProvider().CreateDecryptor(key, IV),
                    CryptoStreamMode.Read);

                byte[] result = new byte[Data.Length];

                cStream.Read(result, 0, result.Length);

                return new ASCIIEncoding().GetString(result, 0, result.Length - 8);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return string.Empty;
            }
        }
    }
}
