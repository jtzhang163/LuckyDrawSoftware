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
            employees = new EmployeeService().FindAll();

            awards = new AwardService().FindAll();

            setting.AppName = new OptionService().FindAll().FirstOrDefault(o => o.Key == "APP_NAME")?.Value;
        }


        public static List<Employee> employees;

        public static List<Award> awards;

        public static SettingViewModel setting = new SettingViewModel();
    }
}
