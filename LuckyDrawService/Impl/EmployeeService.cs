using LuckyDrawDao;
using LuckyDrawDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuckyDrawService
{
    public class EmployeeService: IEmployeeService
    {
        private IEmployeeDao dao = new EmployeeDao();
        public List<Employee> FindAll()
        {
            return dao.FindAll();
        }

        public void Add(Employee emp)
        {
            dao.Add(emp);
        }

        public void Add(List<Employee> emps)
        {
            emps.ForEach(o => Add(o));
        }

        public void Clear()
        {
            dao.Clear();
        }
    }
}
