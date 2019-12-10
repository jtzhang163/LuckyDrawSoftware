using LuckyDrawDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyDrawService
{
    public interface IOptionService
    {
        List<Option> FindAll();

        string Get(string key);

        void Set(string key, string value);
    }
}
