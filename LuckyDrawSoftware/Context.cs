using LuckyDrawDomain;
using LuckyDrawService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyDrawSoftware
{
    public class Context
    {
        public static void Init()
        {
            IOptionService service = new OptionService();
            List<Option> options = service.FindAll();
            setting.AppName = options.FirstOrDefault(o => o.Key == "APP_NAME")?.Value;
        }
        public static SettingViewModel setting = new SettingViewModel();
    }
}
