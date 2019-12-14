using LuckyDrawDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyDrawService
{
    public interface IEmployeeService
    {
        List<Employee> FindAll();

        void Add(Employee emp);

        void Add(List<Employee> emps);

        void Clear();
    }
}
