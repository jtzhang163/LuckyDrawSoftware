using LuckyDrawDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyDrawDao
{
    public interface IAwardDao
    {
        List<Award> FindAll();

        void Add(Award award);

        void Clear();
    }
}
