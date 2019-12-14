using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuckyDrawDomain
{
    public class AwardEmp
    {
        public int Id { get; set; }

        public int AwardId { get; set; }

        public int EmpId { get; set; }

        public AwardEmp(int id, int awardId, int empId)
        {
            Id = id;
            AwardId = awardId;
            EmpId = empId;
        }

        public AwardEmp()
        {
        }
    }
}
