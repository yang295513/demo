using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 文件导入管理系统.sing
{
    class stript
    {
        public string fileName;
        public string zh;
        public string pwd;
        public DateTime dt;
        public stript(string fileName,string zh,string pwd)
        {
            this.fileName = fileName;
            this.zh = zh;
            this.pwd = pwd;
            dt = DateTime.Now;
        }

        public stript(string fileName, string zh, string pwd,string time)
        {
            this.fileName = fileName;
            this.zh = zh;
            this.pwd = pwd;
            dt = DateTime.Parse(time);
        }

        public override bool Equals(object obj)
        {
            stript e = obj as stript;
            return this.zh == e.zh && this.pwd == e.pwd;
        }

        public override int GetHashCode()
        {
            return this.zh.GetHashCode() * 100 + this.pwd.GetHashCode();
        }

        public string toLine()
        {
            return zh + "--" + pwd +"--"+ fileName +"--"+ dt.ToString();
        }
        public string toLine1()
        {
            return zh + "--" + pwd;
        }

    }
}
