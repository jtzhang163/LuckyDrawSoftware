using LuckyDrawDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyDrawDao
{
    public interface IAwardEmpDao
    {
        List<AwardEmp> FindAll();

        void Add(AwardEmp awardEmp);

        void Clear();
    }
}
