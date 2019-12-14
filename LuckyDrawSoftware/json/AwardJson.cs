using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyDrawSoftware.json
{
    public class AwardJson
    {
        public string name { get; set; }

        public string mark { get; set; }

        public int number { get; set; }

        public List<EmpJson> emps { get; set; }
    }
}
