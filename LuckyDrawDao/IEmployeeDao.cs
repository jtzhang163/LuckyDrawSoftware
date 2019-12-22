using LuckyDrawDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyDrawDao
{
    public interface IEmployeeDao
    {
        List<Employee> FindAll();

        void Add(Employee emp);

        void Add(List<Employee> emps);

        void Clear();
    }
}
