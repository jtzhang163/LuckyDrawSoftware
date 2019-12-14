using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuckyDrawDomain
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Mark { get; set; }

        public int Order { get; set; }

        public Employee(int id, string name, string mark, int order)
        {
            Id = id;
            Name = name;
            Mark = mark;
            Order = order;
        }

        public Employee()
        {
        }
    }
}
