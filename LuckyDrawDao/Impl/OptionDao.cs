using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using Dapper;
using LuckyDrawDomain;

namespace LuckyDrawDao
{
    public class OptionDao : IOptionDao
    {
        public List<Option> FindAll()
        {
            using (IDbConnection cnn = new SQLiteConnection(Helper.ConnectionString))
            {
                var output = cnn.Query<Option>("select * from t_option", new DynamicParameters());
                return output.ToList();
            }
        }

        public Option Get(string key)
        {
            using (IDbConnection cnn = new SQLiteConnection(Helper.ConnectionString))
            {
                var output = cnn.Query<Option>("select * from t_option where key = @Key", new { Key = key });
                return output.ToList().FirstOrDefault();
            }
        }

        public void Set(string key, string value)
        {
            using (IDbConnection cnn = new SQLiteConnection(Helper.ConnectionString))
            {
                try
                {
                    var i = cnn.Execute("update t_option set value = @Value where key = @Key", new { Value = value, Key = key });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
