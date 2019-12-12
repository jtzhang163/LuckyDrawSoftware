using LuckyDrawService;
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
        //private IEmployeeService employeeService = new EmployeeService();

        //private IAwardService awardService = new AwardService();

        //private IOptionService optionService = new OptionService();

        private TextBlock[] tbEmployees = new TextBlock[10];

        private int[] remainTime = new int[10];

        private Random random = new Random();

        private bool isOK;

        private void Init()
        {
            Context.Init();

            this.Title = Context.setting.AppName;

            this.MouseRightButtonDown += RightMouseDown;
            this.KeyUp += KeyEvent;
            this.MouseDoubleClick += MouseDbClick;

            for (var i = 0; i < this.tbEmployees.Length; i++)
            {
                this.tbEmployees[i] = (TextBlock)this.FindName("tbEmployee" + (i + 1).ToString("D2"));
            }

            this.player.Play();

            ////显示任务栏
            //this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            //this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        public MainWindow()
        {
            InitializeComponent();

            Init();

            this.isOK = true;
        }


        private bool isRunning = false;

        EventWaitHandle wait = new AutoResetEvent(false);

        int runStatus = 0;

        Thread thread;

        private void Run()
        {
            var employees = Context.employees;
            isRunning = true;
            thread = new Thread(() =>
            {
                var awardIndex = 0;
                do
                {

                    var award = Context.awards[awardIndex];
                    this.Dispatcher.Invoke(() =>
                    {
                        this.player.Volume = 0;
                        this.tbAward.Text = string.Format("{0}      {1}", award.Mark, award.Name);
                    });

                    for (var i = 0; i < this.tbEmployees.Length; i++)
                    {
                        remainTime[i] = i * 15 + 1;
                        this.Dispatcher.Invoke(() =>
                        {
                            this.tbEmployees[i].Text = "";
                        });
                    }
                    ready_sp.Play();
                    runStatus = 1;
                    wait.WaitOne();
                    ready_sp.Stop();
                    going_sp.PlayLooping();
                    runStatus = 2;

                    new Thread(() =>
                    {
                        while ((runStatus == 2 || runStatus == 3) && (remainTime.ToList().Count(o => o > 0) > 0) && isOK)
                        {
                            for (var i = 0; i < this.tbEmployees.Length && isOK; i++)
                            {
                                Thread.Sleep(5);
                                if (remainTime[i] > 0)
                                {
                                    var index = this.random.Next(0, employees.Count);
                                    var employee = employees[index];

                                    this.Dispatcher.Invoke(() =>
                                    {
                                        this.tbEmployees[i].Text = string.Format("{0}  {1}", employee.Mark, employee.Name);
                                    });

                                    if (runStatus == 3)
                                    {
                                        remainTime[i]--;
                                    }
                                }
                            }
                        }

                    }).Start();

                    wait.WaitOne();
                    going_sp.Stop();
                    runStatus = 3;

                    finished_sp.Play();


                    wait.WaitOne();
                    finished_sp.Stop();
                    runStatus = 4;

                    awardIndex++;
                }
                while (awardIndex < Context.awards.Count);

                this.Dispatcher.Invoke(()=> {
                    this.player.Volume = 1;
                });


                isRunning = false;

            });

            thread.Start();
        }


        private SoundPlayer going_sp = new SoundPlayer("./audio/going.wav");
        private SoundPlayer finished_sp = new SoundPlayer("./audio/finished.wav");
        private SoundPlayer ready_sp = new SoundPlayer("./audio/ready.wav");


        public void KeyEvent(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
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
                if (isRunning == false)
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
            this.isOK = false;
            this.player.Stop();

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
