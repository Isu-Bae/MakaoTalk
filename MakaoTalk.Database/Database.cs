using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaoTalk.Database
{
    public interface IDatabase
    {
        bool Open();
        bool Close();
        IEnumerable<T> SelectBy<T>() where T : DomainModel;
        IEnumerable<T> SelectBySQL<T>(string query) where T : DomainModel;
        void Execute(string query);
    }

    public abstract class Database : IDatabase
    {
        public abstract bool Open();
        public abstract bool Close();
        public abstract IEnumerable<T> SelectBy<T>() where T : DomainModel;
        public abstract IEnumerable<T> SelectBySQL<T>(string query) where T : DomainModel;
        public abstract void Execute(string query);

        protected abstract string GetConnectionString();
    }

    public abstract class DomainModel
    {

    }
}
