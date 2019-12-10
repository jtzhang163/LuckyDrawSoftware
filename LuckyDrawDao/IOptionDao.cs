using LuckyDrawDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyDrawDao
{
    public interface IOptionDao
    {
        List<Option> FindAll();

        Option Get(string key);

        void Set(string key, string value);
    }
}
