using LuckyDrawDomain;
using LuckyDrawService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// EmpEditView.xaml 的交互逻辑
    /// </summary>
    public partial class EmpEditView : UserControl
    {

        //一等奖 小米复仇者联盟手环4 5\r\n二等奖 天猫精灵M1智能音箱 24\r\n三等奖 欧铂两用纯棉抱枕 28\r\n幸运奖 你比夜色更美华为nova手机 33\r\n特等奖 膳魔师TCME-400S保温杯 9\r\n

        private bool isEdit = false;
        private IEmployeeService service = new EmployeeService();
        public EmpEditView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.btnSave.Content = "修改";
            var emps = service.FindAll();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < emps.Count; i++)
            {
                var emp = emps[i];
                sb.Append(string.Format("{0} {1}\r\n", emp.Name, emp.Mark));
            }
            this.tbEmpList.Text = sb.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!this.isEdit)
            {
                this.isEdit = true;
                this.tbEmpList.IsEnabled = true;
                this.btnSave.Content = "保存";
                this.tbTip.Text = "";
                return;
            }

            List<Employee> emps = new List<Employee>();
            var text = this.tbEmpList.Text;

            try
            {
                var empTxts = this.tbEmpList.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < empTxts.Length; i++)
                {
                    var empTxt = empTxts[i];
                    var fieldTexts = empTxt.Split(' ');
                    var name = fieldTexts[0];
                    var mark = fieldTexts[1];
                    emps.Add(new Employee(0, name, mark, 0));
                }
            }
            catch (Exception ex)
            {
                this.tbTip.Text = "输入有误：" + ex.Message;
                return;
            }

            new Thread(()=> {

                this.Dispatcher.Invoke(()=>{
                    this.tbTip.Text = "正在保存(可能需要几十秒时间，完成前请不要进行其他操作)...";
                    this.btnSave.IsEnabled = false;
                });

                service.Clear();
                service.Add(emps);

                this.Dispatcher.Invoke(() => {
                    this.tbTip.Text = "保存完成";
                    this.btnSave.IsEnabled = true;
                });

            }).Start();

            this.isEdit = false;
            this.tbEmpList.IsEnabled = false;
            this.btnSave.Content = "修改";
        }
    }
}
