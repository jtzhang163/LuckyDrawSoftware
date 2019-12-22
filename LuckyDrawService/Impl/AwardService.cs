using LuckyDrawDao;
using LuckyDrawDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuckyDrawService
{
    public class AwardService : IAwardService
    {
        private IAwardDao dao = new AwardDao();

        public List<Award> FindAll()
        {
            return dao.FindAll();
        }

        public void Add(Award award)
        {
            dao.Add(award);
        }

        public void Add(List<Award> awards)
        {
            dao.Add(awards);
        }

        public void Clear()
        {
            dao.Clear();
        }
    }
}
