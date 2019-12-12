using LuckyDrawDomain;
using LuckyDrawSoftware.ViewModel;
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

namespace LuckyDrawSoftware.UC
{
    /// <summary>
    /// EmployeeUC.xaml 的交互逻辑
    /// </summary>
    public partial class EmployeeUC : System.Windows.Controls.UserControl
    {
        public EmployeeViewModel employeeVM;
        public EmployeeUC()
        {
            InitializeComponent();
            this.DataContext = employeeVM;
        }

        public void Update(Employee employee)
        {
            employeeVM.Id = employee.Id;
            employeeVM.Name = employee.Name;
            employeeVM.Mark = employee.Mark;
        }
    }
}
