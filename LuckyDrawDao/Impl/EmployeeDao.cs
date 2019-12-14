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
    public class EmployeeDao: IEmployeeDao
    {
        public List<Employee> FindAll()
        {
            using (IDbConnection cnn = new SQLiteConnection(Helper.ConnectionString))
            {
                var output = cnn.Query<Employee>("select * from t_employee", new DynamicParameters());
                return output.Where(o => !string.IsNullOrEmpty(o.Name)).ToList();
            }
        }

        public void Clear()
        {
            using (IDbConnection cnn = new SQLiteConnection(Helper.ConnectionString))
            {
                cnn.Execute("DELETE FROM t_employee", new DynamicParameters());
                cnn.Execute("UPDATE sqlite_sequence SET seq = 0 WHERE name = 't_employee'", new DynamicParameters());
            }
        }
    }
}
