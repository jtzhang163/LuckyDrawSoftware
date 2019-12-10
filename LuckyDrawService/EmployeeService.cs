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
    }
}
