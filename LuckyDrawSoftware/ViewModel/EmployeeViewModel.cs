using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyDrawSoftware.ViewModel
{
    public class EmployeeViewModel : BindableObject
    {

        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                this.SetProperty(ref id, value);
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                this.SetProperty(ref name, value);
            }
        }


        private string mark;
        public string Mark
        {
            get { return mark; }
            set
            {
                this.SetProperty(ref mark, value);
            }
        }

        public EmployeeViewModel(string name, string mark)
        {
            this.name = name;
            this.mark = mark;
        }


    }
}
