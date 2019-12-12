using LuckyDrawDomain;
using LuckyDrawService;
using LuckyDrawSoftware.UC;
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
        public MainWindow()
        {
            InitializeComponent();

            Init();

        }

        private List<EmployeeUC> EmpUCs = new List<EmployeeUC>();

        private List<int> RemainTimes = new List<int>();

        private Random random = new Random();

        private bool isRunning;

        private void Init()
        {
            //数据初始化
            Context.Init();

            //绑定事件
            this.MouseRightButtonDown += RightMouseDown;
            this.KeyUp += KeyEvent;
            this.MouseDoubleClick += MouseDbClick;

            this.isRunning = true;
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
        private void ShowOpening()
        {
            this.tbAwardTip.Text = "奖品如下";
            SetGrid(Context.awards.Count, 1, Context.awards.Count, false);
            for (int i = 0; i < this.EmpUCs.Count; i++)
            {
                var award = Context.awards[i];
                this.EmpUCs[i].Update(new Employee()
                {
                    Mark = award.Mark,
                    Name = string.Format("{0}({1}个)", award.Name, award.Number),
                });
            }
        }


        /// <summary>
        /// 显示奖项标题
        /// </summary>
        private void ShowAwardTitle()
        {

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
            thread = new Thread(() =>
            {
                //开场白
                this.Dispatcher.Invoke(() => { ShowOpening(); });

                runStatus = 0;
                wait.WaitOne();

                var awardIndex = 0;
                var employees = Context.employees;
                do
                {
                    var award = Context.awards[awardIndex];

                    while (award.CurrentCycle * 10 < award.Number)
                    {

                        award.CurrentCycle++;
                        var gridCount = 10;
                        var tmpGridCount = award.Number - (award.CurrentCycle - 1) * 10;
                        if (tmpGridCount < 10)
                        {
                            gridCount = tmpGridCount;
                        }

                        this.Dispatcher.Invoke(() =>
                        {
                            this.player.Volume = 0;
                            this.tbAward.Text = string.Format("{0}      {1}", award.Mark, award.Name);
                            if (award.Number <= 10)
                            {
                                this.tbAwardTip.Text = string.Format("({0}/{0})", award.Number);
                            }
                            else
                            {
                                this.tbAwardTip.Text = string.Format("第{0}轮({1}~{2}/{3})", award.CurrentCycle, (award.CurrentCycle - 1) * 10 + 1, (award.CurrentCycle - 1) * 10 + gridCount, award.Number);
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
                            RemainTimes[i] = i * 10 + 1;
                            this.EmpUCs[i].Update(new Employee());
                        }
                        ready_sp.Play();


                        runStatus = 1;
                        wait.WaitOne();

                        ready_sp.Stop();
                        going_sp.PlayLooping();

                        new Thread(() =>
                        {
                            while ((runStatus == 2 || runStatus == 3) && (RemainTimes.Count(o => o > 0) > 0) && isRunning)
                            {
                                for (var i = 0; i < this.EmpUCs.Count && isRunning; i++)
                                {
                                    Thread.Sleep(5);
                                    if (RemainTimes[i] > 0)
                                    {
                                        var index = this.random.Next(0, employees.Count);
                                        var employee = employees[index];

                                        this.EmpUCs[i].Update(employee);

                                        if (runStatus == 3)
                                        {
                                            RemainTimes[i]--;
                                        }
                                    }
                                }
                            }
                        }).Start();

                        runStatus = 2;
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
                while (awardIndex < Context.awards.Count);

                this.Dispatcher.Invoke(() =>
                {
                    this.player.Volume = 1;
                });
            });

            thread.Start();
        }


        private SoundPlayer going_sp = new SoundPlayer("./audio/going.wav");
        private SoundPlayer finished_sp = new SoundPlayer("./audio/finished.wav");
        private SoundPlayer ready_sp = new SoundPlayer("./audio/ready.wav");


        public void KeyEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                Console.WriteLine("空格");
                Start();
            }
        }


        private bool canStart = true;

        private void Start()
        {
            if (canStart)
            {
                if (runStatus == -1)
                {
                    Run();
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
        }

        private void MouseDbClick(object sender, MouseButtonEventArgs e)
        {
            Start();
        }

        public void RightMouseDown(object sender, MouseButtonEventArgs e)
        {
            new SettingWindow().ShowDialog();
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
    }
}
