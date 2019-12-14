using LuckyDrawDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyDrawService
{
    public interface IAwardService
    {
        List<Award> FindAll();

        void Add(Award award);

        void Add(List<Award> awards);

        void Clear();
    }
}
