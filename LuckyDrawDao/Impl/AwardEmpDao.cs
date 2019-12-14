using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using Dapper;
using LuckyDrawDomain;

namespace LuckyDrawDao
{
    public class AwardEmpDao : IAwardEmpDao
    {
        public List<AwardEmp> FindAll()
        {
            using (IDbConnection cnn = new SQLiteConnection(Helper.ConnectionString))
            {
                var output = cnn.Query<AwardEmp>("select id, award_id AwardId, emp_id EmpId from t_award_emp", new DynamicParameters());
                return output.ToList();
            }
        }

        public void Add(AwardEmp awardEmp)
        {
            using (IDbConnection cnn = new SQLiteConnection(Helper.ConnectionString))
            {
                cnn.Execute("insert into t_award_emp (id, award_id , emp_id) values (null, @AwardId, @EmpId)", awardEmp);
            }
        }

        public void Clear()
        {
            using (IDbConnection cnn = new SQLiteConnection(Helper.ConnectionString))
            {
                cnn.Execute("DELETE FROM t_award_emp", new DynamicParameters());
                cnn.Execute("UPDATE sqlite_sequence SET seq = 0 WHERE name = 't_award_emp'", new DynamicParameters());
            }
        }
    }
}
