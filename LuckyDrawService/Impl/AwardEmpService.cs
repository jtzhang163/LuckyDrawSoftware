using LuckyDrawDao;
using LuckyDrawDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LuckyDrawService
{
    public class AwardEmpService : IAwardEmpService
    {
        private IAwardEmpDao dao = new AwardEmpDao();

        public List<AwardEmp> FindAll()
        {
            return dao.FindAll();
        }

        public void Add(AwardEmp awardEmp)
        {
            new Thread(() =>
            {
                dao.Add(awardEmp);
            }).Start();
        }

        public void Clear()
        {
            dao.Clear();
        }

    }
}
