using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LuckyDrawUtil
{
    public class WriteHtmlUtil
    {
        private static string path_bak = AppDomain.CurrentDomain.BaseDirectory + "html\\draw-list.bak.html";
        private static string path = AppDomain.CurrentDomain.BaseDirectory + "html\\获奖名单.html";

        public static void Write(string json)
        {

            //获取html初始文件
            FileStream fs_bak = new FileStream(path_bak, FileMode.Open);
            StreamReader sr_bak = new StreamReader(fs_bak);
            var content = sr_bak.ReadToEnd();
            sr_bak.Close();
            fs_bak.Close();

            //x轴坐标值
            content = content.Replace("'%DATA%'", json);
            //产量

            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            sw.Write(content);
            sw.Close();
            sw.Close();
        }
    }
}
