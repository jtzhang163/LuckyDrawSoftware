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
    public class AwardDao : IAwardDao
    {
        public List<Award> FindAll()
        {
            using (IDbConnection cnn = new SQLiteConnection(Helper.ConnectionString))
            {
                var output = cnn.Query<Award>("select * from t_award", new DynamicParameters());
                return output.ToList();
                //return output.Where(o => o.Order < 2).ToList();
            }
        }

        public void Clear()
        {
            using (IDbConnection cnn = new SQLiteConnection(Helper.ConnectionString))
            {
                cnn.Execute("DELETE FROM t_award", new DynamicParameters());
                cnn.Execute("UPDATE sqlite_sequence SET seq = 0 WHERE name = 't_award'", new DynamicParameters());
            }
        }

        public void Add(Award award)
        {
            using (IDbConnection cnn = new SQLiteConnection(Helper.ConnectionString))
            {
                cnn.Execute("insert into t_award (id, name, mark, number, 'order') values (null, @Name, @Mark, @Number, @Order)", award);
            }
        }
    }
}
