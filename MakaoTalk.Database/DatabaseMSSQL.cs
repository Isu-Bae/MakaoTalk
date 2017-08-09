using MakaoTalk.Database.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaoTalk.Database
{
    public abstract class DatabaseMSSQL : Database
    {
        private SqlConnection _connection = null;

        #region Public Override Functions
        public override bool Close()
        {
            try
            {
                if (_connection != null)
                {
                    _connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }

        }

        public override bool Open()
        {
            try
            {
                if (_connection == null)
                {
                    var connectionString = GetConnectionString();

                    if (string.IsNullOrEmpty(connectionString))
                        return false;

                    _connection = new SqlConnection(connectionString);
                }

                _connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }
        }

        public override IEnumerable<T> SelectBySQL<T>(string query)
        {
            return Select<T>(query: query);
        }

        public override IEnumerable<T> SelectBy<T>()
        {
            return Select<T>();
        }

        public override void Execute(string query)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
        }
        #endregion

        #region Private Functions
        private IEnumerable<T> Select<T>(string query = "", string tableName = "") where T : DomainModel
        {
            var list = new List<DomainModel>();
            var table = (T)Activator.CreateInstance(typeof(T));

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;

            if (string.IsNullOrEmpty(query))
            {
                if (string.IsNullOrEmpty(tableName))
                    cmd.CommandText = $"select * from {table.GetType().Name}";
                else
                    cmd.CommandText = $"select * from {tableName}";
            }
            else
                cmd.CommandText = query;

            var reader = cmd.ExecuteReader();
            var data = new DataTable();
            data.Load(reader);

            foreach (DataRow row in data.Rows)
            {
                var item = (T)Activator.CreateInstance(typeof(T));

                foreach (DataColumn col in data.Columns)
                {
                    foreach (var pty in item.GetType().GetProperties())
                    {
                        var valueType = Nullable.GetUnderlyingType(pty.PropertyType) ?? pty.PropertyType;

                        if (pty.Name.Equals(col.ColumnName))
                        {
                            pty.SetValue(item, DBNull.Value.Equals(row[col]) ? null : Convert.ChangeType(row[col], valueType), null);
                        }

                        var attr = (SqlAttribute[])pty.GetCustomAttributes(typeof(SqlAttribute), false);

                        if (attr != null && attr.Length > 0)
                        {
                            if (attr[0].FieldName.Equals(col.ColumnName))
                            {
                                pty.SetValue(item, DBNull.Value.Equals(row[col]) ? null : Convert.ChangeType(row[col], valueType), null);
                            }
                        }

                    }
                }
                list.Add(item);
            }

            return list.OfType<T>();
        }
        #endregion
    }
}
