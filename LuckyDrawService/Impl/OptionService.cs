using LuckyDrawDao;
using LuckyDrawDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuckyDrawService
{
    public class OptionService : IOptionService
    {
        private OptionDao dao = new OptionDao();
        public List<Option> FindAll()
        {
            return dao.FindAll();
        }

        public string Get(string key)
        {
            return dao.Get(key)?.Value;
        }

        public void Set(string key, string value)
        {
            dao.Set(key, value);
        }
    }
}
