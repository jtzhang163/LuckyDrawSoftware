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
            employees = new EmployeeService().FindAll();

            new AwardService().FindAll().ForEach(o=> {
                awards.Add(new AwardViewModel()
                {
                    Id = o.Id,
                    Name = o.Name,
                    Mark = o.Mark,
                    Number = o.Number
                });
            });

            setting.AppName = new OptionService().FindAll().FirstOrDefault(o => o.Key == "APP_NAME")?.Value;
        }


        public static List<Employee> employees;

        public static List<AwardViewModel> awards = new List<AwardViewModel>();

        public static SettingViewModel setting = new SettingViewModel();
    }
}
