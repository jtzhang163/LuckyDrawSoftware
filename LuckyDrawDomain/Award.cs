using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuckyDrawDomain
{
    public class Award
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }

        public string Mark { get; set; }

        public int Order { get; set; }

        public Award(int id, string name, int number, string mark, int order)
        {
            Id = id;
            Name = name;
            Number = number;
            Mark = mark;
            Order = order;
        }

        public Award()
        {
        }
    }
}
