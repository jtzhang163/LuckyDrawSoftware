using LuckyDrawService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LuckyDrawSoftware
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private IEmployeeService employeeService = new EmployeeService();

        private IAwardService awardService = new AwardService();

        private IOptionService optionService = new OptionService();
        public MainWindow()
        {
            InitializeComponent();

            ////显示任务栏
            //this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            //this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            Context.Init();

            this.Title = Context.setting.AppName;

            this.MouseRightButtonDown += RightMouseDown;
        }

        public void RightMouseDown(object sender, MouseButtonEventArgs e)
        {
            new SettingWindow().ShowDialog();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(optionService.Get("APP_NAME1"));
        }
    }
}
