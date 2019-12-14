using LuckyDrawService;
using LuckyDrawSoftware.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LuckyDrawSoftware
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        private IEmployeeService employeeService = new EmployeeService();

        private IAwardService awardService = new AwardService();

        private IOptionService optionService = new OptionService();
        public SettingWindow()
        {
            InitializeComponent();
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedTab = this.tabControl.SelectedItem as TabItem;
            var g = new Grid();
            if (selectedTab.Header.ToString() == "系统设置")
            {
                g.Children.Add(new SysSettingView());
            }
            selectedTab.Content = g;
        }
    }
}
