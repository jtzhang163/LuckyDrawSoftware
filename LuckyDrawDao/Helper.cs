using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyDrawDao
{
    public static class Helper
    {

        public static string ConnectionString = ConfigurationManager.ConnectionStrings["sqlite"].ConnectionString;

    }
}
