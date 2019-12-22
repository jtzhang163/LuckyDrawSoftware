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

        public void Add(Employee emp)
        {
            Add(new List<Employee> { emp });
        }

        public void Add(List<Employee> emps)
        {
            if (emps == null || emps.Count == 0)
            {
                return;
            }

            StringBuilder sb = new StringBuilder("insert into t_employee (id, name, mark, 'order') values ");
            emps.ForEach(o =>
            {
                sb.Append(string.Format("(null, '{0}', '{1}', '{2}'),", o.Name, o.Mark, o.Order));
            });
            string sql = sb.ToString().TrimEnd(',');

            using (IDbConnection cnn = new SQLiteConnection(Helper.ConnectionString))
            {
                cnn.Execute(sql, null);
            }
        }
    }
}
