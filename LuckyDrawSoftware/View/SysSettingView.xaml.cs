using LuckyDrawService;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LuckyDrawSoftware.View
{
    /// <summary>
    /// SysSettingView.xaml 的交互逻辑
    /// </summary>
    public partial class SysSettingView : UserControl
    {
        private bool isEdit = false;
        private IOptionService service = new OptionService();
        public SysSettingView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbAppName.Text = Context.setting.AppName;
            this.tbPageShowCount.Text = Context.setting.PageShowCount.ToString();
            this.tbIsOneByOne.Text = Context.setting.IsOneByOne ? "是" : "否";

            this.btnSave.Content = "修改";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!this.isEdit)
            {
                this.tbAppName.IsEnabled = true;
                this.tbPageShowCount.IsEnabled = true;
                this.tbIsOneByOne.IsEnabled = true;
                this.isEdit = true;
                this.btnSave.Content = "保存";
                return;
            }

            if (!int.TryParse(this.tbPageShowCount.Text.Trim(), out int pageShowCount))
            {
                MessageBox.Show("【每页显示个数】输入格式有误！", "错误提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var tbIsOneByOneText = this.tbIsOneByOne.Text.Trim();
            if (tbIsOneByOneText != "是" && tbIsOneByOneText != "否")
            {
                MessageBox.Show("【是否逐一产生】输入格式有误！", "错误提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var appName = this.tbAppName.Text.Trim();
            Context.setting.AppName = appName;
            service.Set("APP_NAME", appName);
            this.tbAppName.IsEnabled = false;


            Context.setting.PageShowCount = pageShowCount;
            service.Set("PAGE_SHOW_COUNT", pageShowCount.ToString());
            this.tbPageShowCount.IsEnabled = false;

            var isOneByOne = tbIsOneByOneText == "是" ? true : false;
            Context.setting.IsOneByOne = isOneByOne;
            service.Set("IS_ONE_BY_ONE", tbIsOneByOneText);
            this.tbIsOneByOne.IsEnabled = false;

            this.isEdit = false;
            this.btnSave.Content = "修改";
        }
    }
}
