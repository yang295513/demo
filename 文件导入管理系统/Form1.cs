using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using 文件导入管理系统.sing;

namespace 文件导入管理系统
{
    public partial class Form1 : Form
    {
        HashSet<stript> data = new HashSet<stript>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "|*.txt";
            if (of.ShowDialog() == DialogResult.OK)
            {              
                string aFirstName = of.FileName.Substring(of.FileName.LastIndexOf("\\") + 1, (of.FileName.LastIndexOf(".") - of.FileName.LastIndexOf("\\") - 1)); //文件名
                read(of.FileName, aFirstName);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBoxfg.Text = "--";
            listView1.Columns.Add("文件名", 100);
            listView1.Columns.Add("导入时间", 200);
            listView1.Columns.Add("冲突条数", 130);
            listView1.Columns.Add("冲突时间", 200);
            readDate();
        }
        public void read(string path,string fileName)//读取到数据库
        {
            if (File.Exists(path) == true)
            {
                this.Text = "正在读取文件";
                StreamReader sr = new StreamReader(path, Encoding.Default);
                string item;
                ListViewItem itemview = new ListViewItem(fileName);
                itemview.SubItems.Add(DateTime.Now.ToString());

                List<stript> da = new List<stript>();
                DateTime time = DateTime.Now;
                string name = "";
                while ((item = sr.ReadLine()) != null)
                {
                    string[] sArray = Regex.Split(item, textBoxfg.Text, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    if (sArray.Count() >= 1)
                    {
                        if (data.Contains(new stript(fileName, sArray[0], sArray[1])) == true)//有重复
                        {
                            stript ss = new stript(fileName, sArray[0], sArray[1]);
                            da.Add(new stript(fileName, sArray[0], sArray[1]));
                            foreach (stript i in data)
                            {
                                if (i.Equals(ss))
                                {
                                    time = i.dt;
                                    name = i.fileName;
                                    break;
                                }
                            }
                        }
                        else//没重复
                        {
                            data.Add(new stript(fileName, sArray[0], sArray[1]));
                        }
                    }
                }
                if (da.Count() > 0)
                {
                    itemview.SubItems.Add(da.Count().ToString());
                    itemview.SubItems.Add(time.ToString());

                    string fileName1 = "重复\\" + DateTime.Now.ToString("MM月dd日 HH时mm分ss秒ff") + "和这一天" + time.ToString("yyyyMMddHHmmssff") + "的" + name + "文件冲突了" + da.Count() + "个.txt";
                    toolStrip1.Text = fileName1;
                    Console.WriteLine(fileName1);
                    StreamWriter sw = new StreamWriter(fileName1, false, Encoding.Default);
                    foreach (var i in da)
                    {
                        sw.WriteLine(i.toLine1());
                    }
                    sw.Close();
                }
                else
                {
                    itemview.SubItems.Add("0");
                    itemview.SubItems.Add("无");
                }
                listView1.Items.Add(itemview);
                sr.Close();
            }
            else
            {
                MessageBox.Show("要读取的文件不存在");
            }
        }

        public void readDate()//读取数据库以及读取列表
        {
            if (File.Exists("data.txt") == true)
            {
                StreamReader sr = new StreamReader("data.txt", Encoding.Default);
                StreamReader srlist = new StreamReader("list.txt", Encoding.Default);
                int flag = 0;
                string item;
                while((item=sr.ReadLine())!=null)
                {
                    string[] sArray = Regex.Split(item, "--", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    if(sArray.Count()==4)
                    {
                        data.Add(new stript(sArray[2], sArray[0], sArray[1], sArray[3]));
                    }
                    else
                    {
                        flag = 1;
                        break;
                    }
                }
                sr.Close();
                srlist.Close();
                if (flag==1)
                {
                    MessageBox.Show("数据库格式不合法");
                    flag = 0;
                }
                else
                {
                    while((item=srlist.ReadLine())!=null)
                    {
                        flag = 0;
                        string[] sArray = Regex.Split(item, "--", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        if(sArray.Count()==4)
                        {
                            ListViewItem items = new ListViewItem(sArray[0]);
                            items.SubItems.Add(sArray[1]);
                            items.SubItems.Add(sArray[2]);
                            items.SubItems.Add(sArray[3]);
                            listView1.Items.Add(items);
                        }
                        else
                        {
                            flag = 1;
                            break;
                        }
                    }
                }
                if(flag==1)
                {
                    MessageBox.Show("list列表有保存数据不合法");
                }
            }
            else
            {
                File.Create("data.txt");
                if(File.Exists("list.txt") ==false)
                {
                    File.Create("list.txt");
                }
            }
        }

        public void writeDate()//数据库写出以及写出列表
        {
           
            StreamWriter sw = new StreamWriter("data.txt", false, Encoding.Default);
            StreamWriter sw1 = new StreamWriter("data1.txt", false, Encoding.Default);
            StreamWriter swlist = new StreamWriter("list.txt", false, Encoding.Default);

            foreach(var i in data)
            {
                sw.WriteLine(i.toLine());
            }

            foreach (var i in data)
            {
                sw1.WriteLine(i.toLine1());
            }
            for(int i=0;i<listView1.Items.Count;i++)
            {
                swlist.WriteLine(listView1.Items[i].SubItems[0].Text + "--" + listView1.Items[i].SubItems[1].Text + "--" + listView1.Items[i].SubItems[2].Text + "--" + listView1.Items[i].SubItems[3].Text);
                //Console.WriteLine(listView1.Items[i].SubItems[0] + "--" + listView1.Items[i].SubItems[1] + "--" + listView1.Items[i].SubItems[2] + "--" + listView1.Items[i].SubItems[3]);
            }

            sw.Flush();
            sw1.Flush();
            swlist.Flush();

            sw.Close();
            sw1.Close();
            swlist.Close();
       }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.Text = System.Windows.Forms.Application.ExecutablePath;
            writeDate();
        }
    }
}
