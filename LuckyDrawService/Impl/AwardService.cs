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
    }
}
