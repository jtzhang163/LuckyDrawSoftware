using LuckyDrawDomain;
using LuckyDrawService;
using LuckyDrawSoftware.ViewModel;
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
            var options = new OptionService().FindAll();
            setting.AppName = options.FirstOrDefault(o => o.Key == "APP_NAME")?.Value;
            setting.PageShowCount = Convert.ToInt32(options.FirstOrDefault(o => o.Key == "PAGE_SHOW_COUNT")?.Value);
            setting.IsOneByOne = options.FirstOrDefault(o => o.Key == "IS_ONE_BY_ONE")?.Value == "是" ? true : false;
        }

        public static SettingViewModel setting = new SettingViewModel();
    }
}
