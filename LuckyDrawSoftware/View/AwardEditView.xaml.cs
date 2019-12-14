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
    /// SysSettingView.xaml 的交互逻辑
    /// </summary>
    public partial class AwardEditView : UserControl
    {

        //一等奖 小米复仇者联盟手环4 5\r\n二等奖 天猫精灵M1智能音箱 24\r\n三等奖 欧铂两用纯棉抱枕 28\r\n幸运奖 你比夜色更美华为nova手机 33\r\n特等奖 膳魔师TCME-400S保温杯 9\r\n

        private bool isEdit = false;
        private IAwardService service = new AwardService();
        public AwardEditView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.btnSave.Content = "修改";
            var awards = service.FindAll();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < awards.Count; i++)
            {
                var award = awards[i];
                sb.Append(string.Format("{0} {1} {2}\r\n", award.Mark, award.Name, award.Number));
            }
            this.tbAwardList.Text = sb.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!this.isEdit)
            {
                this.isEdit = true;
                this.tbAwardList.IsEnabled = true;
                this.btnSave.Content = "保存";
                this.tbTip.Text = "";
                return;
            }

            List<Award> awards = new List<Award>();
            var text = this.tbAwardList.Text;

            try
            {
                var awardTxts = this.tbAwardList.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < awardTxts.Length; i++)
                {
                    var awardTxt = awardTxts[i];
                    var fieldTexts = awardTxt.Split(' ');
                    var mark = fieldTexts[0];
                    var name = fieldTexts[1];
                    var number = Convert.ToInt32(fieldTexts[2]);
                    awards.Add(new Award(0, name, number, mark, 0));
                }
            }
            catch (Exception ex)
            {
                this.tbTip.Text = "输入有误：" + ex.Message;
                return;
            }

            new Thread(()=> {

                this.Dispatcher.Invoke(()=>{
                    this.tbTip.Text = "正在保存...";
                    this.btnSave.IsEnabled = false;
                });

                service.Clear();
                service.Add(awards);

                this.Dispatcher.Invoke(() => {
                    this.tbTip.Text = "保存完成";
                    this.btnSave.IsEnabled = true;
                });

            }).Start();

            this.isEdit = false;
            this.tbAwardList.IsEnabled = false;
            this.btnSave.Content = "修改";
        }
    }
}
