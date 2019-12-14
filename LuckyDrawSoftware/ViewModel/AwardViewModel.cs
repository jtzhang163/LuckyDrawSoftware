using LuckyDrawDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyDrawSoftware.ViewModel
{
    public class AwardViewModel : BindableObject
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

        private int number;
        public int Number
        {
            get { return number; }
            set
            {
                this.SetProperty(ref number, value);
            }
        }

        public int CurrentCycle { get; set; }

        public AwardViewModel(Award award)
        {
            this.Id = award.Id;
            this.Name = award.Name;
            this.Mark = award.Mark;
            this.Number = award.Number;
        }

        public AwardViewModel(string name, string mark)
        {
            this.name = name;
            this.mark = mark;
        }

        public AwardViewModel() : this("", "")
        {
        }
    }
}
