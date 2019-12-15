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

        private int pageShowCount;
        public int PageShowCount
        {
            get { return pageShowCount; }
            set
            {
                this.SetProperty(ref pageShowCount, value);
            }
        }

        private bool isOneByOne;
        public bool IsOneByOne
        {
            get { return isOneByOne; }
            set
            {
                this.SetProperty(ref isOneByOne, value);
            }
        }

        private int refreshInterval;
        public int RefreshInterval
        {
            get { return refreshInterval; }
            set
            {
                this.SetProperty(ref refreshInterval, value);
            }
        }
    }
}
