using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyDrawSoftware
{
    public class SettingViewModel : BindableObject
    {
        private string appName;
        public string AppName
        {
            get { return appName; }
            set
            {
                this.SetProperty(ref appName, value);
            }
        }
    }
}
