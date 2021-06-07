using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using PubOp;

namespace Modification
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        static string strFilePath = Application.StartupPath + @"\Appli.ini";
        OperateIniFile op = new OperateIniFile();
        static string servers = OperateIniFile.ReadIniData("数据库连接", "server", "", strFilePath);
        static string datas = OperateIniFile.ReadIniData("数据库连接", "database", "", strFilePath);
        static string names = OperateIniFile.ReadIniData("数据库连接", "uid", "", strFilePath);
        static string pwds = OperateIniFile.ReadIniData("数据库连接", "pwd", "", strFilePath);
        static string sqlcon = "Data Source=" + servers + ";Initial Catalog=" + datas + ";User ID=" + names + ";pwd=" + pwds + ";";
        //static string sqlcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        SqlConnection conn = new SqlConnection(sqlcon);
        //登录
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (username.Text != "")
                {
                    if (userpwd.Text != "")
                    {
                        conn.Open();
                        string str = string.Format("select 密码,用户等级 from DM_hUsers_操作权限 where 用户名='{0}'", username.Text);
                        SqlDataAdapter sda = new SqlDataAdapter(str, conn);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        conn.Close();
                        if (userpwd.Text == dt.Rows[0]["密码"].ToString())
                        {
                            UserInfo.userName = username.Text;
                            Form1 f1 = new Form1();
                            f1.Show();
                            this.Dispose(false);
                        }
                        else
                        {
                            MessageBox.Show("用户名或密码错误,请重新输入!");
                            username.Text = "";
                            userpwd.Text = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("请输入密码!");
                        userpwd.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("请输入用户名!");
                    username.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接失败");
                username.Text = "";
                userpwd.Text = "";
                username.Focus();
                Console.Write(ex.Message);
            }
        }

        public void Form2Value()
        {
            string uname = username.Text;
        }
    }
}
