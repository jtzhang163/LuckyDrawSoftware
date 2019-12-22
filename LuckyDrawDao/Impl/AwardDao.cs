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
            Add(new List<Award> { award });
        }

        public void Add(List<Award> awards)
        {
            if (awards == null || awards.Count == 0)
            {
                return;
            }

            StringBuilder sb = new StringBuilder("insert into t_award (id, name, mark, number, 'order') values ");
            awards.ForEach(o =>
            {
                sb.Append(string.Format("(null, '{0}', '{1}', '{2}', '{3}'),", o.Name, o.Mark, o.Number, o.Order));
            });
            string sql = sb.ToString().TrimEnd(',');

            using (IDbConnection cnn = new SQLiteConnection(Helper.ConnectionString))
            {
                cnn.Execute(sql, null);
            }
        }
    }
}
