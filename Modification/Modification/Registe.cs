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
    public partial class Registe : Form
    {
        public Registe()
        {
            InitializeComponent();
        }
        string uname = UserInfo.userName;
        static string strFilePath = Application.StartupPath + @"\Appli.ini";
        OperateIniFile op = new OperateIniFile();
        static string servers = OperateIniFile.ReadIniData("数据库连接", "server", "", strFilePath);
        static string datas = OperateIniFile.ReadIniData("数据库连接", "database", "", strFilePath);
        static string names = OperateIniFile.ReadIniData("数据库连接", "uid", "", strFilePath);
        static string pwds = OperateIniFile.ReadIniData("数据库连接", "pwd", "", strFilePath);
        static string sqlcon = "Data Source=" + servers + ";Initial Catalog=" + datas + ";User ID=" + names + ";pwd=" + pwds + ";";
        SqlConnection conn = new SqlConnection(sqlcon);
        //确认修改密码
        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            string str = string.Format("select 密码 from DM_hUsers_操作权限 where 用户名='{0}'", uname);
            SqlDataAdapter sda = new SqlDataAdapter(str, conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            conn.Close();
            if (oldpwd.Text != "")
            {
                if (oldpwd.Text == dt.Rows[0]["密码"].ToString())
                {
                    if (confirmpwd.Text == newpwd.Text)
                    {
                        conn.Open();
                        string str1 = string.Format("update DM_hUsers_操作权限 set 密码='{0}' where 用户名='{1}'", newpwd.Text,uname);
                        SqlCommand cmd = new SqlCommand(str1, conn);
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("密码修改成功!");
                            this.Close();
                            Form1 f1 = new Form1();
                            f1.Show();
                        }
                        conn.Close();
                    }
                    else
                    {
                        MessageBox.Show("两次密码输入不一致,请重新输入!");
                        confirmpwd.Text = "";
                        confirmpwd.Focus();
                    }
            }
                else
                {
                    MessageBox.Show("原始密码错误,请重新输入!");
                    oldpwd.Text = "";
                    oldpwd.Focus();
                }
            }
            else
            {
                return;
                oldpwd.Focus();
            }
        }
        //取消修改
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 f1 = new Form1();
            f1.Show();
        }
    }
}
