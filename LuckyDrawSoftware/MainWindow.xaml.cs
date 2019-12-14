using LuckyDrawDomain;
using LuckyDrawService;
using LuckyDrawSoftware.json;
using LuckyDrawSoftware.UC;
using LuckyDrawSoftware.ViewModel;
using LuckyDrawUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
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
        //private Logger logger = Logger.Default;

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private List<EmployeeUC> EmpUCs = new List<EmployeeUC>();

        private List<int> RemainTimes = new List<int>();

        private Random random = new Random();

        private IAwardEmpService awardEmpService = new AwardEmpService();
        private IAwardService awardService = new AwardService();
        private IEmployeeService employeeService = new EmployeeService();

        private bool isRunning;

        private void Init()
        {
            //数据初始化
            Context.Init();

            //绑定事件
            this.KeyUp += KeyEvent;
            this.MouseDoubleClick += MouseDbClick;

            this.isRunning = true;
            this.player.Play();
        }

        /// <summary>
        /// 设置 Grid 
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="count"></param>
        /// <param name="isEmp"></param>
        private void SetGrid(int rows, int cols, int count, bool isEmp)
        {
            ResetGrid();
            for (int i = 0; i < rows; i++)
            {
                this.emp_grid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < cols; i++)
            {
                this.emp_grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < count; i++)
            {
                var empUC = isEmp ? new EmployeeUC() : new EmployeeUC(2, 3);
                this.EmpUCs.Add(empUC);
                this.RemainTimes.Add(0);
            }

            int index = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (index < this.EmpUCs.Count)
                    {
                        this.emp_grid.Children.Add(this.EmpUCs[index]);
                        Grid.SetRow(this.EmpUCs[index], i);
                        Grid.SetColumn(this.EmpUCs[index], j);
                        index++;
                    }
                }
            }
        }

        /// <summary>
        /// 显示开场白
        /// </summary>
        private void ShowOpening(List<Award> awards)
        {
            this.tbAwardTip.Text = "奖品如下";
            SetGrid(awards.Count, 1, awards.Count, false);
            for (int i = 0; i < this.EmpUCs.Count; i++)
            {
                var award = awards[i];
                this.EmpUCs[i].Update(new Employee()
                {
                    Mark = award.Mark,
                    Name = string.Format("{0}({1}个)", award.Name, award.Number),
                });
            }
        }


        /// <summary>
        /// 设置 Grid 
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="count"></param>
        /// <param name="isEmp"></param>
        private void ShowFinalList()
        {
            var awards = awardService.FindAll();
            var emps = employeeService.FindAll();
            var awardEmps = awardEmpService.FindAll();

            this.Dispatcher.Invoke(()=> {

                this.player.Volume = 1;
                this.tbAward.Text = "抽奖结束";
                this.tbAwardTip.Text = string.Format("共{0}人，详细名单按 Ctrl+F 查看", awardEmps.Count);
                ResetGrid();

                this.emp_grid.RowDefinitions.Add(new RowDefinition());
                this.emp_grid.ColumnDefinitions.Add(new ColumnDefinition());

                List<AwardJson> awardJsons = new List<AwardJson>();

                for (var i = 0; i < awards.Count; i++)
                {
                    var award = awards[i];
                    var awardJson = new AwardJson();
                    
                    var currentAwardEmps = awardEmps.Where(o => o.AwardId == award.Id).ToList();
                    awardJson.name = award.Name;
                    awardJson.mark = award.Mark;
                    awardJson.number = currentAwardEmps.Count;


                    List<EmpJson> empJsons = new List<EmpJson>();
                    for (var j = 0; j < currentAwardEmps.Count; j++)
                    {
                        var awardEmp = currentAwardEmps[j];
                        var emp = emps.FirstOrDefault(o => o.Id == awardEmp.EmpId);
                        if (emp != null)
                        {
                            var empJson = new EmpJson();
                            empJson.name = emp.Name;
                            empJson.mark = emp.Mark;
                            empJsons.Add(empJson);
                        }
                    }

                    awardJson.emps = empJsons;
                    awardJsons.Add(awardJson);
                }

                var json = JsonHelper.Serialize<AwardJson>(awardJsons);

                json = json.Replace('"','\'');

                WriteHtmlUtil.Write(json);
            });
        }

        private void ResetGrid()
        {
            this.emp_grid.Children.Clear();
            this.emp_grid.RowDefinitions.Clear();
            this.emp_grid.ColumnDefinitions.Clear();
            this.EmpUCs.Clear();
            this.RemainTimes.Clear();
        }

        EventWaitHandle wait = new AutoResetEvent(false);

        int runStatus = -1;

        Thread thread;

        private void Run()
        {
            var awards = awardService.FindAll();
            var emps = employeeService.FindAll();
            var awardEmps = awardEmpService.FindAll();

            var showCount = Context.setting.PageShowCount;

            thread = new Thread(() =>
            {
                //开场白
                this.Dispatcher.Invoke(() => { ShowOpening(awards); });

                runStatus = 0;
                wait.WaitOne();

                awardEmpService.Clear();
                var awardIndex = 0;

                do
                {
                    var award = new AwardViewModel(awards[awardIndex]);
                    while (award.CurrentCycle * showCount < award.Number)
                    {
                        award.CurrentCycle++;
                        var gridCount = showCount;
                        var tmpGridCount = award.Number - (award.CurrentCycle - 1) * showCount;
                        if (tmpGridCount < showCount)
                        {
                            gridCount = tmpGridCount;
                        }

                        this.Dispatcher.Invoke(() =>
                        {
                            this.player.Volume = 0;
                            this.tbAward.Text = string.Format("{0}      {1}", award.Mark, award.Name);
                            if (award.Number <= showCount)
                            {
                                this.tbAwardTip.Text = string.Format("({0}/{0})", award.Number);
                            }
                            else
                            {
                                this.tbAwardTip.Text = string.Format("第{0}轮({1}~{2}/{3})", 
                                    award.CurrentCycle, 
                                    (award.CurrentCycle - 1) * showCount + 1, 
                                    (award.CurrentCycle - 1) * showCount + gridCount, award.Number);
                            }

                            if (gridCount > 3)
                            {
                                SetGrid((int)Math.Ceiling(gridCount / 2.0), 2, gridCount, true);
                            }
                            else
                            {
                                SetGrid(gridCount, 1, gridCount, true);
                            }
                        });


                        for (var i = 0; i < gridCount; i++)
                        {
                            RemainTimes[i] = Context.setting.IsOneByOne ? (i * showCount + 1) : 1;
                            this.EmpUCs[i].Update(new Employee());
                        }
                        ready_sp.Play();


                        runStatus = 1;
                        wait.WaitOne();
                        runStatus = 2;

                        ready_sp.Stop();
                        going_sp.PlayLooping();

                        isFrushing = true;
                        var t = new Thread(() =>
                        {
                            while ((runStatus == 2 || runStatus == 3) && RemainTimes.Count(o => o > 0) > 0 && isRunning)
                            {
                                for (var i = 0; i < this.EmpUCs.Count && isRunning; i++)
                                {
                                    if (RemainTimes[i] > 0)
                                    {
                                        var index = this.random.Next(0, emps.Count);
                                        var employee = emps[index];

                                        this.EmpUCs[i].Update(employee);

                                        if (runStatus == 3)
                                        {
                                            RemainTimes[i]--;
                                        }
                                    }

                                    if (RemainTimes[i] <= 0)
                                    {
                                        var emp = emps.FirstOrDefault(o => o.Id == this.EmpUCs[i].employeeVM.Id);
                                        if (emp != null)
                                        {
                                            awardEmpService.Add(new AwardEmp(0, award.Id, emp.Id));
                                            emps.Remove(emp);
                                        }
                                    }
                                }
                                Thread.Sleep(30);
                            }
                            isFrushing = false;
                        });

                        t.Priority = ThreadPriority.Highest;
                        t.Start();

                        wait.WaitOne();


                        going_sp.Stop();
                        finished_sp.Play();
                        runStatus = 3;
                        wait.WaitOne();

                        finished_sp.Stop();
                        //runStatus = 4;

                    }

                    awardIndex++;
                }
                while (awardIndex < awards.Count);

                ShowFinalList();
                runStatus = 5;
            });
            thread.Priority = ThreadPriority.Highest;
            thread.Start();
        }


        private SoundPlayer going_sp = new SoundPlayer("./audio/going.wav");
        private SoundPlayer finished_sp = new SoundPlayer("./audio/finished.wav");
        private SoundPlayer ready_sp = new SoundPlayer("./audio/ready.wav");


        public void KeyEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                Start();
            }
            else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.F)
            {
                System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "html\\获奖名单.html");
                Console.WriteLine("show 获奖名单");
            }
        }


        private bool canStart = true;
        private bool isFrushing = false;

        private void Start()
        {
            if (!canStart)
            {
                return;
            }

            if (runStatus == -1)
            {
                Run();
            }
            else if (runStatus == 3)
            {
                if (!isFrushing)
                {
                    wait.Set();
                }
            }
            else
            {
                wait.Set();
            }
            canStart = false;

            new Thread(() =>
            {
                Thread.Sleep(3000);
                canStart = true;
            }).Start();
        }

        private void MouseDbClick(object sender, MouseButtonEventArgs e)
        {
            Start();
        }

        private void CloseMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.isRunning = false;

            if (thread != null)
            {
                thread.Abort();
            }

            Thread.Sleep(1000);

            this.Close();
            Application.Current.Shutdown(0);
        }

        private void player_MediaEnded(object sender, RoutedEventArgs e)
        {
            //设置一下视频进度，确保从头开始播放
            this.player.Position = TimeSpan.Zero;
            this.player.Play();
        }
    }
}
