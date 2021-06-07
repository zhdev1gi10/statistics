using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections;
using System.Runtime.InteropServices;
using System.Globalization;
using System.IO;
using System.Drawing.Imaging;
using System.Net;
using System.Web.Services;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Text.RegularExpressions; 
using PubOp;
using System.Web;

namespace Modification
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string uname = UserInfo.userName;
        int a = 0;
        static string strFilePath = Application.StartupPath + @"\Appli.ini";
        OperateIniFile op = new OperateIniFile();
        static string servers = OperateIniFile.ReadIniData("数据库连接", "server", "", strFilePath);
        static string datas = OperateIniFile.ReadIniData("数据库连接", "database", "", strFilePath);
        static string names = OperateIniFile.ReadIniData("数据库连接", "uid", "", strFilePath);
        static string pwds = OperateIniFile.ReadIniData("数据库连接", "pwd", "", strFilePath);
        static string datas1 = OperateIniFile.ReadIniData("数据库连接", "路试数据库", "", strFilePath);
        static string photoserver = OperateIniFile.ReadIniData("照片的路径", "server", "", strFilePath);
        static string photoadress = OperateIniFile.ReadIniData("照片的路径", "filename", "", strFilePath);
        static string Choices = OperateIniFile.ReadIniData("照片的路径", "打印照片", "", strFilePath);
        static string formats = OperateIniFile.ReadIniData("A3报表格式", "format", "", strFilePath);
        static string pd = OperateIniFile.ReadIniData("A3报表格式", "pd", "", strFilePath);
        static string dqbh = OperateIniFile.ReadIniData("编号设置", "地区编号", "", strFilePath);
        static string jczbh = OperateIniFile.ReadIniData("编号设置", "检测站编号", "", strFilePath);
        static string wjwg = OperateIniFile.ReadIniData("人工检验项目", "外观检查", "", strFilePath);
        static string wjdj = OperateIniFile.ReadIniData("人工检验项目", "底盘检查", "", strFilePath);
        static string wjsz = OperateIniFile.ReadIniData("人工检验项目", "count", "", strFilePath);
        static string wjwyxrd = OperateIniFile.ReadIniData("人工检验项目", "唯一性认定", "", strFilePath);
        static string wjgzxxzd = OperateIniFile.ReadIniData("人工检验项目", "故障信息诊断", "", strFilePath);
        static string wjyxjc = OperateIniFile.ReadIniData("人工检验项目", "运行检查", "", strFilePath);
        static string wjhcpd = OperateIniFile.ReadIniData("人工检验项目", "核查评定", "", strFilePath);
        static string wjybx = OperateIniFile.ReadIniData("人工检验项目", "一般项", "", strFilePath);
        static string wjgjx = OperateIniFile.ReadIniData("人工检验项目", "关键项", "", strFilePath);
        static string wjfjx = OperateIniFile.ReadIniData("人工检验项目", "分级项", "", strFilePath);
        static string djpdjl = OperateIniFile.ReadIniData("等级评定结论", "结论1", "", strFilePath);
        static string jdz = OperateIniFile.ReadIniData("检测站信息", "地址", "", strFilePath);
        static string jgsmc = OperateIniFile.ReadIniData("检测站信息", "公司名称", "", strFilePath);
        static string jlxfs = OperateIniFile.ReadIniData("检测站信息", "联系方式", "", strFilePath);
        static string sqlcon = "Data Source=" + servers + ";Initial Catalog=" + datas + ";User ID=" + names + ";pwd=" + pwds + ";";
        static string sqlcon1 = "Data Source=" + servers + ";Initial Catalog=" + datas1 + ";User ID=" + names + ";pwd=" + pwds + ";";
        SqlConnection conn = new SqlConnection(sqlcon);
        SqlConnection conn1 = new SqlConnection(sqlcon1);
        //数据全部清空
        public void ClearText()
        {
            fid.Visible = false;
            cllx.Text = "";
            jylb.Text = "";
            qzdz.Text = "";
            ywlx.Text = "";
            foreach (Control ctl in this.tabPage1.Controls)
            {
                if (ctl is TextBox)
                {
                    (ctl as TextBox).Text = "";
                }
            }
            foreach (Control ctls in this.tabPage3.Controls)
            {
                if (ctls is GroupBox)
                {
                    foreach (Control ctlas in ctls.Controls)
                    {
                        if (ctlas is TextBox)
                        {
                            (ctlas as TextBox).Text = "";
                        }
                    }
                }
            }
            foreach (Control ctls1 in this.tabPage3.Controls)
            {
                if (ctls1 is GroupBox)
                {
                    foreach (Control ctlas1 in ctls1.Controls)
                    {
                        if (ctlas1 is Label)
                        {
                            if ((ctlas1 as Label).Text.Contains("×"))
                            {
                                ctlas1.Visible = false;
                            }
                            if ((ctlas1 as Label).Text=="一")
                            {
                                (ctlas1 as Label).Text = "";
                            }
                            if ((ctlas1 as Label).Text == "二")
                            {
                                (ctlas1 as Label).Text = "";
                            }
                            if ((ctlas1 as Label).Text=="不合格")
                            {
                                (ctlas1 as Label).Text = "";
                            }
                        }
                    }
                }
            }
        }
        //查询
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string str3 = string.Format("select 用户等级 from DM_hUsers_操作权限 where 用户名='{0}'", uname);
                SqlDataAdapter sda3 = new SqlDataAdapter(str3, conn);
                DataTable dt3 = new DataTable();
                sda3.Fill(dt3);
                if (dt3.Rows[0]["用户等级"].ToString() != "1")
                {
                    QueryControl();
                }
                conn.Close();
                ClearText();
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox4.Checked = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
                conn.Open();
                if(dt3.Rows[0]["用户等级"].ToString() != "A")
                {
                    string str1 = string.Format("select 车牌号码,车牌颜色,检测次数,检测编号,检测日期,检测时间 from Data_Modification where 检测编号!='' and 检测次数!='' and  (车牌号码 like '%'+'{0}'+'%' or 车牌号码='') and (检测日期 between '{1}' and '{2}' or '{1}'='' or '{2}'='') and 检测时间!='' order by 检测时间 desc", zhphm.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"), dateTimePicker2.Value.ToString("yyyy-MM-dd"));
                    SqlDataAdapter sda1 = new SqlDataAdapter(str1, conn);
                    DataTable dt1 = new DataTable();
                    sda1.Fill(dt1);
                    dataGridView1.DataSource = dt1;
                }
                else
                {
                    string str = string.Format("select 车牌号码,车牌颜色,检测次数,检测编号,检测日期,检测时间 from Data_Modification where 检测编号!='' and 检测次数!='' and  (车牌号码 like '%'+'{0}'+'%' or 车牌号码='') and (检测日期 between '{1}' and '{2}' or '{1}'='' or '{2}'='') and 检测时间!='' order by 检测时间 desc", zhphm.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"), dateTimePicker2.Value.ToString("yyyy-MM-dd"));
                    SqlDataAdapter sda = new SqlDataAdapter(str, conn);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                conn.Close();
                dataGridView1.Columns["检测编号"].Width = 120;
                dataGridView1.ClearSelection();
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                ClearControl();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //加载
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                ClearText();
                tabControl1.SelectedIndex = 1;
                if (Choices == "1")
                {
                    checkBox3.Checked = true;
                }
                else
                {
                    checkBox3.Checked = false;
                }
                conn.Open();
                string str = string.Format("select 用户等级 from DM_hUsers_操作权限 where 用户名='{0}'", uname);
                SqlDataAdapter sda = new SqlDataAdapter(str, conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0]["用户等级"].ToString() != "1")
                {
                    QueryControl();
                }
                //if (dt.Rows[0]["用户等级"].ToString() == "A")
                //{
                //    textbox3.Visible = true;
                //    string strs = Interaction.InputBox("提示信息", "标题", "文本内容", -1, -1);
                //    if (strs != "admin123")
                //    {
                //        MessageBox.Show("请输入验证码!");
                //        this.Close();
                //    }
                //    else
                //    {
                //        QueryControls();
                //        textbox3.Visible = false;
                //    }
                //}
                string str1 = string.Format("SELECT *  FROM [dbo].[车辆类型]");
                SqlDataAdapter sda1 = new SqlDataAdapter(str1, conn);
                DataTable dt1 = new DataTable();
                sda1.Fill(dt1);
                cllx.DataSource = dt1;
                cllx.DisplayMember = "车辆类型";
                cllx.ValueMember = "ID";
                conn.Close();
                CLDEPD();
                cllx.Text = "";
                checkBox1.Checked = false;
                //string strFilePath = Application.StartupPath + @"\Appli.ini";
                //PubOp.OperateIniFile.WriteIniData("数据库连接", "server", "192.168.100.100", strFilePath);
                //PubOp.OperateIniFile.WriteIniData("", "database", "userinfo", strFilePath);
                tczdw.Items.Insert(0, "其他");
                tsjdw.Items.Insert(0, "其他");
                tcllx.Items.Insert(0, "其他");
                string str3 = string.Format("select distinct 车主单位 from dbo.DM_vRegister_固定信息 where 车主单位!=''");
                string str4 = string.Format("select distinct 送检单位 from dbo.DM_vRegister_固定信息 where 送检单位!=''");
                string str5 = string.Format("select distinct 车辆类型 from dbo.DM_vRegister_固定信息 where 车辆类型!=''");
                conn.Open();
                SqlDataAdapter sda3 = new SqlDataAdapter(str3, conn);
                SqlDataAdapter sda4 = new SqlDataAdapter(str4, conn);
                SqlDataAdapter sda5 = new SqlDataAdapter(str5, conn);
                DataTable dt3 = new DataTable();
                DataTable dt4 = new DataTable();
                DataTable dt5 = new DataTable();
                sda3.Fill(dt3);
                sda4.Fill(dt4);
                sda5.Fill(dt5);
                conn.Close();
                tczdw.DataSource = dt3;
                tsjdw.DataSource = dt4;
                tcllx.DataSource = dt5;
                tsjdw.DisplayMember = "送检单位";
                tsjdw.ValueMember = "送检单位";
                tsjdw.SelectedIndex = -1;
                tczdw.DisplayMember = "车主单位";
                tczdw.ValueMember = "车主单位";
                tczdw.SelectedIndex = -1;
                tcllx.DisplayMember = "车辆类型";
                tcllx.ValueMember = "车辆类型";
                tcllx.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public static object TextsIsnull(string str)
        {
            if (str == ""||str=="-")
            {
                str = "0";
            }
            return str;
        }
        //判定的改变
        public void Exitpd()
        {
            try
            {
                #region 整车判断
                string szd = "";
                string szczd = "";
                if (dczd.Visible == false)
                {
                    if (dczdl.Text != "")
                    {
                        szd = "○";
                    }
                    else
                    {
                        szd = " ";
                    }
                }
                else
                {
                    szd = "×";
                }
                if (zczdbd.Visible == false)
                {
                    if (dczczdl.Text != "")
                    {
                        szczd = "○";
                    }
                    else
                    {
                        szczd = " ";
                    }
                }
                else
                {
                    szczd = "×";
                }
                dcpd.Text = szd + szczd;
                #endregion
                #region 动力性判断
                if (wdcspd.Visible == true)
                {
                    dlxpd.Text = "×";
                }
                if (wdcspd.Visible == false)
                {
                    if (wdcs.Text == "")
                    {
                        dlxpd.Text = "";
                    }
                    else
                    {
                        dlxpd.Text = "○";
                    }
                }
                #endregion
                #region 悬架
                //悬架前轴
                string qz = "";
                string qzc = "";
                if (lqz.Visible == false && lqy.Visible == false)
                {
                    if (qzzxsl.Text != "" && qzyxsl.Text != "")
                    {
                        qz = "○";
                    }
                    else
                    {
                        qz = " ";
                    }
                }
                else
                {
                    qz = "×";
                }
                if (lq.Visible == false)
                {
                    if (qzzyc.Text != "")
                    {
                        qzc = "○";
                    }
                    else
                    {
                        qzc = " ";
                    }
                }
                else
                {
                    qzc = "×";
                }
                xjqzpd.Text = qz + qzc;
                //悬架后轴
                string hz = "";
                string hzc = "";
                if (lhz.Visible == false && lhy.Visible == false)
                {
                    if (hzzxsl.Text != "" && hzyxsl.Text != "")
                    {
                        hz = "○";
                    }
                    else
                    {
                        hz = " ";
                    }
                }
                else
                {
                    hz = "×";
                }
                if (lh.Visible == false)
                {
                    if (hzzyc.Text != "")
                    {
                        hzc = "○";
                    }
                    else
                    {
                        hzc = " ";
                    }
                }
                else
                {
                    hzc = "×";
                }
                xjhzpd.Text = hz + hzc;
                #endregion
                #region 前照灯
                //左外
                string zwdgq = "";
                string zwdyc = "";
                string zwdjc = "";
                if (zwgq.Visible == false)
                {
                    if (zwyggq.Text != "")
                    {
                        zwdgq = "○";
                    }
                    else
                    {
                        zwdgq = " ";
                    }
                }
                else
                {
                    zwdgq = "×";
                }
                if (zwyc.Visible == false)
                {
                    if (zwygczH.Text != "")
                    {
                        zwdyc = "○";
                    }
                    else
                    {
                        zwdyc = " ";
                    }
                }
                else
                {
                    zwdyc = "×";
                }
                if (zwjc.Visible == false)
                {
                    if (zwjgczH.Text != "")
                    {
                        zwdjc = "○";
                    }
                    else
                    {
                        zwdjc = " ";
                    }
                }
                else
                {
                    zwdjc = "×";
                }
                zwpd.Text = zwdgq + zwdyc + zwdjc;
                //左内
                string zndgq = "";
                string zndyc = "";
                string zndjc = "";
                if (zngq.Visible == false)
                {
                    if (znyggq.Text != "")
                    {
                        zndgq = "○";
                    }
                    else
                    {
                        zndgq = " ";
                    }
                }
                else
                {
                    zndgq = "×";
                }
                if (znyc.Visible == false)
                {
                    if (znygczH.Text != "")
                    {
                        zndyc = "○";
                    }
                    else
                    {
                        zndyc = " ";
                    }
                }
                else
                {
                    zndyc = "×";
                }
                if (znjc.Visible == false)
                {
                    if (znjgczH.Text != "")
                    {
                        zndjc = "○";
                    }
                    else
                    {
                        zndjc = " ";
                    }
                }
                else
                {
                    zndjc = "×";
                }
                znpd.Text = zndgq + zndyc + zndjc;
                //右内
                string yndgq = "";
                string yndyc = "";
                string yndjc = "";
                if (yngq.Visible == false)
                {
                    if (ynyggq.Text != "")
                    {
                        yndgq = "○";
                    }
                    else
                    {
                        yndgq = " ";
                    }
                }
                else
                {
                    yndgq = "×";
                }
                if (ynyc.Visible == false)
                {
                    if (ynygczH.Text != "")
                    {
                        yndyc = "○";
                    }
                    else
                    {
                        yndyc = " ";
                    }
                }
                else
                {
                    yndyc = "×";
                }
                if (ynjc.Visible == false)
                {
                    if (ynjgczH.Text != "")
                    {
                        yndjc = "○";
                    }
                    else
                    {
                        yndjc = " ";
                    }
                }
                else
                {
                    yndjc = "×";
                }
                ynpd.Text = yndgq + yndyc + yndjc;
                //右外
                string ywdgq = "";
                string ywdyc = "";
                string ywdjc = "";
                if (ywgq.Visible == false)
                {
                    if (ywyggq.Text != "")
                    {
                        ywdgq = "○";
                    }
                    else
                    {
                        ywdgq = " ";
                    }
                }
                else
                {
                    ywdgq = "×";
                }
                if (ywyc.Visible == false)
                {
                    if (ywygczH.Text != "")
                    {
                        ywdyc = "○";
                    }
                    else
                    {
                        ywdyc = " ";
                    }
                }
                else
                {
                    ywdyc = "×";
                }
                if (ywjc.Visible == false)
                {
                    if (ywjgczH.Text != "")
                    {
                        ywdjc = "○";
                    }
                    else
                    {
                        ywdjc = " ";
                    }
                }
                else
                {
                    ywdjc = "×";
                }
                ywpd.Text = ywdgq + ywdyc + ywdjc;
                #endregion
                #region 单轴评定
                //一轴评定
                string yzzd = "";
                string yzbph = "";
                string yzztzb = "";
                string yzytzb = "";
                if (ydzpd.Visible == false)
                {
                    if (ydzzdv.Text != "")
                    {
                        yzzd = "○";
                    }
                    else
                    {
                        yzzd = " ";
                    }
                }
                else
                {
                    yzzd = "×";
                }
                if (ydzbpd.Visible == false)
                {
                    if (ydzbphv.Text != "")
                    {
                        yzbph = "○";
                    }
                    else
                    {
                        yzbph = " ";
                    }
                }
                else
                {
                    yzbph = "×";
                }
                if (yzzzv.Visible == false)
                {
                    if (ydzzzzv.Text != "")
                    {
                        yzztzb = "○";
                    }
                    else
                    {
                        yzztzb = " ";
                    }
                }
                else
                {
                    yzztzb = "×";
                }
                if (yyzzv.Visible == false)
                {
                    if (ydzyzzv.Text != "")
                    {
                        yzytzb = "○";
                    }
                    else
                    {
                        yzytzb = " ";
                    }
                }
                else
                {
                    yzytzb = "×";
                }
                if (lb1.Text == "一")
                {
                    lz1.Text = "①";
                }
                if (lb1.Text == "二")
                {
                    lz1.Text = "②";

                }
                yzpd.Text = yzzd + yzbph + yzztzb + yzytzb;
                //if (yzpd.Text.Replace(" ", "") != "")
                //{
                //    if (yzpd.Text.Contains("×"))
                //    {
                //        yzpd.Text = "×";
                //    }
                //    else
                //    {
                //        if (lb1.Text == "一")
                //        {
                //            yzpd.Text = "一级";
                //        }
                //        else
                //        {
                //            yzpd.Text = "×";
                //        }
                //    }
                //}
                //二轴评定
                string ezzd = "";
                string ezbph = "";
                string ezztzb = "";
                string ezytzb = "";
                if (edzpd.Visible == false)
                {
                    if (edzzdv.Text != "")
                    {
                        ezzd = "○";
                    }
                    else
                    {
                        ezzd = " ";
                    }
                }
                else
                {
                    ezzd = "×";
                }
                if (edzbpd.Visible == false)
                {
                    if (edzbphv.Text != "")
                    {
                        ezbph = "○";
                    }
                    else
                    {
                        ezbph = " ";
                    }
                }
                else
                {
                    ezbph = "×";
                }
                if (ezzzv.Visible == false)
                {
                    if (edzzzzv.Text != "")
                    {
                        ezztzb = "○";
                    }
                    else
                    {
                        ezztzb = " ";
                    }
                }
                else
                {
                    ezztzb = "×";
                }
                if (eyzzv.Visible == false)
                {
                    if (edzyzzv.Text != "")
                    {
                        ezytzb = "○";
                    }
                    else
                    {
                        ezytzb = " ";
                    }
                }
                else
                {
                    ezytzb = "×";
                }
                if (lb2.Text == "一")
                {
                    lz2.Text = "①";
                }
                if (lb2.Text == "二")
                {
                    lz2.Text = "②";
                }
                ezpd.Text = ezzd + ezbph + ezztzb + ezytzb;
                //if (ezpd.Text.Replace(" ", "") != "")
                //{
                //    if (ezpd.Text.Contains("×"))
                //    {
                //        ezpd.Text = "×";
                //    }
                //    else
                //    {
                //        if (lb2.Text == "一")
                //        {
                //            ezpd.Text = "一级";
                //        }
                //        else
                //        {
                //            ezpd.Text = "×";
                //        }
                //    }
                //}
                //三轴评定
                string szzd = "";
                string szbph = "";
                string szztzb = "";
                string szytzb = "";
                if (sdzpd.Visible == false)
                {
                    if (sdzzdv.Text != "")
                    {
                        szzd = "○";
                    }
                    else
                    {
                        szzd = " ";
                    }
                }
                else
                {
                    szzd = "×";
                }
                if (sdzbpd.Visible == false)
                {
                    if (sdzbphv.Text != "")
                    {
                        szbph = "○";
                    }
                    else
                    {
                        szbph = " ";
                    }
                }
                else
                {
                    szbph = "×";
                }
                if (szzzv.Visible == false)
                {
                    if (sdzzzzv.Text != "")
                    {
                        szztzb = "○";
                    }
                    else
                    {
                        szztzb = " ";
                    }
                }
                else
                {
                    szztzb = "×";
                }
                if (syzzv.Visible == false)
                {
                    if (sdzyzzv.Text != "")
                    {
                        szytzb = "○";
                    }
                    else
                    {
                        szytzb = " ";
                    }
                }
                else
                {
                    szytzb = "×";
                }
                if (lb3.Text == "一")
                {
                    lz3.Text = "①";
                }
                if (lb3.Text == "二")
                {
                    lz3.Text = "②";
                }
                szpd.Text = szzd + szbph + szztzb + szytzb;
                //if (szpd.Text.Replace(" ", "") != "")
                //{
                //    if (szpd.Text.Contains("×"))
                //    {
                //        szpd.Text = "×";
                //    }
                //    else
                //    {
                //        if (lb3.Text == "一")
                //        {
                //            szpd.Text = "一级";
                //        }
                //        else
                //        {
                //            szpd.Text = "×";
                //        }
                //    }
                //}
                //四轴评定
                string sizzd = "";
                string sizbph = "";
                string sizztzb = "";
                string sizytzb = "";
                if (sidzpd.Visible == false)
                {
                    if (sidzzdv.Text != "")
                    {
                        sizzd = "○";
                    }
                    else
                    {
                        sizzd = " ";
                    }
                }
                else
                {
                    sizzd = "×";
                }
                if (sidzbpd.Visible == false)
                {
                    if (sidzbphv.Text != "")
                    {
                        sizbph = "○";
                    }
                    else
                    {
                        sizbph = " ";
                    }
                }
                else
                {
                    sizbph = "×";
                }
                if (sizzzv.Visible == false)
                {
                    if (sidzzzzv.Text != "")
                    {
                        sizztzb = "○";
                    }
                    else
                    {
                        sizztzb = " ";
                    }
                }
                else
                {
                    sizztzb = "×";
                }
                if (siyzzv.Visible == false)
                {
                    if (sidzyzzv.Text != "")
                    {
                        sizytzb = "○";
                    }
                    else
                    {
                        sizytzb = " ";
                    }
                }
                else
                {
                    sizytzb = "×";
                }
                if (lb4.Text == "一")
                {
                    lz4.Text = "①";
                }
                if (lb4.Text == "二")
                {
                    lz4.Text = "②";
                }
                sizpd.Text = sizzd + sizbph + sizztzb + sizytzb;
                //if (sizpd.Text.Replace(" ", "") != "")
                //{
                //    if (sizpd.Text.Contains("×"))
                //    {
                //        sizpd.Text = "×";
                //    }
                //    else
                //    {
                //        if (lb4.Text == "一")
                //        {
                //            sizpd.Text = "一级";
                //        }
                //        else
                //        {
                //            sizpd.Text = "×";
                //        }
                //    }
                //}
                //五轴评定
                string wzzd = "";
                string wzbph = "";
                string wzztzb = "";
                string wzytzb = "";
                if (wdzpd.Visible == false)
                {
                    if (wdzzdv.Text != "")
                    {
                        wzzd = "○";
                    }
                    else
                    {
                        wzzd = " ";
                    }
                }
                else
                {
                    wzzd = "×";
                }
                if (wdzbpd.Visible == false)
                {
                    if (wdzbphv.Text != "")
                    {
                        wzbph = "○";
                    }
                    else
                    {
                        wzbph = " ";
                    }
                }
                else
                {
                    wzbph = "×";
                }
                if (wzzzv.Visible == false)
                {
                    if (wdzzzzv.Text != "")
                    {
                        wzztzb = "○";
                    }
                    else
                    {
                        wzztzb = " ";
                    }
                }
                else
                {
                    wzztzb = "×";
                }
                if (wyzzv.Visible == false)
                {
                    if (wdzyzzv.Text != "")
                    {
                        wzytzb = "○";
                    }
                    else
                    {
                        wzytzb = " ";
                    }
                }
                else
                {
                    wzytzb = "×";
                }
                if (lb5.Text == "一")
                {
                    lz5.Text = "①";
                }
                if (lb5.Text == "二")
                {
                    lz5.Text = "②";
                }
                wzpd.Text = wzzd + wzbph + wzztzb + wzytzb;
                //if (wzpd.Text.Replace(" ", "") != "")
                //{
                //    if (wzpd.Text.Contains("×"))
                //    {
                //        wzpd.Text = "×";
                //    }
                //    else
                //    {
                //        if (lb5.Text == "一")
                //        {
                //            wzpd.Text = "一级";
                //        }
                //        else
                //        {
                //            wzpd.Text = "×";
                //        }
                //    }
                //}
                //六轴评定
                string lzzd = "";
                string lzbph = "";
                string lzztzb = "";
                string lzytzb = "";
                if (ldzpd.Visible == false)
                {
                    if (ldzzdv.Text != "")
                    {
                        lzzd = "○";
                    }
                    else
                    {
                        lzzd = " ";
                    }
                }
                else
                {
                    lzzd = "×";
                }
                if (ldzbpd.Visible == false)
                {
                    if (ldzbphv.Text != "")
                    {
                        lzbph = "○";
                    }
                    else
                    {
                        lzbph = " ";
                    }
                }
                else
                {
                    lzbph = "×";
                }
                if (lzzzv.Visible == false)
                {
                    if (ldzzzzv.Text != "")
                    {
                        lzztzb = "○";
                    }
                    else
                    {
                        lzztzb = " ";
                    }
                }
                else
                {
                    lzztzb = "×";
                }
                if (lyzzv.Visible == false)
                {
                    if (ldzyzzv.Text != "")
                    {
                        lzytzb = "○";
                    }
                    else
                    {
                        lzytzb = " ";
                    }
                }
                else
                {
                    lzytzb = "×";
                }
                if (lb6.Text == "一")
                {
                    lz6.Text = "①";
                }
                if (lb6.Text == "二")
                {
                    lz6.Text = "②";
                }
                lzpd.Text = lzzd + lzbph + lzztzb + lzytzb;
                //if (lzpd.Text.Replace(" ", "") != "")
                //{
                //    if (lzpd.Text.Contains("×"))
                //    {
                //        lzpd.Text = "×";
                //    }
                //    else
                //    {
                //        if (lb6.Text == "一")
                //        {
                //            lzpd.Text = "一级";
                //        }
                //        else
                //        {
                //            lzpd.Text = "×";
                //        }
                //    }
                //}
                #endregion
                #region 排放性
                string qygcopj = "";
                string qyghcpj = "";
                string qydcopj = "";
                string qydhcpj = "";
                string qyλpj = "";
                if (qgco.Visible == false)
                {
                    if (qygdsCO.Text != "")
                    {
                        qygcopj = "○";
                    }
                    else
                    {
                        qygcopj = " ";
                    }
                }
                else
                {
                    qygcopj = "×";
                }
                if (qghc.Visible == false)
                {
                    if (qygdsHC.Text != "")
                    {
                        qyghcpj = "○";
                    }
                    else
                    {
                        qyghcpj = " ";
                    }
                }
                else
                {
                    qyghcpj = "×";
                }
                if (qgλ.Visible == false)
                {
                    if (qygdsλ.Text != "")
                    {
                        qyλpj = "○";
                    }
                    else
                    {
                        qyλpj = " ";
                    }
                }
                else
                {
                    qyλpj = "×";
                }
                if (qdco.Visible == false)
                {
                    if (qyddsCO.Text != "")
                    {
                        qydcopj = "○";
                    }
                    else
                    {
                        qydcopj = " ";
                    }
                }
                else
                {
                    qydcopj = "×";
                }
                if (qdhc.Visible == false)
                {
                    if (qyddsHC.Text != "")
                    {
                        qydhcpj = "○";
                    }
                    else
                    {
                        qydhcpj = " ";
                    }
                }
                else
                {
                    qydhcpj = "×";
                }
                qypd.Text = qygcopj + qyghcpj + qyλpj + qydcopj + qydhcpj;
                if (cg1.Visible == true || cg2.Visible == true || cg3.Visible == true || lbgvpd.Visible == true)
                {
                    cypd.Text = "×";
                }
                if (cg1.Visible == false && cg2.Visible == false && cg3.Visible == false && lbgvpd.Visible == false)
                {
                    if (cygxs1.Text == "" && cygxs2.Text == "" && cygxs3.Text == "" && cygxsavg.Text == "" && cyyd1.Text == "" && cyyd2.Text == "" && cyyd3.Text == "" && cyydavg.Text == "" && cygxs100.Text == "" && cygxs90.Text == "" && cygxs80.Text == "" && cylbgv.Text == "")
                    {
                        cypd.Text = "";
                    }
                    else
                    {
                        cypd.Text = "○";
                    }
                }
                #endregion
                #region 车速喇叭与侧滑
                //车速
                if (scsz.Visible == true)
                {
                    cspd.Text = "×";
                }
                if (scsz.Visible == false)
                {
                    if (csb.Text == "")
                    {
                        cspd.Text = "";
                    }
                    else
                    {
                        cspd.Text = "○";
                    }
                }
                //第一侧滑量
                if (ch1pd.Visible == true)
                {
                    chpd.Text = "×";
                }
                if (ch1pd.Visible == false)
                {
                    if (dychl.Text == "")
                    {
                        chpd.Text = "";
                    }
                    else
                    {
                        chpd.Text = "○";
                    }
                }
                //第二侧滑量
                if (ch2pd.Visible == true)
                {
                    chpd2.Text = "×";
                }
                if (ch2pd.Visible == false)
                {
                    if (dechl.Text == "")
                    {
                        chpd2.Text = "";
                    }
                    else
                    {
                        chpd2.Text = "○";
                    }
                }
                //喇叭
                if (slbs.Visible == true)
                {
                    lbpd.Text = "×";
                }
                if (slbs.Visible == false)
                {
                    if (lbsjz.Text == "")
                    {
                        lbpd.Text = "";
                    }
                    else
                    {
                        lbpd.Text = "○";
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //数据绑定
        public void Bind_vRegister(DataTable dt)
        {
            int nerror = 0;
            try
            {
                if (dt.Columns.Contains("底检值"))
                {
                    if (dt.Rows[0]["底检值"].ToString().Replace(" ", "") != "")
                    {
                        djjc.Text = dt.Rows[0]["底检值"].ToString().Substring(0,dt.Rows[0]["底检值"].ToString().Replace(" ", "").Length-1);
                    }
                    else
                    {
                        djjc.Text = dt.Rows[0]["底检值"].ToString();
                    }
                }
                else
                {
                    djjc.Text = "";
                }
                if(dt.Columns.Contains("外观值"))
                {
                    wgjc.Text = dt.Rows[0]["外观值"].ToString();
                }
                else
                {
                    wgjc.Text = "";
                }
                if (dt.Columns.Contains("是否通过"))
                {
                    sftg.Text = dt.Rows[0]["是否通过"].ToString();
                }
                else
                {
                    sftg.Text = "";
                }
                if(dt.Columns.Contains("最大设计车速"))
                {
                    maxsd.Text = dt.Rows[0]["最大设计车速"].ToString();
                }
                else
                {
                    maxsd.Text = "";
                }
                if (dt.Columns.Contains("方向盘自由转动量值"))
                {
                    tb.Text = dt.Rows[0]["方向盘自由转动量值"].ToString();
                }
                else
                {
                    tb.Text = "";
                }
                #region 灯光垂直偏
                if (dt.Columns.Contains("左主远光上下偏差值"))
                {
                    lbzwyc.Text = dt.Rows[0]["左主远光上下偏差值"].ToString();
                }
                else
                {
                    lbzwyc.Text = "";
                }
                if (dt.Columns.Contains("左副远光上下偏差值"))
                {
                    lbznyc.Text = dt.Rows[0]["左副远光上下偏差值"].ToString();
                }
                else
                {
                    lbznyc.Text = "";
                }
                if (dt.Columns.Contains("右副远光上下偏差值"))
                {
                    lbynyc.Text = dt.Rows[0]["右副远光上下偏差值"].ToString();
                }
                else
                {
                    lbynyc.Text = "";
                }
                if (dt.Columns.Contains("右主远光上下偏差值"))
                {
                    lbywyc.Text = dt.Rows[0]["右主远光上下偏差值"].ToString();
                }
                else
                {
                    lbywyc.Text = "";
                }
                if (dt.Columns.Contains("左近光上下偏差值"))
                {
                    lbzjc.Text = dt.Rows[0]["左近光上下偏差值"].ToString();
                }
                else
                {
                    lbzjc.Text = "";
                }
                if (dt.Columns.Contains("右近光上下偏差值"))
                {
                    lbyjc.Text = dt.Rows[0]["右近光上下偏差值"].ToString();
                }
                else
                {
                    lbyjc.Text = "";
                }
                #endregion
                #region 
                fid.Visible = true;
                fid.Text = dt.Rows[0]["FID"].ToString().Replace(" ","");
                if (dt.Rows[0]["座位数"].ToString().Contains("+"))
                {
                    nerror = 1;
                    kczws.Text = (Convert.ToDouble(dt.Rows[0]["座位数"].ToString().Replace(" ", "").Substring(0, 1)) + Convert.ToDouble(dt.Rows[0]["座位数"].ToString().Replace(" ", "").Substring(2, 1))).ToString();
                }
                else
                {
                    kczws.Text = dt.Rows[0]["座位数"].ToString().Replace(" ", "");
                }
                string sqdzs = dt.Rows[0]["底盘类型"].ToString().Replace(" ", "").Substring(2, 1);
                
                double dqdzs;
                if (sqdzs != "")
                {
                    dqdzs = Convert.ToDouble(sqdzs) / 2;
                    qdzs.Text = dqdzs.ToString().Replace(" ", "");//驱动轴数
                }
                if (Convert.ToDouble(sqdzs) / 2 == 1)
                {
                    zcz.Text = dt.Rows[0]["手刹起始轴位"].ToString().Replace(" ", "");//驻车轴
                }
                if (Convert.ToDouble(sqdzs) / 2 == 2)
                {
                    zcz.Text = dt.Rows[0]["手刹起始轴位"].ToString().Replace(" ", "") + "," + (Convert.ToDouble(dt.Rows[0]["手刹起始轴位"].ToString().Replace(" ", "")) + 1).ToString();//驻车轴
                }
                if (dt.Rows[0]["新车"].ToString().Replace(" ","") == "1")
                {
                    ywlx.Text = "新车";
                }
                else
                {
                    ywlx.Text = "在用";
                }
                if (dt.Rows[0]["车辆类型"].ToString().Replace(" ", "").Contains("H") || dt.Rows[0]["车辆类型"].ToString().Replace(" ", "").Contains("Q") || dt.Rows[0]["车辆类型"].ToString().Replace(" ", "").Contains("货") || dt.Rows[0]["车辆类型"].ToString().Replace(" ", "").Contains("挂"))
                {
                    if (dt.Rows[0]["车辆类型"].ToString().Contains("自卸"))
                    {
                        hccsxs.Text = "自卸车";//货车车身形式
                    }
                    else if (dt.Rows[0]["车辆类型"].ToString().Contains("牵引") || dt.Rows[0]["车辆类型"].ToString().Contains("挂"))
                    {
                        hccsxs.Text = "牵引车";
                    }
                    else if (dt.Rows[0]["车辆类型"].ToString().Contains("仓栅"))
                    {
                        hccsxs.Text = "仓栅车";
                    }
                    else if (dt.Rows[0]["车辆类型"].ToString().Contains("厢式"))
                    {
                        hccsxs.Text = "厢式车";
                    }
                    else if (dt.Rows[0]["车辆类型"].ToString().Contains("罐"))
                    {
                        hccsxs.Text = "罐车";
                    }
                    else
                    {
                        hccsxs.Text = "栏板车";
                    }
                }
                #endregion
                #region 车辆基本信息
                if(dt.Columns.Contains("并装轴车"))
                {
                    bzzxs.Text = dt.Rows[0]["并装轴车"].ToString();
                }
                else
                {
                    bzzxs.Text = "";
                }
                if (dt.Columns.Contains("档案号"))
                {
                    dabh.Text = dt.Rows[0]["档案号"].ToString().Replace(" ", "");
                }
                else
                {
                    dabh.Text = "";
                }
                if (dt.Columns.Contains("独立悬架"))
                {
                    if (dt.Rows[0]["独立悬架"].ToString().Replace(" ", "") == "1")
                    {
                        zxzxjxs.Text = "独立";//转向轴悬架形式
                    }
                    else
                    {
                        zxzxjxs.Text = "非独立";
                    }
                }
                else
                {
                    zxzxjxs.Text = "";
                }
                hphm.Text = dt.Rows[0]["车牌号码"].ToString().Replace(" ","");
                hpzl.Text = dt.Rows[0]["车牌颜色"].ToString().Replace(" ", "");
                if(dt.Rows[0]["登记日期"].ToString().Replace(" ", "")!=""&& dt.Rows[0]["登记日期"].ToString().Replace(" ", "")!="-")
                {
                    djrq.Text = dt.Rows[0]["登记日期"].ToString().Replace(" ", "");
                }
                else
                {
                    djrq.Text = "";
                }
                if (dt.Rows[0]["整备质量"].ToString().Replace(" ", "").Contains("-"))
                {
                    zbzl.Text = "";
                }
                else
                {
                    zbzl.Text = dt.Rows[0]["整备质量"].ToString().Replace(" ", "");
                }
                clsbdm.Text = dt.Rows[0]["底盘号码"].ToString().Replace(" ", "");
                xslc.Text = dt.Rows[0]["里程表读数"].ToString().Replace(" ", "");
                ryxs.Text = dt.Rows[0]["燃油类型"].ToString().Replace(" ", "");
                fdjhm.Text = dt.Rows[0]["发动机号码"].ToString().Replace(" ", "");
                cllx.Text = dt.Rows[0]["车辆类型"].ToString().Replace(" ", "");
                dczs.Text = dt.Rows[0]["车轴数"].ToString().Replace(" ", "");
                csys.Text = dt.Rows[0]["车身颜色"].ToString().Replace(" ", "");
                qdxs.Text = dt.Rows[0]["底盘类型"].ToString().Replace(" ", "");//驱动形式
                clxh.Text = dt.Rows[0]["型号"].ToString().Replace(" ", "");
                yrsfdjedg.Text = dt.Rows[0]["发动机额定功率"].ToString().Replace(" ", "");
                dly.Text = dt.Rows[0]["登录员"].ToString().Replace(" ", "");
                if (dt.Columns.Contains("检测项目"))
                {
                    jyxm.Text = dt.Rows[0]["检测项目"].ToString().Replace(" ", "");
                }
                else
                {
                    jyxm.Text = "";
                }
                jylb.Text = dt.Rows[0]["检测类别"].ToString().Replace(" ", "");
                ycy.Text = dt.Rows[0]["引车员"].ToString().Replace(" ", "");
                jyrq.Text = dt.Rows[0]["检测日期"].ToString().Replace(" ", "");
                ppxh.Text = dt.Rows[0]["厂牌型号"].ToString().Replace(" ", "");
                cg.Text = dt.Rows[0]["车高"].ToString().Replace(" ", "");
                kccc.Text = dt.Rows[0]["车长"].ToString().Replace(" ", "");
                ck.Text = dt.Rows[0]["车宽"].ToString().Replace(" ", "");
                if (dt.Columns.Contains("灯制"))
                {
                    nerror = 2;
                    qzdz.Text = dt.Rows[0]["灯制"].ToString().Replace(" ", "");
                }
                else
                {
                    if (dt.Rows[0]["四灯"].ToString().Replace(" ", "") == "1")
                    {
                        qzdz.Text = "四灯";
                    }
                    else
                    {
                        qzdz.Text = "两灯";
                    }
                }
                vin.Text = dt.Rows[0]["底盘号码"].ToString().Replace(" ","");
                if (dt.Rows[0]["总质量"].ToString().Replace(" ", "").Contains("-"))
                {
                    nerror = 3;
                    zzl.Text = "";
                }
                else
                {
                    zzl.Text = dt.Rows[0]["总质量"].ToString().Replace(" ", "");
                }
                drsednjzs.Text =dt.Rows[0]["发动机额定转速"].ToString().Replace(" ", "");
                if (dt.Rows[0]["出厂日期"].ToString().Replace(" ","") != "" && dt.Rows[0]["出厂日期"].ToString().Replace(" ","") != "-")
                {
                    nerror = 4;
                    DateTime a = Convert.ToDateTime(dt.Rows[0]["出厂日期"].ToString());
                    ccrq.Text = a.ToString("yyyy-MM-dd").Replace(" ", "");
                }
                else
                {
                    ccrq.Text = "";
                }
                lsh.Text = dt.Rows[0]["检测编号"].ToString().Replace(" ", "");
                qzdygsnfddtz.Text = dt.Rows[0]["远光光束单独调整"].ToString().Replace(" ", "");
                syr.Text = dt.Rows[0]["车主单位"].ToString().Replace(" ", "");
                if (dt.Columns.Contains("双转向轴"))
                {
                    nerror = 5;
                    if (dt.Rows[0]["双转向轴"].ToString().Replace(" ", "") == "1")
                    {
                        zxzs.Text = "2";
                    }
                    else
                    {
                        zxzs.Text = "1";
                    }
                }
                else
                {
                    zxzs.Text = "1";
                }
                qdlltggxh.Text = dt.Rows[0]["轮胎规格"].ToString().Replace(" ", "");
                qlj.Text = dt.Rows[0]["前轮距"].ToString().Replace(" ", "");
                kclxdj.Text = dt.Rows[0]["客车等级"].ToString().Replace(" ", "");
                fdjednj.Text = dt.Rows[0]["发动机额定扭矩"].ToString().Replace(" ", "");
                sjdw.Text = dt.Rows[0]["送检单位"].ToString().Replace(" ", "");
                bjbh.Text= dt.Rows[0]["检测编号"].ToString().Replace(" ", "");
                dcwkcc.Text= dt.Rows[0]["车长"].ToString().Replace(" ","")+"*"+ dt.Rows[0]["车宽"].ToString().Replace(" ", "") + "*"+ dt.Rows[0]["车高"].ToString().Replace(" ", "");
                jcxb.Text = dt.Rows[0]["线号标识"].ToString();
                yyzh.Text = dt.Rows[0]["营运证号"].ToString();
                #endregion
                #region 原始数据
                yzzlh.Text = dt.Rows[0]["一轴左轴重值"].ToString();
                yzylh.Text = dt.Rows[0]["一轴右轴重值"].ToString();
                yzzdt.Text = dt.Rows[0]["一轴左轴重动态值"].ToString();
                yzydt.Text = dt.Rows[0]["一轴右轴重动态值"].ToString();
                yzzxczd.Text = dt.Rows[0]["一轴求和时左制动力值"].ToString();
                yzyxczd.Text = dt.Rows[0]["一轴求和时右制动力值"].ToString();
                ezzlh.Text = dt.Rows[0]["二轴左轴重值"].ToString();
                ezylh.Text = dt.Rows[0]["二轴右轴重值"].ToString();
                ezzdt.Text = dt.Rows[0]["二轴左轴重动态值"].ToString();
                ezydt.Text = dt.Rows[0]["二轴右轴重动态值"].ToString();
                ezzxczd.Text = dt.Rows[0]["二轴求和时左制动力值"].ToString();
                ezyxczd.Text = dt.Rows[0]["二轴求和时右制动力值"].ToString();
                szzlh.Text = dt.Rows[0]["三轴左轴重值"].ToString();
                szylh.Text = dt.Rows[0]["三轴右轴重值"].ToString();
                szzdt.Text = dt.Rows[0]["三轴左轴重动态值"].ToString();
                szydt.Text = dt.Rows[0]["三轴右轴重动态值"].ToString();
                szzxczd.Text = dt.Rows[0]["三轴求和时左制动力值"].ToString();
                szyxczd.Text = dt.Rows[0]["三轴求和时右制动力值"].ToString();
                sizzlh.Text = dt.Rows[0]["四轴左轴重值"].ToString();
                sizylh.Text = dt.Rows[0]["四轴右轴重值"].ToString();
                sizzdt.Text = dt.Rows[0]["四轴左轴重动态值"].ToString();
                sizydt.Text = dt.Rows[0]["四轴右轴重动态值"].ToString();
                sizzxczd.Text = dt.Rows[0]["四轴求和时左制动力值"].ToString();
                sizyxczd.Text = dt.Rows[0]["四轴求和时右制动力值"].ToString();
                wzzlh.Text = dt.Rows[0]["五轴左轴重值"].ToString();
                wzylh.Text = dt.Rows[0]["五轴右轴重值"].ToString();
                wzzxczd.Text = dt.Rows[0]["五轴求和时左制动力值"].ToString();
                wzyxczd.Text = dt.Rows[0]["五轴求和时右制动力值"].ToString();
                lzzxczd.Text = dt.Rows[0]["六轴求和时左制动力值"].ToString();
                lzyxczd.Text = dt.Rows[0]["六轴求和时右制动力值"].ToString();
                if (Convert.ToDouble(dczs.Text.Replace(" ", "")) >= 3)
                {
                    if (dt.Columns.Contains("一轴复合轴重值"))
                    {
                        if (dt.Rows[0]["一轴复合轴重值"].ToString().Replace(" ", "").ToString() != "")
                        {
                            yzzz.Text = dt.Rows[0]["一轴复合轴重值"].ToString();
                        }
                        else
                        {
                            yzzz.Text = dt.Rows[0]["一轴轴重值"].ToString();
                        }
                    }
                    else
                    {
                        yzzz.Text = dt.Rows[0]["一轴轴重值"].ToString();
                    }
                    if (dt.Columns.Contains("二轴复合轴重值"))
                    {
                        if (dt.Rows[0]["二轴复合轴重值"].ToString().Replace(" ", "").ToString() != "")
                        {
                            ezzz.Text = dt.Rows[0]["二轴复合轴重值"].ToString();
                        }
                        else
                        {
                            ezzz.Text = dt.Rows[0]["二轴轴重值"].ToString();
                        }
                    }
                    else
                    {
                        ezzz.Text = dt.Rows[0]["二轴轴重值"].ToString();
                    }
                    if (dt.Columns.Contains("三轴复合轴重值"))
                    {
                        if (dt.Rows[0]["三轴复合轴重值"].ToString().Replace(" ", "").ToString() != "")
                        {
                            szzz.Text = dt.Rows[0]["三轴复合轴重值"].ToString();
                        }
                        else
                        {
                            szzz.Text = dt.Rows[0]["三轴轴重值"].ToString();
                        }
                    }
                    else
                    {
                        szzz.Text = dt.Rows[0]["三轴轴重值"].ToString();
                    }
                    if (dt.Columns.Contains("四轴复合轴重值"))
                    {
                        if (dt.Rows[0]["四轴复合轴重值"].ToString().Replace(" ", "").ToString() != "")
                        {
                            sizzz.Text = dt.Rows[0]["四轴复合轴重值"].ToString();
                        }
                        else
                        {
                            sizzz.Text = dt.Rows[0]["四轴轴重值"].ToString();
                        }
                    }
                    else
                    {
                        sizzz.Text = dt.Rows[0]["四轴轴重值"].ToString();
                    }
                    if (dt.Columns.Contains("五轴复合轴重值"))
                    {
                        if (dt.Rows[0]["五轴复合轴重值"].ToString().Replace(" ", "").ToString() != "")
                        {
                            wzzz.Text = dt.Rows[0]["五轴复合轴重值"].ToString();
                        }
                        else
                        {
                            wzzz.Text = dt.Rows[0]["五轴轴重值"].ToString();
                        }
                    }
                    else
                    {
                        wzzz.Text = dt.Rows[0]["五轴轴重值"].ToString();
                    }
                    if (dt.Columns.Contains("六轴复合轴重值"))
                    {
                        if (dt.Rows[0]["六轴复合轴重值"].ToString().Replace(" ", "").ToString() != "")
                        {
                            lzzz.Text = dt.Rows[0]["六轴复合轴重值"].ToString();
                        }
                        else
                        {
                            lzzz.Text = dt.Rows[0]["六轴轴重值"].ToString();
                        }
                    }
                    else
                    {
                        lzzz.Text = dt.Rows[0]["六轴轴重值"].ToString();
                    }
                }
                #endregion
                #region 驻车判断
                if (zcz.Text.Contains("1"))
                {
                    if(dt.Columns.Contains("一轴手制动力左值"))
                    {
                        if (dt.Rows[0]["一轴手制动力左值"].ToString().Replace(" ", "").ToString() != "")
                        {
                            yzzzczd.Text = dt.Rows[0]["一轴手制动力左值"].ToString();
                        }
                        else
                        {
                            yzzzczd.Text = dt.Rows[0]["手制动力左值"].ToString();
                        }
                    }
                    else
                    {
                        yzzzczd.Text = dt.Rows[0]["手制动力左值"].ToString();
                    }
                    if (dt.Columns.Contains("一轴手制动力右值"))
                    {
                        if (dt.Rows[0]["一轴手制动力右值"].ToString().Replace(" ", "").ToString() != "")
                        {
                            yzyzczd.Text = dt.Rows[0]["一轴手制动力右值"].ToString();
                        }
                        else
                        {
                            yzyzczd.Text = dt.Rows[0]["手制动力右值"].ToString();
                        }
                    }
                    else
                    {
                        yzyzczd.Text = dt.Rows[0]["手制动力右值"].ToString();
                    }
                }
                if (zcz.Text.Contains("2"))
                {
                    if (dt.Columns.Contains("二轴手制动力左值"))
                    {
                        if (dt.Rows[0]["二轴手制动力左值"].ToString().Replace(" ", "").ToString() != "")
                        {
                            ezzzczd.Text = dt.Rows[0]["二轴手制动力左值"].ToString();
                        }
                        else
                        {
                            ezzzczd.Text = dt.Rows[0]["手制动力左值"].ToString();
                        }
                    }
                    else
                    {
                        ezzzczd.Text = dt.Rows[0]["手制动力左值"].ToString();
                    }
                    if (dt.Columns.Contains("二轴手制动力右值"))
                    {
                        if (dt.Rows[0]["二轴手制动力右值"].ToString().Replace(" ", "").ToString() != "")
                        {
                            ezyzczd.Text = dt.Rows[0]["二轴手制动力右值"].ToString();
                        }
                        else
                        {
                            ezyzczd.Text = dt.Rows[0]["手制动力右值"].ToString();
                        }
                    }
                    else
                    {
                        ezyzczd.Text = dt.Rows[0]["手制动力右值"].ToString();
                    }
                }
                if (zcz.Text.Contains("3"))
                {
                    if (dt.Columns.Contains("三轴手制动力左值"))
                    {
                        if (dt.Rows[0]["三轴手制动力左值"].ToString().Replace(" ", "").ToString() != "")
                        {
                            szzzczd.Text = dt.Rows[0]["三轴手制动力左值"].ToString();
                        }
                        else
                        {
                            szzzczd.Text = dt.Rows[0]["手制动力左值"].ToString();
                        }
                    }
                    else
                    {
                        szzzczd.Text = dt.Rows[0]["手制动力左值"].ToString();
                    }
                    if (dt.Columns.Contains("三轴手制动力右值"))
                    {
                        if (dt.Rows[0]["三轴手制动力右值"].ToString().Replace(" ", "").ToString() != "")
                        {
                            szyzczd.Text = dt.Rows[0]["三轴手制动力右值"].ToString();
                        }
                        else
                        {
                            szyzczd.Text = dt.Rows[0]["手制动力右值"].ToString();
                        }
                    }
                    else
                    {
                        szyzczd.Text = dt.Rows[0]["手制动力右值"].ToString();
                    }
                }
                if (zcz.Text.Contains("4"))
                {
                    if (dt.Columns.Contains("四轴手制动力左值"))
                    {
                        if (dt.Rows[0]["四轴手制动力左值"].ToString().Replace(" ", "").ToString() != "")
                        {
                            sizzzczd.Text = dt.Rows[0]["四轴手制动力左值"].ToString();
                        }
                        else
                        {
                            sizzzczd.Text = dt.Rows[0]["手制动力左值"].ToString();
                        }
                    }
                    else
                    {
                        sizzzczd.Text = dt.Rows[0]["手制动力左值"].ToString();
                    }
                    if (dt.Columns.Contains("四轴手制动力右值"))
                    {
                        if (dt.Rows[0]["四轴手制动力右值"].ToString().Replace(" ", "").ToString() != "")
                        {
                            sizyzczd.Text = dt.Rows[0]["四轴手制动力右值"].ToString();
                        }
                        else
                        {
                            sizyzczd.Text = dt.Rows[0]["手制动力右值"].ToString();
                        }
                    }
                    else
                    {
                        sizyzczd.Text = dt.Rows[0]["手制动力右值"].ToString();
                    }
                }
                if (zcz.Text.Contains("5"))
                {
                    if (dt.Columns.Contains("五轴手制动力左值"))
                    {
                        if (dt.Rows[0]["五轴手制动力左值"].ToString().Replace(" ", "").ToString() != "")
                        {
                            wzzzczd.Text = dt.Rows[0]["五轴手制动力左值"].ToString();
                        }
                        else
                        {
                            wzzzczd.Text = dt.Rows[0]["手制动力左值"].ToString();
                        }
                    }
                    else
                    {
                        wzzzczd.Text = dt.Rows[0]["手制动力左值"].ToString();
                    }
                    if (dt.Columns.Contains("五轴手制动力右值"))
                    {
                        if (dt.Rows[0]["五轴手制动力右值"].ToString().Replace(" ", "").ToString() != "")
                        {
                            wzyzczd.Text = dt.Rows[0]["五轴手制动力右值"].ToString();
                        }
                        else
                        {
                            wzyzczd.Text = dt.Rows[0]["手制动力右值"].ToString();
                        }
                    }
                    else
                    {
                        wzyzczd.Text = dt.Rows[0]["手制动力右值"].ToString();
                    }
                }
                if (zcz.Text.Contains("6"))
                {
                    if (dt.Columns.Contains("六轴手制动力左值"))
                    {
                        if (dt.Rows[0]["六轴手制动力左值"].ToString().Replace(" ", "").ToString() != "")
                        {
                            lzzzczd.Text = dt.Rows[0]["六轴手制动力左值"].ToString();
                        }
                        else
                        {
                            lzzzczd.Text = dt.Rows[0]["手制动力左值"].ToString();
                        }
                    }
                    else
                    {
                        lzzzczd.Text = dt.Rows[0]["手制动力左值"].ToString();
                    }
                    if (dt.Columns.Contains("六轴手制动力右值"))
                    {
                        if (dt.Rows[0]["六轴手制动力右值"].ToString().Replace(" ", "").ToString() != "")
                        {
                            lzyzczd.Text = dt.Rows[0]["六轴手制动力右值"].ToString();
                        }
                        else
                        {
                            lzyzczd.Text = dt.Rows[0]["手制动力右值"].ToString();
                        }
                    }
                    else
                    {
                        lzyzczd.Text = dt.Rows[0]["手制动力右值"].ToString();
                    }
                }
                #endregion
                #region 悬架
                qzzxsl.Text = dt.Rows[0]["悬架前左吸收率值"].ToString();
                qzyxsl.Text = dt.Rows[0]["悬架前右吸收率值"].ToString();
                qzzyc.Text = dt.Rows[0]["悬架前轴吸收率差值"].ToString();
                hzzxsl.Text = dt.Rows[0]["悬架后左吸收率值"].ToString();
                hzyxsl.Text = dt.Rows[0]["悬架后右吸收率值"].ToString();
                hzzyc.Text = dt.Rows[0]["悬架后轴吸收率差值"].ToString();
                string xqpj = "";
                string xqcpj = "";
                string xhpj = "";
                string xhcpj = "";
                if (dt.Rows[0]["悬架前轴吸收率评价"].ToString() == "0")
                {
                    xqpj = "×";
                }
                if (dt.Rows[0]["悬架前轴吸收率评价"].ToString() == "1")
                {
                    xqpj = "○";
                }
                if (dt.Rows[0]["悬架前轴吸收率差评价"].ToString() == "0")
                {
                    xqcpj = "×";
                }
                if (dt.Rows[0]["悬架前轴吸收率差评价"].ToString() == "1")
                {
                    xqcpj = "○";
                }
                xjqzpd.Text = xqpj + xqcpj;
                if (dt.Rows[0]["悬架后轴吸收率评价"].ToString() == "0")
                {
                    xhpj = "×";
                }
                if (dt.Rows[0]["悬架后轴吸收率评价"].ToString() == "1")
                {
                    xhpj = "○";
                }
                if (dt.Rows[0]["悬架后轴吸收率差评价"].ToString() == "0")
                {
                    xhcpj = "×";
                }
                if (dt.Rows[0]["悬架后轴吸收率差评价"].ToString() == "1")
                {
                    xhcpj = "○";
                }
                xjhzpd.Text = xhpj + xhcpj;
                #endregion
                #region 车速喇叭与侧滑
                csb.Text = dt.Rows[0]["车速值"].ToString();
                dychl.Text = dt.Rows[0]["侧滑值"].ToString();
                lbsjz.Text = dt.Rows[0]["喇叭声级值"].ToString();
                if (dt.Columns.Contains("侧滑值2"))
                {
                    dechl.Text = dt.Rows[0]["侧滑值2"].ToString();
                }
                else
                {
                    dechl.Text = "";
                }
                #endregion
                #region 单轴
                ydzzdv.Text = dt.Rows[0]["一轴制动和值"].ToString();
                ydzbphv.Text = dt.Rows[0]["一轴制动差值"].ToString();
                ydzzgcc.Text = dt.Rows[0]["一轴求差时左制动力值"].ToString();
                ydzygcc.Text = dt.Rows[0]["一轴求差时右制动力值"].ToString();
                if (dt.Rows[0]["一轴左拖滞比值"].ToString() != "" && dt.Rows[0]["一轴左拖滞比值"].ToString() != "-")
                {
                    //if (Convert.ToDouble(dt.Rows[0]["一轴左拖滞比值"].ToString()) > 3.5)
                    //{
                    //    ydzzzzv.Text = (Convert.ToDouble(dt.Rows[0]["一轴左拖滞比值"].ToString()) / 2).ToString("0.0");
                    //}
                    //else
                    //{
                    ydzzzzv.Text = dt.Rows[0]["一轴左拖滞比值"].ToString();
                    //}
                }
                if (dt.Rows[0]["一轴右拖滞比值"].ToString() != "" && dt.Rows[0]["一轴右拖滞比值"].ToString() != "-")
                {
                    //if (Convert.ToDouble(dt.Rows[0]["一轴右拖滞比值"].ToString()) > 3.5)
                    //{
                    //    ydzyzzv.Text = (Convert.ToDouble(dt.Rows[0]["一轴右拖滞比值"].ToString()) / 2).ToString("0.0");
                    //}
                    //else
                    //{
                    ydzyzzv.Text = dt.Rows[0]["一轴右拖滞比值"].ToString();
                    //}
                }
                edzzdv.Text = dt.Rows[0]["二轴制动和值"].ToString();
                edzbphv.Text = dt.Rows[0]["二轴制动差值"].ToString();
                edzzgcc.Text = dt.Rows[0]["二轴求差时左制动力值"].ToString();
                edzygcc.Text = dt.Rows[0]["二轴求差时右制动力值"].ToString();
                if (dt.Rows[0]["二轴左拖滞比值"].ToString() != "" && dt.Rows[0]["二轴左拖滞比值"].ToString() != "-")
                {
                    //if (Convert.ToDouble(dt.Rows[0]["二轴左拖滞比值"].ToString()) > 3.5)
                    //{
                    //    edzzzzv.Text = (Convert.ToDouble(dt.Rows[0]["二轴左拖滞比值"].ToString()) / 2).ToString("0.0");
                    //}
                    //else
                    //{
                    edzzzzv.Text = dt.Rows[0]["二轴左拖滞比值"].ToString();
                    //}
                }
                if (dt.Rows[0]["二轴右拖滞比值"].ToString() != "" && dt.Rows[0]["二轴右拖滞比值"].ToString() != "-")
                {
                    //if (Convert.ToDouble(dt.Rows[0]["二轴右拖滞比值"].ToString()) > 3.5)
                    //{
                    //    edzyzzv.Text = (Convert.ToDouble(dt.Rows[0]["二轴右拖滞比值"].ToString()) / 2).ToString("0.0");
                    //}
                    //else
                    //{
                    edzyzzv.Text = dt.Rows[0]["二轴右拖滞比值"].ToString();
                    //}
                }
                sdzzdv.Text = dt.Rows[0]["三轴制动和值"].ToString();
                sdzbphv.Text = dt.Rows[0]["三轴制动差值"].ToString();
                sdzzgcc.Text = dt.Rows[0]["三轴求差时左制动力值"].ToString();
                sdzygcc.Text = dt.Rows[0]["三轴求差时右制动力值"].ToString();
                if (dt.Rows[0]["三轴左拖滞比值"].ToString() != "" && dt.Rows[0]["三轴左拖滞比值"].ToString() != "-")
                {
                    //if (Convert.ToDouble(dt.Rows[0]["三轴左拖滞比值"].ToString()) > 3.5)
                    //{
                    //    sdzzzzv.Text = (Convert.ToDouble(dt.Rows[0]["三轴左拖滞比值"].ToString()) / 2).ToString("0.0");
                    //}
                    //else
                    //{
                    sdzzzzv.Text = dt.Rows[0]["三轴左拖滞比值"].ToString();
                    //}
                }
                if (dt.Rows[0]["三轴右拖滞比值"].ToString() != "" && dt.Rows[0]["三轴右拖滞比值"].ToString() != "-")
                {
                    //if (Convert.ToDouble(dt.Rows[0]["三轴右拖滞比值"].ToString()) > 3.5)
                    //{
                    //    sdzyzzv.Text = (Convert.ToDouble(dt.Rows[0]["三轴右拖滞比值"].ToString()) / 2).ToString("0.0");
                    //}
                    //else
                    //{
                    sdzyzzv.Text = dt.Rows[0]["三轴右拖滞比值"].ToString();
                    //}
                }
                sidzzdv.Text = dt.Rows[0]["四轴制动和值"].ToString();
                sidzbphv.Text = dt.Rows[0]["四轴制动差值"].ToString();
                sidzzgcc.Text = dt.Rows[0]["四轴求差时左制动力值"].ToString();
                sidzygcc.Text = dt.Rows[0]["四轴求差时右制动力值"].ToString();
                if (dt.Rows[0]["四轴左拖滞比值"].ToString() != "" && dt.Rows[0]["四轴左拖滞比值"].ToString() != "-")
                {
                    //if (Convert.ToDouble(dt.Rows[0]["四轴左拖滞比值"].ToString()) > 3.5)
                    //{
                    //    sidzzzzv.Text = (Convert.ToDouble(dt.Rows[0]["四轴左拖滞比值"].ToString()) / 2).ToString("0.0");
                    //}
                    //else
                    //{
                    sidzzzzv.Text = dt.Rows[0]["四轴左拖滞比值"].ToString();
                    //}
                }
                if (dt.Rows[0]["四轴右拖滞比值"].ToString() != "" && dt.Rows[0]["四轴右拖滞比值"].ToString() != "-")
                {
                    //if (Convert.ToDouble(dt.Rows[0]["四轴右拖滞比值"].ToString()) > 3.5)
                    //{
                    //    sidzyzzv.Text = (Convert.ToDouble(dt.Rows[0]["四轴右拖滞比值"].ToString()) / 2).ToString("0.0");
                    //}
                    //else
                    //{
                    sidzyzzv.Text = dt.Rows[0]["四轴右拖滞比值"].ToString();
                    //}
                }
                wdzzdv.Text = dt.Rows[0]["五轴制动和值"].ToString();
                wdzbphv.Text = dt.Rows[0]["五轴制动差值"].ToString();
                wdzzgcc.Text = dt.Rows[0]["五轴求差时左制动力值"].ToString();
                wdzygcc.Text = dt.Rows[0]["五轴求差时右制动力值"].ToString();
                if (dt.Rows[0]["五轴左拖滞比值"].ToString() != "" && dt.Rows[0]["五轴左拖滞比值"].ToString() != "-")
                {
                    //if (Convert.ToDouble(dt.Rows[0]["五轴左拖滞比值"].ToString()) > 3.5)
                    //{
                    //    wdzzzzv.Text = (Convert.ToDouble(dt.Rows[0]["五轴左拖滞比值"].ToString()) / 2).ToString("0.0");
                    //}
                    //else
                    //{
                    wdzzzzv.Text = dt.Rows[0]["五轴左拖滞比值"].ToString();
                    //}
                }
                if (dt.Rows[0]["五轴右拖滞比值"].ToString() != "" && dt.Rows[0]["五轴右拖滞比值"].ToString() != "-")
                {
                    //if (Convert.ToDouble(dt.Rows[0]["五轴右拖滞比值"].ToString()) > 3.5)
                    //{
                    //    wdzyzzv.Text = (Convert.ToDouble(dt.Rows[0]["五轴右拖滞比值"].ToString()) / 2).ToString("0.0");
                    //}
                    //else
                    //{
                    wdzyzzv.Text = dt.Rows[0]["五轴右拖滞比值"].ToString();
                    //}
                }
                ldzzdv.Text = dt.Rows[0]["六轴制动和值"].ToString();
                ldzbphv.Text = dt.Rows[0]["六轴制动差值"].ToString();
                ldzzgcc.Text = dt.Rows[0]["六轴求差时左制动力值"].ToString();
                ldzygcc.Text = dt.Rows[0]["六轴求差时右制动力值"].ToString();
                if (dt.Rows[0]["六轴左拖滞比值"].ToString() != "" && dt.Rows[0]["六轴左拖滞比值"].ToString() != "-")
                {
                    //if (Convert.ToDouble(dt.Rows[0]["六轴左拖滞比值"].ToString()) > 3.5)
                    //{
                    //    ldzzzzv.Text = (Convert.ToDouble(dt.Rows[0]["六轴左拖滞比值"].ToString()) / 2).ToString("0.0");
                    //}
                    //else
                    //{
                    ldzzzzv.Text = dt.Rows[0]["六轴左拖滞比值"].ToString();
                    //}
                }
                if (dt.Rows[0]["六轴右拖滞比值"].ToString() != "" && dt.Rows[0]["六轴右拖滞比值"].ToString() != "-")
                {
                    //if (Convert.ToDouble(dt.Rows[0]["六轴右拖滞比值"].ToString()) > 3.5)
                    //{
                    //    ldzyzzv.Text = (Convert.ToDouble(dt.Rows[0]["六轴右拖滞比值"].ToString()) / 2).ToString("0.0");
                    //}
                    //else
                    //{
                    ldzyzzv.Text = dt.Rows[0]["六轴右拖滞比值"].ToString();
                    //}
                }
                #endregion
                #region 排放性
                qygdsCO.Text = dt.Rows[0]["双怠速CO值"].ToString();
                qygdsHC.Text = dt.Rows[0]["双怠速HC值"].ToString();
                qyddsCO.Text = dt.Rows[0]["怠速CO值"].ToString();
                qyddsHC.Text = dt.Rows[0]["怠速HC值"].ToString();
                string gcopj = "";
                string ghcpj = "";
                string dcopj = "";
                string dhcpj = "";
                if (dt.Rows[0]["怠速CO评价"].ToString() == "0")
                {
                    dcopj = "×";
                }
                if (dt.Rows[0]["怠速CO评价"].ToString() == "1")
                {
                    dcopj = "○";
                }
                if (dt.Rows[0]["怠速HC评价"].ToString() == "0")
                {
                    dhcpj = "×";
                }
                if (dt.Rows[0]["怠速HC评价"].ToString() == "1")
                {
                    dhcpj = "○";
                }
                if (dt.Rows[0]["双怠速CO评价"].ToString() == "0")
                {
                    gcopj = "×";
                }
                if (dt.Rows[0]["双怠速CO评价"].ToString() == "1")
                {
                    gcopj = "○";
                }
                if (dt.Rows[0]["双怠速HC评价"].ToString() == "0")
                {
                    ghcpj = "×";
                }
                if (dt.Rows[0]["双怠速HC评价"].ToString() == "1")
                {
                    ghcpj = "○";
                }
                qypd.Text = gcopj + ghcpj + dcopj + dhcpj;
                //wtgk5025CO.Text = dt.Rows[0]["工况法5025CO值"].ToString();
                //wtgk5025HC.Text = dt.Rows[0]["工况法5025HC值"].ToString();
                //wtgk5025NO.Text = dt.Rows[0]["工况法5025NO值"].ToString();
                //wtgk2540CO.Text = dt.Rows[0]["工况法2540CO值"].ToString();
                //wtgk2540HC.Text = dt.Rows[0]["工况法2540HC值"].ToString();
                //wtgk2540NO.Text = dt.Rows[0]["工况法2540NO值"].ToString();
                cygxs1.Text = dt.Rows[0]["光吸收率值1"].ToString();
                cygxs2.Text = dt.Rows[0]["光吸收率值2"].ToString();
                cygxs3.Text = dt.Rows[0]["光吸收率值3"].ToString();
                cygxsavg.Text = dt.Rows[0]["光吸收率值"].ToString();
                qygdsλ.Text = dt.Rows[0]["空气过量系数值"].ToString();
                Random r = new Random();
                if ((ryxs.Text.Replace(" ","").Contains("汽油")|| ryxs.Text.Replace(" ", "").Contains("天然气")) && (qygdsCO.Text != ""||qygdsHC.Text!=""||qyddsCO.Text!=""||qyddsHC.Text!=""))
                {
                    if (qygdsλ.Text != "")
                    {
                        if (Convert.ToDouble(qygdsλ.Text) < 0.97 || Convert.ToDouble(qygdsλ.Text) > 1.03)
                        {
                            qygdsλ.Text = (0.97 + r.NextDouble() * (1.03 - 0.97)).ToString("0.00");
                        }
                    }
                    else
                    {
                        qygdsλ.Text = (0.97 + r.NextDouble() * (1.03 - 0.97)).ToString("0.00");
                    }
                }
                //cyyd1.Text = dt.Rows[0]["烟度值1"].ToString();
                //cyyd2.Text = dt.Rows[0]["烟度值2"].ToString();
                //cyyd3.Text = dt.Rows[0]["烟度值3"].ToString();
                //cyydavg.Text = dt.Rows[0]["烟度值"].ToString();
                if (dt.Rows[0]["光吸收率评价"].ToString() == "0")
                {
                    cypd.Text = "×";
                }
                if (dt.Rows[0]["光吸收率评价"].ToString() == "1")
                {
                    cypd.Text = "○";
                }
                #endregion
                #region 前照灯
                zwygdg.Text = dt.Rows[0]["左灯高值"].ToString();
                ywygdg.Text = dt.Rows[0]["右灯高值"].ToString();
                if(dt.Columns.Contains("左近灯高值"))
                {
                    zwjgdg.Text = dt.Rows[0]["左近灯高值"].ToString();
                }
                else
                {
                    zwjgdg.Text = "";
                }
                if (dt.Columns.Contains("右近灯高值"))
                {
                    ywjgdg.Text = dt.Rows[0]["右近灯高值"].ToString();
                }
                else
                {
                    ywjgdg.Text = "";
                }
                zwyggq.Text = dt.Rows[0]["左主远光强度值"].ToString();
                znyggq.Text = dt.Rows[0]["左副远光强度值"].ToString();
                ynyggq.Text = dt.Rows[0]["右副远光强度值"].ToString();
                ywyggq.Text = dt.Rows[0]["右主远光强度值"].ToString();
                if (dt.Columns.Contains("左主远光左右偏差值"))
                {
                    zwygsp.Text = dt.Rows[0]["左主远光左右偏差值"].ToString();
                }
                else
                {
                    zwygsp.Text = "";
                }
                if (dt.Columns.Contains("左副远光左右偏差值"))
                {
                    if (znyggq.Text != "" && znyggq.Text != "-")
                    {
                        znygsp.Text = dt.Rows[0]["左副远光左右偏差值"].ToString();
                    }
                    else
                    {
                        znygsp.Text = "";
                    }
                }
                else
                {
                    znygsp.Text = "";
                }
                if (dt.Columns.Contains("右副远光左右偏差值"))
                {
                    if (ynyggq.Text != "" && ynyggq.Text != "-")
                    {
                        ynygsp.Text = dt.Rows[0]["右副远光左右偏差值"].ToString();
                    }
                    else
                    {
                        ynygsp.Text = "";
                    }
                }
                else
                {
                    ynygsp.Text = "";
                }
                if (dt.Columns.Contains("右主远光左右偏差值"))
                {
                    ywygsp.Text = dt.Rows[0]["右主远光左右偏差值"].ToString();
                }
                else
                {
                    ywygsp.Text = "";
                }
                if (dt.Columns.Contains("左近光左右偏差值"))
                {
                    zwjgsp.Text = dt.Rows[0]["左近光左右偏差值"].ToString();
                }
                else
                {
                    zwjgsp.Text = "";
                }
                if (dt.Columns.Contains("右近光左右偏差值"))
                {
                    ywjgsp.Text = dt.Rows[0]["右近光左右偏差值"].ToString();
                }
                else
                {
                    ywjgsp.Text = "";
                }
                #region
                //左主远光上下偏差H值
                if (dt.Rows[0]["左主远光上下偏差值"].ToString() != "" && dt.Rows[0]["左主远光上下偏差值"].ToString() != "-" && dt.Rows[0]["左灯高值"].ToString() != "" && dt.Rows[0]["左灯高值"].ToString() != "-" && dt.Rows[0]["左灯高值"].ToString() != "0")
                {
                    if (!Regex.IsMatch(dt.Rows[0]["左主远光上下偏差值"].ToString(), "^([0-9]{1,}[.][0-9]*)$") || !Regex.IsMatch(dt.Rows[0]["左主远光上下偏差值"].ToString(), "^([0-9]{1,}[.][0-9]*)$"))
                    {
                        double dzwyh = (Convert.ToDouble(dt.Rows[0]["左主远光上下偏差值"].ToString()) + Convert.ToDouble(dt.Rows[0]["左灯高值"].ToString())) / Convert.ToDouble(dt.Rows[0]["左灯高值"].ToString());
                        zwygczH.Text = dzwyh.ToString("0.00");
                    }
                    else
                    {
                        zwygczH.Text = dt.Rows[0]["左主远光上下偏差值"].ToString();
                    }
                }
                else
                {
                    zwygczH.Text = "";
                }
                //左副远光上下偏差H值
                if (dt.Rows[0]["左副远光上下偏差值"].ToString() != "" && dt.Rows[0]["左副远光上下偏差值"].ToString() != "-" && dt.Rows[0]["左灯高值"].ToString() != "" && dt.Rows[0]["左灯高值"].ToString() != "-" && dt.Rows[0]["左灯高值"].ToString() != "0")
                {
                    if (znyggq.Text != "" && znyggq.Text != "-")
                    {
                        if (!Regex.IsMatch(dt.Rows[0]["左副远光上下偏差值"].ToString(), "^([0-9]{1,}[.][0-9]*)$") || !Regex.IsMatch(dt.Rows[0]["左副远光上下偏差值"].ToString(), "^([0-9]{1,}[.][0-9]*)$"))
                        {
                            double dznyh = (Convert.ToDouble(dt.Rows[0]["左副远光上下偏差值"].ToString()) + Convert.ToDouble(dt.Rows[0]["左灯高值"].ToString())) / Convert.ToDouble(dt.Rows[0]["左灯高值"].ToString());
                            znygczH.Text = dznyh.ToString("0.00");
                        }
                        else
                        {
                            znygczH.Text = dt.Rows[0]["左副远光上下偏差值"].ToString();
                        }
                    }
                    else
                    {
                        znygczH.Text = "";
                    }
                }
                else
                {
                    znygczH.Text = "";
                }
                //右主远光上下偏差H值
                if (dt.Rows[0]["右主远光上下偏差值"].ToString() != "" && dt.Rows[0]["右主远光上下偏差值"].ToString() != "-" && dt.Rows[0]["右灯高值"].ToString() != "" && dt.Rows[0]["右灯高值"].ToString() != "-" && dt.Rows[0]["右灯高值"].ToString() != "0")
                {
                    if (!Regex.IsMatch(dt.Rows[0]["右主远光上下偏差值"].ToString(), "^([0-9]{1,}[.][0-9]*)$") || !Regex.IsMatch(dt.Rows[0]["右主远光上下偏差值"].ToString(), "^([0-9]{1,}[.][0-9]*)$"))
                    {
                        double dywyh = (Convert.ToDouble(dt.Rows[0]["右主远光上下偏差值"].ToString()) + Convert.ToDouble(dt.Rows[0]["右灯高值"].ToString())) / Convert.ToDouble(dt.Rows[0]["右灯高值"].ToString());
                        ywygczH.Text = dywyh.ToString("0.00");
                    }
                    else
                    {
                        ywygczH.Text = dt.Rows[0]["右主远光上下偏差值"].ToString();
                    }
                }
                else
                {
                    ywygczH.Text = "";
                }
                //右副远光上下偏差H值
                if (dt.Rows[0]["右副远光上下偏差值"].ToString() != "" && dt.Rows[0]["右副远光上下偏差值"].ToString() != "-" && dt.Rows[0]["右灯高值"].ToString() != "" && dt.Rows[0]["右灯高值"].ToString() != "-" && dt.Rows[0]["右灯高值"].ToString() != "0")
                {
                    if (ynyggq.Text != "" && ynyggq.Text != "-")
                    {
                        if (!Regex.IsMatch(dt.Rows[0]["右副远光上下偏差值"].ToString(), "^([0-9]{1,}[.][0-9]*)$") || !Regex.IsMatch(dt.Rows[0]["右副远光上下偏差值"].ToString(), "^([0-9]{1,}[.][0-9]*)$"))
                        {
                            double dynyh = (Convert.ToDouble(dt.Rows[0]["右副远光上下偏差值"].ToString()) + Convert.ToDouble(dt.Rows[0]["右灯高值"].ToString())) / Convert.ToDouble(dt.Rows[0]["右灯高值"].ToString());
                            ynygczH.Text = dynyh.ToString("0.00");
                        }
                        else
                        {
                            ynygczH.Text = dt.Rows[0]["右副远光上下偏差值"].ToString();
                        }
                    }
                    else
                    {
                        ynygczH.Text = "";
                    }
                }
                else
                {
                    ynygczH.Text = "";
                }
                //左近光
                if (dt.Rows[0]["左近光上下偏差值"].ToString() != "" && dt.Rows[0]["左近光上下偏差值"].ToString() != "-" && dt.Rows[0]["左灯高值"].ToString() != "" && dt.Rows[0]["左灯高值"].ToString() != "-" && dt.Rows[0]["左灯高值"].ToString() != "0")
                {
                    if (!Regex.IsMatch(dt.Rows[0]["左近光上下偏差值"].ToString(), "^([0-9]{1,}[.][0-9]*)$") || !Regex.IsMatch(dt.Rows[0]["左近光上下偏差值"].ToString(), "^([0-9]{1,}[.][0-9]*)$"))
                    {
                        double dzwjh = (Convert.ToDouble(dt.Rows[0]["左近光上下偏差值"].ToString()) + Convert.ToDouble(dt.Rows[0]["左灯高值"].ToString())) / Convert.ToDouble(dt.Rows[0]["左灯高值"].ToString());
                        zwjgczH.Text = dzwjh.ToString("0.00");
                    }
                    else
                    {
                        zwjgczH.Text = dt.Rows[0]["左近光上下偏差值"].ToString();
                    }
                }
                else
                {
                    zwjgczH.Text = "";
                }
                if (znyggq.Text != "" && znyggq.Text != "-")
                {
                    if (dt.Rows[0]["左近光上下偏差值"].ToString() != "" && dt.Rows[0]["左近光上下偏差值"].ToString() != "-" && dt.Rows[0]["左灯高值"].ToString() != "" && dt.Rows[0]["左灯高值"].ToString() != "-" && dt.Rows[0]["左灯高值"].ToString() != "0")
                    {
                        if (!Regex.IsMatch(dt.Rows[0]["左近光上下偏差值"].ToString(), "^([0-9]{1,}[.][0-9]*)$") || !Regex.IsMatch(dt.Rows[0]["左近光上下偏差值"].ToString(), "^([0-9]{1,}[.][0-9]*)$"))
                        {
                            double dznjh = (Convert.ToDouble(dt.Rows[0]["左近光上下偏差值"].ToString()) + Convert.ToDouble(dt.Rows[0]["左灯高值"].ToString())) / Convert.ToDouble(dt.Rows[0]["左灯高值"].ToString());
                            znjgczH.Text = dznjh.ToString("0.00");
                        }
                        else
                        {
                            znjgczH.Text = dt.Rows[0]["左近光上下偏差值"].ToString();
                        }
                    }
                    else
                    {
                        znjgczH.Text = "";
                    }
                }
                else
                {
                    znjgczH.Text = "";
                }
                //右近光
                if (dt.Rows[0]["右近光上下偏差值"].ToString() != "" && dt.Rows[0]["右近光上下偏差值"].ToString() != "-" && dt.Rows[0]["右灯高值"].ToString() != "" && dt.Rows[0]["右灯高值"].ToString() != "-" && dt.Rows[0]["右灯高值"].ToString() != "0")
                {
                    if (!Regex.IsMatch(dt.Rows[0]["右近光上下偏差值"].ToString(), "^([0-9]{1,}[.][0-9]*)$") || !Regex.IsMatch(dt.Rows[0]["右近光上下偏差值"].ToString(), "^([0-9]{1,}[.][0-9]*)$"))
                    {
                        double dywjh = (Convert.ToDouble(dt.Rows[0]["右近光上下偏差值"].ToString()) + Convert.ToDouble(dt.Rows[0]["右灯高值"].ToString())) / Convert.ToDouble(dt.Rows[0]["右灯高值"].ToString());
                        ywjgczH.Text = dywjh.ToString("0.00");
                    }
                    else
                    {
                        ywjgczH.Text = dt.Rows[0]["右近光上下偏差值"].ToString();
                    }
                }
                else
                {
                    ywjgczH.Text = "";
                }
                if (ynyggq.Text != "" && ynyggq.Text != "-")
                {
                    if (dt.Rows[0]["右近光上下偏差值"].ToString() != "" && dt.Rows[0]["右近光上下偏差值"].ToString() != "-" && dt.Rows[0]["右灯高值"].ToString() != "" && dt.Rows[0]["右灯高值"].ToString() != "-" && dt.Rows[0]["右灯高值"].ToString() != "0")
                    {
                        if (!Regex.IsMatch(dt.Rows[0]["右近光上下偏差值"].ToString(), "^([0-9]{1,}[.][0-9]*)$") || !Regex.IsMatch(dt.Rows[0]["右近光上下偏差值"].ToString(), "^([0-9]{1,}[.][0-9]*)$"))
                        {
                            double dynjh = (Convert.ToDouble(dt.Rows[0]["右近光上下偏差值"].ToString()) + Convert.ToDouble(dt.Rows[0]["右灯高值"].ToString())) / Convert.ToDouble(dt.Rows[0]["右灯高值"].ToString());
                            ynjgczH.Text = dynjh.ToString("0.00");
                        }
                        else
                        {
                            ynjgczH.Text = dt.Rows[0]["右近光上下偏差值"].ToString();
                        }
                    }
                    else
                    {
                        ynjgczH.Text = "";
                    }
                }
                else
                {
                    ynjgczH.Text = "";
                }
                #endregion
                #endregion
                #region 整车
                dczdl.Text = dt.Rows[0]["整车制动和值"].ToString();
                dczczdl.Text = dt.Rows[0]["手制动和值"].ToString();
                dcspcz.Text = dt.Rows[0]["整车轴重值"].ToString();
                #endregion
                #region 路试
                if (dt.Columns.Contains("制动初速度值"))
                {
                    lszdcsd.Text = dt.Rows[0]["制动初速度值"].ToString();
                }
                else
                {
                    lszdcsd.Text = "";
                }
                if (dt.Columns.Contains("制动距离值"))
                {
                    lszdjl.Text = dt.Rows[0]["制动距离值"].ToString();
                }
                else
                {
                    lszdjl.Text = "";
                }
                if (dt.Columns.Contains("制动稳定性值"))
                {
                    lszdwdx.Text = dt.Rows[0]["制动稳定性值"].ToString();
                }
                else
                {
                    lszdwdx.Text = "";
                }
                if (dt.Columns.Contains("制动协调时间"))
                {
                    lszdxtsj.Text = dt.Rows[0]["制动协调时间"].ToString();
                }
                else
                {
                    lszdxtsj.Text = "";
                }
                if (dt.Columns.Contains("制动减速度值"))
                {
                    lszdmfdd.Text = dt.Rows[0]["制动减速度值"].ToString();
                }
                else
                {
                    lszdmfdd.Text = "";
                }
                #endregion
                #region 动力经济性
                yhbzz.Text = dt.Rows[0]["油耗标准"].ToString();
                yhscz.Text = dt.Rows[0]["百公里油耗值"].ToString();
                wdcs.Text = dt.Rows[0]["轮边稳定车速"].ToString();
                jzl.Text = dt.Rows[0]["动力性加载力"].ToString();
                if (dt.Rows[0]["百公里油耗评价"].ToString() == "1")
                {
                    jjxpd.Text = "○";
                }
                if (dt.Rows[0]["百公里油耗评价"].ToString() == "0")
                {
                    jjxpd.Text = "×";
                }
                if (dt.Rows[0]["燃油类型"].ToString().Replace(" ", "").Contains("汽油")|| dt.Rows[0]["燃油类型"].ToString().Replace(" ", "").Contains("天然气"))
                {
                    edcs.Text = dt.Rows[0]["额定扭矩工况车速"].ToString();
                    if (dt.Columns.Contains("发动机额定扭矩"))
                    {
                        if (dt.Rows[0]["发动机额定扭矩"].ToString().Replace(" ", "").Contains("-")||dt.Rows[0]["发动机额定扭矩"].ToString().Replace(" ", "")=="")
                        {
                            dbgl.Text = "";
                        }
                        else
                        {
                            dbgl.Text = (Convert.ToDouble(dt.Rows[0]["发动机额定扭矩"].ToString().Replace(" ", "")) * 0.75).ToString("0.0");
                        }
                    }
                    edhand.Text = "额定扭矩工况车速";
                    if (dt.Rows[0]["额定扭矩工况评价"].ToString() == "1")
                    {
                        dlxpd.Text = "○";
                    }
                    if (dt.Rows[0]["额定扭矩工况评价"].ToString() == "0")
                    {
                        dlxpd.Text = "×";
                    }
                }
                if (dt.Rows[0]["燃油类型"].ToString().Replace(" ", "").Contains("柴油"))
                {
                    edcs.Text = dt.Rows[0]["额定功率工况车速"].ToString();
                    edhand.Text = "额定功率工况车速";
                    if (dt.Columns.Contains("发动机额定功率"))
                    {
                        string arcr = dt.Rows[0]["发动机额定功率"].ToString().Replace(" ", "");
                        if (dt.Rows[0]["发动机额定功率"].ToString().Replace(" ", "").Contains("-")||dt.Rows[0]["发动机额定功率"].ToString().Replace(" ", "")=="")
                        {
                            dbgl.Text = "";
                        }
                        else
                        {
                            dbgl.Text = (Convert.ToDouble(dt.Rows[0]["发动机额定功率"].ToString()) * 0.75).ToString("0.0");
                        }
                    }
                    if (dt.Rows[0]["额定功率工况评价"].ToString() == "1")
                    {
                        dlxpd.Text = "○";
                    }
                    if (dt.Rows[0]["额定功率工况评价"].ToString() == "0")
                    {
                        dlxpd.Text = "×";
                    }
                }
                #endregion
                #region 驱动轴空载质量
                double yz=0;
                double ez=0;
                double sz=0;
                double siz=0;
                double wz=0;
                double lz=0;
                if (zcz.Text.Contains("1"))
                {
                     yz = Convert.ToDouble(TextsIsnull(yzzlh.Text)) + Convert.ToDouble(TextsIsnull(yzylh.Text));
                }
                if (zcz.Text.Contains("2"))
                {
                     ez = Convert.ToDouble(TextsIsnull(ezzlh.Text)) + Convert.ToDouble(TextsIsnull(ezylh.Text));
                }
                if (zcz.Text.Contains("3"))
                {
                     sz = Convert.ToDouble(TextsIsnull(szzlh.Text)) + Convert.ToDouble(TextsIsnull(szylh.Text));
                }
               if (zcz.Text.Contains("4"))
                {
                     siz = Convert.ToDouble(TextsIsnull(sizzlh.Text)) + Convert.ToDouble(TextsIsnull(sizylh.Text));
                }
                if (zcz.Text.Contains("5"))
                {
                     wz = Convert.ToDouble(TextsIsnull(wzzlh.Text)) + Convert.ToDouble(TextsIsnull(wzylh.Text));
                }
                if (zcz.Text.Contains("6"))
                {
                     lz = Convert.ToDouble(TextsIsnull(lzzlh.Text)) + Convert.ToDouble(TextsIsnull(lzylh.Text));
                }
                qdzkazl.Text = (yz + ez + sz + siz + wz + lz).ToString();
                #endregion
            }
            catch (Exception ex)
            {
                conn.Close();
                string err = "数据绑定问题,错误代码:" + nerror.ToString();
                MessageBox.Show(ex.ToString());
            }
        }
        //页面所有文本框为只读
        public void QueryControl()
        {
            foreach (Control ctl in this.tabPage1.Controls)
            {
                if (ctl is TextBox)
                {
                    (ctl as TextBox).ReadOnly = true;
                }
            }
            foreach (Control ctls in this.tabPage3.Controls)
            {
                if (ctls is GroupBox)
                {
                    foreach (Control ctlas in ctls.Controls)
                    {
                        if (ctlas is TextBox)
                        {
                            (ctlas as TextBox).ReadOnly = true;
                        }
                    }
                }
            }
        }
        //页面所有文本框都可以修改
        public void QueryControls()
        {
            foreach (Control ctl in this.tabPage1.Controls)
            {
                if (ctl is TextBox)
                {
                    (ctl as TextBox).ReadOnly = false;
                }
            }
            foreach (Control ctls in this.tabPage3.Controls)
            {
                if (ctls is GroupBox)
                {
                    foreach (Control ctlas in ctls.Controls)
                    {
                        if (ctlas is TextBox)
                        {
                            (ctlas as TextBox).ReadOnly = false;
                        }
                    }
                }
            }
        }
        //页面所有文本清空
        public void ClearControl()
        {
            foreach (Control ctl in this.tabPage1.Controls)
            {
                if (ctl is TextBox)
                {
                    (ctl as TextBox).Text = "";
                }
            }
            foreach (Control ctls in this.tabPage3.Controls)
            {
                if (ctls is GroupBox)
                {
                    foreach (Control ctlas in ctls.Controls)
                    {
                        if (ctlas is TextBox)
                        {
                            (ctlas as TextBox).Text = "";
                        }
                    }
                }
            }
        }
        //修改数据  
        public void Exit_Modification()
        {
            try
            {
                string sfdlxj = "";
                if(zxzxjxs.Text=="独立")
                {
                    sfdlxj = "1";
                }
                else
                {
                    sfdlxj = "0";
                }
                string yhpj = "";
                if (jjxpd.Text == "○")
                {
                    yhpj = "1";
                }
                if (jjxpd.Text == "×")
                {
                    yhpj = "0";
                }
                string ywlxs = "";
                if(ywlx.Text.Contains("在用"))
                {
                    ywlxs = "0";
                }
                else
                {
                    ywlxs = "1";
                }
                string sednjcs = "";
                string sedglcs = "";
                string njgkpj = "";
                string glgkpj = "";
                if (ryxs.Text.Replace(" ", "").Contains("汽油"))
                {
                    sednjcs = edcs.Text;
                    if (dlxpd.Text == "○")
                    {
                        njgkpj = "1";
                    }
                    if (dlxpd.Text == "×")
                    {
                        njgkpj = "0";
                    }
                }
                if (ryxs.Text.Replace(" ", "").Contains("柴油"))
                {
                    sedglcs = edcs.Text;
                    if (dlxpd.Text == "○")
                    {
                        glgkpj = "1";
                    }
                    if (dlxpd.Text == "×")
                    {
                        glgkpj = "0";
                    }
                }
                #region 灯光的判断
                string zdg = zwygdg.Text;//左灯高
                string ydg = ywygdg.Text;//右灯高
                string zwych = zwygczH.Text;//左外远光垂直H值
                string znych = znygczH.Text;//左内远光垂直H值
                string ynych = ynygczH.Text;//右内远光垂直H值
                string ywych = ywygczH.Text;//右外远光垂直H值
                string zjch = zwjgczH.Text;//左近光垂直H值
                string yjch = ywjgczH.Text;//右近光垂直H值
                string znjch = znjgczH.Text;
                string ynjch = ynjgczH.Text;
                string dzwyc = "";//左外远光垂直H值
                string dznyc = "";//左内远光垂直H值
                string dynyc = "";//右内远光垂直H值
                string dywyc = "";//右外远光垂直H值
                string zjc = "";//左近光垂直H值
                string yjc = "";//右近光垂直H值
                                //左外远光垂直偏差值
                if (zdg != "" && zwych != "" && zdg != "0")
                {
                    dzwyc = (Convert.ToDouble(zdg) * Convert.ToDouble(zwych) - Convert.ToDouble(zdg)).ToString();
                }
                //左内远光垂直偏差值
                if (zdg != "" && znych != "" && zdg != "0")
                {
                    dznyc = (Convert.ToDouble(zdg) * Convert.ToDouble(znych) - Convert.ToDouble(zdg)).ToString();
                }
                //右外远光垂直偏差值
                if (ydg != "" && ywych != "" && ydg != "0")
                {
                    dywyc = (Convert.ToDouble(ydg) * Convert.ToDouble(ywych) - Convert.ToDouble(ydg)).ToString();
                }
                //右内远光垂直偏差值
                if (ydg != "" && ynych != "" && ydg != "0")
                {
                    dynyc = (Convert.ToDouble(ydg) * Convert.ToDouble(ynych) - Convert.ToDouble(ydg)).ToString();
                }

                //左近光垂直偏差值
                if (zdg != "" && zjch != "" && zdg != "0")
                {
                    zjc = (Convert.ToDouble(zdg) * Convert.ToDouble(zjch) - Convert.ToDouble(zdg)).ToString();
                }
                //右近光垂直偏差值
                if (ydg != "" && yjch != "" && ydg != "0")
                {
                    yjc = (Convert.ToDouble(ydg) * Convert.ToDouble(yjch) - Convert.ToDouble(ydg)).ToString();
                }
                //左近光垂直偏差值
                if (zdg != "" && znjch != "" && zdg != "0")
                {
                    zjc = (Convert.ToDouble(zdg) * Convert.ToDouble(znjch) - Convert.ToDouble(zdg)).ToString();
                }
                //右近光垂直偏差值
                if (ydg != "" && ynjch != "" && ydg != "0")
                {
                    yjc = (Convert.ToDouble(ydg) * Convert.ToDouble(ynjch) - Convert.ToDouble(ydg)).ToString();
                }
                #endregion
                string xjc = "";
                if (ywlx.Text == "新车")
                {
                    xjc = "1";
                }
                else
                {
                    xjc = "0";
                }
                #region 驻车制动力的判断
                string zczdlz = "";
                string zczdly = "";
                if (zcz.Text.Contains("1"))
                {
                    zczdlz = yzzzczd.Text;
                    zczdly = yzyzczd.Text;
                }
                if (zcz.Text.Contains("2"))
                {
                    zczdlz = ezzzczd.Text;
                    zczdly = ezyzczd.Text;
                }
                if (zcz.Text.Contains("3"))
                {
                    zczdlz = szzzczd.Text;
                    zczdly = szyzczd.Text;
                }
                if (zcz.Text.Contains("4"))
                {
                    zczdlz = sizzzczd.Text;
                    zczdly = sizyzczd.Text;
                }
                if (zcz.Text.Contains("5"))
                {
                    zczdlz = wzzzczd.Text;
                    zczdly = wzyzczd.Text;
                }
                if (zcz.Text.Contains("6"))
                {
                    zczdlz = lzzzczd.Text;
                    zczdly = lzyzczd.Text;
                }
                #endregion
                #region 整车评价
                string zczdhpj = "";
                string szdhpj = "";
                if (dcpd.Text != "" && dcpd.Text != "-")
                {
                    if (dcpd.Text.Substring(0, 1) == "○")
                    {
                        zczdhpj = "1";
                    }
                    if (dcpd.Text.Substring(0, 1) == "×")
                    {
                        zczdhpj = "0";
                    }
                    if (dcpd.Text.Substring(1, 1) == "○")
                    {
                        szdhpj = "1";
                    }
                    if (dcpd.Text.Substring(1, 1) == "×")
                    {
                        szdhpj = "0";
                    }
                }
                #endregion
                #region 一轴评价
                string yzzdpj = "";
                string yzbphpj = "";
                string yzzzzvpj = "";
                string yzyzzvpj = "";
                if (yzpd.Text != "" && yzpd.Text != "-")
                {
                    if (yzpd.Text.Substring(0, 1) == "○")
                    {
                        yzzdpj = "1";
                    }
                    if (yzpd.Text.Substring(0, 1) == "×")
                    {
                        yzzdpj = "0";
                    }
                    if (yzpd.Text.Substring(1, 1) == "○")
                    {
                        yzbphpj = "1";
                    }
                    if (yzpd.Text.Substring(1, 1) == "×")
                    {
                        yzbphpj = "0";
                    }
                    if (yzpd.Text.Substring(2, 1) == "○")
                    {
                        yzzzzvpj = "1";
                    }
                    if (yzpd.Text.Substring(2, 1) == "×")
                    {
                        yzzzzvpj = "0";
                    }
                    if (yzpd.Text.Substring(3, 1) == "○")
                    {
                        yzyzzvpj = "1";
                    }
                    if (yzpd.Text.Substring(3, 1) == "×")
                    {
                        yzyzzvpj = "0";
                    }
                }
                #endregion
                #region 二轴评价
                string ezzdpj = "";
                string ezbphpj = "";
                string ezzzzvpj = "";
                string ezyzzvpj = "";
                if (ezpd.Text != "" && ezpd.Text != "-")
                {
                    if (ezpd.Text.Substring(0, 1) == "○")
                    {
                        ezzdpj = "1";
                    }
                    if (ezpd.Text.Substring(0, 1) == "×")
                    {
                        ezzdpj = "0";
                    }
                    if (ezpd.Text.Substring(1, 1) == "○")
                    {
                        ezbphpj = "1";
                    }
                    if (ezpd.Text.Substring(1, 1) == "×")
                    {
                        ezbphpj = "0";
                    }
                    if (ezpd.Text.Substring(2, 1) == "○")
                    {
                        ezzzzvpj = "1";
                    }
                    if (ezpd.Text.Substring(2, 1) == "×")
                    {
                        ezzzzvpj = "0";
                    }
                    if (ezpd.Text.Substring(3, 1) == "○")
                    {
                        ezyzzvpj = "1";
                    }
                    if (ezpd.Text.Substring(3, 1) == "×")
                    {
                        ezyzzvpj = "0";
                    }
                }
                #endregion
                #region 三轴评价
                string szzdpj = "";
                string szbphpj = "";
                string szzzzvpj = "";
                string szyzzvpj = "";
                if (szpd.Text != "" && szpd.Text != "-")
                {
                    if (szpd.Text.Substring(0, 1) == "○")
                    {
                        szzdpj = "1";
                    }
                    if (szpd.Text.Substring(0, 1) == "×")
                    {
                        szzdpj = "0";
                    }
                    if (szpd.Text.Substring(1, 1) == "○")
                    {
                        szbphpj = "1";
                    }
                    if (szpd.Text.Substring(1, 1) == "×")
                    {
                        szbphpj = "0";
                    }
                    if (szpd.Text.Substring(2, 1) == "○")
                    {
                        szzzzvpj = "1";
                    }
                    if (szpd.Text.Substring(2, 1) == "×")
                    {
                        szzzzvpj = "0";
                    }
                    if (szpd.Text.Substring(3, 1) == "○")
                    {
                        szyzzvpj = "1";
                    }
                    if (szpd.Text.Substring(3, 1) == "×")
                    {
                        szyzzvpj = "0";
                    }
                }
                #endregion
                #region 四轴评价
                string sizzdpj = "";
                string sizbphpj = "";
                string sizzzzvpj = "";
                string sizyzzvpj = "";
                if (sizpd.Text != "" && sizpd.Text != "-")
                {
                    if (sizpd.Text.Substring(0, 1) == "○")
                    {
                        sizzdpj = "1";
                    }
                    if (sizpd.Text.Substring(0, 1) == "×")
                    {
                        sizzdpj = "0";
                    }
                    if (sizpd.Text.Substring(1, 1) == "○")
                    {
                        sizbphpj = "1";
                    }
                    if (sizpd.Text.Substring(1, 1) == "×")
                    {
                        sizbphpj = "0";
                    }
                    if (sizpd.Text.Substring(2, 1) == "○")
                    {
                        sizzzzvpj = "1";
                    }
                    if (sizpd.Text.Substring(2, 1) == "×")
                    {
                        sizzzzvpj = "0";
                    }
                    if (sizpd.Text.Substring(3, 1) == "○")
                    {
                        sizyzzvpj = "1";
                    }
                    if (sizpd.Text.Substring(3, 1) == "×")
                    {
                        sizyzzvpj = "0";
                    }
                }
                #endregion
                #region 五轴评价
                string wzzdpj = "";
                string wzbphpj = "";
                string wzzzzvpj = "";
                string wzyzzvpj = "";
                if (wzpd.Text != "" && wzpd.Text != "-")
                {
                    if (wzpd.Text.Substring(0, 1) == "○")
                    {
                        wzzdpj = "1";
                    }
                    if (wzpd.Text.Substring(0, 1) == "×")
                    {
                        wzzdpj = "0";
                    }
                    if (wzpd.Text.Substring(1, 1) == "○")
                    {
                        wzbphpj = "1";
                    }
                    if (wzpd.Text.Substring(1, 1) == "×")
                    {
                        wzbphpj = "0";
                    }
                    if (wzpd.Text.Substring(2, 1) == "○")
                    {
                        wzzzzvpj = "1";
                    }
                    if (wzpd.Text.Substring(2, 1) == "×")
                    {
                        wzzzzvpj = "0";
                    }
                    if (wzpd.Text.Substring(3, 1) == "○")
                    {
                        wzyzzvpj = "1";
                    }
                    if (wzpd.Text.Substring(3, 1) == "×")
                    {
                        wzyzzvpj = "0";
                    }
                }
                #endregion
                #region 六轴评价
                string lzzdpj = "";
                string lzbphpj = "";
                string lzzzzvpj = "";
                string lzyzzvpj = "";
                if (lzpd.Text != "" && lzpd.Text != "-")
                {
                    if (lzpd.Text.Substring(0, 1) == "○")
                    {
                        lzzdpj = "1";
                    }
                    if (lzpd.Text.Substring(0, 1) == "×")
                    {
                        lzzdpj = "0";
                    }
                    if (lzpd.Text.Substring(1, 1) == "○")
                    {
                        lzbphpj = "1";
                    }
                    if (lzpd.Text.Substring(1, 1) == "×")
                    {
                        lzbphpj = "0";
                    }
                    if (lzpd.Text.Substring(2, 1) == "○")
                    {
                        lzzzzvpj = "1";
                    }
                    if (lzpd.Text.Substring(2, 1) == "×")
                    {
                        lzzzzvpj = "0";
                    }
                    if (lzpd.Text.Substring(3, 1) == "○")
                    {
                        lzyzzvpj = "1";
                    }
                    if (lzpd.Text.Substring(3, 1) == "×")
                    {
                        lzyzzvpj = "0";
                    }
                }
                #endregion
                #region 喇叭侧滑与车速
                string cspj = "";
                string lbpj = "";
                string chpj = "";
                string ch2pj = "";
                if (cspd.Text != "" && cspd.Text != "-")
                {
                    if (cspd.Text == "○")
                    {
                        cspj = "1";
                    }
                    if (cspd.Text == "×")
                    {
                        cspj = "0";
                    }
                }
                if (chpd.Text != "" && chpd.Text != "-")
                {
                    if (chpd.Text == "○")
                    {
                        chpj = "1";
                    }
                    if (chpd.Text == "×")
                    {
                        chpj = "0";
                    }
                }
                if (lbpd.Text != "" && lbpd.Text != "-")
                {
                    if (lbpd.Text == "○")
                    {
                        lbpj = "1";
                    }
                    if (lbpd.Text == "×")
                    {
                        lbpj = "0";
                    }
                }
                if (chpd2.Text != "" && chpd2.Text != "-")
                {
                    if (chpd2.Text == "○")
                    {
                        ch2pj = "1";
                    }
                    if (chpd2.Text == "×")
                    {
                        ch2pj = "0";
                    }
                }
                #endregion
                #region 左外灯光评价
                string zwgqpj=  "";
                string zwychpj = "";
                string zwjchpj = "";
                if (zwpd.Text != "" && zwpd.Text != "-")
                {
                    if (zwpd.Text.Substring(0, 1) == "○")
                    {
                        zwgqpj = "1";
                    }
                    if (zwpd.Text.Substring(0, 1) == "×")
                    {
                        zwgqpj = "0";
                    }
                    if (zwpd.Text.Substring(1, 1) == "○")
                    {
                        zwychpj = "1";
                    }
                    if (zwpd.Text.Substring(1, 1) == "×")
                    {
                        zwychpj = "0";
                    }
                    if (zwpd.Text.Substring(2, 1) == "○")
                    {
                        zwjchpj = "1";
                    }
                    if (zwpd.Text.Substring(2, 1) == "×")
                    {
                        zwjchpj = "0";
                    }
                }
                #endregion
                #region 左内灯光评价
                string zngqpj = "";
                string znychpj = "";
                if (znpd.Text != "" && znpd.Text != "-")
                {
                    if (znpd.Text.Substring(0, 1) == "○")
                    {
                        zngqpj = "1";
                    }
                    if (znpd.Text.Substring(0, 1) == "×")
                    {
                        zngqpj = "0";
                    }
                    if (znpd.Text.Substring(1, 1) == "○")
                    {
                        znychpj = "1";
                    }
                    if (znpd.Text.Substring(1, 1) == "×")
                    {
                        znychpj = "0";
                    }
                }
                #endregion
                #region 右内灯光评价
                string yngqpj = "";
                string ynychpj = "";
                if (ynpd.Text != "" && ynpd.Text != "-")
                {
                    if (ynpd.Text.Substring(0, 1) == "○")
                    {
                        yngqpj = "1";
                    }
                    if (ynpd.Text.Substring(0, 1) == "×")
                    {
                        yngqpj = "0";
                    }
                    if (ynpd.Text.Substring(1, 1) == "○")
                    {
                        ynychpj = "1";
                    }
                    if (ynpd.Text.Substring(1, 1) == "×")
                    {
                        ynychpj = "0";
                    }
                }
                #endregion
                #region 右外灯光评价
                string ywgqpj = "";
                string ywychpj = "";
                string ywjchpj = "";
                if (ywpd.Text != "" && ywpd.Text != "-")
                {
                    if (ywpd.Text.Substring(0, 1) == "○")
                    {
                        ywgqpj = "1";
                    }
                    if (ywpd.Text.Substring(0, 1) == "×")
                    {
                        ywgqpj = "0";
                    }
                    if (ywpd.Text.Substring(1, 1) == "○")
                    {
                        ywychpj = "1";
                    }
                    if (ywpd.Text.Substring(1, 1) == "×")
                    {
                        ywychpj = "0";
                    }
                    if (ywpd.Text.Substring(2, 1) == "○")
                    {
                        ywjchpj = "1";
                    }
                    if (ywpd.Text.Substring(2, 1) == "×")
                    {
                        ywjchpj = "0";
                    }
                }
                #endregion
                #region 悬架
                string xqhpj = "";
                string xqcpj = "";
                string xhhpj = "";
                string xhcpj = "";
                if (xjqzpd.Text != "" && xjqzpd.Text != "-")
                {
                    if (xjqzpd.Text.Substring(0, 1) == "○")
                    {
                        xqhpj = "1";
                    }
                    if (xjqzpd.Text.Substring(0, 1) == "×")
                    {
                        xqhpj = "0";
                    }
                    if (xjqzpd.Text.Substring(1, 1) == "○")
                    {
                        xqcpj = "1";
                    }
                    if (xjqzpd.Text.Substring(1, 1) == "×")
                    {
                        xqcpj = "0";
                    }
                }
                if (xjhzpd.Text != "" && xjhzpd.Text != "-")
                {
                    if (xjhzpd.Text.Substring(0, 1) == "○")
                    {
                        xhhpj = "1";
                    }
                    if (xjhzpd.Text.Substring(0, 1) == "×")
                    {
                        xhhpj = "0";
                    }
                    if (xjhzpd.Text.Substring(1, 1) == "○")
                    {
                        xhcpj = "1";
                    }
                    if (xjhzpd.Text.Substring(1, 1) == "×")
                    {
                        xhcpj = "0";
                    }
                }
                #endregion
                #region 排放性
                string gcopj = "";
                string ghcpj = "";
                string gλpj = "";
                string dcopj = "";
                string dhcpj = "";
                string gxspj = "";
                if (qypd.Text != "" && qypd.Text != "-")
                {
                    if (qypd.Text.Substring(0, 1) == "○")
                    {
                        gcopj = "1";
                    }
                    if (qypd.Text.Substring(0, 1) == "×")
                    {
                        gcopj = "0";
                    }
                    if (qypd.Text.Substring(1, 1) == "○")
                    {
                        ghcpj = "1";
                    }
                    if (qypd.Text.Substring(1, 1) == "×")
                    {
                        ghcpj = "0";
                    }
                    if (qypd.Text.Substring(2, 1) == "○")
                    {
                        gλpj = "1";
                    }
                    if (qypd.Text.Substring(2, 1) == "×")
                    {
                        gλpj = "0";
                    }
                    if (qypd.Text.Substring(3, 1) == "○")
                    {
                        dcopj = "1";
                    }
                    if (qypd.Text.Substring(3, 1) == "×")
                    {
                        dcopj = "0";
                    }
                    if (qypd.Text.Substring(4, 1) == "○")
                    {
                        dhcpj = "1";
                    }
                    if (qypd.Text.Substring(4, 1) == "×")
                    {
                        dhcpj = "0";
                    }
                }
                if (cypd.Text != "" && cypd.Text != "-")
                {
                    if (cypd.Text == "○")
                    {
                        gxspj = "1";
                    }
                    if (cypd.Text == "×")
                    {
                        gxspj = "0";
                    }
                }
                #endregion
                #region 拖滞力的判断
                string yztzl = "";
                string yyzzl = "";
                string eztzl = "";
                string eyzzl = "";
                string sztzl = "";
                string syzzl = "";
                string siztzl = "";
                string siyzzl = "";
                string wztzl = "";
                string wyzzl = "";
                string lztzl = "";
                string lyzzl = "";
                //判断一轴轴重不为空
                if (yzzz.Text != "" && yzzz.Text != "-")
                {
                    if (ydzzzzv.Text != "" && ydzzzzv.Text != "-")
                    {
                        yztzl = (Convert.ToDouble(ydzzzzv.Text) / 100 * (Convert.ToDouble(yzzz.Text) * 0.98)).ToString("0.0");//一轴左拖滞力值
                    }
                    if (ydzyzzv.Text != "" && ydzyzzv.Text != "-")
                    {
                        yyzzl = (Convert.ToDouble(ydzyzzv.Text) / 100 * (Convert.ToDouble(yzzz.Text) * 0.98)).ToString("0.0");//一轴右拖滞力值
                    }
                }
                //判断二轴轴重不为空
                if (ezzz.Text != "" && ezzz.Text != "-")
                {
                    if (edzzzzv.Text != "" && edzzzzv.Text != "-")
                    {
                        eztzl = (Convert.ToDouble(edzzzzv.Text) / 100 * (Convert.ToDouble(ezzz.Text) * 0.98)).ToString("0.0");//二轴左拖滞力值
                    }
                    if (edzyzzv.Text != "" && edzyzzv.Text != "-")
                    {
                        eyzzl = (Convert.ToDouble(edzyzzv.Text) / 100 * (Convert.ToDouble(ezzz.Text) * 0.98)).ToString("0.0");//二轴右拖滞力值
                    }
                }
                //判断三轴轴重不为空
                if (szzz.Text != "" && szzz.Text != "-")
                {
                    if (sdzzzzv.Text != "" && sdzzzzv.Text != "-")
                    {
                        sztzl = (Convert.ToDouble(sdzzzzv.Text) / 100 * (Convert.ToDouble(szzz.Text) * 0.98)).ToString("0.0");//三轴左拖滞力值
                    }
                    if (sdzyzzv.Text != "" && sdzyzzv.Text != "-")
                    {
                        syzzl = (Convert.ToDouble(sdzyzzv.Text) / 100 * (Convert.ToDouble(szzz.Text) * 0.98)).ToString("0.0");//三轴右拖滞力值
                    }
                }
                //判断四轴轴重不为空
                if (sizzz.Text != "" && sizzz.Text != "-")
                {
                    if (sidzzzzv.Text != "" && sidzzzzv.Text != "-")
                    {
                        siztzl = (Convert.ToDouble(sidzzzzv.Text) / 100 * (Convert.ToDouble(sizzz.Text) * 0.98)).ToString("0.0");//四轴左拖滞力值
                    }
                    if (sidzyzzv.Text != "" && sidzyzzv.Text != "-")
                    {
                        siyzzl = (Convert.ToDouble(sidzyzzv.Text) / 100 * (Convert.ToDouble(sizzz.Text) * 0.98)).ToString("0.0");//四轴右拖滞力值
                    }
                }
                //判断五轴轴重不为空
                if (wzzz.Text != "" && wzzz.Text != "-")
                {
                    if (wdzzzzv.Text != "" && wdzzzzv.Text != "-")
                    {
                        wztzl = (Convert.ToDouble(wdzzzzv.Text) / 100 * (Convert.ToDouble(wzzz.Text) * 0.98)).ToString("0.0");//五轴左拖滞力值
                    }
                    if (wdzyzzv.Text != "" && wdzyzzv.Text != "-")
                    {
                        wyzzl = (Convert.ToDouble(wdzyzzv.Text) / 100 * (Convert.ToDouble(wzzz.Text) * 0.98)).ToString("0.0");//五轴右拖滞力值
                    }
                }
                //判断六轴轴重不为空
                if (lzzz.Text != "" && lzzz.Text != "-")
                {
                    if (ldzzzzv.Text != "" && ldzzzzv.Text != "-")
                    {
                        lztzl = (Convert.ToDouble(ldzzzzv.Text) / 100 * (Convert.ToDouble(lzzz.Text) * 0.98)).ToString("0.0");//六轴左拖滞力值
                    }
                    if (ldzyzzv.Text != "" && ldzyzzv.Text != "-")
                    {
                        lyzzl = (Convert.ToDouble(ldzyzzv.Text) / 100 * (Convert.ToDouble(lzzz.Text) * 0.98)).ToString("0.0");//六轴右拖滞力值
                    }
                }
                #endregion
                conn.Open();
                string str = string.Format("update Data_Modification set 悬架前左吸收率值 ='{0}',悬架前右吸收率值 ='{1}',悬架后左吸收率值 ='{2}',悬架后右吸收率值 ='{3}',悬架前轴吸收率差值 ='{4}',悬架后轴吸收率差值 ='{5}',悬架前轴吸收率评价 ='{6}',悬架后轴吸收率评价 ='{7}',悬架前轴吸收率差评价 ='{8}',悬架后轴吸收率差评价 ='{9}' where 检测次数 = '{10}' and 检测编号 ='{11}' and 检测时间='{12}'", qzzxsl.Text, qzyxsl.Text, hzzxsl.Text, hzyxsl.Text, qzzyc.Text, hzzyc.Text, xqhpj, xhhpj, xqcpj, xhcpj,  dataGridView1.SelectedRows[0].Cells["检测次数"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["检测编号"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["检测时间"].Value.ToString());
                SqlCommand cmd = new SqlCommand(str, conn);
                int i = cmd.ExecuteNonQuery();
                string str5 = string.Format("update Data_Modification set 总质量='{0}',号牌种类='{1}',底盘号码='{2}',型号='{3}',发动机号码='{4}',出厂日期='{5}',车身颜色='{6}',车长='{7}',车宽='{8}',车高='{9}',车牌号码='{10}',登记日期='{11}',里程表读数='{12}',前轮距='{13}',座位数='{14}',远光光束单独调整='{15}',燃油类型='{16}',营运证号='{17}',车主单位='{18}',送检单位='{19}',新车='{20}' where 检测次数='{21}' and 检测编号='{22}' and 检测时间='{23}'", zzl.Text, hpzl.Text, vin.Text, clxh.Text, fdjhm.Text, ccrq.Text, csys.Text, kccc.Text, ck.Text, cg.Text,hphm.Text,djrq.Text,xslc.Text,qlj.Text,kczws.Text,qzdygsnfddtz.Text,ryxs.Text,yyzh.Text,syr.Text,sjdw.Text,ywlxs, dataGridView1.SelectedRows[0].Cells["检测次数"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["检测编号"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["检测时间"].Value.ToString());
                SqlCommand cmd5 = new SqlCommand(str5, conn);
                int h = cmd5.ExecuteNonQuery();
                #region 
                //string str1 = string.Format("update Data_Modification  set 底检值 ='{0}',外观值 ='{1}',百公里油耗评价 ='{2}',动力性加载力 ='{3}',额定功率工况评价 ='{4}',额定扭矩工况评价 ='{5}',百公里油耗值 ='{6}',整车轴重值 ='{7}',光吸收率值 ='{8}',轮边稳定车速 ='{9}',额定功率工况车速 ='{10}',额定扭矩工况车速 ='{11}',一轴左拖滞力值 ='{12}',一轴右拖滞力值 ='{13}',二轴左拖滞力值 ='{14}',二轴右拖滞力值 ='{15}',三轴左拖滞力值 ='{16}',三轴右拖滞力值 ='{17}',四轴左拖滞力值 ='{18}',四轴右拖滞力值 ='{19}',五轴左拖滞力值 ='{20}',五轴右拖滞力值 ='{21}',六轴左拖滞力值 ='{22}',六轴右拖滞力值 ='{23}',一轴手制动力左值 ='{24}',一轴手制动力右值 ='{25}',二轴手制动力左值 ='{26}',二轴手制动力右值 ='{27}',三轴手制动力左值 ='{28}',三轴手制动力右值 ='{29}',四轴手制动力左值 ='{30}',四轴手制动力右值 ='{31}',五轴手制动力左值 ='{32}',五轴手制动力右值 ='{33}',六轴手制动力左值 ='{34}',六轴手制动力右值 ='{35}',车速值 ='{36}',怠速CO值 ='{37}',怠速HC值 ='{38}',双怠速CO值 ='{39}',双怠速HC值 ='{40}',一轴求和时左制动力值 ='{41}',一轴求和时右制动力值 ='{42}',一轴求差时右制动力值 ='{43}',一轴求差时左制动力值 ='{44}',一轴制动和值 ='{45}',一轴制动差值 ='{46}',一轴左拖滞比值 ='{47}',一轴右拖滞比值 ='{48}',二轴求和时左制动力值 ='{49}',二轴求和时右制动力值 ='{50}',二轴求差时左制动力值 ='{51}',二轴求差时右制动力值 ='{52}',二轴制动和值 ='{53}',二轴制动差值 ='{54}',二轴左拖滞比值 ='{55}',二轴右拖滞比值 ='{56}',三轴求和时左制动力值 ='{57}',三轴求和时右制动力值 ='{58}',三轴求差时左制动力值 ='{59}',三轴求差时右制动力值 ='{60}',三轴制动和值 ='{61}',三轴制动差值 ='{62}',三轴左拖滞比值 ='{63}',三轴右拖滞比值 ='{64}',四轴求和时左制动力值 ='{65}',四轴求和时右制动力值 ='{66}',四轴求差时左制动力值 ='{67}',四轴求差时右制动力值 ='{68}',四轴制动和值 ='{69}',四轴制动差值 ='{70}',四轴左拖滞比值 ='{71}',四轴右拖滞比值 ='{72}',五轴求和时左制动力值 ='{73}',五轴求和时右制动力值 ='{74}',五轴求差时左制动力值 ='{75}',五轴求差时右制动力值 ='{76}',五轴制动和值 ='{77}',五轴制动差值 ='{78}',五轴左拖滞比值 ='{79}',五轴右拖滞比值 ='{80}',六轴求和时左制动力值 ='{81}',六轴求和时右制动力值 ='{82}',六轴求差时左制动力值 ='{83}',六轴求差时右制动力值 ='{84}',六轴制动和值 ='{85}',六轴制动差值 ='{86}',六轴左拖滞比值 ='{87}',六轴右拖滞比值 ='{88}',整车制动和值 ='{89}',手制动和值 ='{90}'," + "左灯高值 ='{91}',右灯高值 ='{92}',左主远光强度值 ='{93}',右主远光强度值 ='{94}',左副远光强度值 ='{95}',右副远光强度值 ='{96}',左主远光左右偏差值 ='{97}',左副远光左右偏差值 ='{98}',右副远光左右偏差值 ='{99}',右主远光左右偏差值 ='{100}',左近光左右偏差值 ='{101}',右近光左右偏差值 ='{102}',喇叭声级值 ='{103}',侧滑值 ='{104}',侧滑值2 ='{105}',光吸收率值1 ='{106}',光吸收率值2 ='{107}',光吸收率值3 ='{108}',空气过量系数值 ='{109}',左主远光上下偏差值 ='{110}',左副远光上下偏差值 ='{111}',右主远光上下偏差值 ='{112}',右副远光上下偏差值 ='{113}',左近光上下偏差值 ='{114}',右近光上下偏差值 ='{115}',整车制动和评价 ='{116}',手制动和评价 ='{117}',一轴制动和评价 ='{118}',一轴制动差评价 ='{119}',一轴左拖滞比评价 ='{120}',一轴右拖滞比评价 ='{121}',二轴制动和评价 ='{122}',二轴制动差评价 ='{123}',二轴左拖滞比评价 ='{124}',二轴右拖滞比评价 ='{125}',三轴制动和评价 ='{126}',三轴制动差评价 ='{127}',三轴左拖滞比评价 ='{128}',三轴右拖滞比评价 ='{129}',四轴制动和评价 ='{130}',四轴制动差评价 ='{131}',四轴左拖滞比评价 ='{132}',四轴右拖滞比评价 ='{133}',五轴制动和评价 ='{134}',五轴制动差评价 ='{135}',五轴左拖滞比评价 ='{136}',五轴右拖滞比评价 ='{137}',六轴制动和评价 ='{138}',六轴制动差评价 ='{139}',六轴左拖滞比评价 ='{140}',六轴右拖滞比评价 ='{141}',喇叭声级评价 ='{142}',侧滑评价 ='{143}',车速评价 ='{144}',怠速CO评价 ='{145}',怠速HC评价 ='{146}',双怠速CO评价 ='{147}',双怠速HC评价 ='{148}',空气过量系数评价 ='{149}',左主远光强度评价 ='{150}',左副远光强度评价 ='{151}',右副远光强度评价 ='{152}',右主远光强度评价 ='{153}',左主远光上下偏差评价 ='{154}',左副远光上下偏差评价 ='{155}',右副远光上下偏差评价 ='{156}',右主远光上下偏差评价 ='{157}',左近光上下偏差评价 ='{158}',右近光上下偏差评价 ='{159}',光吸收率评价 ='{160}',一轴复合轴重值='{161}',二轴复合轴重值='{162}',三轴复合轴重值='{163}',四轴复合轴重值='{164}',五轴复合轴重值='{165}',六轴复合轴重值='{166}',一轴左轴重值='{167}',一轴右轴重值='{168}',二轴左轴重值='{169}',二轴右轴重值='{170}',三轴左轴重值='{171}',三轴右轴重值='{172}',四轴左轴重值='{173}',四轴右轴重值='{174}',五轴左轴重值='{175}',五轴右轴重值='{176}',左近灯高值='{177}',右近灯高值='{178}' where 检测次数 ='{179}' and 检测编号 ='{180}' and 检测时间='{181}'", djjc.Text, wgjc.Text, yhpj, jzl.Text, glgkpj, njgkpj, yhscz.Text, dcspcz.Text, cygxsavg.Text, wdcs.Text, sedglcs, sednjcs, yztzl, yyzzl, eztzl, eyzzl, sztzl, syzzl, siztzl, siyzzl, wztzl, wyzzl, lztzl, lyzzl, yzzzczd.Text, yzyzczd.Text, ezzzczd.Text, ezyzczd.Text, szzzczd.Text, szyzczd.Text, sizzzczd.Text, sizyzczd.Text, wzzzczd.Text, wzyzczd.Text, lzzzczd.Text, lzyzczd.Text, csb.Text, qyddsCO.Text, qyddsHC.Text, qygdsCO.Text, qygdsHC.Text, yzzxczd.Text, yzyxczd.Text, ydzygcc.Text, ydzzgcc.Text, ydzzdv.Text, ydzbphv.Text, ydzzzzv.Text, ydzyzzv.Text, ezzxczd.Text, ezyxczd.Text, edzzgcc.Text, edzygcc.Text, edzzdv.Text, edzbphv.Text, edzzzzv.Text, edzyzzv.Text, szzxczd.Text, szyxczd.Text, sdzzgcc.Text, sdzygcc.Text, sdzzdv.Text, sdzbphv.Text, sdzzzzv.Text, sdzyzzv.Text, sizzxczd.Text, sizyxczd.Text, sidzzgcc.Text, sidzygcc.Text, sidzzdv.Text, sidzbphv.Text, sidzzzzv.Text, sidzyzzv.Text, wzzxczd.Text, wzyxczd.Text, wdzzgcc.Text, wdzygcc.Text, wdzzdv.Text, wdzbphv.Text, wdzzzzv.Text, wdzyzzv.Text, lzzxczd.Text, lzyxczd.Text, ldzzgcc.Text, ldzygcc.Text, ldzzdv.Text, ldzbphv.Text, ldzzzzv.Text, ldzyzzv.Text, dczdl.Text, dczczdl.Text, zwygdg.Text, ywygdg.Text, zwyggq.Text, ywyggq.Text, znyggq.Text, ynyggq.Text, zwygsp.Text, znygsp.Text, ynygsp.Text, ywygsp.Text, zwjgsp.Text, ywjgsp.Text, lbsjz.Text, dychl.Text, dechl.Text, cygxs1.Text, cygxs2.Text, cygxs3.Text, qygdsλ.Text, dzwyc, dznyc, dywyc, dynyc, zjc, yjc, zczdhpj, szdhpj, yzzdpj, yzbphpj, yzzzzvpj, yzyzzvpj, ezzdpj, ezbphpj, ezzzzvpj, ezyzzvpj, szzdpj, szbphpj, szzzzvpj, szyzzvpj, sizzdpj, sizbphpj, sizzzzvpj, sizyzzvpj, wzzdpj, wzbphpj, wzzzzvpj, wzyzzvpj, lzzdpj, lzbphpj, lzzzzvpj, lzyzzvpj, lbsjz.Text, chpj, cspj, dcopj, dhcpj, gcopj, ghcpj, gλpj, zwgqpj, zngqpj, yngqpj, ywgqpj, zwychpj, znychpj, ynychpj, ywychpj, zwjchpj, ywjchpj, gxspj, yzzz.Text, ezzz.Text, szzz.Text, sizzz.Text, wzzz.Text, lzzz.Text, yzzlh.Text, yzylh.Text, ezzlh.Text, ezylh.Text, szzlh.Text, szylh.Text, sizzlh.Text, sizylh.Text, wzzlh.Text, wzylh.Text, zwjgdg.Text, ywjgdg.Text,dataGridView1.SelectedRows[0].Cells["检测次数"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["检测编号"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["检测时间"].Value.ToString());
                #endregion
                #region
                string str1 = string.Format("update Data_Modification  set 底检值 ='{0}',外观值 ='{1}',百公里油耗评价 ='{2}',动力性加载力 ='{3}',额定功率工况评价 ='{4}',额定扭矩工况评价 ='{5}',百公里油耗值 ='{6}',整车轴重值 ='{7}',光吸收率值 ='{8}',轮边稳定车速 ='{9}',额定功率工况车速 ='{10}',额定扭矩工况车速 ='{11}',一轴左拖滞力值 ='{12}',一轴右拖滞力值 ='{13}',二轴左拖滞力值 ='{14}',二轴右拖滞力值 ='{15}',三轴左拖滞力值 ='{16}',三轴右拖滞力值 ='{17}',四轴左拖滞力值 ='{18}',四轴右拖滞力值 ='{19}',五轴左拖滞力值 ='{20}',五轴右拖滞力值 ='{21}',六轴左拖滞力值 ='{22}',六轴右拖滞力值 ='{23}',手制动力左值 ='{24}',手制动力右值 ='{25}',车速值 ='{26}',怠速CO值 ='{27}',怠速HC值 ='{28}',双怠速CO值 ='{29}',双怠速HC值 ='{30}',一轴求和时左制动力值 ='{31}',一轴求和时右制动力值 ='{32}',一轴求差时右制动力值 ='{33}',一轴求差时左制动力值 ='{34}',一轴制动和值 ='{35}',一轴制动差值 ='{36}',一轴左拖滞比值 ='{37}',一轴右拖滞比值 ='{38}',二轴求和时左制动力值 ='{39}',二轴求和时右制动力值 ='{40}',二轴求差时左制动力值 ='{41}',二轴求差时右制动力值 ='{42}',二轴制动和值 ='{43}',二轴制动差值 ='{44}',二轴左拖滞比值 ='{45}',二轴右拖滞比值 ='{46}',三轴求和时左制动力值 ='{47}',三轴求和时右制动力值 ='{48}',三轴求差时左制动力值 ='{49}',三轴求差时右制动力值 ='{50}',三轴制动和值 ='{51}',三轴制动差值 ='{52}',三轴左拖滞比值 ='{53}',三轴右拖滞比值 ='{54}',四轴求和时左制动力值 ='{55}',四轴求和时右制动力值 ='{56}',四轴求差时左制动力值 ='{57}',四轴求差时右制动力值 ='{58}',四轴制动和值 ='{59}',四轴制动差值 ='{60}',四轴左拖滞比值 ='{61}',四轴右拖滞比值 ='{62}',五轴求和时左制动力值 ='{63}',五轴求和时右制动力值 ='{64}',五轴求差时左制动力值 ='{65}',五轴求差时右制动力值 ='{66}',五轴制动和值 ='{67}',五轴制动差值 ='{68}',五轴左拖滞比值 ='{69}',五轴右拖滞比值 ='{70}',六轴求和时左制动力值 ='{71}',六轴求和时右制动力值 ='{72}',六轴求差时左制动力值 ='{73}',六轴求差时右制动力值 ='{74}',六轴制动和值 ='{75}',六轴制动差值 ='{76}',六轴左拖滞比值 ='{77}',六轴右拖滞比值 ='{78}',整车制动和值 ='{79}',手制动和值 ='{80}'," + "左灯高值 ='{81}',右灯高值 ='{82}',左主远光强度值 ='{83}',右主远光强度值 ='{84}',左副远光强度值 ='{85}',右副远光强度值 ='{86}',左主远光左右偏差值 ='{87}',左副远光左右偏差值 ='{88}',右副远光左右偏差值 ='{89}',右主远光左右偏差值 ='{90}',左近光左右偏差值 ='{91}',右近光左右偏差值 ='{92}',喇叭声级值 ='{93}',侧滑值 ='{94}',侧滑值2 ='{95}',光吸收率值1 ='{96}',光吸收率值2 ='{97}',光吸收率值3 ='{98}',空气过量系数值 ='{99}',左主远光上下偏差值 ='{100}',左副远光上下偏差值 ='{101}',右主远光上下偏差值 ='{102}',右副远光上下偏差值 ='{103}',左近光上下偏差值 ='{104}',右近光上下偏差值 ='{105}',整车制动和评价 ='{106}',手制动和评价 ='{107}',一轴制动和评价 ='{108}',一轴制动差评价 ='{109}',一轴左拖滞比评价 ='{110}',一轴右拖滞比评价 ='{111}',二轴制动和评价 ='{112}',二轴制动差评价 ='{113}',二轴左拖滞比评价 ='{114}',二轴右拖滞比评价 ='{115}',三轴制动和评价 ='{116}',三轴制动差评价 ='{117}',三轴左拖滞比评价 ='{118}',三轴右拖滞比评价 ='{119}',四轴制动和评价 ='{120}',四轴制动差评价 ='{121}',四轴左拖滞比评价 ='{122}',四轴右拖滞比评价 ='{123}',五轴制动和评价 ='{124}',五轴制动差评价 ='{125}',五轴左拖滞比评价 ='{126}',五轴右拖滞比评价 ='{127}',六轴制动和评价 ='{128}',六轴制动差评价 ='{129}',六轴左拖滞比评价 ='{130}',六轴右拖滞比评价 ='{131}',喇叭声级评价 ='{132}',侧滑评价 ='{133}',车速评价 ='{134}',怠速CO评价 ='{135}',怠速HC评价 ='{136}',双怠速CO评价 ='{137}',双怠速HC评价 ='{138}',空气过量系数评价 ='{139}',左主远光强度评价 ='{140}',左副远光强度评价 ='{141}',右副远光强度评价 ='{142}',右主远光强度评价 ='{143}',左主远光上下偏差评价 ='{144}',左副远光上下偏差评价 ='{145}',右副远光上下偏差评价 ='{146}',右主远光上下偏差评价 ='{147}',左近光上下偏差评价 ='{148}',右近光上下偏差评价 ='{149}',光吸收率评价 ='{150}',一轴轴重值='{151}',二轴轴重值='{152}',三轴轴重值='{153}',四轴轴重值='{154}',五轴轴重值='{155}',六轴轴重值='{156}',一轴左轴重值='{157}',一轴右轴重值='{158}',二轴左轴重值='{159}',二轴右轴重值='{160}',三轴左轴重值='{161}',三轴右轴重值='{162}',四轴左轴重值='{163}',四轴右轴重值='{164}',五轴左轴重值='{165}',五轴右轴重值='{166}' where 检测次数 ='{167}' and 检测编号 ='{168}' and 检测时间='{169}'", djjc.Text, wgjc.Text, yhpj, jzl.Text, glgkpj, njgkpj, yhscz.Text, dcspcz.Text, cygxsavg.Text, wdcs.Text, sedglcs, sednjcs, yztzl, yyzzl, eztzl, eyzzl, sztzl, syzzl, siztzl, siyzzl, wztzl, wyzzl, lztzl, lyzzl, zczdlz, zczdly, csb.Text, qyddsCO.Text, qyddsHC.Text, qygdsCO.Text, qygdsHC.Text, yzzxczd.Text, yzyxczd.Text, ydzygcc.Text, ydzzgcc.Text, ydzzdv.Text, ydzbphv.Text, ydzzzzv.Text, ydzyzzv.Text, ezzxczd.Text, ezyxczd.Text, edzzgcc.Text, edzygcc.Text, edzzdv.Text, edzbphv.Text, edzzzzv.Text, edzyzzv.Text, szzxczd.Text, szyxczd.Text, sdzzgcc.Text, sdzygcc.Text, sdzzdv.Text, sdzbphv.Text, sdzzzzv.Text, sdzyzzv.Text, sizzxczd.Text, sizyxczd.Text, sidzzgcc.Text, sidzygcc.Text, sidzzdv.Text, sidzbphv.Text, sidzzzzv.Text, sidzyzzv.Text, wzzxczd.Text, wzyxczd.Text, wdzzgcc.Text, wdzygcc.Text, wdzzdv.Text, wdzbphv.Text, wdzzzzv.Text, wdzyzzv.Text, lzzxczd.Text, lzyxczd.Text, ldzzgcc.Text, ldzygcc.Text, ldzzdv.Text, ldzbphv.Text, ldzzzzv.Text, ldzyzzv.Text, dczdl.Text, dczczdl.Text, zwygdg.Text, ywygdg.Text, zwyggq.Text, ywyggq.Text, znyggq.Text, ynyggq.Text, zwygsp.Text, znygsp.Text, ynygsp.Text, ywygsp.Text, zwjgsp.Text, ywjgsp.Text, lbsjz.Text, dychl.Text, dechl.Text, cygxs1.Text, cygxs2.Text, cygxs3.Text, qygdsλ.Text, dzwyc, dznyc, dywyc, dynyc, zjc, yjc, zczdhpj, szdhpj, yzzdpj, yzbphpj, yzzzzvpj, yzyzzvpj, ezzdpj, ezbphpj, ezzzzvpj, ezyzzvpj, szzdpj, szbphpj, szzzzvpj, szyzzvpj, sizzdpj, sizbphpj, sizzzzvpj, sizyzzvpj, wzzdpj, wzbphpj, wzzzzvpj, wzyzzvpj, lzzdpj, lzbphpj, lzzzzvpj, lzyzzvpj, lbsjz.Text, chpj, cspj, dcopj, dhcpj, gcopj, ghcpj, gλpj, zwgqpj, zngqpj, yngqpj, ywgqpj, zwychpj, znychpj, ynychpj, ywychpj, zwjchpj, ywjchpj, gxspj, yzzz.Text, ezzz.Text, szzz.Text, sizzz.Text, wzzz.Text, lzzz.Text, yzzlh.Text, yzylh.Text, ezzlh.Text, ezylh.Text, szzlh.Text, szylh.Text, sizzlh.Text, sizylh.Text, wzzlh.Text, wzylh.Text, dataGridView1.SelectedRows[0].Cells["检测次数"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["检测编号"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["检测时间"].Value.ToString());
                #endregion
                SqlCommand cmd1 = new SqlCommand(str1, conn);
                int n = cmd1.ExecuteNonQuery();
                string str2 = string.Format("update Data_Modification set 检测类别 ='{0}',检测日期 ='{1}' where 检测次数 ='{2}' and 检测编号 ='{3}' and 检测时间='{4}'", jylb.Text, jyrq.Text, dataGridView1.SelectedRows[0].Cells["检测次数"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["检测编号"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["检测时间"].Value.ToString());
                SqlCommand cmd2 = new SqlCommand(str2, conn);
                int m = cmd2.ExecuteNonQuery();
                conn.Close();
                if(i>0&&m>0&&n>0&&h>0)
                {
                    //MessageBox.Show("成功");
                }
                else
                {
                    MessageBox.Show("失败");
                }
                conn.Open();
                string str3 = string.Format("select * from Data_Modification where 检测次数 ='{0}' and 检测编号 ='{1}'", dataGridView1.SelectedRows[0].Cells["检测次数"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["检测编号"].Value.ToString());
                SqlDataAdapter sda = new SqlDataAdapter(str3, conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                conn.Close();
                if (dt.Columns.Contains("侧滑2评价"))
                {
                    conn.Open();
                    string str4 = string.Format("update Data_Modification set 侧滑2评价 ='{0}' where 检测次数 ='{1}' and 检测编号 ='{2}' and 检测时间='{3}'", ch2pj, dataGridView1.SelectedRows[0].Cells["检测次数"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["检测编号"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["检测时间"].Value.ToString());
                    SqlCommand cmd3 = new SqlCommand(str4, conn);
                    int u = cmd3.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.ToString());
            }
        }
       
        //修改密码
        private void button3_Click(object sender, EventArgs e)
        {
            
            Registe rg = new Registe();
            rg.Show();
            this.Hide();
        }
        public static object TextIsnulls(string str)
        {
            if (str == ""||str=="-")
            {
                str = "0";
            }
            return str;
        }
        #region 计算轴制动率与制动不平衡率
        //一轴左行车制动力发生改变
        private void yzzxczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //一轴制动率
                if(Convert.ToDouble(TextIsnulls(dczs.Text))>=3)
                {
                    ydzzdv.Text = ((Convert.ToDouble(TextIsnulls(yzzxczd.Text)) + Convert.ToDouble(TextIsnulls(yzyxczd.Text))) / Convert.ToDouble(TextIsnulls((yzzz.Text))) * 100).ToString("0.0");
                }
               else
                {
                    ydzzdv.Text = ((Convert.ToDouble(TextIsnulls(yzzxczd.Text)) + Convert.ToDouble(TextIsnulls(yzyxczd.Text))) / (Convert.ToDouble(TextIsnulls((yzzlh.Text)))+Convert.ToDouble(TextIsnulls(yzylh.Text))) * 100).ToString("0.0");
                }
                if (dcspcz.Text != "" && dcspcz.Text != "-")
                {
                    //整车制动率
                    dczdl.Text = ((Convert.ToDouble(TextIsnulls(yzzxczd.Text)) + Convert.ToDouble(TextIsnulls(yzyxczd.Text)) + Convert.ToDouble(TextIsnulls(ezzxczd.Text)) + Convert.ToDouble(TextIsnulls(ezyxczd.Text)) + Convert.ToDouble(TextIsnulls(szzxczd.Text)) + Convert.ToDouble(TextIsnulls(szyxczd.Text)) + Convert.ToDouble(TextIsnulls(sizzxczd.Text)) + Convert.ToDouble(TextIsnulls(sizyxczd.Text)) + Convert.ToDouble(TextIsnulls(wzzxczd.Text)) + Convert.ToDouble(TextIsnulls(wzyxczd.Text)) + Convert.ToDouble(TextIsnulls(lzzxczd.Text)) + Convert.ToDouble(TextIsnulls(lzyxczd.Text))) / Convert.ToDouble(TextIsnulls(dcspcz.Text))* 100).ToString("0.0");
                }
                if (yzzxczd.Text != "" && yzzxczd.Text != "-" && yzzxczd.Text != "0" && yzyxczd.Text != "" && yzyxczd.Text != "-" && yzyxczd.Text != "0" && ydzzgcc.Text != "" && ydzzgcc.Text != "-" && ydzygcc.Text != "" && ydzygcc.Text != "-")
                {
                    double gcczc = System.Math.Abs(Convert.ToDouble(ydzzgcc.Text) - Convert.ToDouble(ydzygcc.Text));//过程差最大点之差
                    if (Convert.ToDouble(yzzxczd.Text) >= Convert.ToDouble(yzyxczd.Text))
                    {
                        ydzbphv.Text = (gcczc / Convert.ToDouble(yzzxczd.Text) * 100).ToString("0.0");//一轴不平衡率
                    }
                    else
                    {
                        ydzbphv.Text = (gcczc / Convert.ToDouble(yzyxczd.Text) * 100).ToString("0.0");//一轴不平衡率
                    }
                }
                M1judge();
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //一轴右行车制动力发生改变
        private void yzyxczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //一轴制动率
                if (Convert.ToDouble(TextIsnulls(dczs.Text))>=3)
                {
                    ydzzdv.Text = ((Convert.ToDouble(TextIsnulls(yzzxczd.Text)) + Convert.ToDouble(TextIsnulls(yzyxczd.Text))) / Convert.ToDouble(TextIsnulls((yzzz.Text))) * 100).ToString("0.0");
                }
                else
                {
                    ydzzdv.Text = ((Convert.ToDouble(TextIsnulls(yzzxczd.Text)) + Convert.ToDouble(TextIsnulls(yzyxczd.Text))) / (Convert.ToDouble(TextIsnulls((yzzlh.Text))) + Convert.ToDouble(TextIsnulls(yzylh.Text))) * 100).ToString("0.0");
                }
                if (dcspcz.Text != "" && dcspcz.Text != "-")
                {
                    //整车制动率
                    dczdl.Text = ((Convert.ToDouble(TextIsnulls(yzzxczd.Text)) + Convert.ToDouble(TextIsnulls(yzyxczd.Text)) + Convert.ToDouble(TextIsnulls(ezzxczd.Text)) + Convert.ToDouble(TextIsnulls(ezyxczd.Text)) + Convert.ToDouble(TextIsnulls(szzxczd.Text)) + Convert.ToDouble(TextIsnulls(szyxczd.Text)) + Convert.ToDouble(TextIsnulls(sizzxczd.Text)) + Convert.ToDouble(TextIsnulls(sizyxczd.Text)) + Convert.ToDouble(TextIsnulls(wzzxczd.Text)) + Convert.ToDouble(TextIsnulls(wzyxczd.Text)) + Convert.ToDouble(TextIsnulls(lzzxczd.Text)) + Convert.ToDouble(TextIsnulls(lzyxczd.Text))) / Convert.ToDouble(TextIsnulls(dcspcz.Text)) * 100).ToString("0.0");
                }
                if (yzzxczd.Text != "" && yzzxczd.Text != "-" && yzzxczd.Text != "0" && yzyxczd.Text != "" && yzyxczd.Text != "-" && yzyxczd.Text != "0" && ydzzgcc.Text != "" && ydzzgcc.Text != "-" && ydzygcc.Text != "" && ydzygcc.Text != "-")
                {
                    double gcczc = System.Math.Abs(Convert.ToDouble(ydzzgcc.Text) - Convert.ToDouble(ydzygcc.Text));//过程差最大点之差
                    if (Convert.ToDouble(yzzxczd.Text) >= Convert.ToDouble(yzyxczd.Text))
                    {
                        ydzbphv.Text = (gcczc / Convert.ToDouble(yzzxczd.Text) * 100).ToString("0.0");//一轴不平衡率
                    }
                    else
                    {
                        ydzbphv.Text = (gcczc / Convert.ToDouble(yzyxczd.Text) * 100).ToString("0.0");//一轴不平衡率
                    }
                }
                M1judge();
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //二轴左行车制动力发生改变
        private void ezzxczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //二轴制动率
                if (Convert.ToDouble(TextIsnulls(dczs.Text))>=3)
                {
                    edzzdv.Text = ((Convert.ToDouble(TextIsnulls(ezzxczd.Text)) + Convert.ToDouble(TextIsnulls(ezyxczd.Text))) / Convert.ToDouble(TextIsnulls(ezzz.Text)) * 100).ToString("0.0");
                }
                else
                {
                    edzzdv.Text = ((Convert.ToDouble(TextIsnulls(ezzxczd.Text)) + Convert.ToDouble(TextIsnulls(ezyxczd.Text))) / (Convert.ToDouble(TextIsnulls(ezzlh.Text))+Convert.ToDouble(TextIsnulls(ezylh.Text))) * 100).ToString("0.0");
                }
                if (dcspcz.Text != "" && dcspcz.Text != "-")
                {
                    //整车制动率
                    dczdl.Text = ((Convert.ToDouble(TextIsnulls(yzzxczd.Text)) + Convert.ToDouble(TextIsnulls(yzyxczd.Text)) + Convert.ToDouble(TextIsnulls(ezzxczd.Text)) + Convert.ToDouble(TextIsnulls(ezyxczd.Text)) + Convert.ToDouble(TextIsnulls(szzxczd.Text)) + Convert.ToDouble(TextIsnulls(szyxczd.Text)) + Convert.ToDouble(TextIsnulls(sizzxczd.Text)) + Convert.ToDouble(TextIsnulls(sizyxczd.Text)) + Convert.ToDouble(TextIsnulls(wzzxczd.Text)) + Convert.ToDouble(TextIsnulls(wzyxczd.Text)) + Convert.ToDouble(TextIsnulls(lzzxczd.Text)) + Convert.ToDouble(TextIsnulls(lzyxczd.Text))) / Convert.ToDouble(TextIsnulls(dcspcz.Text))* 100).ToString("0.0");
                }
                if (zxzs.Text == "2")
                {
                    if (ezzxczd.Text != "" && ezyxczd.Text != "" && ezzxczd.Text != "-" && ezyxczd.Text != "-" && ezzxczd.Text != "0" && ezyxczd.Text != "0" && edzzgcc.Text != "" && edzzgcc.Text != "-" && edzygcc.Text != "" && edzygcc.Text != "-")
                    {
                        double gcczc = System.Math.Abs(Convert.ToDouble(edzzgcc.Text) - Convert.ToDouble(edzygcc.Text));//过程差最大点之差
                        if (Convert.ToDouble(ezzxczd.Text) >= Convert.ToDouble(ezyxczd.Text))
                        {
                            edzbphv.Text = (gcczc / Convert.ToDouble(ezzxczd.Text) * 100).ToString("0.0");//二轴不平衡率
                        }
                        else
                        {
                            edzbphv.Text = (gcczc / Convert.ToDouble(ezyxczd.Text) * 100).ToString("0.0");//二轴不平衡率
                        }
                    }
                }
                else
                {
                    if (ezzxczd.Text != "" && ezyxczd.Text != "" && ezzxczd.Text != "-" && ezyxczd.Text != "-" && ezzxczd.Text != "0" && ezyxczd.Text != "0" && edzzgcc.Text != "" && edzzgcc.Text != "-" && edzygcc.Text != "" && edzygcc.Text != "-")
                    {
                        double gcczc = System.Math.Abs(Convert.ToDouble(edzzgcc.Text) - Convert.ToDouble(edzygcc.Text));//过程差最大点之差
                        if (edzzdv.Text != "" && edzzdv.Text != "-")
                        {
                            //二轴制动率不小于60%
                            if (Convert.ToDouble(edzzdv.Text) >= 60)
                            {
                                if (Convert.ToDouble(ezzxczd.Text) >= Convert.ToDouble(ezyxczd.Text))
                                {
                                    edzbphv.Text = (gcczc / Convert.ToDouble(ezzxczd.Text) * 100).ToString("0.0");//二轴不平衡率
                                }
                                else
                                {
                                    edzbphv.Text = (gcczc / Convert.ToDouble(ezyxczd.Text) * 100).ToString("0.0");//二轴不平衡率
                                }
                            }
                            else
                            {
                                if (ezzlh.Text != "" && ezzlh.Text != "-" && ezzlh.Text != "0" && ezylh.Text != "" && ezylh.Text != "-" && ezylh.Text != "0")
                                {
                                    edzbphv.Text = (gcczc / (Convert.ToDouble(ezzlh.Text) + Convert.ToDouble(ezylh.Text)) * 100).ToString("0.0");//二轴不平衡率
                                }
                            }
                        }
                    }
                }
                M1judge();
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //二轴右行车制动力发生改变
        private void ezyxczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //二轴制动率
                if (Convert.ToDouble(TextIsnulls(dczs.Text)) >= 3)
                {
                    edzzdv.Text = ((Convert.ToDouble(TextIsnulls(ezzxczd.Text)) + Convert.ToDouble(TextIsnulls(ezyxczd.Text))) / Convert.ToDouble(TextIsnulls(ezzz.Text)) * 100).ToString("0.0");
                }
                else
                {
                    edzzdv.Text = ((Convert.ToDouble(TextIsnulls(ezzxczd.Text)) + Convert.ToDouble(TextIsnulls(ezyxczd.Text))) / (Convert.ToDouble(TextIsnulls(ezzlh.Text)) + Convert.ToDouble(TextIsnulls(ezylh.Text))) * 100).ToString("0.0");
                }
                if (dcspcz.Text != "" && dcspcz.Text != "-")
                {
                    //整车制动率
                    dczdl.Text = ((Convert.ToDouble(TextIsnulls(yzzxczd.Text)) + Convert.ToDouble(TextIsnulls(yzyxczd.Text)) + Convert.ToDouble(TextIsnulls(ezzxczd.Text)) + Convert.ToDouble(TextIsnulls(ezyxczd.Text)) + Convert.ToDouble(TextIsnulls(szzxczd.Text)) + Convert.ToDouble(TextIsnulls(szyxczd.Text)) + Convert.ToDouble(TextIsnulls(sizzxczd.Text)) + Convert.ToDouble(TextIsnulls(sizyxczd.Text)) + Convert.ToDouble(TextIsnulls(wzzxczd.Text)) + Convert.ToDouble(TextIsnulls(wzyxczd.Text)) + Convert.ToDouble(TextIsnulls(lzzxczd.Text)) + Convert.ToDouble(TextIsnulls(lzyxczd.Text))) / Convert.ToDouble(TextIsnulls(dcspcz.Text)) * 100).ToString("0.0");
                }
                if (zxzs.Text == "2")
                {
                    if (ezzxczd.Text != "" && ezyxczd.Text != "" && ezzxczd.Text != "-" && ezyxczd.Text != "-" && ezzxczd.Text != "0" && ezyxczd.Text != "0" && edzzgcc.Text != "" && edzzgcc.Text != "-" && edzygcc.Text != "" && edzygcc.Text != "-")
                    {
                        double gcczc = System.Math.Abs(Convert.ToDouble(edzzgcc.Text) - Convert.ToDouble(edzygcc.Text));//过程差最大点之差
                        if (Convert.ToDouble(ezzxczd.Text) >= Convert.ToDouble(ezyxczd.Text))
                        {
                            edzbphv.Text = (gcczc / Convert.ToDouble(ezzxczd.Text) * 100).ToString("0.0");//二轴不平衡率
                        }
                        else
                        {
                            edzbphv.Text = (gcczc / Convert.ToDouble(ezyxczd.Text) * 100).ToString("0.0");//二轴不平衡率
                        }
                    }
                }
                else
                {
                    if (ezzxczd.Text != "" && ezyxczd.Text != "" && ezzxczd.Text != "-" && ezyxczd.Text != "-" && ezzxczd.Text != "0" && ezyxczd.Text != "0" && edzzgcc.Text != "" && edzzgcc.Text != "-" && edzygcc.Text != "" && edzygcc.Text != "-")
                    {
                        double gcczc = System.Math.Abs(Convert.ToDouble(edzzgcc.Text) - Convert.ToDouble(edzygcc.Text));//过程差最大点之差
                        if (edzzdv.Text != "" && edzzdv.Text != "-")
                        {
                            //二轴制动率不小于60%
                            if (Convert.ToDouble(edzzdv.Text) >= 60)
                            {
                                if (Convert.ToDouble(ezzxczd.Text) >= Convert.ToDouble(ezyxczd.Text))
                                {
                                    edzbphv.Text = (gcczc / Convert.ToDouble(ezzxczd.Text) * 100).ToString("0.0");//二轴不平衡率
                                }
                                else
                                {
                                    edzbphv.Text = (gcczc / Convert.ToDouble(ezyxczd.Text) * 100).ToString("0.0");//二轴不平衡率
                                }
                            }
                            else
                            {
                                if (ezzlh.Text != "" && ezzlh.Text != "-" && ezzlh.Text != "0" && ezylh.Text != "" && ezylh.Text != "-" && ezylh.Text != "0")
                                {
                                    edzbphv.Text = (gcczc / (Convert.ToDouble(ezzlh.Text) + Convert.ToDouble(ezylh.Text)) * 100).ToString("0.0");//二轴不平衡率
                                }
                            }
                        }
                    }
                }
                M1judge();
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //三轴左行车制动力发生改变
        private void szzxczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //三轴制动率
                if(Convert.ToDouble(TextIsnulls(dczs.Text))>=3)
                {
                    sdzzdv.Text = ((Convert.ToDouble(TextIsnulls(szzxczd.Text)) + Convert.ToDouble(TextIsnulls(szyxczd.Text))) / Convert.ToDouble(TextIsnulls(szzz.Text)) * 100).ToString("0.0");
                }
               else
                {
                    sdzzdv.Text = ((Convert.ToDouble(TextIsnulls(szzxczd.Text)) + Convert.ToDouble(TextIsnulls(szyxczd.Text))) / (Convert.ToDouble(TextIsnulls(szzlh.Text))+Convert.ToDouble(TextIsnulls(szylh.Text))) * 100).ToString("0.0");
                }
                if (dcspcz.Text != "" && dcspcz.Text != "-")
                {
                    //整车制动率
                    dczdl.Text = ((Convert.ToDouble(TextIsnulls(yzzxczd.Text)) + Convert.ToDouble(TextIsnulls(yzyxczd.Text)) + Convert.ToDouble(TextIsnulls(ezzxczd.Text)) + Convert.ToDouble(TextIsnulls(ezyxczd.Text)) + Convert.ToDouble(TextIsnulls(szzxczd.Text)) + Convert.ToDouble(TextIsnulls(szyxczd.Text)) + Convert.ToDouble(TextIsnulls(sizzxczd.Text)) + Convert.ToDouble(TextIsnulls(sizyxczd.Text)) + Convert.ToDouble(TextIsnulls(wzzxczd.Text)) + Convert.ToDouble(TextIsnulls(wzyxczd.Text)) + Convert.ToDouble(TextIsnulls(lzzxczd.Text)) + Convert.ToDouble(TextIsnulls(lzyxczd.Text))) / Convert.ToDouble(TextIsnulls(dcspcz.Text)) * 100).ToString("0.0");
                }
                if (szzxczd.Text != "" && szyxczd.Text != "" && szzxczd.Text != "-" && szyxczd.Text != "-" && szzxczd.Text != "0" && szyxczd.Text != "0" && sdzzgcc.Text != "" && sdzzgcc.Text != "-" && sdzygcc.Text != "" && sdzygcc.Text != "-")
                {
                    double gcczc = System.Math.Abs(Convert.ToDouble(sdzzgcc.Text) - Convert.ToDouble(sdzygcc.Text));//过程差最大点之差
                    if (sdzzdv.Text != "" && sdzzdv.Text != "-")
                    {
                        //三轴制动率不小于60%
                        if (Convert.ToDouble(sdzzdv.Text) >= 60)
                        {
                            if (Convert.ToDouble(szzxczd.Text) >= Convert.ToDouble(szyxczd.Text))
                            {
                                sdzbphv.Text = (gcczc / Convert.ToDouble(szzxczd.Text) * 100).ToString("0.0");//三轴不平衡率
                            }
                            else
                            {
                                sdzbphv.Text = (gcczc / Convert.ToDouble(szyxczd.Text) * 100).ToString("0.0");//三轴不平衡率
                            }
                        }
                        else
                        {
                            if (szzlh.Text != "" && szzlh.Text != "-" && szzlh.Text != "0" && szylh.Text != "" && szylh.Text != "-" && szylh.Text != "0")
                            {
                                sdzbphv.Text = (gcczc / (Convert.ToDouble(szzlh.Text) + Convert.ToDouble(szylh.Text)) * 100).ToString("0.0");//三轴不平衡率
                            }
                        }
                    }
                }
                M1judge();
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        } 
        //三轴右行车制动力发生改变
        private void szyxczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //三轴制动率
                if (Convert.ToDouble(TextIsnulls(dczs.Text)) >= 3)
                {
                    sdzzdv.Text = ((Convert.ToDouble(TextIsnulls(szzxczd.Text)) + Convert.ToDouble(TextIsnulls(szyxczd.Text))) / Convert.ToDouble(TextIsnulls(szzz.Text)) * 100).ToString("0.0");
                }
                else
                {
                    sdzzdv.Text = ((Convert.ToDouble(TextIsnulls(szzxczd.Text)) + Convert.ToDouble(TextIsnulls(szyxczd.Text))) / (Convert.ToDouble(TextIsnulls(szzlh.Text)) + Convert.ToDouble(TextIsnulls(szylh.Text))) * 100).ToString("0.0");
                }
                if (dcspcz.Text != "" && dcspcz.Text != "-")
                {
                    //整车制动率
                    dczdl.Text = ((Convert.ToDouble(TextIsnulls(yzzxczd.Text)) + Convert.ToDouble(TextIsnulls(yzyxczd.Text)) + Convert.ToDouble(TextIsnulls(ezzxczd.Text)) + Convert.ToDouble(TextIsnulls(ezyxczd.Text)) + Convert.ToDouble(TextIsnulls(szzxczd.Text)) + Convert.ToDouble(TextIsnulls(szyxczd.Text)) + Convert.ToDouble(TextIsnulls(sizzxczd.Text)) + Convert.ToDouble(TextIsnulls(sizyxczd.Text)) + Convert.ToDouble(TextIsnulls(wzzxczd.Text)) + Convert.ToDouble(TextIsnulls(wzyxczd.Text)) + Convert.ToDouble(TextIsnulls(lzzxczd.Text)) + Convert.ToDouble(TextIsnulls(lzyxczd.Text))) / Convert.ToDouble(TextIsnulls(dcspcz.Text)) * 100).ToString("0.0");
                }
                if (szzxczd.Text != "" && szyxczd.Text != "" && szzxczd.Text != "-" && szyxczd.Text != "-" && szzxczd.Text != "0" && szyxczd.Text != "0" && sdzzgcc.Text != "" && sdzzgcc.Text != "-" && sdzygcc.Text != "" && sdzygcc.Text != "-")
                {
                    double gcczc = System.Math.Abs(Convert.ToDouble(sdzzgcc.Text) - Convert.ToDouble(sdzygcc.Text));//过程差最大点之差
                    if (sdzzdv.Text != "" && sdzzdv.Text != "-")
                    {
                        //三轴制动率不小于60%
                        if (Convert.ToDouble(sdzzdv.Text) >= 60)
                        {
                            if (Convert.ToDouble(szzxczd.Text) >= Convert.ToDouble(szyxczd.Text))
                            {
                                sdzbphv.Text = (gcczc / Convert.ToDouble(szzxczd.Text) * 100).ToString("0.0");//三轴不平衡率
                            }
                            else
                            {
                                sdzbphv.Text = (gcczc / Convert.ToDouble(szyxczd.Text) * 100).ToString("0.0");//三轴不平衡率
                            }
                        }
                        else
                        {
                            if (szzlh.Text != "" && szzlh.Text != "-" && szzlh.Text != "0" && szylh.Text != "" && szylh.Text != "-" && szylh.Text != "0")
                            {
                                sdzbphv.Text = (gcczc / (Convert.ToDouble(szzlh.Text) + Convert.ToDouble(szylh.Text)) * 100).ToString("0.0");//三轴不平衡率
                            }
                        }
                    }
                }
                M1judge();
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //四轴左行车制动力发生改变
        private void sizzxczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //四轴制动率
                if(Convert.ToDouble(TextIsnulls(dczs.Text))>=3)
                {
                    sidzzdv.Text = ((Convert.ToDouble(TextIsnulls(sizzxczd.Text)) + Convert.ToDouble(TextIsnulls(sizyxczd.Text))) / Convert.ToDouble(TextIsnulls(sizzz.Text)) * 100).ToString("0.0");
                }
                else
                {
                    sidzzdv.Text = ((Convert.ToDouble(TextIsnulls(sizzxczd.Text)) + Convert.ToDouble(TextIsnulls(sizyxczd.Text))) / (Convert.ToDouble(TextIsnulls(sizzlh.Text))+Convert.ToDouble(TextIsnulls(sizylh.Text))) * 100).ToString("0.0");
                }
                if (dcspcz.Text != "" && dcspcz.Text != "-")
                {
                    //整车制动率
                    dczdl.Text = ((Convert.ToDouble(TextIsnulls(yzzxczd.Text)) + Convert.ToDouble(TextIsnulls(yzyxczd.Text)) + Convert.ToDouble(TextIsnulls(ezzxczd.Text)) + Convert.ToDouble(TextIsnulls(ezyxczd.Text)) + Convert.ToDouble(TextIsnulls(szzxczd.Text)) + Convert.ToDouble(TextIsnulls(szyxczd.Text)) + Convert.ToDouble(TextIsnulls(sizzxczd.Text)) + Convert.ToDouble(TextIsnulls(sizyxczd.Text)) + Convert.ToDouble(TextIsnulls(wzzxczd.Text)) + Convert.ToDouble(TextIsnulls(wzyxczd.Text)) + Convert.ToDouble(TextIsnulls(lzzxczd.Text)) + Convert.ToDouble(TextIsnulls(lzyxczd.Text))) / Convert.ToDouble(TextIsnulls(dcspcz.Text)) * 100).ToString("0.0");
                }
                if (sizzxczd.Text != "" && sizyxczd.Text != "" && sizzxczd.Text != "-" && sizyxczd.Text != "-" && sizzxczd.Text != "0" && sizyxczd.Text != "0" && sidzzgcc.Text != "" && sidzzgcc.Text != "-" && sidzygcc.Text != "" && sidzygcc.Text != "-")
                {
                    double gcczc = System.Math.Abs(Convert.ToDouble(sidzzgcc.Text) - Convert.ToDouble(sidzygcc.Text));//过程差最大点之差
                    if (sidzzdv.Text != "" && sidzzdv.Text != "-")
                    {
                        // 四轴制动率不小于60 %
                        if (Convert.ToDouble(sidzzdv.Text) >= 60)
                        {
                            if (Convert.ToDouble(sizzxczd.Text) >= Convert.ToDouble(sizyxczd.Text))
                            {
                                sidzbphv.Text = (gcczc / Convert.ToDouble(sizzxczd.Text) * 100).ToString("0.0");//四轴不平衡率
                            }
                            else
                            {
                                sidzbphv.Text = (gcczc / Convert.ToDouble(sizyxczd.Text) * 100).ToString("0.0");//四轴不平衡率
                            }
                        }
                        else
                        {
                            if (sizzlh.Text != "" && sizzlh.Text != "-" && sizzlh.Text != "0" && sizylh.Text != "" && sizylh.Text != "-" && sizylh.Text != "0")
                            {
                                sidzbphv.Text = (gcczc / (Convert.ToDouble(sizzlh.Text) + Convert.ToDouble(sizylh.Text)) * 100).ToString("0.0");//四轴不平衡率
                            }
                        }
                    }
                }
                M1judge();
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //四轴右行车制动力发生改变
        private void sizyxczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //四轴制动率
                if (Convert.ToDouble(TextIsnulls(dczs.Text)) >= 3)
                {
                    sidzzdv.Text = ((Convert.ToDouble(TextIsnulls(sizzxczd.Text)) + Convert.ToDouble(TextIsnulls(sizyxczd.Text))) / Convert.ToDouble(TextIsnulls(sizzz.Text))* 100).ToString("0.0");
                }
                else
                {
                    sidzzdv.Text = ((Convert.ToDouble(TextIsnulls(sizzxczd.Text)) + Convert.ToDouble(TextIsnulls(sizyxczd.Text))) / (Convert.ToDouble(TextIsnulls(sizzlh.Text)) + Convert.ToDouble(TextIsnulls(sizylh.Text))) * 100).ToString("0.0");
                }
                if (dcspcz.Text != "" && dcspcz.Text != "-")
                {
                    //整车制动率
                    dczdl.Text = ((Convert.ToDouble(TextIsnulls(yzzxczd.Text)) + Convert.ToDouble(TextIsnulls(yzyxczd.Text)) + Convert.ToDouble(TextIsnulls(ezzxczd.Text)) + Convert.ToDouble(TextIsnulls(ezyxczd.Text)) + Convert.ToDouble(TextIsnulls(szzxczd.Text)) + Convert.ToDouble(TextIsnulls(szyxczd.Text)) + Convert.ToDouble(TextIsnulls(sizzxczd.Text)) + Convert.ToDouble(TextIsnulls(sizyxczd.Text)) + Convert.ToDouble(TextIsnulls(wzzxczd.Text)) + Convert.ToDouble(TextIsnulls(wzyxczd.Text)) + Convert.ToDouble(TextIsnulls(lzzxczd.Text)) + Convert.ToDouble(TextIsnulls(lzyxczd.Text))) / Convert.ToDouble(TextIsnulls(dcspcz.Text)) * 100).ToString("0.0");
                }
                if (sizzxczd.Text != "" && sizyxczd.Text != "" && sizzxczd.Text != "-" && sizyxczd.Text != "-" && sizzxczd.Text != "0" && sizyxczd.Text != "0" && sidzzgcc.Text != "" && sidzzgcc.Text != "-" && sidzygcc.Text != "" && sidzygcc.Text != "-")
                {
                    double gcczc = System.Math.Abs(Convert.ToDouble(sidzzgcc.Text) - Convert.ToDouble(sidzygcc.Text));//过程差最大点之差
                    if (sidzzdv.Text != "" && sidzzdv.Text != "-")
                    {
                        // 四轴制动率不小于60 %
                        if (Convert.ToDouble(sidzzdv.Text) >= 60)
                        {
                            if (Convert.ToDouble(sizzxczd.Text) >= Convert.ToDouble(sizyxczd.Text))
                            {
                                sidzbphv.Text = (gcczc / Convert.ToDouble(sizzxczd.Text) * 100).ToString("0.0");//四轴不平衡率
                            }
                            else
                            {
                                sidzbphv.Text = (gcczc / Convert.ToDouble(sizyxczd.Text) * 100).ToString("0.0");//四轴不平衡率
                            }
                        }
                        else
                        {
                            if (sizzlh.Text != "" && sizzlh.Text != "-" && sizzlh.Text != "0" && sizylh.Text != "" && sizylh.Text != "-" && sizylh.Text != "0")
                            {
                                sidzbphv.Text = (gcczc / (Convert.ToDouble(sizzlh.Text) + Convert.ToDouble(sizylh.Text)) * 100).ToString("0.0");//四轴不平衡率
                            }
                        }
                    }
                }
                M1judge();
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //五轴左行车制动力发生改变
        private void wzzxczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //五轴制动率
                if(Convert.ToDouble(TextIsnulls(dczs.Text))>=3)
                {
                    wdzzdv.Text = ((Convert.ToDouble(TextIsnulls(wzzxczd.Text)) + Convert.ToDouble(TextIsnulls(wzyxczd.Text))) / Convert.ToDouble(TextIsnulls(wzzz.Text)) * 100).ToString("0.0");
                }
               else
                {
                    wdzzdv.Text = ((Convert.ToDouble(TextIsnulls(wzzxczd.Text)) + Convert.ToDouble(TextIsnulls(wzyxczd.Text))) / (Convert.ToDouble(TextIsnulls(wzzlh.Text))+Convert.ToDouble(TextIsnulls(wzylh.Text)))* 100).ToString("0.0");
                }
                if (dcspcz.Text != "" && dcspcz.Text != "-")
                {
                    //整车制动率
                    dczdl.Text = ((Convert.ToDouble(TextIsnulls(yzzxczd.Text)) + Convert.ToDouble(TextIsnulls(yzyxczd.Text)) + Convert.ToDouble(TextIsnulls(ezzxczd.Text)) + Convert.ToDouble(TextIsnulls(ezyxczd.Text)) + Convert.ToDouble(TextIsnulls(szzxczd.Text)) + Convert.ToDouble(TextIsnulls(szyxczd.Text)) + Convert.ToDouble(TextIsnulls(sizzxczd.Text)) + Convert.ToDouble(TextIsnulls(sizyxczd.Text)) + Convert.ToDouble(TextIsnulls(wzzxczd.Text)) + Convert.ToDouble(TextIsnulls(wzyxczd.Text)) + Convert.ToDouble(TextIsnulls(lzzxczd.Text)) + Convert.ToDouble(TextIsnulls(lzyxczd.Text))) / Convert.ToDouble(TextIsnulls(dcspcz.Text)) * 100).ToString("0.0");
                }
                if (wzzxczd.Text != "" && wzyxczd.Text != "" && wzzxczd.Text != "-" && wzyxczd.Text != "-" && wzzxczd.Text != "0" && wzyxczd.Text != "0" && wdzzgcc.Text != "" && wdzzgcc.Text != "-" && wdzygcc.Text != "" && wdzygcc.Text != "-")
                {
                    double gcczc = System.Math.Abs(Convert.ToDouble(wdzzgcc.Text) - Convert.ToDouble(wdzygcc.Text));//过程差最大点之差
                    if (wdzzdv.Text != "" && wdzzdv.Text != "-")
                    {
                        //五轴制动率不小于60%
                        if (Convert.ToDouble(wdzzdv.Text) >= 60)
                        {
                            if (Convert.ToDouble(wzzxczd.Text) >= Convert.ToDouble(wzyxczd.Text))
                            {
                                wdzbphv.Text = (gcczc / Convert.ToDouble(wzzxczd.Text) * 100).ToString("0.0");//五轴不平衡率
                            }
                            else
                            {
                                wdzbphv.Text = (gcczc / Convert.ToDouble(wzyxczd.Text) * 100).ToString("0.0");//五轴不平衡率
                            }
                        }
                        else
                        {
                            if (wzzlh.Text != "" && wzzlh.Text != "-" && wzzlh.Text != "0" && wzylh.Text != "" && wzylh.Text != "-" && wzylh.Text != "0")
                            {
                                wdzbphv.Text = (gcczc / (Convert.ToDouble(wzzlh.Text) + Convert.ToDouble(wzylh.Text)) * 100).ToString("0.0");//五轴不平衡率
                            }
                        }
                    }
                }
                M1judge();
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //五轴右行车制动力发生改变
        private void wzyxczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //五轴制动率
                if (Convert.ToDouble(TextIsnulls(dczs.Text)) >= 3)
                {
                    wdzzdv.Text = ((Convert.ToDouble(TextIsnulls(wzzxczd.Text)) + Convert.ToDouble(TextIsnulls(wzyxczd.Text))) / Convert.ToDouble(TextIsnulls(wzzz.Text))* 100).ToString("0.0");
                }
                else
                {
                    wdzzdv.Text = ((Convert.ToDouble(TextIsnulls(wzzxczd.Text)) + Convert.ToDouble(TextIsnulls(wzyxczd.Text))) / (Convert.ToDouble(TextIsnulls(wzzlh.Text)) + Convert.ToDouble(TextIsnulls(wzylh.Text))) * 100).ToString("0.0");
                }
                if (dcspcz.Text != "" && dcspcz.Text != "-")
                {
                    //整车制动率
                    dczdl.Text = ((Convert.ToDouble(TextIsnulls(yzzxczd.Text)) + Convert.ToDouble(TextIsnulls(yzyxczd.Text)) + Convert.ToDouble(TextIsnulls(ezzxczd.Text)) + Convert.ToDouble(TextIsnulls(ezyxczd.Text)) + Convert.ToDouble(TextIsnulls(szzxczd.Text)) + Convert.ToDouble(TextIsnulls(szyxczd.Text)) + Convert.ToDouble(TextIsnulls(sizzxczd.Text)) + Convert.ToDouble(TextIsnulls(sizyxczd.Text)) + Convert.ToDouble(TextIsnulls(wzzxczd.Text)) + Convert.ToDouble(TextIsnulls(wzyxczd.Text)) + Convert.ToDouble(TextIsnulls(lzzxczd.Text)) + Convert.ToDouble(TextIsnulls(lzyxczd.Text))) / Convert.ToDouble(TextIsnulls(dcspcz.Text)) * 100).ToString("0.0");
                }
                if (wzzxczd.Text != "" && wzyxczd.Text != "" && wzzxczd.Text != "-" && wzyxczd.Text != "-" && wzzxczd.Text != "0" && wzyxczd.Text != "0" && wdzzgcc.Text != "" && wdzzgcc.Text != "-" && wdzygcc.Text != "" && wdzygcc.Text != "-")
                {
                    double gcczc = System.Math.Abs(Convert.ToDouble(wdzzgcc.Text) - Convert.ToDouble(wdzygcc.Text));//过程差最大点之差
                    if (wdzzdv.Text != "" && wdzzdv.Text != "-")
                    {
                        //五轴制动率不小于60%
                        if (Convert.ToDouble(wdzzdv.Text) >= 60)
                        {
                            if (Convert.ToDouble(wzzxczd.Text) >= Convert.ToDouble(wzyxczd.Text))
                            {
                                wdzbphv.Text = (gcczc / Convert.ToDouble(wzzxczd.Text) * 100).ToString("0.0");//五轴不平衡率
                            }
                            else
                            {
                                wdzbphv.Text = (gcczc / Convert.ToDouble(wzyxczd.Text) * 100).ToString("0.0");//五轴不平衡率
                            }
                        }
                        else
                        {
                            if (wzzlh.Text != "" && wzzlh.Text != "-" && wzzlh.Text != "0" && wzylh.Text != "" && wzylh.Text != "-" && wzylh.Text != "0")
                            {
                                wdzbphv.Text = (gcczc / (Convert.ToDouble(wzzlh.Text) + Convert.ToDouble(wzylh.Text)) * 100).ToString("0.0");//五轴不平衡率
                            }
                        }
                    }
                }
                M1judge();
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //六轴左行车制动力发生改变
        private void lzzxczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //六轴制动率
                if(Convert.ToDouble(TextIsnulls(dczs.Text))>=3)
                {
                    ldzzdv.Text = ((Convert.ToDouble(TextIsnulls(lzzxczd.Text)) + Convert.ToDouble(TextIsnulls(lzyxczd.Text))) / Convert.ToDouble(TextIsnulls(lzzz.Text))* 100).ToString("0.0");
                }
                else
                {
                    ldzzdv.Text = ((Convert.ToDouble(TextIsnulls(lzzxczd.Text)) + Convert.ToDouble(TextIsnulls(lzyxczd.Text))) / (Convert.ToDouble(TextIsnulls(lzzlh.Text))+Convert.ToDouble(TextIsnulls(lzylh.Text))) * 100).ToString("0.0");
                }
                if (dcspcz.Text != "" && dcspcz.Text != "-")
                {
                    //整车制动率
                    dczdl.Text = ((Convert.ToDouble(TextIsnulls(yzzxczd.Text)) + Convert.ToDouble(TextIsnulls(yzyxczd.Text)) + Convert.ToDouble(TextIsnulls(ezzxczd.Text)) + Convert.ToDouble(TextIsnulls(ezyxczd.Text)) + Convert.ToDouble(TextIsnulls(szzxczd.Text)) + Convert.ToDouble(TextIsnulls(szyxczd.Text)) + Convert.ToDouble(TextIsnulls(sizzxczd.Text)) + Convert.ToDouble(TextIsnulls(sizyxczd.Text)) + Convert.ToDouble(TextIsnulls(wzzxczd.Text)) + Convert.ToDouble(TextIsnulls(wzyxczd.Text)) + Convert.ToDouble(TextIsnulls(lzzxczd.Text)) + Convert.ToDouble(TextIsnulls(lzyxczd.Text))) / Convert.ToDouble(TextIsnulls(dcspcz.Text))* 100).ToString("0.0");
                }
                if (lzzxczd.Text != "" && lzyxczd.Text != "" && lzzxczd.Text != "-" && lzyxczd.Text != "-" && lzzxczd.Text != "0" && lzyxczd.Text != "0" && ldzzgcc.Text != "" && ldzzgcc.Text != "-" && ldzygcc.Text != "" && ldzygcc.Text != "-")
                {
                    double gcczc = System.Math.Abs(Convert.ToDouble(ldzzgcc.Text) - Convert.ToDouble(ldzygcc.Text));//过程差最大点之差
                    if (ldzzdv.Text != "" && ldzzdv.Text != "-")
                    {
                        //六轴制动率不小于60%
                        if (Convert.ToDouble(ldzzdv.Text) >= 60)
                        {
                            if (Convert.ToDouble(lzzxczd.Text) >= Convert.ToDouble(lzyxczd.Text))
                            {
                                ldzbphv.Text = (gcczc / Convert.ToDouble(lzzxczd.Text) * 100).ToString("0.0");//六轴不平衡率
                            }
                            else
                            {
                                ldzbphv.Text = (gcczc / Convert.ToDouble(lzyxczd.Text) * 100).ToString("0.0");//六轴不平衡率
                            }
                        }
                        else
                        {
                            if (lzzlh.Text != "" && lzzlh.Text != "-" && lzzlh.Text != "0" && lzylh.Text != "" && lzylh.Text != "-" && lzylh.Text != "0")
                            {
                                ldzbphv.Text = (gcczc / (Convert.ToDouble(lzzlh.Text) + Convert.ToDouble(lzylh.Text)) * 100).ToString("0.0");//六轴不平衡率
                            }
                        }
                    }
                }
                M1judge();
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //六轴右行车制动力发生改变
        private void lzyxczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //六轴制动率
                if (Convert.ToDouble(TextIsnulls(dczs.Text)) >= 3)
                {
                    ldzzdv.Text = ((Convert.ToDouble(TextIsnulls(lzzxczd.Text)) + Convert.ToDouble(TextIsnulls(lzyxczd.Text))) / Convert.ToDouble(TextIsnulls(lzzz.Text))* 100).ToString("0.0");
                }
                else
                {
                    ldzzdv.Text = ((Convert.ToDouble(TextIsnulls(lzzxczd.Text)) + Convert.ToDouble(TextIsnulls(lzyxczd.Text))) / (Convert.ToDouble(TextIsnulls(lzzlh.Text)) + Convert.ToDouble(TextIsnulls(lzylh.Text))) * 100).ToString("0.0");
                }
                if (dcspcz.Text != "" && dcspcz.Text != "-")
                {
                    //整车制动率
                    dczdl.Text = ((Convert.ToDouble(TextIsnulls(yzzxczd.Text)) + Convert.ToDouble(TextIsnulls(yzyxczd.Text)) + Convert.ToDouble(TextIsnulls(ezzxczd.Text)) + Convert.ToDouble(TextIsnulls(ezyxczd.Text)) + Convert.ToDouble(TextIsnulls(szzxczd.Text)) + Convert.ToDouble(TextIsnulls(szyxczd.Text)) + Convert.ToDouble(TextIsnulls(sizzxczd.Text)) + Convert.ToDouble(TextIsnulls(sizyxczd.Text)) + Convert.ToDouble(TextIsnulls(wzzxczd.Text)) + Convert.ToDouble(TextIsnulls(wzyxczd.Text)) + Convert.ToDouble(TextIsnulls(lzzxczd.Text)) + Convert.ToDouble(TextIsnulls(lzyxczd.Text))) / Convert.ToDouble(TextIsnulls(dcspcz.Text)) * 100).ToString("0.0");
                }
                if (lzzxczd.Text != "" && lzyxczd.Text != "" && lzzxczd.Text != "-" && lzyxczd.Text != "-" && lzzxczd.Text != "0" && lzyxczd.Text != "0" && ldzzgcc.Text != "" && ldzzgcc.Text != "-" && ldzygcc.Text != "" && ldzygcc.Text != "-")
                {
                    double gcczc = System.Math.Abs(Convert.ToDouble(ldzzgcc.Text) - Convert.ToDouble(ldzygcc.Text));//过程差最大点之差
                    if (ldzzdv.Text != "" && ldzzdv.Text != "-")
                    {
                        //六轴制动率不小于60%
                        if (Convert.ToDouble(ldzzdv.Text) >= 60)
                        {
                            if (Convert.ToDouble(lzzxczd.Text) >= Convert.ToDouble(lzyxczd.Text))
                            {
                                ldzbphv.Text = (gcczc / Convert.ToDouble(lzzxczd.Text) * 100).ToString("0.0");//六轴不平衡率
                            }
                            else
                            {
                                ldzbphv.Text = (gcczc / Convert.ToDouble(lzyxczd.Text) * 100).ToString("0.0");//六轴不平衡率
                            }
                        }
                        else
                        {
                            if (lzzlh.Text != "" && lzzlh.Text != "-" && lzzlh.Text != "0" && lzylh.Text != "" && lzylh.Text != "-" && lzylh.Text != "0")
                            {
                                ldzbphv.Text = (gcczc / (Convert.ToDouble(lzzlh.Text) + Convert.ToDouble(lzylh.Text)) * 100).ToString("0.0");//六轴不平衡率
                            }
                        }
                    }
                }
                M1judge();
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //一轴左过程差最大点发生改变             
        private void ydzzgcc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (ydzzgcc.Text != "" && ydzzgcc.Text != "-" && ydzygcc.Text != "" && ydzygcc.Text != "-")
                {
                    double a = System.Math.Abs(Convert.ToDouble(ydzzgcc.Text) - Convert.ToDouble(ydzygcc.Text));//过程差最大点之差
                    if (yzzxczd.Text != "" && yzzxczd.Text != "-" && yzzxczd.Text != "0" && yzyxczd.Text != "" && yzyxczd.Text != "-" && yzyxczd.Text != "0")
                    {
                        if (Convert.ToDouble(yzzxczd.Text) > Convert.ToDouble(yzyxczd.Text))
                        {
                            double m = a / Convert.ToDouble(yzzxczd.Text) * 100;
                            ydzbphv.Text = m.ToString("0.0");//为一轴不平衡率赋值
                        }
                        else
                        {
                            double m = a / Convert.ToDouble(yzyxczd.Text) * 100;
                            ydzbphv.Text = m.ToString("0.0");//为一轴不平衡率赋值
                        }
                    }
                }
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //一轴右过程差最大点发生改变   
        private void ydzygcc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (ydzzgcc.Text != "" && ydzzgcc.Text != "-" && ydzygcc.Text != "" && ydzygcc.Text != "-")
                {
                    double a = System.Math.Abs(Convert.ToDouble(ydzzgcc.Text) - Convert.ToDouble(ydzygcc.Text));//过程差最大点之差
                    if (yzzxczd.Text != "" && yzzxczd.Text != "-" && yzzxczd.Text != "0" && yzyxczd.Text != "" && yzyxczd.Text != "-" && yzyxczd.Text != "0")
                    {
                        if (Convert.ToDouble(yzzxczd.Text) > Convert.ToDouble(yzyxczd.Text))
                        {
                            double m = a / Convert.ToDouble(yzzxczd.Text) * 100;
                            ydzbphv.Text = m.ToString("0.0");//为一轴不平衡率赋值
                        }
                        else
                        {
                            double m = a / Convert.ToDouble(yzyxczd.Text) * 100;
                            ydzbphv.Text = m.ToString("0.0");//为一轴不平衡率赋值
                        }
                    }
                }
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //二轴左过程差最大点发生改变
        private void edzzgcc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (edzzgcc.Text != "" && edzzgcc.Text != "-" && edzygcc.Text != "" && edzygcc.Text != "-")
                {
                    double a = System.Math.Abs(Convert.ToDouble(edzzgcc.Text) - Convert.ToDouble(edzygcc.Text));//过程差最大点之差
                    if (zxzs.Text == "2")
                    {
                        if (ezzxczd.Text != "" && ezzxczd.Text != "-" && ezzxczd.Text != "0" && ezyxczd.Text != "" && ezyxczd.Text != "-" && ezyxczd.Text != "0")
                        {
                            if (Convert.ToDouble(ezzxczd.Text) > Convert.ToDouble(ezyxczd.Text))
                            {
                                double m = a / Convert.ToDouble(ezzxczd.Text) * 100;
                                edzbphv.Text = m.ToString("0.0");//为二轴不平衡率赋值
                            }
                            else
                            {
                                double m = a / Convert.ToDouble(ezyxczd.Text) * 100;
                                edzbphv.Text = m.ToString("0.0");//为二轴不平衡率赋值
                            }
                        }
                    }
                    else
                    {
                        if (edzzdv.Text != "" && edzzdv.Text != "-")
                        {
                            //判断二轴制动率是否大于60
                            if (Convert.ToDouble(edzzdv.Text) >= 60)
                            {
                                if (ezzxczd.Text != "" && ezzxczd.Text != "-" && ezzxczd.Text != "0" && ezyxczd.Text != "" && ezyxczd.Text != "-" && ezyxczd.Text != "0")
                                {
                                    //判断二轴左轮荷与右轮荷的大小
                                    if (Convert.ToDouble(ezzxczd.Text) > Convert.ToDouble(ezyxczd.Text))
                                    {
                                        double m = a / Convert.ToDouble(ezzxczd.Text) * 100;
                                        edzbphv.Text = m.ToString("0.0");//为二轴不平衡率赋值
                                    }
                                    else
                                    {
                                        double m = a / Convert.ToDouble(ezyxczd.Text) * 100;
                                        edzbphv.Text = m.ToString("0.0");//为二轴不平衡率赋值
                                    }
                                }
                            }
                            else
                            {
                                if (ezzlh.Text != "" && ezzlh.Text != "-" && ezzlh.Text != "0" && ezylh.Text != "" && ezylh.Text != "-" && ezylh.Text != "0")
                                {
                                    edzbphv.Text = (a / (Convert.ToDouble(ezzlh.Text) + Convert.ToDouble(ezylh.Text))* 100).ToString("0.0");//二轴不平衡率
                                }
                            }
                        }
                    }
                }
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //二轴右过程差最大点发生改变
        private void edzygcc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (edzzgcc.Text != "" && edzzgcc.Text != "-" && edzygcc.Text != "" && edzygcc.Text != "-")
                {
                    double a = System.Math.Abs(Convert.ToDouble(edzzgcc.Text) - Convert.ToDouble(edzygcc.Text));//过程差最大点之差
                    if (zxzs.Text == "2")
                    {
                        if (ezzxczd.Text != "" && ezzxczd.Text != "-" && ezzxczd.Text != "0" && ezyxczd.Text != "" && ezyxczd.Text != "-" && ezyxczd.Text != "0")
                        {
                            if (Convert.ToDouble(ezzxczd.Text) > Convert.ToDouble(ezyxczd.Text))
                            {
                                double m = a / Convert.ToDouble(ezzxczd.Text) * 100;
                                edzbphv.Text = m.ToString("0.0");//为二轴不平衡率赋值
                            }
                            else
                            {
                                double m = a / Convert.ToDouble(ezyxczd.Text) * 100;
                                edzbphv.Text = m.ToString("0.0");//为二轴不平衡率赋值
                            }
                        }
                    }
                    else
                    {
                        if (edzzdv.Text != "" && edzzdv.Text != "-")
                        {
                            //判断二轴制动率是否大于60
                            if (Convert.ToDouble(edzzdv.Text) >= 60)
                            {
                                if (ezzxczd.Text != "" && ezzxczd.Text != "-" && ezzxczd.Text != "0" && ezyxczd.Text != "" && ezyxczd.Text != "-" && ezyxczd.Text != "0")
                                {
                                    //判断二轴左轮荷与右轮荷的大小
                                    if (Convert.ToDouble(ezzxczd.Text) > Convert.ToDouble(ezyxczd.Text))
                                    {
                                        double m = a / Convert.ToDouble(ezzxczd.Text) * 100;
                                        edzbphv.Text = m.ToString("0.0");//为二轴不平衡率赋值
                                    }
                                    else
                                    {
                                        double m = a / Convert.ToDouble(ezyxczd.Text) * 100;
                                        edzbphv.Text = m.ToString("0.0");//为二轴不平衡率赋值
                                    }
                                }
                            }
                            else
                            {
                                if (ezzlh.Text != "" && ezzlh.Text != "-" && ezzlh.Text != "0" && ezylh.Text != "" && ezylh.Text != "-" && ezylh.Text != "0")
                                {
                                    edzbphv.Text = (a / (Convert.ToDouble(ezzlh.Text) + Convert.ToDouble(ezylh.Text))* 100).ToString("0.0");//二轴不平衡率
                                }
                            }
                        }
                    }
                }
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //三轴左过程差最大点发生改变
        private void sdzzgcc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (sdzzgcc.Text != "" && sdzzgcc.Text != "-" && sdzygcc.Text != "" && sdzygcc.Text != "-")
                {
                    double a = System.Math.Abs(Convert.ToDouble(sdzzgcc.Text) - Convert.ToDouble(sdzygcc.Text));//过程差最大点之差
                    if (sdzzdv.Text != "" && sdzzdv.Text != "-")
                    {
                        //判断三轴制动率是否大于60
                        if (Convert.ToDouble(sdzzdv.Text) >= 60)
                        {
                            if (szzxczd.Text != "" && szzxczd.Text != "-" && szzxczd.Text != "0" && szyxczd.Text != "" && szyxczd.Text != "-" && szyxczd.Text != "0")
                            {
                                //判断三轴左行车制动力与右行车制动力的大小
                                if (Convert.ToDouble(szzxczd.Text) > Convert.ToDouble(szyxczd.Text))
                                {
                                    double m = a / Convert.ToDouble(szzxczd.Text) * 100;
                                    sdzbphv.Text = m.ToString("0.0");//为三轴不平衡率赋值
                                }
                                else
                                {
                                    double m = a / Convert.ToDouble(szyxczd.Text) * 100;
                                    sdzbphv.Text = m.ToString("0.0");//为三轴不平衡率赋值
                                }
                            }
                        }
                        else
                        {
                            if (szzlh.Text != "" && szzlh.Text != "-" && szzlh.Text != "0" && szylh.Text != "" && szylh.Text != "-" && szylh.Text != "0")
                            {
                                sdzbphv.Text = (a / (Convert.ToDouble(szzlh.Text) + Convert.ToDouble(szylh.Text))* 100).ToString("0.0");//三轴不平衡率
                            }
                        }
                    }
                }
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //三轴右过程差最大点发生改变
        private void sdzygcc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (sdzzgcc.Text != "" && sdzzgcc.Text != "-" && sdzygcc.Text != "" && sdzygcc.Text != "-")
                {
                    double a = System.Math.Abs(Convert.ToDouble(sdzzgcc.Text) - Convert.ToDouble(sdzygcc.Text));//过程差最大点之差
                    if (sdzzdv.Text != "" && sdzzdv.Text != "-")
                    {
                        //判断三轴制动率是否大于60
                        if (Convert.ToDouble(sdzzdv.Text) >= 60)
                        {
                            if (szzxczd.Text != "" && szzxczd.Text != "-" && szzxczd.Text != "0" && szyxczd.Text != "" && szyxczd.Text != "-" && szyxczd.Text != "0")
                            {
                                //判断三轴左行车制动力与右行车制动力的大小
                                if (Convert.ToDouble(szzxczd.Text) > Convert.ToDouble(szyxczd.Text))
                                {
                                    double m = a / Convert.ToDouble(szzxczd.Text) * 100;
                                    sdzbphv.Text = m.ToString("0.0");//为三轴不平衡率赋值
                                }
                                else
                                {
                                    double m = a / Convert.ToDouble(szyxczd.Text) * 100;
                                    sdzbphv.Text = m.ToString("0.0");//为三轴不平衡率赋值
                                }
                            }
                        }
                        else
                        {
                            if (szzlh.Text != "" && szzlh.Text != "-" && szzlh.Text != "0" && szylh.Text != "" && szylh.Text != "-" && szylh.Text != "0")
                            {
                                sdzbphv.Text = (a / (Convert.ToDouble(szzlh.Text) + Convert.ToDouble(szylh.Text)) * 100).ToString("0.0");//三轴不平衡率
                            }
                        }
                    }
                }
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //四轴左过程差最大点发生改变
        private void sidzzgcc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (sidzzgcc.Text != "" && sidzzgcc.Text != "-" && sidzygcc.Text != "" && sidzygcc.Text != "-")
                {
                    double a = System.Math.Abs(Convert.ToDouble(sidzzgcc.Text) - Convert.ToDouble(sidzygcc.Text));//过程差最大点之差
                    if (sidzzdv.Text != "" && sidzzdv.Text != "-")
                    {
                        //判断四轴制动率是否大于60
                        if (Convert.ToDouble(sidzzdv.Text) >= 60)
                        {
                            if (sizzxczd.Text != "" && sizzxczd.Text != "-" && sizzxczd.Text != "0" && sizyxczd.Text != "" && sizyxczd.Text != "-" && sizyxczd.Text != "0")
                            {
                                //判断四轴左行车制动力与右行车制动力的大小
                                if (Convert.ToDouble(sizzxczd.Text) > Convert.ToDouble(sizyxczd.Text))
                                {
                                    double m = a / Convert.ToDouble(sizzxczd.Text) * 100;
                                    sidzbphv.Text = m.ToString("0.0");//为四轴不平衡率赋值
                                }
                                else
                                {
                                    double m = a / Convert.ToDouble(sizyxczd.Text) * 100;
                                    sidzbphv.Text = m.ToString("0.0");//为四轴不平衡率赋值
                                }
                            }
                        }
                        else
                        {
                            if (sizzlh.Text != "" && sizzlh.Text != "-" && sizzlh.Text != "0" && sizylh.Text != "" && sizylh.Text != "-" && sizylh.Text != "0")
                            {
                                sidzbphv.Text = (a / (Convert.ToDouble(sizzlh.Text) + Convert.ToDouble(sizylh.Text)) * 100).ToString("0.0");//四轴不平衡率
                            }
                        }
                    }
                }
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //四轴右过程差最大点发生改变
        private void sidzygcc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (sidzzgcc.Text != "" && sidzzgcc.Text != "-" && sidzygcc.Text != "" && sidzygcc.Text != "-")
                {
                    double a = System.Math.Abs(Convert.ToDouble(sidzzgcc.Text) - Convert.ToDouble(sidzygcc.Text));//过程差最大点之差
                    if (sidzzdv.Text != "" && sidzzdv.Text != "-")
                    {
                        //判断四轴制动率是否大于60
                        if (Convert.ToDouble(sidzzdv.Text) >= 60)
                        {
                            if (sizzxczd.Text != "" && sizzxczd.Text != "-" && sizzxczd.Text != "0" && sizyxczd.Text != "" && sizyxczd.Text != "-" && sizyxczd.Text != "0")
                            {
                                //判断四轴左行车制动力与右行车制动力的大小
                                if (Convert.ToDouble(sizzxczd.Text) > Convert.ToDouble(sizyxczd.Text))
                                {
                                    double m = a / Convert.ToDouble(sizzxczd.Text) * 100;
                                    sidzbphv.Text = m.ToString("0.0");//为四轴不平衡率赋值
                                }
                                else
                                {
                                    double m = a / Convert.ToDouble(sizyxczd.Text) * 100;
                                    sidzbphv.Text = m.ToString("0.0");//为四轴不平衡率赋值
                                }
                            }
                        }
                        else
                        {
                            if (sizzlh.Text != "" && sizzlh.Text != "-" && sizzlh.Text != "0" && sizylh.Text != "" && sizylh.Text != "-" && sizylh.Text != "0")
                            {
                                sidzbphv.Text = (a / (Convert.ToDouble(sizzlh.Text) + Convert.ToDouble(sizylh.Text)) * 100).ToString("0.0");//四轴不平衡率
                            }
                        }
                    }
                }
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //五轴左过程差最大点发生改变
        private void wdzzgcc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (wdzzgcc.Text != "" && wdzzgcc.Text != "-" && wdzygcc.Text != "" && wdzygcc.Text != "-")
                {
                    double a = System.Math.Abs(Convert.ToDouble(wdzzgcc.Text) - Convert.ToDouble(wdzygcc.Text));//过程差最大点之差
                    if (wdzzdv.Text != "" && wdzzdv.Text != "-")
                    {
                        //判断五轴制动率是否大于60
                        if (Convert.ToDouble(wdzzdv.Text) >= 60)
                        {
                            if (wzzxczd.Text != "" && wzzxczd.Text != "-" && wzzxczd.Text != "0" && wzyxczd.Text != "" && wzyxczd.Text != "-" && wzyxczd.Text != "0")
                            {
                                //判断五轴左行车制动力与右行车制动力的大小
                                if (Convert.ToDouble(wzzxczd.Text) > Convert.ToDouble(wzyxczd.Text))
                                {
                                    double m = a / Convert.ToDouble(wzzxczd.Text) * 100;
                                    wdzbphv.Text = m.ToString("0.0");//为五轴不平衡率赋值
                                }
                                else
                                {
                                    double m = a / Convert.ToDouble(wzyxczd.Text) * 100;
                                    wdzbphv.Text = m.ToString("0.0");//为五轴不平衡率赋值
                                }
                            }
                        }
                        else
                        {
                            if (wzzlh.Text != "" && wzzlh.Text != "-" && wzzlh.Text != "0" && wzylh.Text != "" && wzylh.Text != "-" && wzylh.Text != "0")
                            {
                                wdzbphv.Text = (a / (Convert.ToDouble(wzzlh.Text) + Convert.ToDouble(wzylh.Text)) * 100).ToString("0.0");//五轴不平衡率
                            }
                        }
                    }
                }
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //五轴右过程差最大点发生改变
        private void wdzygcc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (wdzzgcc.Text != "" && wdzzgcc.Text != "-" && wdzygcc.Text != "" && wdzygcc.Text != "-")
                {
                    double a = System.Math.Abs(Convert.ToDouble(wdzzgcc.Text) - Convert.ToDouble(wdzygcc.Text));//过程差最大点之差
                    if (wdzzdv.Text != "" && wdzzdv.Text != "-")
                    {
                        //判断五轴制动率是否大于60
                        if (Convert.ToDouble(wdzzdv.Text) >= 60)
                        {
                            if (wzzxczd.Text != "" && wzzxczd.Text != "-" && wzzxczd.Text != "0" && wzyxczd.Text != "" && wzyxczd.Text != "-" && wzyxczd.Text != "0")
                            {
                                //判断五轴左行车制动力与右行车制动力的大小
                                if (Convert.ToDouble(wzzxczd.Text) > Convert.ToDouble(wzyxczd.Text))
                                {
                                    double m = a / Convert.ToDouble(wzzxczd.Text) * 100;
                                    wdzbphv.Text = m.ToString("0.0");//为五轴不平衡率赋值
                                }
                                else
                                {
                                    double m = a / Convert.ToDouble(wzyxczd.Text) * 100;
                                    wdzbphv.Text = m.ToString("0.0");//为五轴不平衡率赋值
                                }
                            }
                        }
                        else
                        {
                            if (wzzlh.Text != "" && wzzlh.Text != "-" && wzzlh.Text != "0" && wzylh.Text != "" && wzylh.Text != "-" && wzylh.Text != "0")
                            {
                                wdzbphv.Text = (a / (Convert.ToDouble(wzzlh.Text) + Convert.ToDouble(wzylh.Text))* 100).ToString("0.0");//五轴不平衡率
                            }
                        }
                    }
                }
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //六轴左过程差最大点发生改变
        private void ldzzgcc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (ldzzgcc.Text != "" && ldzzgcc.Text != "-" && ldzygcc.Text != "" && ldzygcc.Text != "-")
                {
                    double a = System.Math.Abs(Convert.ToDouble(ldzzgcc.Text) - Convert.ToDouble(ldzygcc.Text));//过程差最大点之差
                    if (ldzzdv.Text != "" && ldzzdv.Text != "-")
                    {
                        //判断六轴制动率是否大于60
                        if (Convert.ToDouble(ldzzdv.Text) >= 60)
                        {
                            if (lzzxczd.Text != "" && lzzxczd.Text != "-" && lzzxczd.Text != "0" && lzyxczd.Text != "" && lzyxczd.Text != "-" && lzyxczd.Text != "0")
                            {
                                //判断六轴左行车制动力与右行车制动力的大小
                                if (Convert.ToDouble(lzzxczd.Text) > Convert.ToDouble(lzyxczd.Text))
                                {
                                    double m = a / Convert.ToDouble(lzzxczd.Text) * 100;
                                    ldzbphv.Text = m.ToString("0.0");//为六轴不平衡率赋值
                                }
                                else
                                {
                                    double m = a / Convert.ToDouble(lzyxczd.Text) * 100;
                                    ldzbphv.Text = m.ToString("0.0");//为六轴不平衡率赋值
                                }
                            }
                        }
                        else
                        {
                            if (lzzlh.Text != "" && lzzlh.Text != "-" && lzzlh.Text != "0" && lzylh.Text != "" && lzylh.Text != "-" && lzylh.Text != "0")
                            {
                                ldzbphv.Text = (a / (Convert.ToDouble(lzzlh.Text) + Convert.ToDouble(lzylh.Text)) * 100).ToString("0.0");//六轴不平衡率
                            }
                        }
                    }
                }
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //六轴右过程差最大点发生改变
        private void ldzygcc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (ldzzgcc.Text != "" && ldzzgcc.Text != "-" && ldzygcc.Text != "" && ldzygcc.Text != "-")
                {
                    double a = System.Math.Abs(Convert.ToDouble(ldzzgcc.Text) - Convert.ToDouble(ldzygcc.Text));//过程差最大点之差
                    if (ldzzdv.Text != "" && ldzzdv.Text != "-")
                    {
                        //判断六轴制动率是否大于60
                        if (Convert.ToDouble(ldzzdv.Text) >= 60)
                        {
                            if (lzzxczd.Text != "" && lzzxczd.Text != "-" && lzzxczd.Text != "0" && lzyxczd.Text != "" && lzyxczd.Text != "-" && lzyxczd.Text != "0")
                            {
                                //判断六轴左行车制动力与右行车制动力的大小
                                if (Convert.ToDouble(lzzxczd.Text) > Convert.ToDouble(lzyxczd.Text))
                                {
                                    double m = a / Convert.ToDouble(lzzxczd.Text) * 100;
                                    ldzbphv.Text = m.ToString("0.0");//为六轴不平衡率赋值
                                }
                                else
                                {
                                    double m = a / Convert.ToDouble(lzyxczd.Text) * 100;
                                    ldzbphv.Text = m.ToString("0.0");//为六轴不平衡率赋值
                                }
                            }
                        }
                        else
                        {
                            if (lzzlh.Text != "" && lzzlh.Text != "-" && lzzlh.Text != "0" && lzylh.Text != "" && lzylh.Text != "-" && lzylh.Text != "0")
                            {
                                ldzbphv.Text = (a / (Convert.ToDouble(lzzlh.Text) + Convert.ToDouble(lzylh.Text)) * 100).ToString("0.0");//六轴不平衡率
                            }
                        }
                    }
                }
                SZXZjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
        //行改变事件
        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            try
            {
                ClearControl();
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                if (this.dataGridView1.SelectedRows.Count > 0)
                {
                    if (dataGridView1.SelectedRows[0].Cells["检测编号"].Value.ToString() != "")
                    {
                        conn.Open();
                        string str = string.Format("select * from Data_Modification where 检测次数='{0}' and 检测编号='{1}' and 检测时间='{2}'", dataGridView1.SelectedRows[0].Cells["检测次数"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["检测编号"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["检测时间"].Value.ToString());
                        SqlDataAdapter sda = new SqlDataAdapter(str, conn);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        conn.Close();
                        a++;
                        ClearText();
                        checkBox1.Checked = false;
                        checkBox2.Checked = false;
                        checkBox4.Checked = false;
                        Bind_vRegister(dt);//数据绑定

                        #region 整车判定
                        string zdhpj = "";
                        string szdhpj = "";
                        if (dt.Rows[0]["整车制动和评价"].ToString() == "1")
                        {
                            zdhpj = "○";
                        }
                        if (dt.Rows[0]["整车制动和评价"].ToString() == "0")
                        {
                            zdhpj = "×";
                        }
                        if (dt.Rows[0]["手制动和评价"].ToString() == "1")
                        {
                            szdhpj = "○";
                        }
                        if (dt.Rows[0]["手制动和评价"].ToString() == "0")
                        {
                            szdhpj = "×";
                        }
                        dcpd.Text = zdhpj + szdhpj;
                        #endregion
                        #region 一轴评价
                        string yzzdpj = "";
                        string yzbphpj = "";
                        string yzzzzvpj = "";
                        string yzyzzvpj = "";
                        if (dt.Rows[0]["一轴制动和评价"].ToString() == "1")
                        {
                            yzzdpj = "○";
                        }
                        if (dt.Rows[0]["一轴制动和评价"].ToString() == "0")
                        {
                            yzzdpj = "×";
                        }
                        if (dt.Rows[0]["一轴制动差评价"].ToString() == "1")
                        {
                            yzbphpj = "○";
                        }
                        if (dt.Rows[0]["一轴制动差评价"].ToString() == "0")
                        {
                            yzbphpj = "×";
                        }
                        if (dt.Rows[0]["一轴左拖滞比评价"].ToString() == "1")
                        {
                            yzzzzvpj = "○";
                        }
                        if (dt.Rows[0]["一轴左拖滞比评价"].ToString() == "0")
                        {
                            yzzzzvpj = "×";
                        }
                        if (dt.Rows[0]["一轴右拖滞比评价"].ToString() == "1")
                        {
                            yzyzzvpj = "○";
                        }
                        if (dt.Rows[0]["一轴右拖滞比评价"].ToString() == "0")
                        {
                            yzyzzvpj = "×";
                        }
                        yzpd.Text = yzzdpj + yzbphpj + yzzzzvpj + yzyzzvpj;
                        #endregion
                        #region 二轴评价 
                        string ezzdpj = "";
                        string ezbphpj = "";
                        string ezzzzvpj = "";
                        string ezyzzvpj = "";
                        string sdasf = dt.Rows[0]["二轴制动和评价"].ToString();
                        if (dt.Rows[0]["二轴制动和评价"].ToString() == "1")
                        {
                            ezzdpj = "○";
                        }
                        if (dt.Rows[0]["二轴制动和评价"].ToString() == "0")
                        {
                            ezzdpj = "×";
                        }
                        if (dt.Rows[0]["二轴制动差评价"].ToString() == "1")
                        {
                            ezbphpj = "○";
                        }
                        if (dt.Rows[0]["二轴制动差评价"].ToString() == "0")
                        {
                            ezbphpj = "×";
                        }
                        if (dt.Rows[0]["二轴左拖滞比评价"].ToString() == "1")
                        {
                            ezzzzvpj = "○";
                        }
                        if (dt.Rows[0]["二轴左拖滞比评价"].ToString() == "0")
                        {
                            ezzzzvpj = "×";
                        }
                        if (dt.Rows[0]["二轴右拖滞比评价"].ToString() == "1")
                        {
                            ezyzzvpj = "○";
                        }
                        if (dt.Rows[0]["二轴右拖滞比评价"].ToString() == "0")
                        {
                            ezyzzvpj = "×";
                        }
                        ezpd.Text = ezzdpj + ezbphpj + ezzzzvpj + ezyzzvpj;

                        #endregion
                        #region 三轴评价 
                        string szzdpj = "";
                        string szbphpj = "";
                        string szzzzvpj = "";
                        string szyzzvpj = "";
                        if (dt.Rows[0]["三轴制动和评价"].ToString() == "1")
                        {
                            szzdpj = "○";
                        }
                        if (dt.Rows[0]["三轴制动和评价"].ToString() == "0")
                        {
                            szzdpj = "×";
                        }
                        if (dt.Rows[0]["三轴制动差评价"].ToString() == "1")
                        {
                            szbphpj = "○";
                        }
                        if (dt.Rows[0]["三轴制动差评价"].ToString() == "0")
                        {
                            szbphpj = "×";
                        }
                        if (dt.Rows[0]["三轴左拖滞比评价"].ToString() == "1")
                        {
                            szzzzvpj = "○";
                        }
                        if (dt.Rows[0]["三轴左拖滞比评价"].ToString() == "0")
                        {
                            szzzzvpj = "×";
                        }
                        if (dt.Rows[0]["三轴右拖滞比评价"].ToString() == "1")
                        {
                            szyzzvpj = "○";
                        }
                        if (dt.Rows[0]["三轴右拖滞比评价"].ToString() == "0")
                        {
                            szyzzvpj = "×";
                        }
                        szpd.Text = szzdpj + szbphpj + szzzzvpj + szyzzvpj;

                        #endregion
                        #region 四轴评价 
                        string sizzdpj = "";
                        string sizbphpj = "";
                        string sizzzzvpj = "";
                        string sizyzzvpj = "";
                        if (dt.Rows[0]["四轴制动和评价"].ToString() == "1")
                        {
                            sizzdpj = "○";
                        }
                        if (dt.Rows[0]["四轴制动和评价"].ToString() == "0")
                        {
                            sizzdpj = "×";
                        }
                        if (dt.Rows[0]["四轴制动差评价"].ToString() == "1")
                        {
                            sizbphpj = "○";
                        }
                        if (dt.Rows[0]["四轴制动差评价"].ToString() == "0")
                        {
                            sizbphpj = "×";
                        }
                        if (dt.Rows[0]["四轴左拖滞比评价"].ToString() == "1")
                        {
                            sizzzzvpj = "○";
                        }
                        if (dt.Rows[0]["四轴左拖滞比评价"].ToString() == "0")
                        {
                            sizzzzvpj = "×";
                        }
                        if (dt.Rows[0]["四轴右拖滞比评价"].ToString() == "1")
                        {
                            sizyzzvpj = "○";
                        }
                        if (dt.Rows[0]["四轴右拖滞比评价"].ToString() == "0")
                        {
                            sizyzzvpj = "×";
                        }
                        sizpd.Text = sizzdpj + sizbphpj + sizzzzvpj + sizyzzvpj;

                        #endregion
                        #region 五轴评价 
                        string wzzdpj = "";
                        string wzbphpj = "";
                        string wzzzzvpj = "";
                        string wzyzzvpj = "";
                        if (dt.Rows[0]["五轴制动和评价"].ToString() == "1")
                        {
                            wzzdpj = "○";
                        }
                        if (dt.Rows[0]["五轴制动和评价"].ToString() == "0")
                        {
                            wzzdpj = "×";
                        }
                        if (dt.Rows[0]["五轴制动差评价"].ToString() == "1")
                        {
                            wzbphpj = "○";
                        }
                        if (dt.Rows[0]["五轴制动差评价"].ToString() == "0")
                        {
                            wzbphpj = "×";
                        }
                        if (dt.Rows[0]["五轴左拖滞比评价"].ToString() == "1")
                        {
                            wzzzzvpj = "○";
                        }
                        if (dt.Rows[0]["五轴左拖滞比评价"].ToString() == "0")
                        {
                            wzzzzvpj = "×";
                        }
                        if (dt.Rows[0]["五轴右拖滞比评价"].ToString() == "1")
                        {
                            wzyzzvpj = "○";
                        }
                        if (dt.Rows[0]["五轴右拖滞比评价"].ToString() == "0")
                        {
                            wzyzzvpj = "×";
                        }
                        wzpd.Text = wzzdpj + wzbphpj + wzzzzvpj + wzyzzvpj;

                        #endregion
                        #region 六轴评价 
                        string lzzdpj = "";
                        string lzbphpj = "";
                        string lzzzzvpj = "";
                        string lzyzzvpj = "";
                        if (dt.Rows[0]["六轴制动和评价"].ToString() == "1")
                        {
                            lzzdpj = "○";
                        }
                        if (dt.Rows[0]["六轴制动和评价"].ToString() == "0")
                        {
                            lzzdpj = "×";
                        }
                        if (dt.Rows[0]["六轴制动差评价"].ToString() == "1")
                        {
                            lzbphpj = "○";
                        }
                        if (dt.Rows[0]["六轴制动差评价"].ToString() == "0")
                        {
                            lzbphpj = "×";
                        }
                        if (dt.Rows[0]["六轴左拖滞比评价"].ToString() == "1")
                        {
                            lzzzzvpj = "○";
                        }
                        if (dt.Rows[0]["六轴左拖滞比评价"].ToString() == "0")
                        {
                            lzzzzvpj = "×";
                        }
                        if (dt.Rows[0]["六轴右拖滞比评价"].ToString() == "1")
                        {
                            lzyzzvpj = "○";
                        }
                        if (dt.Rows[0]["六轴右拖滞比评价"].ToString() == "0")
                        {
                            lzyzzvpj = "×";
                        }
                        lzpd.Text = lzzdpj + lzbphpj + lzzzzvpj + lzyzzvpj;

                        #endregion
                        #region 左外灯判定
                        string zwgqpj = "";
                        string zwycpj = "";
                        string zwjcpj = "";
                        string zwyspj = "";
                        string zwjspj = "";
                        if (dt.Rows[0]["左主远光强度评价"].ToString() == "1")
                        {
                            zwgqpj = "○";
                        }
                        if (dt.Rows[0]["左主远光强度评价"].ToString() == "0")
                        {
                            zwgqpj = "×";
                        }
                        if (dt.Rows[0]["左主远光上下偏差评价"].ToString() == "1")
                        {
                            zwycpj = "○";
                        }
                        if (dt.Rows[0]["左主远光上下偏差评价"].ToString() == "0")
                        {
                            zwycpj = "×";
                        }
                        if(dt.Rows[0]["左主远光左右偏差评价"].ToString()=="1")
                        {
                            zwyspj = "○";
                        }
                        if (dt.Rows[0]["左主远光左右偏差评价"].ToString() == "0")
                        {
                            zwyspj = "×";
                        }
                        if (dt.Rows[0]["左近光上下偏差评价"].ToString() == "1")
                        {
                            zwjcpj = "○";
                        }
                        if (dt.Rows[0]["左近光上下偏差评价"].ToString() == "0")
                        {
                            zwjcpj = "×";
                        }
                        if (dt.Rows[0]["左近光左右偏差评价"].ToString() == "1")
                        {
                            zwjspj = "○";
                        }
                        if (dt.Rows[0]["左近光左右偏差评价"].ToString() == "0")
                        {
                            zwjspj = "×";
                        }
                        zwpd.Text = zwgqpj + zwycpj + zwyspj + zwjcpj + zwjspj;
                        #endregion
                        #region 左内灯判定
                        string zngqpj = "";
                        string znycpj = "";
                        string znjcpj = "";
                        string znyspj = "";
                        string znjspj = "";
                        if (znyggq.Text != "" && znyggq.Text != "-")
                        {
                            if (dt.Rows[0]["左副远光强度评价"].ToString() == "1")
                            {
                                zngqpj = "○";
                            }
                            if (dt.Rows[0]["左副远光强度评价"].ToString() == "0")
                            {
                                zngqpj = "×";
                            }
                            if (dt.Rows[0]["左副远光上下偏差评价"].ToString() == "1")
                            {
                                znycpj = "○";
                            }
                            if (dt.Rows[0]["左副远光上下偏差评价"].ToString() == "0")
                            {
                                znycpj = "×";
                            }
                            if (dt.Rows[0]["左近光上下偏差评价"].ToString() == "1")
                            {
                                znjcpj = "○";
                            }
                            if (dt.Rows[0]["左近光上下偏差评价"].ToString() == "0")
                            {
                                znjcpj = "×";
                            }

                            if (dt.Rows[0]["左副远光左右偏差评价"].ToString() == "1")
                            {
                                znyspj = "○";
                            }
                            if (dt.Rows[0]["左副远光左右偏差评价"].ToString() == "0")
                            {
                                znyspj = "×";
                            }
                            if (dt.Rows[0]["左近光左右偏差评价"].ToString() == "1")
                            {
                                znjspj = "○";
                            }
                            if (dt.Rows[0]["左近光左右偏差评价"].ToString() == "0")
                            {
                                znjspj = "×";
                            }
                            znpd.Text = zngqpj + znycpj + znyspj + znjcpj + znjspj;
                        }
                        #endregion
                        #region 右内灯判定
                        string yngqpj = "";
                        string ynycpj = "";
                        string ynjcpj = "";
                        string ynyspj = "";
                        string ynjspj = "";
                        if (ynyggq.Text != "" && ynyggq.Text != "-")
                        {
                            if (dt.Rows[0]["右副远光强度评价"].ToString() == "1")
                            {
                                yngqpj = "○";
                            }
                            if (dt.Rows[0]["右副远光强度评价"].ToString() == "0")
                            {
                                yngqpj = "×";
                            }
                            if (dt.Rows[0]["右副远光上下偏差评价"].ToString() == "1")
                            {
                                ynycpj = "○";
                            }
                            if (dt.Rows[0]["右副远光上下偏差评价"].ToString() == "0")
                            {
                                ynycpj = "×";
                            }
                            if (dt.Rows[0]["右近光上下偏差评价"].ToString() == "1")
                            {
                                ynjcpj = "○";
                            }
                            if (dt.Rows[0]["右近光上下偏差评价"].ToString() == "0")
                            {
                                ynjcpj = "×";
                            }

                            if (dt.Rows[0]["右副远光左右偏差评价"].ToString() == "1")
                            {
                                ynyspj = "○";
                            }
                            if (dt.Rows[0]["右副远光左右偏差评价"].ToString() == "0")
                            {
                                ynyspj = "×";
                            }
                            if (dt.Rows[0]["右近光左右偏差评价"].ToString() == "1")
                            {
                                ynjspj = "○";
                            }
                            if (dt.Rows[0]["右近光左右偏差评价"].ToString() == "0")
                            {
                                ynjspj = "×";
                            }
                            ynpd.Text = yngqpj + ynycpj + ynyspj + ynjcpj + ynjspj;
                        }
                        #endregion
                        #region 右外灯判定
                        string ywgqpj = "";
                        string ywycpj = "";
                        string ywjcpj = "";
                        string ywyspj = "";
                        string ywjspj = "";
                        if (dt.Rows[0]["右主远光强度评价"].ToString() == "1")
                        {
                            ywgqpj = "○";
                        }
                        if (dt.Rows[0]["右主远光强度评价"].ToString() == "0")
                        {
                            ywgqpj = "×";
                        }
                        if (dt.Rows[0]["右主远光上下偏差评价"].ToString() == "1")
                        {
                            ywycpj = "○";
                        }
                        if (dt.Rows[0]["右主远光上下偏差评价"].ToString() == "0")
                        {
                            ywycpj = "×";
                        }
                        if (dt.Rows[0]["右近光上下偏差评价"].ToString() == "1")
                        {
                            ywjcpj = "○";
                        }
                        if (dt.Rows[0]["右近光上下偏差评价"].ToString() == "0")
                        {
                            ywjcpj = "×";
                        }

                        if (dt.Rows[0]["右主远光左右偏差评价"].ToString() == "1")
                        {
                            ywyspj = "○";
                        }
                        if (dt.Rows[0]["右主远光左右偏差评价"].ToString() == "0")
                        {
                            ywyspj = "×";
                        }
                        if (dt.Rows[0]["右近光左右偏差评价"].ToString() == "1")
                        {
                            ywjspj = "○";
                        }
                        if (dt.Rows[0]["右近光左右偏差评价"].ToString() == "0")
                        {
                            ywjspj = "×";
                        }
                        ywpd.Text = ywgqpj + ywycpj + ywyspj + ywjcpj + ywjspj;
                        #endregion
                        #region 悬架
                        string xqpj = "";
                        string xqcpj = "";
                        string xhpj = "";
                        string xhcpj = "";
                        if (dt.Rows[0]["悬架前轴吸收率评价"].ToString() == "0")
                        {
                            xqpj = "×";
                        }
                        if (dt.Rows[0]["悬架前轴吸收率评价"].ToString() == "1")
                        {
                            xqpj = "○";
                        }
                        if (dt.Rows[0]["悬架前轴吸收率差评价"].ToString() == "0")
                        {
                            xqcpj = "×";
                        }
                        if (dt.Rows[0]["悬架前轴吸收率差评价"].ToString() == "1")
                        {
                            xqcpj = "○";
                        }
                        xjqzpd.Text = xqpj + xqcpj;
                        if (dt.Rows[0]["悬架后轴吸收率评价"].ToString() == "0")
                        {
                            xhpj = "×";
                        }
                        if (dt.Rows[0]["悬架后轴吸收率评价"].ToString() == "1")
                        {
                            xhpj = "○";
                        }
                        if (dt.Rows[0]["悬架后轴吸收率差评价"].ToString() == "0")
                        {
                            xhcpj = "×";
                        }
                        if (dt.Rows[0]["悬架后轴吸收率差评价"].ToString() == "1")
                        {
                            xhcpj = "○";
                        }
                        xjhzpd.Text = xhpj + xhcpj;
                        #endregion
                        #region 车速喇叭与侧滑
                        if (dt.Rows[0]["车速评价"].ToString() == "0")
                        {
                            cspd.Text = "×";
                        }
                        if (dt.Rows[0]["车速评价"].ToString() == "1")
                        {
                            cspd.Text = "○";
                        }
                        if (dt.Rows[0]["侧滑评价"].ToString() == "0")
                        {
                            chpd.Text = "×";
                        }
                        if (dt.Rows[0]["侧滑评价"].ToString() == "1")
                        {
                            chpd.Text = "○";
                        }
                        if (dt.Rows[0]["喇叭声级评价"].ToString() == "0")
                        {
                            lbpd.Text = "×";
                        }
                        if (dt.Rows[0]["喇叭声级评价"].ToString() == "1")
                        {
                            lbpd.Text = "○";
                        }
                        #endregion
                        #region 排放性
                        string gcopj = "";
                        string ghcpj = "";
                        string gλpj = "";
                        string dcopj = "";
                        string dhcpj = "";
                        if (dt.Rows[0]["怠速CO评价"].ToString() == "0")
                        {
                            dcopj = "×";
                        }
                        if (dt.Rows[0]["怠速CO评价"].ToString() == "1")
                        {
                            dcopj = "○";
                        }
                        if (dt.Rows[0]["怠速HC评价"].ToString() == "0")
                        {
                            dhcpj = "×";
                        }
                        if (dt.Rows[0]["怠速HC评价"].ToString() == "1")
                        {
                            dhcpj = "○";
                        }
                        if (dt.Rows[0]["双怠速CO评价"].ToString() == "0")
                        {
                            gcopj = "×";
                        }
                        if (dt.Rows[0]["双怠速CO评价"].ToString() == "1")
                        {
                            gcopj = "○";
                        }
                        if (dt.Rows[0]["双怠速HC评价"].ToString() == "0")
                        {
                            ghcpj = "×";
                        }
                        if (dt.Rows[0]["双怠速HC评价"].ToString() == "1")
                        {
                            ghcpj = "○";
                        }
                        if (dt.Rows[0]["光吸收率评价"].ToString() == "0")
                        {
                            cypd.Text = "×";
                        }
                        if (dt.Rows[0]["光吸收率评价"].ToString() == "1")
                        {
                            cypd.Text = "○";
                        }
                        if (dt.Rows[0]["空气过量系数评价"].ToString() == "0")
                        {
                            gλpj = "×";
                        }
                        if (dt.Rows[0]["空气过量系数评价"].ToString() == "1")
                        {
                            gλpj = "○";
                        }
                        qypd.Text = gcopj + ghcpj + gλpj + dcopj + dhcpj;
                        #endregion
                        #region 动力经济性
                        if (dt.Rows[0]["百公里油耗评价"].ToString() == "0")
                        {
                            jjxpd.Text = "×";
                        }
                        if (dt.Rows[0]["百公里油耗评价"].ToString() == "1")
                        {
                            jjxpd.Text = "○";
                        }
                        if (dt.Rows[0]["燃油类型"].ToString().Replace(" ", "").ToString().Contains("汽油"))
                        {
                            if (dt.Rows[0]["额定扭矩工况评价"].ToString() == "0")
                            {
                                dlxpd.Text = "×";
                            }
                            if (dt.Rows[0]["额定扭矩工况评价"].ToString() == "1")
                            {
                                dlxpd.Text = "○";
                            }
                        }
                        if (dt.Rows[0]["燃油类型"].ToString().Replace(" ", "").ToString().Contains("柴油"))
                        {
                            if (dt.Rows[0]["额定功率工况评价"].ToString() == "0")
                            {
                                dlxpd.Text = "×";
                            }
                            if (dt.Rows[0]["额定功率工况评价"].ToString() == "1")
                            {
                                dlxpd.Text = "○";
                            }
                        }
                        #endregion
                        SDSjudge();
                        Exitpd();
                    }
                    else
                    {
                        MessageBox.Show("此车还未检测完成");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        ToolTip tp = new ToolTip();

        public bool ReceiveEventFlag { get; private set; }
        public double ticksPerSecond { get; private set; }
       
        #region 阻滞率
        public void ZZVjudge()
        {
            yzzzv.Visible = false;
            ezzzv.Visible = false;
            szzzv.Visible = false;
            sizzzv.Visible = false;
            wzzzv.Visible = false;
            lzzzv.Visible = false;
            yyzzv.Visible = false;
            eyzzv.Visible = false;
            syzzv.Visible = false;
            siyzzv.Visible = false;
            wyzzv.Visible = false;
            lyzzv.Visible = false;
            string yzzzzv = ydzzzzv.Text;//一轴左阻滞率
            string yzyzzv = ydzyzzv.Text;//一轴右阻滞率
            string ezzzzv = edzzzzv.Text;//二轴左阻滞率
            string ezyzzv = edzyzzv.Text;//二轴右阻滞率
            string szzzzv = sdzzzzv.Text;//三轴左阻滞率
            string szyzzv = sdzyzzv.Text;//三轴右阻滞率
            string sizzzzv = sidzzzzv.Text;//四轴左阻滞率
            string sizyzzv = sidzyzzv.Text;//四轴右阻滞率
            string wzzzzv = wdzzzzv.Text;//五轴左阻滞率
            string wzyzzv = wdzyzzv.Text;//五轴右阻滞率
            string lzzzzv = ldzzzzv.Text;//六轴左阻滞率
            string lzyzzv = ldzyzzv.Text;//六轴右阻滞率
            #region 阻滞率判定
            //一轴左阻滞率判定
            if (yzzzzv != "" && yzzzzv != "-")
            {
                if (Convert.ToDouble(yzzzzv) <= 3.5)
                {
                    yzzzv.Visible = false;
                }
                else
                {
                    yzzzv.Visible = true;
                }
            }
            //一轴右阻滞率判定
            if (yzyzzv != "" && yzyzzv != "-")
            {
                if (Convert.ToDouble(yzyzzv) <= 3.5)
                {
                    yyzzv.Visible = false;
                }
                else
                {
                    yyzzv.Visible = true;
                }
            }
            //二轴左阻滞率判定
            if (ezzzzv != "" && ezzzzv != "-")
            {
                if (Convert.ToDouble(ezzzzv) <= 3.5)
                {
                    ezzzv.Visible = false;
                }
                else
                {
                    ezzzv.Visible = true;
                }
            }
            //二轴右阻滞率判定
            if (ezyzzv != "" && ezyzzv != "-")
            {
                if (Convert.ToDouble(ezyzzv) <= 3.5)
                {
                    eyzzv.Visible = false;
                }
                else
                {
                    eyzzv.Visible = true;
                }
            }
            //三轴左阻滞率判定
            if (szzzzv != "" && szzzzv != "-")
            {
                if (Convert.ToDouble(szzzzv) <= 3.5)
                {
                    szzzv.Visible = false;
                }
                else
                {
                    szzzv.Visible = true;
                }
            }
            //三轴右阻滞率判定
            if (szyzzv != "" && szyzzv != "-")
            {
                if (Convert.ToDouble(szyzzv) <= 3.5)
                {
                    syzzv.Visible = false;
                }
                else
                {
                    syzzv.Visible = true;
                }
            }
            //四轴左阻滞率判定
            if (sizzzzv != "" && sizzzzv != "-")
            {
                if (Convert.ToDouble(sizzzzv) <= 3.5)
                {
                    sizzzv.Visible = false;
                }
                else
                {
                    sizzzv.Visible = true;
                }
            }
            //四轴右阻滞率判定
            if (sizyzzv != "" && sizyzzv != "-")
            {
                if (Convert.ToDouble(sizyzzv) <= 3.5)
                {
                    siyzzv.Visible = false;
                }
                else
                {
                    siyzzv.Visible = true;
                }
            }
            //五轴左阻滞率判定
            if (wzzzzv != "" && wzzzzv != "-")
            {
                if (Convert.ToDouble(wzzzzv) <= 3.5)
                {
                    wzzzv.Visible = false;
                }
                else
                {
                    wzzzv.Visible = true;
                }
            }
            //五轴右阻滞率判定
            if (wzyzzv != "" && wzyzzv != "-")
            {
                if (Convert.ToDouble(wzyzzv) <= 3.5)
                {
                    wyzzv.Visible = false;
                }
                else
                {
                    wyzzv.Visible = true;
                }
            }
            //六轴左阻滞率判定
            if (lzzzzv != "" && lzzzzv != "-")
            {
                if (Convert.ToDouble(lzzzzv) <= 3.5)
                {
                    lzzzv.Visible = false;
                }
                else
                {
                    lzzzv.Visible = true;
                }
            }
            //六轴右阻滞率判定
            if (lzyzzv != "" && lzyzzv != "-")
            {
                if (Convert.ToDouble(lzyzzv) <= 3.5)
                {
                    lyzzv.Visible = false;
                }
                else
                {
                    lyzzv.Visible = true;
                }
            }
            #endregion
            
            CLDEPD();
        }
        //一轴左阻滞率超出限值鼠标经过事件
        private void yzzzv_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("一轴左阻滞率值超出标准限值范围", yzzzv, 10000);
        }
        //一轴左阻滞率超出限值鼠标离开事件
        private void yzzzv_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(yzzzv);
        }
        //一轴右阻滞率超出限值鼠标经过事件
        private void yyzzv_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("一轴右阻滞率值超出标准限值范围", yyzzv, 10000);
        }
        //一轴右阻滞率超出限值鼠标离开事件
        private void yyzzv_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(yyzzv);
        }
        //二轴左阻滞率超出限值鼠标经过事件
        private void ezzzv_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("二轴左阻滞率值超出标准限值范围", ezzzv, 10000);
        }
        //二轴左阻滞率超出限值鼠标离开事件
        private void ezzzv_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(ezzzv);
        }
        //二轴右阻滞率超出限值鼠标经过事件
        private void eyzzv_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("二轴右阻滞率值超出标准限值范围", eyzzv, 10000);
        }
        //二轴右阻滞率超出限值鼠标离开事件
        private void eyzzv_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(eyzzv);
        }
        //三轴左阻滞率超出限值鼠标经过事件
        private void szzzv_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("三轴左阻滞率值超出标准限值范围", szzzv, 10000);
        }
        //三轴左阻滞率超出限值鼠标离开事件
        private void szzzv_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(szzzv);
        }
        //三轴右阻滞率超出限值鼠标经过事件
        private void syzzv_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("三轴右阻滞率值超出标准限值范围", syzzv, 10000);
        }
        //三轴右阻滞率超出限值鼠标离开事件
        private void syzzv_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(syzzv);
        }
        //四轴左阻滞率超出限值鼠标经过事件
        private void sizzzv_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("四轴左阻滞率值超出标准限值范围", sizzzv, 10000);
        }
        //四轴左阻滞率超出限值鼠标离开事件
        private void sizzzv_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(sizzzv);
        }
        //四轴右阻滞率超出限值鼠标经过事件
        private void siyzzv_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("四轴右阻滞率值超出标准限值范围", siyzzv, 10000);
        }
        //四轴右阻滞率超出限值鼠标离开事件
        private void siyzzv_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(siyzzv);
        }
        //五轴左阻滞率超出限值鼠标经过事件
        private void wzzzv_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("五轴左阻滞率值超出标准限值范围", wzzzv, 10000);
        }
        //五轴左阻滞率超出限值鼠标离开事件
        private void wzzzv_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(wzzzv);
        }
        //五轴右阻滞率超出限值鼠标经过事件
        private void wyzzv_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("五轴右阻滞率值超出标准限值范围", wyzzv, 10000);
        }
        //五轴右阻滞率超出限值鼠标离开事件
        private void wyzzv_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(wyzzv);
        }
        //六轴左阻滞率超出限值鼠标经过事件
        private void lzzzv_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("六轴左阻滞率值超出标准限值范围", lzzzv, 10000);
        }
        //六轴左阻滞率超出限值鼠标离开事件
        private void lzzzv_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(lzzzv);
        }
        //六轴右阻滞率超出限值鼠标经过事件
        private void lyzzv_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("六轴右阻滞率值超出标准限值范围", lyzzv, 10000);
        }
        //六轴右阻滞率超出限值鼠标离开事件
        private void lyzzv_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(lyzzv);
        }
        #endregion

        #region 整车制动率判断
        public void DCZDjudge()
        {
            dczd.Visible = false;
            string sdczd = dczdl.Text;//整车制动率
            //string sdczd = dt.Rows[0]["整车制动和值"].ToString();
            double ddczd;
           
            if (sdczd == ""|| sdczd=="-")
            {
                ddczd = 0;
            }
            else
            {
                ddczd = Convert.ToDouble(sdczd);               
            }
            if (sdczd != "" && sdczd != "-"&&ddczd < 60 )
            {
                dczd.Visible = true;
            }
            else
            {
                dczd.Visible = false;
            }
            CLDEPD();
        }
        //整车制动率超出限值鼠标经过事件
        private void dczd_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("整车制动率值超出标准限值范围", dczd,10000);
        }
        //整车制动率超出限值鼠标离开事件
        private void dczd_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(dczd);
        }
        #endregion

        #region 驻车制动率判断
        //驻车制动标准判断
        public void ZCZDjudge()
        {

            zczdbd.Visible = false;
            string szczds = dczczdl.Text;//驻车制动率
            string szczd = dcspcz.Text;//整车轴重
            string szzl = zzl.Text;//总质量
            string szbzl = zbzl.Text;//整备质量
            double dzczdl;//驻车制动率
            double dzzzl;//总质量
            double dzczz;//整车轴重
            double dzbzl;//整备质量
            #region 判断变量值是否存在
            //判断整车轴重值是否存在
            if (szczd == "" || szczd == "-")
            {
                dzczz = 0;
            }
            else
            {
                dzczz = Convert.ToDouble(szczd);
            }
            //判断总质量值是否存在
            if (szzl == "" || szzl == "-")
            {
                dzzzl = 0;
            }
            else
            {
                dzzzl = Convert.ToDouble(szzl);
            }
            //判断整备质量值是否存在
            if (szbzl == "" || szbzl == "-")
            {
                dzbzl = 0;
            }
            else
            {
                dzbzl = Convert.ToDouble(szbzl);
            }
            //判断驻车制动率是否有值
            if (szczds == ""|| szczds=="-")
            {
                dzczdl = 0;
            }
            else
            {
                dzczdl = Convert.ToDouble(szczds);
            }
            #endregion
            #region 判断驻车制动率
            ////总质量为整备质量的1.2倍以下
            //if (dzzzl < dzbzl * 1.2)
            //{
            //    //驻车制动率小于整车轴重的15%
            //    if (dzczdl < dzczz * 0.15)
            //    {
            //        zczdbd.Visible = true;
            //    }
            //    else
            //    {
            //        zczdbd.Visible = false;
            //    }
            //}
            //else
            //{
            //    //驻车制动率小于整车轴重的20%
            //    if (dzczdl < dzczz * 0.2)
            //    {
            //        zczdbd.Visible = true;
            //    }
            //    else
            //    {
            //        zczdbd.Visible = false;
            //    }
            //}
            #endregion

            //驻车制动率小于20
            if (szczds != "" && szczds != "-"&&dzczdl < 20 )
            {
                zczdbd.Visible = true;
            }
            else
            {
                zczdbd.Visible = false;
            }
            CLDEPD();
        }
        //鼠标经过事件
        private void zczdbd_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("驻车制动力值超出标准范围", zczdbd,10000);
        }
        //鼠标离开事件
        private void zczdbd_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(zczdbd);
        }
        #endregion

        #region 轴制动率判断
        //判断是否为双转向轴的M1、N1类车
        public void M1N1judge()
        {
            ydzpd.Visible = false;
            edzpd.Visible = false;
            sdzpd.Visible = false;
            sidzpd.Visible = false;
            wdzpd.Visible = false;
            ldzpd.Visible = false;
            try
            {
                #region 对轴制动率的判断
                double szxz;
                double yzzdv;
                double ezzdv;
                double szzdv;
                double sizzdv;
                double wzzdv;
                double lzzdv;
                string zszss = zxzs.Text;//转向轴数
                string ydzzdvs = ydzzdv.Text;//一轴制动率
                string edzzdvs = edzzdv.Text;//二轴制动率
                string sdzzdvs = sdzzdv.Text;//三轴制动率
                string sidzzdvs = sidzzdv.Text;//四轴制动率
                string wdzzdvs = wdzzdv.Text;//五轴制动率
                string ldzzdvs = ldzzdv.Text;//六轴制动率
                //string zszss = dt.Rows[0]["双转向轴"].ToString();
                //string ydzzdvs = dt.Rows[0]["一轴制动和值"].ToString();
                //string edzzdvs = dt.Rows[0]["二轴制动和值"].ToString();
                //string sdzzdvs = dt.Rows[0]["三轴制动和值"].ToString();
                //string sidzzdvs = dt.Rows[0]["四轴制动和值"].ToString();
                //string wdzzdvs = dt.Rows[0]["五轴制动和值"].ToString();
                //string ldzzdvs = dt.Rows[0]["六轴制动和值"].ToString();
                if (zszss == ""|| zszss=="-")
                {
                    szxz = 0;
                }
                else
                {
                    szxz = Convert.ToDouble(zszss);
                }
                if (ydzzdvs == ""|| ydzzdvs=="")
                {
                    yzzdv = 0;
                }
                else
                {
                    yzzdv= Convert.ToDouble(ydzzdvs);
                }
                if (edzzdvs == ""|| edzzdvs=="-")
                {
                    ezzdv = 0;
                }
                else
                {
                    ezzdv = Convert.ToDouble(edzzdvs);
                }
                if (sdzzdvs == ""|| sdzzdvs=="-")
                {
                    szzdv = 0;
                }
                else
                {
                    szzdv = Convert.ToDouble(sdzzdvs);
                }
                if (sidzzdvs == ""|| sidzzdvs=="")
                {
                    sizzdv = 0;
                }
                else
                {
                    sizzdv = Convert.ToDouble(sidzzdvs);
                }
                if (wdzzdvs == ""|| wdzzdvs=="-")
                {
                    wzzdv = 0;
                }
                else
                {
                    wzzdv = Convert.ToDouble(wdzzdvs);
                }
                if (ldzzdvs == ""|| ldzzdvs=="-")
                {
                     lzzdv = 0;
                }
                else
                {
                    lzzdv = Convert.ToDouble(ldzzdvs);
                }
                #endregion
                //判断该车辆为为单轴车辆
                if (szxz == 1)
                {
                    #region 单转向轴
                    //如果一轴制动率小于60
                    if (ydzzdvs != "" && ydzzdvs != "-"&&yzzdv < 60)
                    {
                        ydzpd.Visible = true;
                    }
                    else
                    {
                        ydzpd.Visible = false;
                    }
                    //如果二轴制动率小于20
                    if (edzzdvs != "" && edzzdvs != "-"&&ezzdv < 20)
                    {
                        edzpd.Visible = true;
                    }
                    else
                    {
                        edzpd.Visible = false;
                    }
                    //如果三轴制动率小于20
                    if (sdzzdvs != "" && sdzzdvs != "-"&&szzdv < 20)
                    {
                        sdzpd.Visible = true;
                    }
                    else
                    {
                        sdzpd.Visible = false;
                    }
                    //如果四轴制动率小于20
                    if (sidzzdvs != "" && sidzzdvs != "-"&&sizzdv < 20)
                    {
                        sidzpd.Visible = true;
                    }
                    else
                    {
                        sidzpd.Visible = false;
                    }
                    //如果五轴制动率小于20
                    if (wdzzdvs != "" && wdzzdvs != "-"&&wzzdv < 20)
                    {
                        wdzpd.Visible = true;
                    }
                    else
                    {
                        wdzpd.Visible = false;
                    }
                    //如果六轴制动率小于20
                    if (ldzzdvs != "" && ldzzdvs != "-"&&lzzdv < 20)
                    {
                        ldzpd.Visible = true;
                    }
                    else
                    {
                        ldzpd.Visible = false;
                    }
                    #endregion
                }
                else
                {
                    #region 双转向轴
                    //如果一轴制动率小于60
                    if (ydzzdvs != "" && ydzzdvs != "-"&&yzzdv < 60)
                    {
                        ydzpd.Visible = true;
                    }
                    else
                    {
                        ydzpd.Visible = false;
                    }
                    //如果二轴制动率小于60
                    if (edzzdvs != "" && edzzdvs != "-"&&ezzdv < 60)
                    {
                        edzpd.Visible = true;
                    }
                    else
                    {
                        edzpd.Visible = false;
                    }
                    //如果三轴制动率小于20
                    if (sdzzdvs != "" && sdzzdvs != "-"&&szzdv < 20)
                    {
                        sdzpd.Visible = true;
                    }
                    else
                    {
                        sdzpd.Visible = false;
                    }
                    //如果四轴制动率小于20
                    if (sidzzdvs != "" && sidzzdvs != "-"&&sizzdv < 20)
                    {
                        sidzpd.Visible = true;
                    }
                    else
                    {
                        sidzpd.Visible = false;
                    }
                    //如果五轴制动率小于20
                    if (wdzzdvs != "" && wdzzdvs != "-"&&wzzdv < 20)
                    {
                        wdzpd.Visible = true;
                    }
                    else
                    {
                        wdzpd.Visible = false;
                    }
                    //如果六轴制动率小于20
                    if (ldzzdvs != "" && ldzzdvs != "-"&&lzzdv < 20)
                    {
                        ldzpd.Visible = true;
                    }
                    else
                    {
                        ldzpd.Visible = false;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
        //判断是否为双转向轴的N2、N3类车
        public void N23judge()
        {
            ydzpd.Visible = false;
            edzpd.Visible = false;
            sdzpd.Visible = false;
            sidzpd.Visible = false;
            wdzpd.Visible = false;
            ldzpd.Visible = false;
            #region 对轴制动率的判断
            double szxz;
            double yzzdv;
            double ezzdv;
            double szzdv;
            double sizzdv;
            double wzzdv;
            double lzzdv;
            string zszss = zxzs.Text;//总质量
            string ydzzdvs = ydzzdv.Text;//一轴制动率
            string edzzdvs = edzzdv.Text;//二轴制动率
            string sdzzdvs = sdzzdv.Text;//三轴制动率
            string sidzzdvs = sidzzdv.Text;//四轴制动率
            string wdzzdvs = wdzzdv.Text;//五轴制动率
            string ldzzdvs = ldzzdv.Text;//六轴制动率
            if (zszss == ""|| zszss=="-")
            {
                szxz = 0;
            }
            else
            {
                szxz = Convert.ToDouble(zszss);
            }
            if (ydzzdvs == ""|| ydzzdvs=="-")
            {
                yzzdv = 0;
            }
            else
            {
                yzzdv = Convert.ToDouble(ydzzdvs);
            }
            if (edzzdvs == ""|| edzzdvs=="-")
            {
                ezzdv = 0;
            }
            else
            {
                ezzdv = Convert.ToDouble(edzzdvs);
            }
            if (sdzzdvs == ""|| sdzzdvs=="-")
            {
                szzdv = 0;
            }
            else
            {
                szzdv = Convert.ToDouble(sdzzdvs);
            }
            if (sidzzdvs == ""|| sidzzdvs=="-")
            {
                sizzdv = 0;
            }
            else
            {
                sizzdv = Convert.ToDouble(sidzzdvs);
            }
            if (wdzzdvs == ""|| wdzzdvs=="-")
            {
                wzzdv = 0;
            }
            else
            {
                wzzdv = Convert.ToDouble(wdzzdvs);
            }
            if (ldzzdvs == ""|| ldzzdvs=="-")
            {
                lzzdv = 0;
            }
            else
            {
                lzzdv = Convert.ToDouble(ldzzdvs);
            }
            #endregion
            //判断该车辆为为单轴车辆
            if (szxz == 1)
            {
                #region 单转向轴
                //如果一轴制动率小于60
                if (ydzzdvs != "" && ydzzdvs != "-"&&yzzdv < 60)
                {
                    ydzpd.Visible = true;
                }
                else
                {
                    ydzpd.Visible = false;
                }
                //如果二轴制动率小于50
                if (edzzdvs != "" && edzzdvs != "-"&&ezzdv < 50)
                {
                    edzpd.Visible = true;
                }
                else
                {
                    edzpd.Visible = false;
                }
                //如果三轴制动率小于50
                if (sdzzdvs != "" && sdzzdvs != "-"&&szzdv < 50)
                {
                    sdzpd.Visible = true;
                }
                else
                {
                    sdzpd.Visible = false;
                }
                //如果四轴制动率小于50
                if (sidzzdvs != "" && sidzzdvs != "-"&&sizzdv < 50)
                {
                    sidzpd.Visible = true;
                }
                else
                {
                    sidzpd.Visible = false;
                }
                //如果五轴制动率小于50
                if (wdzzdvs != "" && wdzzdvs != "-"&&wzzdv < 50)
                {
                    wdzpd.Visible = true;
                }
                else
                {
                    wdzpd.Visible = false;
                }
                //如果六轴制动率小于50
                if (ldzzdvs != "" && ldzzdvs != "-"&&lzzdv < 50)
                {
                    ldzpd.Visible = true;
                }
                else
                {
                    ldzpd.Visible = false;
                }
                #endregion
            }
            else
            {
                #region 双转向轴
                //如果一轴制动率小于60
                if ( ydzzdvs != "" && ydzzdvs != "-"&&yzzdv < 60 )
                {
                    ydzpd.Visible = true;
                }
                else
                {
                    ydzpd.Visible = false;
                }
                //如果二轴制动率小于60
                if (edzzdvs != "" && edzzdvs != "-"&&ezzdv < 60  )
                {
                    edzpd.Visible = true;
                }
                else
                {
                    edzpd.Visible = false;
                }
                //如果三轴制动率小于50
                if (sdzzdvs != "" && sdzzdvs != "-"&&szzdv < 50 )
                {
                    sdzpd.Visible = true;
                }
                else
                {
                    sdzpd.Visible = false;
                }
                //如果四轴制动率小于50
                if (sidzzdvs != "" && sidzzdvs != "-"&&sizzdv < 50  )
                {
                    sidzpd.Visible = true;
                }
                else
                {
                    sidzpd.Visible = false;
                }
                //如果五轴制动率小于50
                if (wdzzdvs != "" && wdzzdvs != "-"&&wzzdv < 50  )
                {
                    wdzpd.Visible = true;
                }
                else
                {
                    wdzpd.Visible = false;
                }
                //如果六轴制动率小于50
                if (ldzzdvs != "" && ldzzdvs != "-"&&lzzdv < 50  )
                {
                    ldzpd.Visible = true;
                }
                else
                {
                    ldzpd.Visible = false;
                }
                #endregion
            }
        }
        //判断是否为双转向轴的M2、M3类车
        public void M23judge()
        {
            ydzpd.Visible = false;
            edzpd.Visible = false;
            sdzpd.Visible = false;
            sidzpd.Visible = false;
            wdzpd.Visible = false;
            ldzpd.Visible = false;
            #region 对轴制动率的判断
            double szxz;
            double yzzdv;
            double ezzdv;
            double szzdv;
            double sizzdv;
            double wzzdv;
            double lzzdv;
            string zszss = zxzs.Text;//转向轴数
            string ydzzdvs = ydzzdv.Text;//一轴制动率
            string edzzdvs = edzzdv.Text;//二轴制动率
            string sdzzdvs = sdzzdv.Text;//三轴制动率
            string sidzzdvs = sidzzdv.Text;//四轴制动率
            string wdzzdvs = wdzzdv.Text;//五轴制动率
            string ldzzdvs = ldzzdv.Text;//六轴制动率
            if (zszss == ""|| zszss=="-")
            {
                szxz = 0;
            }
            else
            {
                szxz = Convert.ToDouble(zszss);
            }
            if (ydzzdvs == ""|| ydzzdvs=="-")
            {
                yzzdv = 0;
            }
            else
            {
                yzzdv = Convert.ToDouble(ydzzdvs);
            }
            if (edzzdvs == ""|| edzzdvs=="-")
            {
                ezzdv = 0;
            }
            else
            {
                ezzdv = Convert.ToDouble(edzzdvs);
            }
            if (sdzzdvs == ""|| sdzzdvs=="-")
            {
                szzdv = 0;
            }
            else
            {
                szzdv = Convert.ToDouble(sdzzdvs);
            }
            if (sidzzdvs == ""|| sidzzdvs=="-")
            {
                sizzdv = 0;
            }
            else
            {
                sizzdv = Convert.ToDouble(sidzzdvs);
            }
            if (wdzzdvs == ""|| wdzzdvs=="-")
            {
                wzzdv = 0;
            }
            else
            {
                wzzdv = Convert.ToDouble(wdzzdvs);
            }
            if (ldzzdvs == ""|| ldzzdvs=="-")
            {
                lzzdv = 0;
            }
            else
            {
                lzzdv = Convert.ToDouble(ldzzdvs);
            }
            #endregion
            //判断该车辆为为单轴车辆
            if (szxz == 1)
            {
                #region 单轴车
                //如果一轴制动率小于60
                if ( ydzzdvs != "" && ydzzdvs != "-" &&yzzdv < 60)
                {
                    ydzpd.Visible = true;
                }
                else
                {
                    ydzpd.Visible = false;
                }
                //如果二轴制动率小于50
                if (edzzdvs != "" && edzzdvs != "-" && ezzdv < 50)
                {
                    edzpd.Visible = true;
                }
                else
                {
                    edzpd.Visible = false;
                }
                //如果三轴制动率小于50
                if (sdzzdvs != "" && sdzzdvs != "-" && szzdv < 50)
                {
                    sdzpd.Visible = true;
                }
                else
                {
                    sdzpd.Visible = false;
                }
                //如果四轴制动率小于50
                if (sidzzdvs != "" && sidzzdvs != "-" && sizzdv < 50)
                {
                    sidzpd.Visible = true;
                }
                else
                {
                    sidzpd.Visible = false;
                }
                //如果五轴制动率小于50
                if (wdzzdvs != "" && wdzzdvs != "-" && wzzdv < 50)
                {
                    wdzpd.Visible = true;
                }
                else
                {
                    wdzpd.Visible = false;
                }
                //如果六轴制动率小于50
                if (ldzzdvs != "" && ldzzdvs != "-" && lzzdv < 50)
                {
                    ldzpd.Visible = true;
                }
                else
                {
                    ldzpd.Visible = false;
                }
                #endregion
            }
            else
            {
                #region 双轴车
                //如果一轴制动率小于60
                if (ydzzdvs != "" && ydzzdvs != "-" && yzzdv < 60)
                {
                    ydzpd.Visible = true;
                }
                else
                {
                    ydzpd.Visible = false;
                }
                //如果二轴制动率小于60
                if (edzzdvs != "" && edzzdvs != "-" && ezzdv < 60)
                {
                    edzpd.Visible = true;
                }
                else
                {
                    edzpd.Visible = false;
                }
                //如果三轴制动率小于50
                if (sdzzdvs != "" && sdzzdvs != "-" && szzdv < 50)
                {
                    sdzpd.Visible = true;
                }
                else
                {
                    sdzpd.Visible = false;
                }
                //如果四轴制动率小于50
                if (sidzzdvs != "" && sidzzdvs != "-" && sizzdv < 50)
                {
                    sidzpd.Visible = true;
                }
                else
                {
                    sidzpd.Visible = false;
                }
                //如果五轴制动率小于50
                if (wdzzdvs != "" && wdzzdvs != "-" && wzzdv < 50)
                {
                    wdzpd.Visible = true;
                }
                else
                {
                    wdzpd.Visible = false;
                }
                //如果六轴制动率小于50
                if (ldzzdvs != "" && ldzzdvs != "-" && lzzdv < 50)
                {
                    ldzpd.Visible = true;
                }
                else
                {
                    ldzpd.Visible = false;
                }
                #endregion
            }
        }
        //判断是否为双转向轴的M2、M3类车
        public void M2judge()
        {
            ydzpd.Visible = false;
            edzpd.Visible = false;
            sdzpd.Visible = false;
            sidzpd.Visible = false;
            wdzpd.Visible = false;
            ldzpd.Visible = false;
            #region 对轴制动率的判断
            double szxz;
            double yzzdv;
            double ezzdv;
            double szzdv;
            double sizzdv;
            double wzzdv;
            double lzzdv;
            string zszss = zxzs.Text;//总质量
            string ydzzdvs = ydzzdv.Text;//一轴制动率
            string edzzdvs = edzzdv.Text;//二轴制动率
            string sdzzdvs = sdzzdv.Text;//三轴制动率
            string sidzzdvs = sidzzdv.Text;//四轴制动率
            string wdzzdvs = wdzzdv.Text;//五轴制动率
            string ldzzdvs = ldzzdv.Text;//六轴制动率
            if (zszss == ""|| zszss=="-")
            {
                szxz = 0;
            }
            else
            {
                szxz = Convert.ToDouble(zszss);
            }
            if (ydzzdvs == ""|| ydzzdvs=="-")
            {
                yzzdv = 0;
            }
            else
            {
                yzzdv = Convert.ToDouble(ydzzdvs);
            }
            if (edzzdvs == ""|| edzzdvs=="-")
            {
                ezzdv = 0;
            }
            else
            {
                ezzdv = Convert.ToDouble(edzzdvs);
            }
            if (sdzzdvs == ""|| sdzzdvs=="-")
            {
                szzdv = 0;
            }
            else
            {
                szzdv = Convert.ToDouble(sdzzdvs);
            }
            if (sidzzdvs == ""|| sidzzdvs=="-")
            {
                sizzdv = 0;
            }
            else
            {
                sizzdv = Convert.ToDouble(sidzzdvs);
            }
            if (wdzzdvs == ""|| wdzzdvs=="-")
            {
                wzzdv = 0;
            }
            else
            {
                wzzdv = Convert.ToDouble(wdzzdvs);
            }
            if (ldzzdvs == ""|| ldzzdvs=="-")
            {
                lzzdv = 0;
            }
            else
            {
                lzzdv = Convert.ToDouble(ldzzdvs);
            }
            #endregion
            //判断该车辆为为单轴车辆
            if (szxz == 1)
            {
                #region 单转向轴
                //如果一轴制动率小于60
                if (ydzzdvs != "" && ydzzdvs != "-" && yzzdv < 60)
                {
                    ydzpd.Visible = true;
                }
                else
                {
                    ydzpd.Visible = false;
                }
                //如果二轴制动率小于40
                if (edzzdvs != "" && edzzdvs != "-" && ezzdv < 40)
                {
                    edzpd.Visible = true;
                }
                else
                {
                    edzpd.Visible = false;
                }
                //如果三轴制动率小于40
                if (sdzzdvs != "" && sdzzdvs != "-" && szzdv < 40)
                {
                    sdzpd.Visible = true;
                }
                else
                {
                    sdzpd.Visible = false;
                }
                //如果四轴制动率小于40
                if (sidzzdvs != "" && sidzzdvs != "-" && sizzdv < 40)
                {
                    sidzpd.Visible = true;
                }
                else
                {
                    sidzpd.Visible = false;
                }
                //如果五轴制动率小于40
                if (wdzzdvs != "" && wdzzdvs != "-" && wzzdv < 40)
                {
                    wdzpd.Visible = true;
                }
                else
                {
                    wdzpd.Visible = false;
                }
                //如果六轴制动率小于40
                if (ldzzdvs != "" && ldzzdvs != "-" && lzzdv < 40)
                {
                    ldzpd.Visible = true;
                }
                else
                {
                    ldzpd.Visible = false;
                }
                #endregion
            }
            else
            {
                #region 双转向轴
                //如果一轴制动率小于60
                if (ydzzdvs != "" && ydzzdvs != "-" && yzzdv < 60)
                {
                    ydzpd.Visible = true;
                }
                else
                {
                    ydzpd.Visible = false;
                }
                //如果二轴制动率小于60
                if (edzzdvs != "" && edzzdvs != "-" && ezzdv < 60)
                {
                    edzpd.Visible = true;
                }
                else
                {
                    edzpd.Visible = false;
                }
                //如果三轴制动率小于40
                if (sdzzdvs != "" && sdzzdvs != "-" && szzdv < 40)
                {
                    sdzpd.Visible = true;
                }
                else
                {
                    sdzpd.Visible = false;
                }
                //如果四轴制动率小于40
                if (sidzzdvs != "" && sidzzdvs != "-" && sizzdv < 40)
                {
                    sidzpd.Visible = true;
                }
                else
                {
                    sidzpd.Visible = false;
                }
                //如果五轴制动率小于40
                if (wdzzdvs != "" && wdzzdvs != "-" && wzzdv < 40)
                {
                    wdzpd.Visible = true;
                }
                else
                {
                    wdzpd.Visible = false;
                }
                //如果六轴制动率小于40
                if (ldzzdvs != "" && ldzzdvs != "-" && lzzdv < 40)
                {
                    ldzpd.Visible = true;
                }
                else
                {
                    ldzpd.Visible = false;
                }
                #endregion
            }
        }
        //判断是否为半挂车
        public void bgjudge()
        {
            ydzpd.Visible = false;
            edzpd.Visible = false;
            sdzpd.Visible = false;
            sidzpd.Visible = false;
            wdzpd.Visible = false;
            ldzpd.Visible = false;
            //一轴
            if (!ydzzdv.Text.Replace(" ","").Contains("-")&&ydzzdv.Text.Replace(" ","")!="")
            {
                if(Convert.ToDouble(ydzzdv.Text.Replace(" ",""))>=55)
                {
                    ydzpd.Visible = false;
                }
                else
                {
                    ydzpd.Visible = true;
                }
            }
            //二轴
            if (!edzzdv.Text.Replace(" ", "").Contains("-") && edzzdv.Text.Replace(" ", "") != "")
            {
                if (Convert.ToDouble(edzzdv.Text.Replace(" ", "")) >= 55)
                {
                    edzpd.Visible = false;
                }
                else
                {
                    edzpd.Visible = true;
                }
            }
            //三轴
            if (!sdzzdv.Text.Replace(" ", "").Contains("-") && sdzzdv.Text.Replace(" ", "") != "")
            {
                if (Convert.ToDouble(sdzzdv.Text.Replace(" ", "")) >= 55)
                {
                    sdzpd.Visible = false;
                }
                else
                {
                    sdzpd.Visible = true;
                }
            }
            //四轴
            if (!sidzzdv.Text.Replace(" ", "").Contains("-") && sidzzdv.Text.Replace(" ", "") != "")
            {
                if (Convert.ToDouble(sidzzdv.Text.Replace(" ", "")) >= 55)
                {
                    sidzpd.Visible = false;
                }
                else
                {
                    sidzpd.Visible = true;
                }
            }
            //五轴
            if (!wdzzdv.Text.Replace(" ", "").Contains("-") && wdzzdv.Text.Replace(" ", "") != "")
            {
                if (Convert.ToDouble(wdzzdv.Text.Replace(" ", "")) >= 55)
                {
                    wdzpd.Visible = false;
                }
                else
                {
                    wdzpd.Visible = true;
                }
            }
            //六轴
            if (!ldzzdv.Text.Replace(" ", "").Contains("-") && ldzzdv.Text.Replace(" ", "") != "")
            {
                if (Convert.ToDouble(ldzzdv.Text.Replace(" ", "")) >= 55)
                {
                    ldzpd.Visible = false;
                }
                else
                {
                    ldzpd.Visible = true;
                }
            }
        }
        //判断该车为几类车
        public void M1judge()
        {
            string czws = kczws.Text;//座位数
            string szzl=zzl.Text;//总质量
            string scllx = cllx.Text;//车辆类型
            string ydzzdvs = ydzzdv.Text;//一轴制动率
            string edzzdvs = edzzdv.Text;//二轴制动率
            string sdzzdvs = sdzzdv.Text;//三轴制动率
            string sidzzdvs = sidzzdv.Text;//四轴制动率
            string wdzzdvs = wdzzdv.Text;//五轴制动率
            string ldzzdvs = ldzzdv.Text;//六轴制动率
            double yzzdv;//一轴制动率
            double ezzdv;//二轴制动率
            double szzdv;//三轴制动率
            double sizzdv;//四轴制动率
            double wzzdv;//五轴制动率
            double lzzdv;//六轴制动率
            double dzzl;//总质量
            double dzws;//座位数
            #region 判断值是否存在
            //判断座位数值是否存在
            if (czws == "" || czws == "-")
            {
                dzws = 0;
            }
            else
            {
                dzws = Convert.ToDouble(czws);
            }
            //判断总质量值是否存在
            if (szzl == "" || szzl == "-")
            {
                dzzl = 0;
            }
            else
            {
                dzzl = Convert.ToDouble(szzl);
            }
            //判断一轴制动率值是否存在
            if (ydzzdvs == "" || ydzzdvs == "")
            {
                yzzdv = 0;
            }
            else
            {
                yzzdv = Convert.ToDouble(ydzzdvs);
            }
            //判断二轴制动率值是否存在
            if (edzzdvs == "" || edzzdvs == "-")
            {
                ezzdv = 0;
            }
            else
            {
                ezzdv = Convert.ToDouble(edzzdvs);
            }
            //判断三轴制动率值是否存在
            if (sdzzdvs == "" || sdzzdvs == "-")
            {
                szzdv = 0;
            }
            else
            {
                szzdv = Convert.ToDouble(sdzzdvs);
            }
            //判断四轴制动率值是否存在
            if (sidzzdvs == "" || sidzzdvs == "")
            {
                sizzdv = 0;
            }
            else
            {
                sizzdv = Convert.ToDouble(sidzzdvs);
            }
            //判断五轴制动率值是否存在
            if (wdzzdvs == "" || wdzzdvs == "-")
            {
                wzzdv = 0;
            }
            else
            {
                wzzdv = Convert.ToDouble(wdzzdvs);
            }
            //判断六轴制动率值是否存在
            if (ldzzdvs == "" || ldzzdvs == "-")
            {
                lzzdv = 0;
            }
            else
            {
                lzzdv = Convert.ToDouble(ldzzdvs);
            }
            #endregion
            
            #region 新标准
            //载客车辆
            if (scllx.Contains("客"))
            {
                //总质量大于1t且座位数不多于8个(M1类车)
                if (szzl != "" && szzl != "-" && czws != "" && czws != "-" && dzzl > 1000 && dzws <= 8)
                {
                    M1N1judge();
                }
                //总质量不多于5t且座位数多于8个(M2类车)
               else if (szzl != "" && szzl != "-" && czws != "" && czws != "-" && dzzl <= 5000 && dzws > 8)
                {
                    if (dzzl > 3500)
                    {
                        M2judge();
                    }
                    else
                    {
                        M23judge();
                    }
                }
                //总质量多于5t(M3类车)
                else
                {
                    //M23judge();
                    M2judge();
                }
            }
            //载货车辆
          else  if (scllx.Contains("货"))
            {
                //总质量不大于3.5t(N1类车)
                if (szzl != "" && szzl != "-" && dzzl <= 3500)
                {
                    M1N1judge();
                }
                //总质量在3.5t与12t之间(N2类车)
               else if (szzl != "" && szzl != "-" && dzzl > 3500 && dzzl <= 12000)
                {
                    M23judge();
                }
                //总质量大于12t(N3类车)
                else
                {
                    M23judge();
                }
            }
            else
            {
                if (cllx.Text.Replace(" ", "").Contains("牵引车"))
                {
                    M23judge();
                }
                else if (cllx.Text.Replace(" ", "").Contains("半挂"))
                {
                    bgjudge();
                }
                else if (cllx.Text.Replace(" ", "").Contains("全挂"))
                {
                    bgjudge();
                }
                else
                {
                    M1N1judge();
                }
            }
            #endregion
            
            CLDEPD();
        }
        //一轴制动率超限鼠标经过事件
        private void ydzpd_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("一轴制动率超出标准限值范围",ydzpd,10000);
        }
        //一轴制动率超限鼠标离开事件
        private void ydzpd_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(ydzpd);
        }
        //二轴制动率超限鼠标经过事件
        private void edzpd_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("二轴制动率超出标准限值范围", edzpd, 10000);
        }
        //二轴制动率超限鼠标离开事件
        private void edzpd_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(edzpd);
        }
        //三轴制动率超限鼠标经过事件
        private void sdzpd_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("三轴制动率超出标准限值范围", sdzpd, 10000);
        }
        //三轴制动率超限鼠标离开事件
        private void sdzpd_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(sdzpd);
        }
        //四轴制动率超限鼠标经过事件
        private void sidzpd_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("四轴制动率超出标准限值范围", sidzpd, 10000);
        }
        //四轴制动率超限鼠标离开事件
        private void sidzpd_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(sidzpd);
        }
        //五轴制动率超限鼠标经过事件
        private void wdzpd_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("五轴制动率超出标准限值范围", wdzpd, 10000);
        }
        //五轴制动率超限鼠标离开事件
        private void wdzpd_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(wdzpd);
        }
        //六轴制动率超限鼠标经过事件
        private void ldzpd_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("六轴制动率超出标准限值范围", ldzpd, 10000);
        }
        //六轴制动率超限鼠标离开事件
        private void ldzpd_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(ldzpd);
        }
        #endregion

        #region 不平衡率判断
        //新标准
        public void SZXZjudge()
        {
            double dzdv;//整车制动率
            double dzxzs;//转向轴数
            double dyzbphv;//一轴不平衡率
            double dezbphv;//二轴不平衡率
            double dszbphv;//三轴不平衡率
            double dsizbphv;//四轴不平衡率
            double dwzbphv;//五轴不平衡率
            double dlzbphv;//六轴不平衡率
            string szdv = dczdl.Text;//整车制动率
            string zszss = zxzs.Text;//转向轴数
            string ydzbphvs = ydzbphv.Text;//一轴不平衡率
            string edzbphvs = edzbphv.Text;//二轴不平衡率
            string sdzbphvs = sdzbphv.Text;//三轴不平衡率
            string sidzbphvs = sidzbphv.Text;//四轴不平衡率
            string wdzbphvs = wdzbphv.Text;//五轴不平衡率
            string ldzbphvs = ldzbphv.Text;//六轴不平衡率
            #region 判断值是否存在
            //判断整车制动率值是否存在
            if (szdv == "" || szdv == "-")
            {
                dzdv = 0;
            }
            else
            {
                dzdv = Convert.ToDouble(szdv);
            }
            //判断转向轴数值是否存在
            if (zszss == "" || zszss == "-")
            {
                dzxzs = 1;
            }
            else
            {
                dzxzs = Convert.ToDouble(zszss);
            }
            //判断一轴不平衡率值是否有值
            if (ydzbphvs == "" || ydzbphvs == "-")
            {
                dyzbphv = 0;
            }
            else
            {
                dyzbphv = Convert.ToDouble(ydzbphvs);
            }
            //判断二轴不平衡率是否有值
            if (edzbphvs == "" || edzbphvs == "-")
            {
                dezbphv = 0;
            }
            else
            {
                dezbphv = Convert.ToDouble(edzbphvs);
            }
            //判断三轴不平衡率是否有值
            if (sdzbphvs == "" || sdzbphvs == "-")
            {
                dszbphv = 0;
            }
            else
            {
                dszbphv = Convert.ToDouble(sdzbphvs);
            }
            //判断四轴不平衡率是否有值
            if (sidzbphvs == "" || sidzbphvs == "-")
            {
                dsizbphv = 0;
            }
            else
            {
                dsizbphv = Convert.ToDouble(sidzbphvs);
            }
            //判断五轴不平衡率是否有值
            if (wdzbphvs == "" || wdzbphvs == "-")
            {
                dwzbphv = 0;
            }
            else
            {
                dwzbphv = Convert.ToDouble(wdzbphvs);
            }
            //判断六轴不平衡率是否有值
            if (ldzbphvs == "" || ldzbphvs == "-")
            {
                dlzbphv = 0;
            }
            else
            {
                dlzbphv = Convert.ToDouble(ldzbphvs);
            }
            #endregion


            #region 在用车标准
            #region 一轴
            if (dyzbphv <= 20)
            {
                ydzbpd.Visible = false;
                lb1.Text = "一";
            }
            else if (dyzbphv <= 24)
            {
                ydzbpd.Visible = false;
                lb1.Text = "二";
            }
            else
            {
                ydzbpd.Visible = true;
                lb1.Text = "不合格";
            }
            #endregion
            #region 二轴
            if (zxzs.Text == "2")
            {
                if (dezbphv <= 20)
                {
                    edzbpd.Visible = false;
                    lb2.Text = "一";
                }
                else if (dezbphv <= 24)
                {
                    edzbpd.Visible = false;
                    lb2.Text = "二";
                }
                else
                {
                    edzbpd.Visible = true;
                    lb2.Text = "不合格";
                }
            }
            else
            {
                //二轴
                if (edzzdv.Text != "" && edzzdv.Text != "-")
                {
                    if (Convert.ToDouble(edzzdv.Text) >= 60)
                    {
                        if (dezbphv <= 24)
                        {
                            edzbpd.Visible = false;
                            lb2.Text = "一";
                        }
                        else if (dezbphv <= 30)
                        {
                            edzbpd.Visible = false;
                            lb2.Text = "二";
                        }
                        else
                        {
                            edzbpd.Visible = true;
                            lb2.Text = "不合格";
                        }
                    }
                    else
                    {
                        if (dezbphv <= 8)
                        {
                            edzbpd.Visible = false;
                            lb2.Text = "一";
                        }
                        else if (dezbphv <= 10)
                        {
                            edzbpd.Visible = false;
                            lb2.Text = "二";
                        }
                        else
                        {
                            edzbpd.Visible = true;
                            lb2.Text = "不合格";
                        }
                    }
                }
            }
            #endregion
            #region 三轴
            if (sdzzdv.Text != "" && sdzzdv.Text != "-")
            {
                if (Convert.ToDouble(sdzzdv.Text) >= 60)
                {
                    if (dszbphv <= 24)
                    {
                        sdzbpd.Visible = false;
                        lb3.Text = "一";
                    }
                    else if (dszbphv <= 30)
                    {
                        sdzbpd.Visible = false;
                        lb3.Text = "二";
                    }
                    else
                    {
                        sdzbpd.Visible = true;
                        lb3.Text = "不合格";
                    }
                }
                else
                {
                    if (dszbphv <= 8)
                    {
                        sdzbpd.Visible = false;
                        lb3.Text = "一";
                    }
                    else if (dszbphv <= 10)
                    {
                        sdzbpd.Visible = false;
                        lb3.Text = "二";
                    }
                    else
                    {
                        sdzbpd.Visible = true;
                        lb3.Text = "不合格";
                    }
                }
            }
            #endregion
            #region 四轴
            if (sidzzdv.Text != "" && sidzzdv.Text != "-")
            {
                if (Convert.ToDouble(sidzzdv.Text) >= 60)
                {
                    if (dsizbphv <= 24)
                    {
                        sidzbpd.Visible = false;
                        lb4.Text = "一";
                    }
                    else if (dsizbphv <= 30)
                    {
                        sidzbpd.Visible = false;
                        lb4.Text = "二";
                    }
                    else
                    {
                        sidzbpd.Visible = true;
                        lb4.Text = "不合格";
                    }
                }
                else
                {
                    if (dsizbphv <= 8)
                    {
                        sidzbpd.Visible = false;
                        lb4.Text = "一";
                    }
                    else if (dsizbphv <= 10)
                    {
                        sidzbpd.Visible = false;
                        lb4.Text = "二";
                    }
                    else
                    {
                        sidzbpd.Visible = true;
                        lb4.Text = "不合格";
                    }
                }
            }
            #endregion
            #region 五轴
            if (wdzzdv.Text != "" && wdzzdv.Text != "-")
            {
                if (Convert.ToDouble(wdzzdv.Text) >= 60)
                {
                    if (dwzbphv <= 24)
                    {
                        wdzbpd.Visible = false;
                        lb5.Text = "一";
                    }
                    else if (dwzbphv <= 30)
                    {
                        wdzbpd.Visible = false;
                        lb5.Text = "二";
                    }
                    else
                    {
                        wdzbpd.Visible = true;
                        lb5.Text = "不合格";
                    }
                }
                else
                {
                    if (dwzbphv <= 8)
                    {
                        wdzbpd.Visible = false;
                        lb5.Text = "一";
                    }
                    else if (dwzbphv <= 10)
                    {
                        wdzbpd.Visible = false;
                        lb5.Text = "二";
                    }
                    else
                    {
                        wdzbpd.Visible = true;
                        lb5.Text = "不合格";
                    }
                }
            }
            #endregion
            #region 六轴
            if (ldzzdv.Text != "" && ldzzdv.Text != "-")
            {
                if (Convert.ToDouble(ldzzdv.Text) >= 60)
                {
                    if (dlzbphv <= 24)
                    {
                        ldzbpd.Visible = false;
                        lb6.Text = "一";
                    }
                    else if (dlzbphv <= 30)
                    {
                        ldzbpd.Visible = false;
                        lb6.Text = "二";
                    }
                    else
                    {
                        ldzbpd.Visible = true;
                        lb6.Text = "不合格";
                    }
                }
                else
                {
                    if (dlzbphv <= 8)
                    {
                        ldzbpd.Visible = false;
                        lb6.Text = "一";
                    }
                    else if (dlzbphv <= 10)
                    {
                        ldzbpd.Visible = false;
                        lb6.Text = "二";
                    }
                    else
                    {
                        ldzbpd.Visible = true;
                        lb6.Text = "不合格";
                    }
                }
            }
            #endregion
            #endregion
        }
        //一轴不平衡率超限鼠标经过事件
        private void ydzbpd_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("一轴不平衡率超出标准限值范围", ydzbpd,10000);
        }
        //一轴不平衡率超限鼠标离开事件
        private void ydzbpd_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(ydzbpd);
        }
        //二轴不平衡率超限鼠标经过事件
        private void edzbpd_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("二轴不平衡率超出标准限值范围", edzbpd,10000);
        }
        //二轴不平衡率超限鼠标离开事件
        private void edzbpd_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(edzbpd);
        }
        //三轴不平衡率超限鼠标经过事件
        private void sdzbpd_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("三轴不平衡率超出标准限值范围", sdzbpd,10000);
        }
        //三轴不平衡率超限鼠标离开事件
        private void sdzbpd_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(sdzbpd);
        }
        //四轴不平衡率超限鼠标经过事件
        private void sidzbpd_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("四轴不平衡率超出标准限值范围", sidzbpd,10000);
        }
        //四轴不平衡率超限鼠标离开事件
        private void sidzbpd_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(sidzbpd);
        }
        //五轴不平衡率超限鼠标经过事件
        private void wdzbpd_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("五轴不平衡率超出标准限值范围", wdzbpd,10000);
        }
        //五轴不平衡率超限鼠标离开事件
        private void wdzbpd_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(wdzbpd);
        }
        //六轴不平衡率超限鼠标经过事件
        private void ldzbpd_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("六轴不平衡率超出标准限值范围", ldzbpd,10000);
        }
        //六轴不平衡率超限鼠标离开事件
        private void ldzbpd_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(ldzbpd);
        }
        #endregion

        #region 车速判断
        //车速标准判断
        public void CSZjudge()
        {
            scsz.Visible = false;
            string scszs = csb.Text;//车速
            double csz;
            if (scszs == ""|| scszs=="-")
            {
                csz = 0;
            }
            else
            {
                csz = Convert.ToDouble(scszs);
            }
            if (scszs != "" && scszs != "-" && (csz < 32.8 || csz > 40))
            {
                scsz.Visible = true;
            }
            else
            {
                scsz.Visible = false;
            }
            CLDEPD();
        }
        //车速超限鼠标经过事件
        private void scsz_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("车速值超出标准限值范围",scsz,10000);
        }
        //车速超限鼠标离开事件
        private void scsz_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(scsz);
        }
        #endregion

        #region 喇叭判断
        //喇叭声级值标准判断
        public void LBjudge()
        {
            slbs.Visible = false;
            string slb = lbsjz.Text;//喇叭声级值s
            double lbsjzs;
            if (slb == ""|| slb=="-")
            {
                lbsjzs = 0;
            }
            else
            {
                lbsjzs = Convert.ToDouble(slb);
            }
            if ( slb != ""&& slb!="-"&&(lbsjzs < 90 || lbsjzs > 115))
            {
                slbs.Visible = true;
            }
            else
            {
                slbs.Visible = false;
            }
            CLDEPD();
        }
        //喇叭声级值超限鼠标经过事件
        private void slbs_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("喇叭声级值超出标准限值范围", slbs,10000);
        }
        //喇叭声级值超限鼠标离开事件
        private void slbs_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(slbs);
        }
        #endregion

        #region 侧滑量判断
        //侧滑量标准限值判断
        public void CHjudge()
        {
            ch1pd.Visible = false;
            ch2pd.Visible = false;
            string ch1 = dychl.Text;//第一转向轮侧滑量
            string ch2 = dechl.Text;//第二转向轮侧滑量
            double chl1;
            double chl2;
            if (ch1 == ""|| ch1=="-")
            {
                chl1 = 0;
            }
            else
            {
                chl1 = Convert.ToDouble(ch1);
            }
            if (ch2 == ""|| ch2=="-")
            {
                chl2 = 0;
            }
            else
            {
                chl2 = Convert.ToDouble(ch2);
            }
            if (chl1 > 5 || chl1 < -5)
            {
                ch1pd.Visible = true;
            }
            else
            {
                ch1pd.Visible = false;
            }
            if (chl2 > 5 || chl2 < -5)
            {
                ch2pd.Visible = true;
            }
            else
            {
                ch2pd.Visible = false;
            }
            CLDEPD();
        }
        //第一侧滑量超限鼠标经过事件
        private void ch1pd_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("第一转向轮侧滑量超出标准限值范围", ch1pd,10000);
        }
        //第一侧滑量超限鼠标离开事件
        private void ch1pd_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(ch1pd);
        }
        //第二侧滑量超限鼠标经过事件
        private void ch2pd_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("第二转向轮侧滑量超出标准限值范围", ch2pd,10000);
        }
        //第二侧滑量超限鼠标离开事件
        private void ch2pd_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(ch2pd);

        }
        #endregion

        #region 悬架判断
        public void XJjudge()
        {
            lqz.Visible = false;
            lqy.Visible = false;
            lhz.Visible = false;
            lhy.Visible = false;
            lq.Visible = false;
            lh.Visible = false;
            string qzxjzxsvs = qzzxsl.Text;//悬架前轴左吸收率
            string qzxjyxsvs = qzyxsl.Text;//悬架前轴右吸收率
            string hzxjzxsvs = hzzxsl.Text;//悬架后轴左吸收率
            string hzxjyxsvs = hzyxsl.Text;//悬架后轴右吸收率
            string qzxjzycs = qzzyc.Text;//悬架前轴左右吸收率差
            string hzxjzycs = hzzyc.Text;//悬架后轴左右吸收率差
            double qzzxs;
            double qzyxs;
            double hzzxs;
            double hzyxs;
            double qzzycs;
            double hzzycs;
            #region 判断值是否存在
                    
            //悬架前轴左吸收率是否有值
            if (qzxjzxsvs == ""|| qzxjzxsvs=="-")
            {
                qzzxs = 0;
            }
            else
            {
                qzzxs = Convert.ToDouble(qzxjzxsvs);
            }
            //悬架前轴右吸收率是否有值
            if (qzxjyxsvs == ""|| qzxjyxsvs=="-")
            {
                qzyxs = 0;
            }
            else
            {
                qzyxs = Convert.ToDouble(qzxjyxsvs);
            }
            //悬架后轴左吸收率是否有值
            if (hzxjzxsvs == ""|| hzxjzxsvs=="-")
            {
                hzzxs = 0;
            }
            else
            {
                hzzxs = Convert.ToDouble(hzxjzxsvs);
            }
            //悬架后轴右吸收率是否有值
            if (hzxjyxsvs == ""|| hzxjyxsvs=="-")
            {
                hzyxs = 0;
            }
            else
            {
                hzyxs = Convert.ToDouble(hzxjyxsvs);
            }
            //悬架前轴左右吸收率插是否有值
            if (qzxjzycs == ""|| qzxjzycs=="-")
            {
                qzzycs = 0;
            }
            else
            {
                qzzycs = Convert.ToDouble(qzxjzycs);
            }
            //悬架后轴左右吸收率差是否有值
            if (hzxjzycs == ""|| hzxjzycs=="-")
            {
                hzzycs = 0;
            }
            else
            {
                hzzycs = Convert.ToDouble(hzxjzycs);
            }
            #endregion
          
          //判断悬架前轴左吸收率是否小于40%
          if (qzxjzxsvs != "" && qzxjzxsvs != "-" && qzzxs < 40)
          {
              lqz.Visible = true;
          }
          else
          {
              lqz.Visible = false;
          }
          //判断悬架前轴右吸收率是否小于40%
          if (qzxjyxsvs != "" && qzxjyxsvs != "-"&& qzyxs < 40)
          {
             lqy.Visible = true;
          }
          else
          {
             lqy.Visible = false;
          }
          //判断悬架后轴左吸收率是否小于40%
         if (hzxjzxsvs != "" && hzxjzxsvs != "-"&&hzzxs < 40 )
         {
            lhz.Visible = true;
         }
         else
         {
            lhz.Visible = false;
         }
         //判断悬架后轴右吸收率是否小于40%
        if (hzxjyxsvs != "" && hzxjyxsvs != "-"&& hzyxs < 40)
        {
            lhy.Visible = true;
        }
        else
        {
             lhy.Visible = false;
        }
        //判断悬架前轴左右吸收率差是否大于15%
        if (qzzycs > 15)
        {
            lq.Visible = true;
        }
        else
        {
           lq.Visible = false;
        }
       //判断悬架后轴左右吸收率差是否大于15%
       if (hzzycs > 15)
       {
          lh.Visible = true;
       }
       else
       {
           lh.Visible = false;
        }
      CLDEPD();
    }
        //悬架前轴左吸收率超限鼠标经过事件
        private void lqz_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("悬架前轴左吸收率值超出标准限值范围", lqz,10000);
        }
        //悬架前轴左吸收率超限鼠标离开事件
        private void lqz_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(lqz);
        }
        //悬架前轴右吸收率超限鼠标经过事件
        private void lqy_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("悬架前轴右吸收率值超出标准限值范围", lqy,10000);
        }
       //悬架前轴右吸收率超限鼠标离开事件
        private void lqy_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(lqy);
        }
        //悬架前轴吸收率左右差超限鼠标经过事件
        private void lq_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("悬架前轴吸收率左右差值超出标准限值范围", lq,10000);
        }
        //悬架前轴吸收率左右差超限鼠标离开事件
        private void lq_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(lq);
        }
        ////悬架后轴左吸收率超限鼠标经过事件
        private void lhz_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("悬架后轴左吸收率值超出标准限值范围", lhz,10000);
        }
        //悬架后轴左吸收率超限鼠标离开事件
        private void lhz_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(lhz);
        }
        //悬架后轴右吸收率超限鼠标经过事件
        private void lhy_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("悬架后轴右吸收率值超出标准限值范围", lhy,10000);
        }
        //悬架后轴右吸收率超限鼠标离开事件
        private void lhy_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(lhy);
        }
        //悬架后轴吸收率左右差超限鼠标经过事件
        private void lh_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("悬架后轴吸收率左右差值超出标准限值范围", lh,10000);
        }
        //悬架后轴吸收率左右差超限鼠标离开事件
        private void lh_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(lh);
        }
        #endregion

        #region 前照灯判断
        //前照灯判断
        public void DZjudge()
        {
            zwgq.Visible = false;
            zngq.Visible = false;
            yngq.Visible = false;
            ywgq.Visible = false;
            zwjc.Visible = false;
            znjc.Visible = false;
            ynjc.Visible = false;
            ywjc.Visible = false;
            zwyc.Visible = false;
            znyc.Visible = false;
            ynyc.Visible = false;
            ywyc.Visible = false;
            #region 变量
            string scllx = cllx.Text;//车辆类型
            string szws = kczws.Text;//座位数
            string szzl = zzl.Text;//总质量
            string sqzdz = qzdz.Text;//前照灯制
            string szwgq = zwyggq.Text;//左外灯远光光强
            string szngq = znyggq.Text;//左内灯远光光强
            string syngq = ynyggq.Text;//右内灯远光光强
            string sywgq = ywyggq.Text;//右外灯远光光强
            string szwych = zwygczH.Text;//左外灯远光垂直H值
            string sznych = znygczH.Text;//左内灯远光垂直H值
            string synych = ynygczH.Text;//右内灯远光垂直H值
            string sywych = ywygczH.Text;//右外灯远光垂直H值
            string szwjch = zwjgczH.Text;//左外灯近光垂直H值
            string sznjch = znjgczH.Text;//左内灯近光垂直H值
            string synjch = ynjgczH.Text;//右内灯近光垂直H值
            string sywjch = ywjgczH.Text;//右外灯近光垂直H值
            double dzwygq;//左外灯远光光强
            double dznygq;//左内灯远光光强
            double dynygq;//右内灯远光光强
            double dywygq;//右外灯远光光强
            double dzwyh;//左外灯远光垂直H值
            double dznyh;//左内灯远光垂直H值
            double dynyh;//右内灯远光垂直H值
            double dywyh;//右外灯远光垂直H值
            double dzwjh;//左外灯近光垂直H值
            double dznjh;//左内灯近光垂直H值
            double dynjh;//右内灯近光垂直H值
            double dywjh;//右外灯近光垂直H值
            #endregion
            #region 判断值是否存在            
            //判断左外灯远光光强是否有值
            if (szwgq == ""|| szwgq=="-")
            {
                dzwygq = 0;
            }
            else
            {
                dzwygq = Convert.ToDouble(szwgq);
            }
            //判断左内灯远光光强
            if (szngq == ""|| szngq=="-")
            {
                dznygq = 0;
            }
            else
            {
                dznygq = Convert.ToDouble(szngq);
            }
            //判断右内灯远光光强
            if (syngq == ""|| syngq=="-")
            {
                dynygq = 0;
            }
            else
            {
                dynygq = Convert.ToDouble(syngq);
            }
            //判断右外灯远光光强
            if (sywgq == ""|| sywgq=="-")
            {
                dywygq = 0;
            }
            else
            {
                dywygq = Convert.ToDouble(sywgq);
            }
            //判断左外灯远光垂直H值
            if (szwych == ""|| szwych=="-")
            {
                dzwyh = 0;
            }
            else
            {
                dzwyh = Convert.ToDouble(szwych);
            }
            //判断左内灯远光垂直H值
            if (sznych == ""|| sznych=="-")
            {
                dznyh = 0;
            }
            else
            {
                dznyh = Convert.ToDouble(sznych);
            }
            //判断右内灯远光垂直H值
            if (synych == ""|| synych=="-")
            {
                dynyh = 0;
            }
            else
            {
                dynyh = Convert.ToDouble(synych);
            }
            //判断右外灯远光垂直H值
            if (sywych == ""|| sywych=="-")
            {
                dywyh = 0;
            }
            else
            {
                dywyh = Convert.ToDouble(sywych);
            }
            //判断左外灯近光垂直H值
            if (szwjch == ""|| szwjch=="-")
            {
                dzwjh = 0;
            }
            else
            {
                dzwjh = Convert.ToDouble(szwjch);
            }
            //判断左内灯近光垂直H值
            if (sznjch == ""|| sznjch=="-")
            {
                dznjh = 0;
            }
            else
            {
                dznjh = Convert.ToDouble(sznjch);
            }
            //判断右内灯近光垂直H值
            if (synjch == ""|| synjch=="-")
            {
                dynjh = 0;
            }
            else
            {
                dynjh = Convert.ToDouble(synjch);
            }
            //判断右外灯近光垂直H值
            if (sywjch == ""|| sywjch=="-")
            {
                dywjh = 0;
            }
            else
            {
                dywjh = Convert.ToDouble(sywjch);
            }
            #endregion
            if (sqzdz == "四灯")
            {
                #region 四灯制
                //判断左外灯远光光强是否小于12000
                if (szwgq != "" && szwgq != "-" && dzwygq < 15000)
                {
                    zwgq.Visible = true;
                }
                else
                {
                    zwgq.Visible = false;
                }
                //判断右外灯远光光强是否小于12000
                if (sywgq != "" && sywgq != "-" && dywygq < 15000)
                {
                    ywgq.Visible = true;
                }
                else
                {
                    ywgq.Visible = false;
                }
                #endregion
            }
            else
            {
                #region 两灯制
                //判断左外灯远光光强是否小于15000
                if (szwgq != "" && szwgq != "-" && dzwygq < 15000)
                {
                    zwgq.Visible = true;
                }
                else
                {
                    zwgq.Visible = false;
                }
                //判断右外灯远光光强是否小于15000
                if (sywgq != "" && sywgq != "-" && dywygq < 15000)
                {
                    ywgq.Visible = true;
                }
                else
                {
                    ywgq.Visible = false;
                }
                #endregion
            }
            #region 灯光水平偏移量限值判断
            if (zwygsp.Text != "" && zwygsp.Text != "-")
            {
                if (Convert.ToDouble(zwygsp.Text) >= -170 && Convert.ToDouble(zwygsp.Text) <= 350)
                {
                    zwysp.Visible = false;
                }
                else
                {
                    zwysp.Visible = true;
                }
            }
            if (znygsp.Text != "" && znygsp.Text != "-")
            {
                if (Convert.ToDouble(znygsp.Text) >= -170 && Convert.ToDouble(znygsp.Text) <= 350)
                {
                    znysp.Visible = false;
                }
                else
                {
                    znysp.Visible = true;
                }
            }

            if (ywygsp.Text != "" && ywygsp.Text != "-")
            {
                if (Convert.ToDouble(ywygsp.Text) >= -350 && Convert.ToDouble(ywygsp.Text) <= 350)
                {
                    ywysp.Visible = false;
                }
                else
                {
                    ywysp.Visible = true;
                }
            }
            if (ynygsp.Text != "" && ynygsp.Text != "-")
            {
                if (Convert.ToDouble(ynygsp.Text) >= -350 && Convert.ToDouble(ynygsp.Text) <= 350)
                {
                    ynysp.Visible = false;
                }
                else
                {
                    ynysp.Visible = true;
                }
            }

            if (zwjgsp.Text != "" && zwjgsp.Text != "-")
            {
                if (Convert.ToDouble(zwjgsp.Text) >= -170 && Convert.ToDouble(zwjgsp.Text) <= 350)
                {
                    zwjsp.Visible = false;
                }
                else
                {
                    zwjsp.Visible = true;
                }
            }
            if (znjgsp.Text != "" && znjgsp.Text != "-")
            {
                if (Convert.ToDouble(znjgsp.Text) >= -170 && Convert.ToDouble(znjgsp.Text) <= 350)
                {
                    znjsp.Visible = false;
                }
                else
                {
                    znjsp.Visible = true;
                }
            }
            if (ywjgsp.Text != "" && ywjgsp.Text != "-")
            {
                if (Convert.ToDouble(ywjgsp.Text) >= -170 && Convert.ToDouble(ywjgsp.Text) <= 350)
                {
                    ywjsp.Visible = false;
                }
                else
                {
                    ywjsp.Visible = true;
                }
            }
            if (ynjgsp.Text != "" && ynjgsp.Text != "-")
            {
                if (Convert.ToDouble(ynjgsp.Text) >= -170 && Convert.ToDouble(ynjgsp.Text) <= 350)
                {
                    ynjsp.Visible = false;
                }
                else
                {
                    ynjsp.Visible = true;
                }
            }
            #endregion
            #region 垂直偏移量判断
            if (scllx.Contains("客"))
            {
                //M1类车
                if (szws != "" && szws != "-" && szzl != "" && szzl != "-" && Convert.ToDouble(szws) <= 8 && Convert.ToDouble(szzl) > 1000)
                {
                    #region 近光范围判断
                    if (szwjch != "" && szwjch != "-")
                    {
                        //近光垂直H值是否在0.7~0.9范围内
                        if (Convert.ToDouble(szwjch) >= 0.7 && Convert.ToDouble(szwjch) <= 0.9)
                        {
                            zwjc.Visible = false;
                        }
                        else
                        {
                            zwjc.Visible = true;
                        }
                    }
                    if (sznjch != "" && sznjch != "-")
                    {
                        //近光垂直H值是否在0.7~0.9范围内
                        if (Convert.ToDouble(sznjch) >= 0.7 && Convert.ToDouble(sznjch) <= 0.9)
                        {
                            znjc.Visible = false;
                        }
                        else
                        {
                            znjc.Visible = true;
                        }
                    }
                    if (sywjch != "" && sywjch != "-")
                    {
                        //近光垂直H值是否在0.7~0.9范围内
                        if (Convert.ToDouble(sywjch) >= 0.7 && Convert.ToDouble(sywjch) <= 0.9)
                        {
                            ywjc.Visible = false;
                        }
                        else
                        {
                            ywjc.Visible = true;
                        }
                    }
                    if (synjch != "" && synjch != "-")
                    {
                        //近光垂直H值是否在0.7~0.9范围内
                        if (Convert.ToDouble(synjch) >= 0.7 && Convert.ToDouble(synjch) <= 0.9)
                        {
                            ynjc.Visible = false;
                        }
                        else
                        {
                            ynjc.Visible = true;
                        }
                    }
                    #endregion
                    #region 远光范围判断
                    if (szwych != "" && szwych != "-")
                    {
                        //左外远光垂直H值
                        if (Convert.ToDouble(szwych) >= 0.85 && Convert.ToDouble(szwych) <= 0.95)
                        {
                            zwyc.Visible = false;
                        }
                        else
                        {
                            zwyc.Visible = true;
                        }
                    }
                    if (sznych != "" && sznych != "-")
                    {
                        //左内远光垂直H值
                        if (Convert.ToDouble(sznych) >= 0.85 && Convert.ToDouble(sznych) <= 0.95)
                        {
                            znyc.Visible = false;
                        }
                        else
                        {
                            znyc.Visible = true;
                        }
                    }
                    if (sywych != "" && sywych != "-")
                    {
                        //右外远光垂直H值
                        if (Convert.ToDouble(sywych) >= 0.85 && Convert.ToDouble(sywych) <= 0.95)
                        {
                            ywyc.Visible = false;
                        }
                        else
                        {
                            ywyc.Visible = true;
                        }
                    }
                    if (synych != "" && synych != "-")
                    {
                        //右内远光垂直H值
                        if (Convert.ToDouble(synych) >= 0.85 && Convert.ToDouble(synych) <= 0.95)
                        {
                            ynyc.Visible = false;
                        }
                        else
                        {
                            ynyc.Visible = true;
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 近光范围判断
                    if (szwjch != "" && szwjch != "-")
                    {
                        //近光垂直H值是否在0.6~0.8范围内
                        if (Convert.ToDouble(szwjch) >= 0.6 && Convert.ToDouble(szwjch) <= 0.8)
                        {
                            zwjc.Visible = false;
                        }
                        else
                        {
                            zwjc.Visible = true;
                        }
                    }
                    if (sznjch != "" && sznjch != "-")
                    {
                        //近光垂直H值是否在0.6~0.8范围内
                        if (Convert.ToDouble(sznjch) >= 0.6 && Convert.ToDouble(sznjch) <= 0.8)
                        {
                            znjc.Visible = false;
                        }
                        else
                        {
                            znjc.Visible = true;
                        }
                    }
                    if (sywjch != "" && sywjch != "-")
                    {
                        //近光垂直H值是否在0.6~0.8范围内
                        if (Convert.ToDouble(sywjch) >= 0.6 && Convert.ToDouble(sywjch) <= 0.8)
                        {
                            ywjc.Visible = false;
                        }
                        else
                        {
                            ywjc.Visible = true;
                        }
                    }
                    if (synjch != "" && synjch != "-")
                    {
                        //近光垂直H值是否在0.6~0.8范围内
                        if (Convert.ToDouble(synjch) >= 0.6 && Convert.ToDouble(synjch) <= 0.8)
                        {
                            ynjc.Visible = false;
                        }
                        else
                        {
                            ynjc.Visible = true;
                        }
                    }
                    #endregion
                    #region 远光范围判断
                    if (szwych != "" && szwych != "-")
                    {
                        //左外远光垂直H值
                        if (Convert.ToDouble(szwych) >= 0.8 && Convert.ToDouble(szwych) <= 0.95)
                        {
                            zwyc.Visible = false;
                        }
                        else
                        {
                            zwyc.Visible = true;
                        }
                    }
                    if (sznych != "" && sznych != "-")
                    {
                        //左内远光垂直H值
                        if (Convert.ToDouble(sznych) >= 0.8 && Convert.ToDouble(sznych) <= 0.95)
                        {
                            znyc.Visible = false;
                        }
                        else
                        {
                            znyc.Visible = true;
                        }
                    }
                    if (sywych != "" && sywych != "-")
                    {
                        //右外远光垂直H值
                        if (Convert.ToDouble(sywych) >= 0.8 && Convert.ToDouble(sywych) <= 0.95)
                        {
                            ywyc.Visible = false;
                        }
                        else
                        {
                            ywyc.Visible = true;
                        }
                    }
                    if (synych != "" && synych != "-")
                    {
                        //右内远光垂直H值
                        if (Convert.ToDouble(synych) >= 0.8 && Convert.ToDouble(synych) <= 0.95)
                        {
                            ynyc.Visible = false;
                        }
                        else
                        {
                            ynyc.Visible = true;
                        }
                    }
                    #endregion
                }
            }
            else
            {
                #region 近光范围判断
                if (szwjch != "" && szwjch != "-")
                {
                    //近光垂直H值是否在0.6~0.8范围内
                    if (Convert.ToDouble(szwjch) >= 0.6 && Convert.ToDouble(szwjch) <= 0.8)
                    {
                        zwjc.Visible = false;
                    }
                    else
                    {
                        zwjc.Visible = true;
                    }
                }
                if (sznjch != "" && sznjch != "-")
                {
                    //近光垂直H值是否在0.6~0.8范围内
                    if (Convert.ToDouble(sznjch) >= 0.6 && Convert.ToDouble(sznjch) <= 0.8)
                    {
                        znjc.Visible = false;
                    }
                    else
                    {
                        znjc.Visible = true;
                    }
                }
                if (sywjch != "" && sywjch != "-")
                {
                    //近光垂直H值是否在0.6~0.8范围内
                    if (Convert.ToDouble(sywjch) >= 0.6 && Convert.ToDouble(sywjch) <= 0.8)
                    {
                        ywjc.Visible = false;
                    }
                    else
                    {
                        ywjc.Visible = true;
                    }
                }
                if (synjch != "" && synjch != "-")
                {
                    //近光垂直H值是否在0.6~0.8范围内
                    if (Convert.ToDouble(synjch) >= 0.6 && Convert.ToDouble(synjch) <= 0.8)
                    {
                        ynjc.Visible = false;
                    }
                    else
                    {
                        ynjc.Visible = true;
                    }
                }
                #endregion
                #region 远光范围判断
                if (szwych != "" && szwych != "-")
                {
                    //左外远光垂直H值
                    if (Convert.ToDouble(szwych) >= 0.8 && Convert.ToDouble(szwych) <= 0.95)
                    {
                        zwyc.Visible = false;
                    }
                    else
                    {
                        zwyc.Visible = true;
                    }
                }
                if (sznych != "" && sznych != "-")
                {
                    //左内远光垂直H值
                    if (Convert.ToDouble(sznych) >= 0.8 && Convert.ToDouble(sznych) <= 0.95)
                    {
                        znyc.Visible = false;
                    }
                    else
                    {
                        znyc.Visible = true;
                    }
                }
                if (sywych != "" && sywych != "-")
                {
                    //右外远光垂直H值
                    if (Convert.ToDouble(sywych) >= 0.8 && Convert.ToDouble(sywych) <= 0.95)
                    {
                        ywyc.Visible = false;
                    }
                    else
                    {
                        ywyc.Visible = true;
                    }
                }
                if (synych != "" && synych != "-")
                {
                    //右内远光垂直H值
                    if (Convert.ToDouble(synych) >= 0.8 && Convert.ToDouble(synych) <= 0.95)
                    {
                        ynyc.Visible = false;
                    }
                    else
                    {
                        ynyc.Visible = true;
                    }
                }
                #endregion
            }
            #endregion
            CLDEPD();
        }
        //左外灯远光光强值超限鼠标经过事件
        private void zwgq_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("左外灯远光光强超出标准限值范围", zwgq,10000);
        }
        //左外灯远光光强值超限鼠标离开事件限
        private void zwgq_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(zwgq);
        }
        //左内灯远光光强值超限鼠标经过事件
        private void zngq_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("左内灯远光光强值超出标准限值范围", zngq,10000);
        }
        //左内灯远光光强值超限鼠标离开事件
        private void zngq_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(zngq);
        }
        //右内灯远光光强值超限鼠标经过事
        private void yngq_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("右内灯远光光强值超出标准限值范围", yngq,10000);
        }
        //右内灯远光光强值超限鼠标离开事件
        private void yngq_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(yngq);
        }
        //右外灯远光光强值超限鼠标经过事件
        private void ywgq_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("右外灯远光光强值超出标准限值范围", ywgq,10000);
        }
        //右外灯远光光强值超限鼠标离开事件限
        private void ywgq_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(ywgq);
        }

        //左外远光垂直偏移量H值超限鼠标经过事件
        private void zwyc_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("左外远光垂直偏移量H值超出标准限值范围", zwyc,10000);
        }
        //左外远光垂直偏移量H值超限鼠标离开事件
        private void zwyc_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(zwyc);
        }
        //左内远光垂直偏移量H值超限鼠标经过事件
        private void znyc_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("左内远光垂直偏移量H值超出标准限值范围", znyc,10000);
        }
        //左内远光垂直偏移量H值超限鼠标离开事件
        private void znyc_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(znyc);
        }
        //右内远光垂直偏移量H值超限鼠标经过事件
        private void ynyc_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("右内远光垂直偏移量H值超出标准限值范围", ynyc,10000);
        }
        //右内远光垂直偏移量H值超限鼠标离开事件
        private void ynyc_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(ynyc);
        }
        //右外远光垂直偏移量H值超限鼠标经过事件
        private void ywyc_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("右外远光垂直偏移量H值超出标准限值范围", ywyc,10000);
        }
        //右外远光垂直偏移量H值超限鼠标离开事件
        private void ywyc_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(ywyc);
        }

        //左外近光垂直偏移量H值超限鼠标经过事件
        private void zwjc_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("左外远光垂直偏移量H值超出标准限值范围", zwjc,10000);
        }
        //左外近光垂直偏移量H值超限鼠标离开事件
        private void zwjc_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(zwjc);
        }
        //左内近光垂直偏移量H值超限鼠标经过事件
        private void znjc_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("左内远光垂直偏移量H值超出标准限值范围", znjc,10000);
        }
        //左内近光垂直偏移量H值超限鼠标离开事件
        private void znjc_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(znjc);
        }
        //右内近光垂直偏移量H值超限鼠标经过事件
        private void ynjc_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("右内远光垂直偏移量H值超出标准限值范围", ynjc,10000);
        }
        //右内近光垂直偏移量H值超限鼠标离开事件
        private void ynjc_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(ynjc);
        }
        //右外近光垂直偏移量H值超限鼠标经过事件
        private void ywjc_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("右外远光垂直偏移量H值超出标准限值范围", ywjc,10000);
        }
        //右外近光垂直偏移量H值超限鼠标离开事件
        private void ywjc_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(ywjc);
        }
        //左外远光水平偏移量超出标准限值范围鼠标经过事件
        private void zwysp_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("左外灯远光水平偏移量超出标准限值范围", zwysp, 10000);
        }
        //左外远光水平偏移量超出标准限值范围鼠标离开事件
        private void zwysp_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(zwysp);
        }
        //左内远光水平偏移量超出标准限值范围鼠标经过事件
        private void znysp_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("左内灯远光水平偏移量超出标准限值范围", znysp, 10000);
        }
        //左内远光水平偏移量超出标准限值范围鼠标离开事件
        private void znysp_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(znysp);
        }
        //右内远光水平偏移量超出标准限值范围鼠标经过事件
        private void ynysp_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("右内灯远光水平偏移量超出标准限值范围", ynysp, 10000);
        }
        //右内远光水平偏移量超出标准限值范围鼠标离开事件
        private void ynysp_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(ynysp);
        }
        //右外远光水平偏移量超出标准限值范围鼠标经过事件
        private void ywysp_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("右外灯远光水平偏移量超出标准限值范围", ywysp, 10000);
        }
        //右外远光水平偏移量超出标准限值范围鼠标离开事件
        private void ywysp_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(ywysp);
        }
        //左外近光水平偏移量超出标准限值范围鼠标经过事件
        private void zwjsp_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("左外灯近光水平偏移量超出标准限值范围", zwjsp, 10000);
        }
        //左外近光水平偏移量超出标准限值范围鼠标离开事件
        private void zwjsp_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(zwjsp);
        }
        //左内近光水平偏移量超出标准限值范围鼠标经过事件
        private void znjsp_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("左内灯近光水平偏移量超出标准限值范围", znjsp, 10000);
        }
        //左内近光水平偏移量超出标准限值范围鼠标离开事件
        private void znjsp_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(znjsp);
        }
        //右内近光水平偏移量超出标准限值范围鼠标经过事件
        private void ynjsp_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("右内灯近光水平偏移量超出标准限值范围", ynjsp, 10000);
        }
        //右内近光水平偏移量超出标准限值范围鼠标离开事件
        private void ynjsp_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(ynjsp);
        }
        //右外近光水平偏移量超出标准限值范围鼠标经过事件
        private void ywjsp_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("右外灯近光水平偏移量超出标准限值范围", ywjsp, 10000);
        }
        //右外近光水平偏移量超出标准限值范围鼠标离开事件
        private void ywjsp_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(ywjsp);
        }
        #endregion

        #region 双怠速判断
        public void SDSjudge()
        {
            try
            {
                qgλ.Visible = false;
                qgco.Visible = false;
                qghc.Visible = false;
                qdco.Visible = false;
                qdhc.Visible = false;
                #region 变量
                string szzl = zzl.Text;//总质量
                string sccrq = ccrq.Text;//出厂日期
                string scllx = cllx.Text;//车辆类型
                string szws = kczws.Text;//座位数
                double dzzl;
                double dzws;
                string sgdsco = qygdsCO.Text;//高怠速CO
                string sgdshc = qygdsHC.Text;//高怠速HC
                string sddsco = qyddsCO.Text;//低怠速CO
                string sddshc = qyddsHC.Text;//低怠速HC
                string sgdsλ = qygdsλ.Text;//高怠速λ
                double dgco;
                double dghc;
                double ddco;
                double ddhc;
                double dgλ;
                #endregion
                #region 判断变量是否存在
                //高怠速CO
                if (sgdsco == ""|| sgdsco=="-")
                {
                    dgco = 0;
                }
                else
                {
                    dgco = Convert.ToDouble(sgdsco);
                }
                //高怠速HC
                if (sgdshc == ""|| sgdshc=="-")
                {
                    dghc = 0;
                }
                else
                {
                    dghc = Convert.ToDouble(sgdshc);
                }
                //低怠速CO
                if (sddsco == ""|| sddsco=="-")
                {
                    ddco = 0;
                }
                else
                {
                    ddco = Convert.ToDouble(sddsco);
                }
                //低怠速HC
                if (sddshc == ""|| sddshc=="-")
                {
                    ddhc = 0;
                }
                else
                {
                    ddhc = Convert.ToDouble(sddshc);
                }
                //高怠速λ
                if (sgdsλ == ""|| sgdsλ== "sgdsλ")
                {
                    dgλ = 0;
                }
                else
                {
                    dgλ = Convert.ToDouble(sgdsλ);
                }
                //总质量
                if (szzl == ""|| szzl=="-")
                {
                    dzzl = 0;
                }
                else
                {
                    dzzl = Convert.ToDouble(szzl);
                }
                //座位数
                if (szws == ""|| szws=="-")
                {
                    dzws = 0;
                }
                else
                {
                    if (szws == "2+3"||szws=="3+2")
                    {
                        szws = "5";
                    }
                    dzws = Convert.ToDouble(szws);
                }
                #endregion
                #region 判断双怠速排气污染物排放限值
                //1.M1类车
                if ((szws != "" && szws != "-" && szzl != "" && szzl != "-"&&dzws <= 8 && dzzl > 1000 ) && !scllx.Contains("货"))
                {
                    //重型汽车
                    if (dzzl > 3500)
                    {
                        try
                        {
                            if (DateTime.Parse(sccrq) < DateTime.Parse("1995-07-01"))
                            {
                                #region 1995年7月1日前生产的重型汽车
                                //高怠速CO
                                if (dgco > 3.5)
                                {
                                    qgco.Visible = true;
                                }
                                else
                                {
                                    qgco.Visible = false;
                                }
                                //高怠速HC
                                if (dghc > 1200)
                                {
                                    qghc.Visible = true;
                                }
                                else
                                {
                                    qghc.Visible = false;
                                }
                                //低怠速CO
                                if (ddco > 5.0)
                                {
                                    qdco.Visible = true;
                                }
                                else
                                {
                                    qdco.Visible = false;
                                }
                                //低怠速HC
                                if (ddhc > 2000)
                                {
                                    qdhc.Visible = true;
                                }
                                else
                                {
                                    qdhc.Visible = false;
                                }
                                #endregion
                            }
                            else
                            {
                                #region 1995年7月1日起生产的重型汽车
                                //高怠速CO
                                if (dgco > 3.0)
                                {
                                    qgco.Visible = true;
                                }
                                else
                                {
                                    qgco.Visible = false;
                                }
                                //高怠速HC
                                if (dghc > 900)
                                {
                                    qghc.Visible = true;
                                }
                                else
                                {
                                    qghc.Visible = false;
                                }
                                //低怠速CO
                                if (ddco > 4.5)
                                {
                                    qdco.Visible = true;
                                }
                                else
                                {
                                    qdco.Visible = false;
                                }
                                //低怠速HC
                                if (ddhc > 1200)
                                {
                                    qdhc.Visible = true;
                                }
                                else
                                {
                                    qdhc.Visible = false;
                                }
                                #endregion
                            }
                            if (DateTime.Parse(sccrq) >= DateTime.Parse("2004-09-01"))
                            {
                                #region 2004年9月1日起生产的重型汽车
                                //高怠速CO
                                if (dgco > 0.7)
                                {
                                    qgco.Visible = true;
                                }
                                else
                                {
                                    qgco.Visible = false;
                                }
                                //高怠速HC
                                if (dghc > 200)
                                {
                                    qghc.Visible = true;
                                }
                                else
                                {
                                    qghc.Visible = false;
                                }
                                //低怠速CO
                                if (ddco > 1.5)
                                {
                                    qdco.Visible = true;
                                }
                                else
                                {
                                    qdco.Visible = false;
                                }
                                //低怠速HC
                                if (ddhc > 250)
                                {
                                    qdhc.Visible = true;
                                }
                                else
                                {
                                    qdhc.Visible = false;
                                }
                                #endregion
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex);
                        }
                    }
                }
                //2.M2类车
                if ((szws != ""&& szws!="-"&& szzl != "" && szzl != "-" && dzzl <= 5000 && dzws > 8 ) && !scllx.Contains("货"))
                {
                    //重型汽车
                    if (dzzl > 3500)
                    {
                        try
                        {
                            if (DateTime.Parse(sccrq) < DateTime.Parse("1995-07-01"))
                            {
                                #region 1995年7月1日前生产的重型汽车
                                //高怠速CO
                                if (dgco > 3.5)
                                {
                                    qgco.Visible = true;
                                }
                                else
                                {
                                    qgco.Visible = false;
                                }
                                //高怠速HC
                                if (dghc > 1200)
                                {
                                    qghc.Visible = true;
                                }
                                else
                                {
                                    qghc.Visible = false;
                                }
                                //低怠速CO
                                if (ddco > 5.0)
                                {
                                    qdco.Visible = true;
                                }
                                else
                                {
                                    qdco.Visible = false;
                                }
                                //低怠速HC
                                if (ddhc > 2000)
                                {
                                    qdhc.Visible = true;
                                }
                                else
                                {
                                    qdhc.Visible = false;
                                }
                                #endregion
                            }
                            else
                            {
                                #region 1995年7月1日起生产的重型汽车
                                //高怠速CO
                                if (dgco > 3.0)
                                {
                                    qgco.Visible = true;
                                }
                                else
                                {
                                    qgco.Visible = false;
                                }
                                //高怠速HC
                                if (dghc > 900)
                                {
                                    qghc.Visible = true;
                                }
                                else
                                {
                                    qghc.Visible = false;
                                }
                                //低怠速CO
                                if (ddco > 4.5)
                                {
                                    qdco.Visible = true;
                                }
                                else
                                {
                                    qdco.Visible = false;
                                }
                                //低怠速HC
                                if (ddhc > 1200)
                                {
                                    qdhc.Visible = true;
                                }
                                else
                                {
                                    qdhc.Visible = false;
                                }
                                #endregion
                            }
                            if (DateTime.Parse(sccrq) >= DateTime.Parse("2004-09-01"))
                            {
                                #region 2004年9月1日起生产的重型汽车
                                //高怠速CO
                                if (dgco > 0.7)
                                {
                                    qgco.Visible = true;
                                }
                                else
                                {
                                    qgco.Visible = false;
                                }
                                //高怠速HC
                                if (dghc > 200)
                                {
                                    qghc.Visible = true;
                                }
                                else
                                {
                                    qghc.Visible = false;
                                }
                                //低怠速CO
                                if (ddco > 1.5)
                                {
                                    qdco.Visible = true;
                                }
                                else
                                {
                                    qdco.Visible = false;
                                }
                                //低怠速HC
                                if (ddhc > 250)
                                {
                                    qdhc.Visible = true;
                                }
                                else
                                {
                                    qdhc.Visible = false;
                                }
                                #endregion
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex);
                        }
                    }
                }
                //3.M3类车(重型汽车)
                if (szzl!=""&& szzl!="-"&&dzzl > 5000 && !scllx.Contains("货"))
                {
                    try
                    {
                        if (DateTime.Parse(sccrq) < DateTime.Parse("1995-07-01"))
                        {
                            #region 1995年7月1日前生产的重型汽车
                            //高怠速CO
                            if (dgco > 3.5)
                            {
                                qgco.Visible = true;
                            }
                            else
                            {
                                qgco.Visible = false;
                            }
                            //高怠速HC
                            if (dghc > 1200)
                            {
                                qghc.Visible = true;
                            }
                            else
                            {
                                qghc.Visible = false;
                            }
                            //低怠速CO
                            if (ddco > 5.0)
                            {
                                qdco.Visible = true;
                            }
                            else
                            {
                                qdco.Visible = false;
                            }
                            //低怠速HC
                            if (ddhc > 2000)
                            {
                                qdhc.Visible = true;
                            }
                            else
                            {
                                qdhc.Visible = false;
                            }
                            #endregion
                        }
                        else
                        {
                            #region 1995年7月1日起生产的重型汽车
                            //高怠速CO
                            if (dgco > 3.0)
                            {
                                qgco.Visible = true;
                            }
                            else
                            {
                                qgco.Visible = false;
                            }
                            //高怠速HC
                            if (dghc > 900)
                            {
                                qghc.Visible = true;
                            }
                            else
                            {
                                qghc.Visible = false;
                            }
                            //低怠速CO
                            if (ddco > 4.5)
                            {
                                qdco.Visible = true;
                            }
                            else
                            {
                                qdco.Visible = false;
                            }
                            //低怠速HC
                            if (ddhc > 1200)
                            {
                                qdhc.Visible = true;
                            }
                            else
                            {
                                qdhc.Visible = false;
                            }
                            #endregion
                        }
                        if (DateTime.Parse(sccrq) >= DateTime.Parse("2004-09-01"))
                        {
                            #region 2004年9月1日起生产的重型汽车
                            //高怠速CO
                            if (dgco > 0.7)
                            {
                                qgco.Visible = true;
                            }
                            else
                            {
                                qgco.Visible = false;
                            }
                            //高怠速HC
                            if (dghc > 200)
                            {
                                qghc.Visible = true;
                            }
                            else
                            {
                                qghc.Visible = false;
                            }
                            //低怠速CO
                            if (ddco > 1.5)
                            {
                                qdco.Visible = true;
                            }
                            else
                            {
                                qdco.Visible = false;
                            }
                            //低怠速HC
                            if (ddhc > 250)
                            {
                                qdhc.Visible = true;
                            }
                            else
                            {
                                qdhc.Visible = false;
                            }
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
                //4.N2类车(重型汽车)
                if ((szzl!=""&& szzl!="-"&&dzzl > 3500 && dzzl <= 12000) && scllx.Contains("货"))
                {
                    try
                    {
                        if (DateTime.Parse(sccrq) < DateTime.Parse("1995-07-01"))
                        {
                            #region 1995年7月1日前生产的重型汽车
                            //高怠速CO
                            if (dgco > 3.5)
                            {
                                qgco.Visible = true;
                            }
                            else
                            {
                                qgco.Visible = false;
                            }
                            //高怠速HC
                            if (dghc > 1200)
                            {
                                qghc.Visible = true;
                            }
                            else
                            {
                                qghc.Visible = false;
                            }
                            //低怠速CO
                            if (ddco > 5.0)
                            {
                                qdco.Visible = true;
                            }
                            else
                            {
                                qdco.Visible = false;
                            }
                            //低怠速HC
                            if (ddhc > 2000)
                            {
                                qdhc.Visible = true;
                            }
                            else
                            {
                                qdhc.Visible = false;
                            }
                            #endregion
                        }
                        else
                        {
                            #region 1995年7月1日起生产的重型汽车
                            //高怠速CO
                            if (dgco > 3.0)
                            {
                                qgco.Visible = true;
                            }
                            else
                            {
                                qgco.Visible = false;
                            }
                            //高怠速HC
                            if (dghc > 900)
                            {
                                qghc.Visible = true;
                            }
                            else
                            {
                                qghc.Visible = false;
                            }
                            //低怠速CO
                            if (ddco > 4.5)
                            {
                                qdco.Visible = true;
                            }
                            else
                            {
                                qdco.Visible = false;
                            }
                            //低怠速HC
                            if (ddhc > 1200)
                            {
                                qdhc.Visible = true;
                            }
                            else
                            {
                                qdhc.Visible = false;
                            }
                            #endregion
                        }
                        if (DateTime.Parse(sccrq) >= DateTime.Parse("2004-09-01"))
                        {
                            #region 2004年9月1日起生产的重型汽车
                            //高怠速CO
                            if (dgco > 0.7)
                            {
                                qgco.Visible = true;
                            }
                            else
                            {
                                qgco.Visible = false;
                            }
                            //高怠速HC
                            if (dghc > 200)
                            {
                                qghc.Visible = true;
                            }
                            else
                            {
                                qghc.Visible = false;
                            }
                            //低怠速CO
                            if (ddco > 1.5)
                            {
                                qdco.Visible = true;
                            }
                            else
                            {
                                qdco.Visible = false;
                            }
                            //低怠速HC
                            if (ddhc > 250)
                            {
                                qdhc.Visible = true;
                            }
                            else
                            {
                                qdhc.Visible = false;
                            }
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
                //5.N3类车(重型汽车)
                if (szzl!=""&& szzl!="-"&&dzzl > 12000 && scllx.Contains("货"))
                {
                    try
                    {
                        if (DateTime.Parse(sccrq) < DateTime.Parse("1995-07-01"))
                        {
                            #region 1995年7月1日前生产的重型汽车
                            //高怠速CO
                            if (dgco > 3.5)
                            {
                                qgco.Visible = true;
                            }
                            else
                            {
                                qgco.Visible = false;
                            }
                            //高怠速HC
                            if (dghc > 1200)
                            {
                                qghc.Visible = true;
                            }
                            else
                            {
                                qghc.Visible = false;
                            }
                            //低怠速CO
                            if (ddco > 5.0)
                            {
                                qdco.Visible = true;
                            }
                            else
                            {
                                qdco.Visible = false;
                            }
                            //低怠速HC
                            if (ddhc > 2000)
                            {
                                qdhc.Visible = true;
                            }
                            else
                            {
                                qdhc.Visible = false;
                            }
                            #endregion
                        }
                        else
                        {
                            #region 1995年7月1日起生产的重型汽车
                            //高怠速CO
                            if (dgco > 3.0)
                            {
                                qgco.Visible = true;
                            }
                            else
                            {
                                qgco.Visible = false;
                            }
                            //高怠速HC
                            if (dghc > 900)
                            {
                                qghc.Visible = true;
                            }
                            else
                            {
                                qghc.Visible = false;
                            }
                            //低怠速CO
                            if (ddco > 4.5)
                            {
                                qdco.Visible = true;
                            }
                            else
                            {
                                qdco.Visible = false;
                            }
                            //低怠速HC
                            if (ddhc > 1200)
                            {
                                qdhc.Visible = true;
                            }
                            else
                            {
                                qdhc.Visible = false;
                            }
                            #endregion
                        }
                        if (DateTime.Parse(sccrq) >= DateTime.Parse("2004-09-01"))
                        {
                            #region 2004年9月1日起生产的重型汽车
                            //高怠速CO
                            if (dgco > 0.7)
                            {
                                qgco.Visible = true;
                            }
                            else
                            {
                                qgco.Visible = false;
                            }
                            //高怠速HC
                            if (dghc > 200)
                            {
                                qghc.Visible = true;
                            }
                            else
                            {
                                qghc.Visible = false;
                            }
                            //低怠速CO
                            if (ddco > 1.5)
                            {
                                qdco.Visible = true;
                            }
                            else
                            {
                                qdco.Visible = false;
                            }
                            //低怠速HC
                            if (ddhc > 250)
                            {
                                qdhc.Visible = true;
                            }
                            else
                            {
                                qdhc.Visible = false;
                            }
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
                //6.小于5座(含5座)的微型面包车
                if (szws!=""&& szws!="-"&&dzws <= 5 && scllx.Contains("微型面包车"))
                {
                    try
                    {
                        if (DateTime.Parse(sccrq) < DateTime.Parse("2001-05-31"))
                        {
                            #region 2001年5月31日前生产的5座(含5座)的微型面包车
                            //高怠速CO
                            if (dgco > 3.0)
                            {
                                qgco.Visible = true;
                            }
                            else
                            {
                                qgco.Visible = false;
                            }
                            //高怠速HC
                            if (dghc > 900)
                            {
                                qghc.Visible = true;
                            }
                            else
                            {
                                qghc.Visible = false;
                            }
                            //低怠速CO
                            if (ddco > 4.5)
                            {
                                qdco.Visible = true;
                            }
                            else
                            {
                                qdco.Visible = false;
                            }
                            //低怠速HC
                            if (ddhc > 900)
                            {
                                qdhc.Visible = true;
                            }
                            else
                            {
                                qdhc.Visible = false;
                            }
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
               else 
                {
                    #region 总质量小于3.5吨的汽车
                    //高怠速CO
                    if (dgco > 0.5)
                    {
                        qgco.Visible = true;
                    }
                    else
                    {
                        qgco.Visible = false;
                    }
                    //高怠速HC
                    if (dghc > 150)
                    {
                        qghc.Visible = true;
                    }
                    else
                    {
                        qghc.Visible = false;
                    }
                    //低怠速CO
                    if (ddco > 1.0)
                    {
                        qdco.Visible = true;
                    }
                    else
                    {
                        qdco.Visible = false;
                    }
                    //低怠速HC
                    if (ddhc > 200)
                    {
                        qdhc.Visible = true;
                    }
                    else
                    {
                        qdhc.Visible = false;
                    }
                    #endregion
                }
                #endregion
                //判断高怠速λ值
                if (sgdsλ != ""&& sgdsλ!="-" && (dgλ < 0.97 || dgλ > 1.03))
                {
                    qgλ.Visible = true;
                }
                else
                {
                    qgλ.Visible = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            CLDEPD();
        }
        //高怠速CO值超限鼠标经过事件
        private void qgco_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("高怠速CO值超出标准限值范围", qgco,10000);
        }
        //高怠速CO值超限鼠标离开事件
        private void qgco_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(qgco);
        }
        //高怠速HC值超限鼠标经过事件
        private void qghc_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("高怠速HC值超出标准限值范围", qghc,10000);
        }
        //高怠速HC值超限鼠标离开事件
        private void qghc_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(qghc);
        }
        //低怠速CO值超限鼠标经过事件
        private void qdco_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("低怠速CO值超出标准限值范围", qdco,10000);
        }
        //低怠速CO值超限鼠标离开事件
        private void qdco_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(qdco);
        }
        //低怠速HC值超限鼠标经过事件
        private void qdhc_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("低怠速HC值超出标准限值范围", qdhc,10000);
        }
        //低怠速HC值超限鼠标离开事件
        private void qdhc_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(qdhc);
        }
        //高怠速λ值超限鼠标经过事件
        private void qgλ_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("高怠速λ值超出标准限值范围", qgλ,10000);
        }
        //高怠速λ值超限鼠标离开事件
        private void qgλ_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(qgλ);
        }
        #endregion

        #region 自由加速法判断
        public void ZYJSjudge()
        {
            try
            {
                cg1.Visible = false;
                cg2.Visible = false;
                cg3.Visible = false;
                string szzl = zzl.Text;//总质量
                string scc = kccc.Text;//车长
                string sccrq = ccrq.Text;//出厂日期
                string scgx1 = cygxs1.Text;//光吸收系数1
                string scgx2 = cygxs2.Text;//光吸收系数2
                string scgx3 = cygxs3.Text;//光吸收系数3
                double dzzl;//总质量
                double dcc;//车长
                double dgxs1;//光吸收系数1
                double dgxs2;//光吸收系数2
                double dgxs3;//光吸收系数3
                #region 判断变量是否有值
                //总质量
                if (szzl == "" || szzl == "-")
                {
                    dzzl = 0;
                }
                else
                {
                    dzzl = Convert.ToDouble(szzl);
                }
                //车长
                if (scc == "" || scc == "-")
                {
                    dcc = 0;
                }
                else
                {
                    dcc = Convert.ToDouble(scc);
                }
                //光吸收系数1
                if (scgx1 == "" || scgx1 == "-")
                {
                    dgxs1 = 0;
                }
                else
                {
                    dgxs1 = Convert.ToDouble(scgx1);
                }
                //光吸收系数2
                if (scgx2 == "" || scgx2 == "-")
                {
                    dgxs2 = 0;
                }
                else
                {
                    dgxs2 = Convert.ToDouble(scgx2);
                }
                //光吸收系数3
                if (scgx3 == "" || scgx3 == "-")
                {
                    dgxs3 = 0;
                }
                else
                {
                    dgxs3 = Convert.ToDouble(scgx3);
                }
                #endregion

                //2001年5月1日与2005年7月1日之间生产的汽车
                if(sccrq=="")
                {
                    sccrq = "2012-05-16";
                }
                if (DateTime.Parse(sccrq) >= DateTime.Parse("2001-05-01") && DateTime.Parse(sccrq) < DateTime.Parse("2005-07-01"))
                {
                    #region 总质量不低于20吨
                    //总质量不低于20吨
                    if (dzzl > 20000)
                    {
                        //光吸收系数1超过2.5
                        if (dgxs1 > 2.5)
                        {
                            cg1.Visible = true;
                        }
                        else
                        {
                            cg1.Visible = false;
                        }
                        //光吸收系数2超过2.5
                        if (dgxs2 > 2.5)
                        {
                            cg2.Visible = true;
                        }
                        else
                        {
                            cg2.Visible = false;
                        }
                        //光吸收系数3超过2.5
                        if (dgxs3 > 2.5)
                        {
                            cg3.Visible = true;
                        }
                        else
                        {
                            cg3.Visible = false;
                        }
                    }
                    #endregion
                    #region 车长不少于12米
                    //车长不少于12米
                    if (dcc > 12000)
                    {
                        //光吸收系数1超过2.5
                        if (dgxs1 > 2.5)
                        {
                            cg1.Visible = true;
                        }
                        else
                        {
                            cg1.Visible = false;
                        }
                        //光吸收系数2超过2.5
                        if (dgxs2 > 2.5)
                        {
                            cg2.Visible = true;
                        }
                        else
                        {
                            cg2.Visible = false;
                        }
                        //光吸收系数3超过2.5
                        if (dgxs3 > 2.5)
                        {
                            cg3.Visible = true;
                        }
                        else
                        {
                            cg3.Visible = false;
                        }
                    }
                    #endregion
                }
                //2005年7月1日起生产的汽车
                if (DateTime.Parse(sccrq) >= DateTime.Parse("2005-07-01"))
                {
                    #region 总质量不低于20吨
                    //总质量不低于20吨
                    if (dzzl > 20000)
                    {
                        //光吸收系数1超过3
                        if (dgxs1 > 2.5)
                        {
                            cg1.Visible = true;
                        }
                        else
                        {
                            cg1.Visible = false;
                        }
                        //光吸收系数2超过3
                        if (dgxs2 > 2.5)
                        {
                            cg2.Visible = true;
                        }
                        else
                        {
                            cg2.Visible = false;
                        }
                        //光吸收系数3超过3
                        if (dgxs3 > 2.5)
                        {
                            cg3.Visible = true;
                        }
                        else
                        {
                            cg3.Visible = false;
                        }
                    }
                    #endregion
                    #region 车长不少于12米
                    //车长不少于12米
                    if (dcc > 12000)
                    {
                        //光吸收系数1超过3
                        if (dgxs1 > 2.5)
                        {
                            cg1.Visible = true;
                        }
                        else
                        {
                            cg1.Visible = false;
                        }
                        //光吸收系数2超过3
                        if (dgxs2 > 2.5)
                        {
                            cg2.Visible = true;
                        }
                        else
                        {
                            cg2.Visible = false;
                        }
                        //光吸收系数3超过3
                        if (dgxs3 > 2.5)
                        {
                            cg3.Visible = true;
                        }
                        else
                        {
                            cg3.Visible = false;
                        }
                    }
                    #endregion
                }
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //光吸收系数1超出标准限值鼠标经过事件
        private void cg1_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("光吸收系数1值超出标准限值范围", cg1,10000);
        }
        //光吸收系数1超出标准限值鼠标离开事件
        private void cg1_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(cg1);
        }
        //光吸收系数2超出标准限值鼠标经过事件
        private void cg2_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("光吸收系数2值超出标准限值范围", cg2,10000);
        }
        //光吸收系数2超出标准限值鼠标离开事件
        private void cg2_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(cg2);
        }
        //光吸收系数3超出标准限值鼠标经过事件
        private void cg3_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("光吸收系数3值超出标准限值范围", cg3,10000);
        }
        //光吸收系数3超出标准限值鼠标离开事件
        private void cg3_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(cg3);
        }
        #endregion
        //车辆等级评定
        public void CLDEPD()
        {
            try
            {
                string ddjpd = "";
                if (dczd.Visible == true || zczdbd.Visible == true || ch1pd.Visible == true || ch2pd.Visible == true || slbs.Visible == true || zwgq.Visible == true || zngq.Visible == true || yngq.Visible == true || ywgq.Visible == true || ydzpd.Visible == true || edzpd.Visible == true || sdzpd.Visible == true || sidzpd.Visible == true || wdzpd.Visible == true || ldzpd.Visible == true || ydzbpd.Visible == true || edzbpd.Visible == true || sdzbpd.Visible == true || sidzbpd.Visible == true || wdzbpd.Visible == true || ldzbpd.Visible == true || wdcspd.Visible == true || lbgvpd.Visible == true || yzzzv.Visible == true || yyzzv.Visible == true || ezzzv.Visible == true || eyzzv.Visible == true || szzzv.Visible == true || syzzv.Visible == true || sizzzv.Visible == true || siyzzv.Visible == true || wzzzv.Visible == true || wyzzv.Visible == true || lzzzv.Visible == true || lyzzv.Visible == true || lqz.Visible == true || lqy.Visible == true || lq.Visible == true || lhz.Visible == true || lhy.Visible == true || lh.Visible == true || cg1.Visible == true || cg2.Visible == true || cg3.Visible == true || qgco.Visible == true || qghc.Visible == true || qgλ.Visible == true || qdco.Visible == true || qdhc.Visible == true)
                {
                    checkBox1.Checked = false;
                    checkBox2.Checked = false;
                    checkBox4.Checked = true;
                }
                else
                {
                    int count1 = 0;
                    if (zwyc.Visible == true || znyc.Visible == true || ywyc.Visible == true || ynyc.Visible == true)
                    {
                        count1++;
                    }
                    if (zwjc.Visible == true || znjc.Visible == true || ywjc.Visible == true || ynjc.Visible == true)
                    {
                        count1++;
                    }
                    if (scsz.Visible == true)
                    {
                        count1++;
                    }
                    //外观检查
                    string[] strarrwg = wgjc.Text.Replace(" ", "").Split(',');
                    string bswg = "";
                    for (int i = 0; i < strarrwg.Length; i++)
                    {
                        if (wgjc.Text.Replace(" ", "") != "" && wjwg.Replace(" ", "") != "")
                        {
                            if (wjwg.Replace(" ", "").Contains(strarrwg[i]))
                            {
                                if (wjgjx.Replace(" ", "").Contains(strarrwg[i]))
                                {
                                    bswg = "1";
                                }
                                if (wjybx.Replace(" ", "").Contains(strarrwg[i]))
                                {
                                    count1++;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    //底盘检查
                    string[] strarrdj = djjc.Text.Replace(" ", "").Split(',');
                    string bsdp = "";
                    for (int i = 0; i < strarrdj.Length; i++)
                    {
                        if (wjdj.Replace(" ", "") != "" && djjc.Text.Replace(" ", "") != "")
                        {
                            if (wjdj.Replace(" ", "").Contains(strarrdj[i]))
                            {
                                if (wjgjx.Replace(" ", "").Contains(strarrdj[i]))
                                {
                                    bsdp = "2";
                                }
                                if (wjybx.Replace(" ", "").Contains(strarrdj[i]))
                                {
                                    count1++;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    //唯一性认定
                    string bswy = "";
                    string[] strarrwyx = wyxrd.Text.Replace(" ", "").Split(',');
                    for (int i = 0; i < strarrwyx.Length; i++)
                    {
                        if (wyxrd.Text.Replace(" ", "") != "" && wjwyxrd.Replace(" ", "") != "")
                        {
                            if (wjwyxrd.Replace(" ", "").Contains(strarrwyx[i]))
                            {
                                bswy = "3";
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    //故障信息诊断
                    string bsgz = "";
                    string[] strarrgzxx = gzxxzd.Text.Replace(" ", "").Split(',');
                    for (int i = 0; i < strarrgzxx.Length; i++)
                    {
                        if (gzxxzd.Text.Replace(" ", "") != "" && wjgzxxzd.Replace(" ", "") != "")
                        {
                            if (wjgzxxzd.Replace(" ", "").Contains(strarrgzxx[i]))
                            {
                                bsgz = "4";
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    //运行检查
                    string bsyx = "";
                    string[] strarryx = yxjc.Text.Replace(" ", "").Split(',');
                    for (int i = 0; i < strarryx.Length; i++)
                    {
                        if (yxjc.Text.Replace(" ", "") != "" && wjyxjc.Replace(" ", "") != "")
                        {
                            if (wjyxjc.Replace(" ", "").Contains(strarryx[i]))
                            {
                                if (wjgjx.Replace(" ", "").Contains(strarryx[i]))
                                {
                                    bsyx = "5";
                                }
                                if (wjybx.Replace(" ", "").Contains(strarryx[i]))
                                {
                                    count1++;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    //核查评定
                    string bshc = "";
                    string[] strarrhc = hcpd.Text.Replace(" ", "").Split(',');
                    for (int i = 0; i < strarrhc.Length; i++)
                    {
                        if (hcpd.Text.Replace(" ", "") != "" && wjhcpd.Replace(" ", "") != "")
                        {
                            if (wjhcpd.Replace(" ", "").Contains(strarrhc[i]))
                            {
                                if (wjfjx.Replace(" ", "").Contains(strarrhc[i]))
                                {
                                    bshc = "6";
                                    break;
                                }
                                else
                                {
                                    bshc = "";
                                }
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    string ejc = "";
                    string[] strejx = ejxm.Text.Replace(" ", "").Split(',');
                    for (int i = 0; i < strejx.Length; i++)
                    {
                        if(ejxm.Text.Replace(" ","")!=""&&wjfjx.Replace(" ","")!="")
                        {
                            if(wjfjx.Replace(" ","").Contains(strejx[i]))
                            {
                                ejc = "二级";
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    if (wjx.Text.Replace(" ", "").Contains("×")||bswg=="1"||bsdp=="2"||bswy=="3"||bsgz=="4"||bsyx=="5"||bshc=="6")
                    {
                        ddjpd = "不合格";
                        checkBox1.Checked = false;
                        checkBox2.Checked = false;
                        checkBox4.Checked = true;
                    }
                    else
                    {
                        if (count1 <= 3)
                        {
                            if (lz1.Text == "②" || lz2.Text == "②" || lz3.Text == "②" || lz4.Text == "②" || lz5.Text == "②" || lz6.Text == "②"||ejc=="二级")
                            {
                                ddjpd = "二级";
                                checkBox2.Checked = true;
                                checkBox4.Checked = false;
                            }
                            else
                            {
                                ddjpd = "一级";
                                checkBox1.Checked = true;
                                checkBox4.Checked = false;
                            }
                        }
                        else if (count1 <= 6||ejc=="二级")
                        {
                            ddjpd = "二级";
                            checkBox2.Checked = true;
                            checkBox4.Checked = false;
                        }
                        else
                        {
                            ddjpd = "不合格";
                            checkBox1.Checked = false;
                            checkBox2.Checked = false;
                            checkBox4.Checked = true;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //综检记录单数据
        public void Print(bool bPrint,string sender)
        {
            try
            {
                if(zwyggq.Text.Replace(" ","")!=""&& zwyggq.Text.Replace(" ", "") != "-")
                {
                    if(zwygczH.Text.Replace(" ","")=="")
                    {
                        zwygczH.Text = "0";
                    }
                    if (zwjgczH.Text.Replace(" ", "") == "")
                    {
                        zwjgczH.Text = "0";
                    }
                }
                if (ywyggq.Text.Replace(" ", "") != "" && ywyggq.Text.Replace(" ", "") != "-")
                {
                    if (ywygczH.Text.Replace(" ", "") == "")
                    {
                        ywygczH.Text = "0";
                    }
                    if (ywjgczH.Text.Replace(" ", "") == "")
                    {
                        ywjgczH.Text = "0";
                    }
                }
                #region 人工检验结果
                //唯一性认定
                ArrayList al = new ArrayList();
                ArrayList wyxs = new ArrayList();
                string rgwyx = "";
                string rgwyxpd = "";
                string[] strwyx = wyxrd.Text.Replace(" ", "").Split(',');
                if (wyxrd.Text.Replace(" ", "") != "" && wjwyxrd.Replace(" ", "") != "")
                {
                    for (int i = 0; i < strwyx.Length; i++)
                    {
                        if (wjwyxrd.Replace(" ", "").Contains(strwyx[i]))
                        {
                            if (wjgjx.Replace(" ", "").Contains(strwyx[i]))
                            {
                                wyxs.Add(strwyx[i]);
                                rgwyx = string.Join("、", (string[])wyxs.ToArray(typeof(string)));
                                rgwyxpd = "不合格";
                                if (wjsz == "18565")
                                {
                                    string wyxxm = OperateIniFile.ReadIniData("count18565", (i + 1).ToString(), "", strFilePath);
                                    al.Add(wyxxm);
                                }
                                else
                                {
                                    string wyxxm = OperateIniFile.ReadIniData("count198", (i + 11).ToString(), "", strFilePath);
                                    al.Add(wyxxm);
                                }
                            }
                        }
                    }
                }
                else
                {
                    rgwyx = "无";
                    rgwyxpd = "合格";
                }
                //故障信息诊断
                string rggzxx = "";
                string rggzxxpd = "";
                ArrayList gzxxs = new ArrayList();
                string[] strwgzxx = gzxxzd.Text.Replace(" ", "").Split(',');
                if (gzxxzd.Text.Replace(" ", "") != "" && wjgzxxzd.Replace(" ", "") != "")
                {
                    for (int i = 0; i < strwgzxx.Length; i++)
                    {
                        if (wjgzxxzd.Replace(" ", "").Contains(strwgzxx[i]))
                        {
                            gzxxs.Add(strwgzxx[i]);
                           rggzxx = string.Join("、", (string[])gzxxs.ToArray(typeof(string)));
                            rggzxxpd = "不合格";
                            if (wjsz == "18565")
                            {
                                string wyxxm = OperateIniFile.ReadIniData("count18565", (i + 12).ToString(), "", strFilePath);
                                al.Add(wyxxm);
                            }
                            else
                            {
                                string wyxxm = OperateIniFile.ReadIniData("count198", (i + 20).ToString(), "", strFilePath);
                                al.Add(wyxxm);
                            }
                        }
                    }
                }
                else
                {
                    rggzxx = "无";
                    rggzxxpd = "合格";
                }
                //外观检查
                string rgwg = "";
                string rgwgpd = "";
                ArrayList wgs = new ArrayList();
                string[] strwg = wgjc.Text.Replace(" ", "").Split(',');
                if (wgjc.Text.Replace(" ", "") != "" && wjwg.Replace(" ", "") != "")
                {
                    for (int i = 0; i < strwg.Length; i++)
                    {
                        if (wjwg.Replace(" ", "").Contains(strwg[i]))
                        {
                            wgs.Add(strwg[i]);
                            rgwg = string.Join("、", (string[])wgs.ToArray(typeof(string)));
                            rgwgpd = "不合格";
                            if (wjsz == "18565")
                            {
                                string wyxxm = OperateIniFile.ReadIniData("count18565", (i + 16).ToString(), "", strFilePath);
                                al.Add(wyxxm);
                            }
                            else
                            {
                                string wyxxm = OperateIniFile.ReadIniData("count198", (i + 24).ToString(), "", strFilePath);
                                al.Add(wyxxm);
                            }
                        }
                    }
                }
                else
                {
                    rgwg = "无";
                    rgwgpd = "合格";
                }
                //运行检查
                string rgyx = "";
                string rgyxpd = "";
                ArrayList yxs = new ArrayList();
                string[] stryx = yxjc.Text.Replace(" ", "").Split(',');
                if (yxjc.Text.Replace(" ", "") != "" && wjyxjc.Replace(" ", "") != "")
                {
                    for (int i = 0; i < stryx.Length; i++)
                    {
                        if (wjyxjc.Replace(" ", "").Contains(stryx[i]))
                        {
                            yxs.Add(stryx[i]);
                            rgyx = string.Join("、", (string[])yxs.ToArray(typeof(string)));
                            rgyxpd = "不合格";
                            if (wjsz == "18565")
                            {
                                string wyxxm = OperateIniFile.ReadIniData("count18565", (i + 69).ToString(), "", strFilePath);
                                al.Add(wyxxm);
                            }
                            else
                            {
                                string wyxxm = OperateIniFile.ReadIniData("count198", (i + 72).ToString(), "", strFilePath);
                                al.Add(wyxxm);
                            }
                        }
                    }
                }
                else
                {
                    rgyx = "无";
                    rgyxpd = "合格";
                }
                //底盘检查
                string rgdp = "";
                string rgdppd = "";
                ArrayList dps = new ArrayList();
                string[] strdp = djjc.Text.Replace(" ", "").Split(',');
                if (djjc.Text.Replace(" ", "") != "" && wjdj.Replace(" ", "") != "")
                {
                    for (int i = 0; i < strdp.Length; i++)
                    {
                        if (wjdj.Replace(" ", "").Contains(strdp[i]))
                        {
                            dps.Add(strdp[i]);
                            rgdp= string.Join("、", (string[])dps.ToArray(typeof(string)));
                            rgdppd = "不合格";
                            if (wjsz == "18565")
                            {
                                string wyxxm = OperateIniFile.ReadIniData("count18565", (i + 85).ToString(), "", strFilePath);
                                al.Add(wyxxm);
                            }
                            else
                            {
                                string wyxxm = OperateIniFile.ReadIniData("count198", (i + 87).ToString(), "", strFilePath);
                                al.Add(wyxxm);
                            }
                        }
                    }
                }
                else
                {
                    rgdp = "无";
                    rgdppd = "合格";
                }
                //核查评定
                string rghc = "";
                string rghcpd = "";
                ArrayList hcs = new ArrayList();
                string[] strhc = hcpd.Text.Replace(" ", "").Split(',');
                if (ywlx.Text.Contains("在用"))
                {
                    rghc = "";
                    rghcpd = "";
                }
                else
                {
                    if (hcpd.Text.Replace(" ", "") != "" && wjhcpd.Replace(" ", "") != "")
                    {
                        for (int i = 0; i < strhc.Length; i++)
                        {
                            if (wjhcpd.Replace(" ", "").Contains(strhc[i]))
                            {
                                if (!wjfjx.Contains(strhc[i]))
                                {
                                    rghc = "无";
                                   rghcpd = "1级";
                                    break;
                                }
                                else
                                {
                                    hcs.Add(strhc[i]);
                                    rghc = string.Join("、", (string[])hcs.ToArray(typeof(string)));
                                    rghcpd = "不合格";
                                    string wyxxm = OperateIniFile.ReadIniData("count198", (i + 1).ToString(), "", strFilePath);
                                    al.Add(wyxxm);
                                }
                            }
                        }
                    }
                    else
                    {
                        rghc= "无";
                        rghcpd = "合格";
                    }
                }
                #endregion
                #region
                #region
                ArrayList arraylist = new ArrayList();
                string sywlbs = "";
                if (ywlx.Text.Replace(" ", "").Contains("在用"))
                {
                    sywlbs = "在用";
                }
                else
                {
                    sywlbs = "申请";
                }
                string yhpd = "";
                if (jjxpd.Text == "○")
                {
                    yhpd = "合格";
                }
                if (jjxpd.Text == "×")
                {
                    yhpd = "不合格";
                }
                string yhbzxz = "";
                if (yhbzz.Text.Replace(" ", "") != "" && yhbzz.Text.Replace(" ", "") != "-")
                {
                    yhbzxz = Convert.ToDouble(yhbzz.Text).ToString("0.0");
                }
                string wdcsxz = "";
                string edcspd = "";
                string dlpj = "";
                if (wdcs.Text.Replace(" ", "") != "" && wdcs.Text.Replace(" ", "") != "-" && edcs.Text.Replace(" ", "") != "" && edcs.Text.Replace(" ", "") != "-")
                {
                    if (Convert.ToDouble(wdcs.Text) >= Convert.ToDouble(edcs.Text))
                    {
                        wdcsxz = edcs.Text;
                        edcspd = "一级";
                        dlpj = "①";
                    }
                    else
                    {
                        wdcsxz = edcs.Text;
                        edcspd = "不合格";
                        dlpj = "×";
                    }
                }
                #endregion
                #region 单轴
                string yzxzs = "";
                string ezxzs = "";
                string siwlxz = "";
                string yzpds = "";
                string ezpds = "";
                string szpds = "";
                string sizpds = "";
                string wzpds = "";
                string lzpds = "";
                //载客车辆
                if (cllx.Text.Contains("客"))
                {
                    //总质量大于1t且座位数不多于8个(M1类车)
                    if (zzl.Text != "" && zzl.Text != "-" && kczws.Text != "" && kczws.Text != "-" && Convert.ToDouble(zzl.Text) > 1000 && Convert.ToDouble(kczws.Text) <= 8)
                    {
                        if (zxzs.Text == "2")
                        {
                            #region 双转向轴
                            yzxzs = "≥60";
                            //一轴
                            if (Convert.ToDouble(TextIsnulls(ydzzdv.Text)) >= 60)
                            {
                                yzpds = "合格";
                            }
                            else
                            {
                                yzpds = "不合格";
                            }
                            ezxzs = "≥60";
                            //二轴
                            if (Convert.ToDouble(TextIsnulls(edzzdv.Text)) >= 60)
                            {
                                ezpds = "合格";
                            }
                            else
                            {
                                ezpds = "不合格";
                            }
                            siwlxz = "≥20";
                            //三轴
                            if (Convert.ToDouble(TextIsnulls(sdzzdv.Text)) >= 20)
                            {
                                szpds = "合格";
                            }
                            else
                            {
                                szpds = "不合格";
                            }
                            //四轴
                            if (Convert.ToDouble(TextIsnulls(sidzzdv.Text)) >= 20)
                            {
                                sizpds = "合格";
                            }
                            else
                            {
                                sizpds = "不合格";
                            }
                            //五轴
                            if (Convert.ToDouble(TextIsnulls(wdzzdv.Text)) >= 20)
                            {
                                wzpds = "合格";
                            }
                            else
                            {
                                wzpds = "不合格";
                            }
                            //六轴
                            if (Convert.ToDouble(TextIsnulls(ldzzdv.Text)) >= 20)
                            {
                                lzpds = "合格";
                            }
                            else
                            {
                                lzpds = "不合格";
                            }
                            #endregion
                        }
                        else
                        {
                            #region 单转向轴
                            yzxzs = "≥60";
                            //一轴
                            if (Convert.ToDouble(TextIsnulls(ydzzdv.Text)) >= 60)
                            {
                                yzpds = "合格";
                            }
                            else
                            {
                                yzpds = "不合格";
                            }
                            ezxzs = "≥20";
                            //二轴
                            if (Convert.ToDouble(TextIsnulls(edzzdv.Text)) >= 20)
                            {
                                ezpds = "合格";
                            }
                            else
                            {
                                ezpds = "不合格";
                            }
                            siwlxz = "≥20";
                            //三轴
                            if (Convert.ToDouble(TextIsnulls(sdzzdv.Text)) >= 20)
                            {
                                szpds = "合格";
                            }
                            else
                            {
                                szpds = "不合格";
                            }
                            //四轴
                            if (Convert.ToDouble(TextIsnulls(sidzzdv.Text)) >= 20)
                            {
                                sizpds = "合格";
                            }
                            else
                            {
                                sizpds = "不合格";
                            }
                            //五轴
                            if (Convert.ToDouble(TextIsnulls(wdzzdv.Text)) >= 20)
                            {
                                wzpds = "合格";
                            }
                            else
                            {
                                wzpds = "不合格";
                            }
                            //六轴
                            if (Convert.ToDouble(TextIsnulls(ldzzdv.Text)) >= 20)
                            {
                                lzpds = "合格";
                            }
                            else
                            {
                                lzpds = "不合格";
                            }
                            #endregion
                        }
                    }
                    //总质量不多于5t且座位数多于8个(M2类车)
                    else if (zzl.Text != "" && zzl.Text != "-" && kczws.Text != "" && kczws.Text != "-" && Convert.ToDouble(zzl.Text) <= 5000 && Convert.ToDouble(kczws.Text) > 8)
                    {
                        if (Convert.ToDouble(zzl.Text) > 3500)
                        {
                            if (zxzs.Text == "2")
                            {
                                #region 双转向轴
                                yzxzs = "≥60";
                                //一轴
                                if (Convert.ToDouble(TextIsnulls(ydzzdv.Text)) >= 60)
                                {
                                    yzpds = "合格";
                                }
                                else
                                {
                                    yzpds = "不合格";
                                }
                                ezxzs = "≥60";
                                //二轴
                                if (Convert.ToDouble(TextIsnulls(edzzdv.Text)) >= 60)
                                {
                                    ezpds = "合格";
                                }
                                else
                                {
                                    ezpds = "不合格";
                                }
                                siwlxz = "≥40";
                                //三轴
                                if (Convert.ToDouble(TextIsnulls(sdzzdv.Text)) >= 40)
                                {
                                    szpds = "合格";
                                }
                                else
                                {
                                    szpds = "不合格";
                                }
                                //四轴
                                if (Convert.ToDouble(TextIsnulls(sidzzdv.Text)) >= 40)
                                {
                                    sizpds = "合格";
                                }
                                else
                                {
                                    sizpds = "不合格";
                                }
                                //五轴
                                if (Convert.ToDouble(TextIsnulls(wdzzdv.Text)) >= 40)
                                {
                                    wzpds = "合格";
                                }
                                else
                                {
                                    wzpds = "不合格";
                                }
                                //六轴
                                if (Convert.ToDouble(TextIsnulls(ldzzdv.Text)) >= 40)
                                {
                                    lzpds = "合格";
                                }
                                else
                                {
                                    lzpds = "不合格";
                                }
                                #endregion
                            }
                            else
                            {
                                #region 单转向轴
                                yzxzs = "≥60";
                                //一轴
                                if (Convert.ToDouble(TextIsnulls(ydzzdv.Text)) >= 60)
                                {
                                    yzpds = "合格";
                                }
                                else
                                {
                                    yzpds = "不合格";
                                }
                                ezxzs = "≥40";
                                //二轴
                                if (Convert.ToDouble(TextIsnulls(edzzdv.Text)) >= 40)
                                {
                                    ezpds = "合格";
                                }
                                else
                                {
                                    ezpds = "不合格";
                                }
                                siwlxz = "≥40";
                                //三轴
                                if (Convert.ToDouble(TextIsnulls(sdzzdv.Text)) >= 40)
                                {
                                    szpds = "合格";
                                }
                                else
                                {
                                    szpds = "不合格";
                                }
                                //四轴
                                if (Convert.ToDouble(TextIsnulls(sidzzdv.Text)) >= 40)
                                {
                                    sizpds = "合格";
                                }
                                else
                                {
                                    sizpds = "不合格";
                                }
                                //五轴
                                if (Convert.ToDouble(TextIsnulls(wdzzdv.Text)) >= 40)
                                {
                                    wzpds = "合格";
                                }
                                else
                                {
                                    wzpds = "不合格";
                                }
                                //六轴
                                if (Convert.ToDouble(TextIsnulls(ldzzdv.Text)) >= 40)
                                {
                                    lzpds = "合格";
                                }
                                else
                                {
                                    lzpds = "不合格";
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            if (zxzs.Text == "2")
                            {
                                #region 双转向轴
                                yzxzs = "≥60";
                                //一轴
                                if (Convert.ToDouble(TextIsnulls(ydzzdv.Text)) >= 60)
                                {
                                    yzpds = "合格";
                                }
                                else
                                {
                                    yzpds = "不合格";
                                }
                                ezxzs = "≥60";
                                //二轴
                                if (Convert.ToDouble(TextIsnulls(edzzdv.Text)) >= 60)
                                {
                                    ezpds = "合格";
                                }
                                else
                                {
                                    ezpds = "×";
                                }
                                siwlxz = "≥50";
                                //三轴
                                if (Convert.ToDouble(TextIsnulls(sdzzdv.Text)) >= 50)
                                {
                                    szpds = "合格";
                                }
                                else
                                {
                                    szpds = "不合格";
                                }
                                //四轴
                                if (Convert.ToDouble(TextIsnulls(sidzzdv.Text)) >= 50)
                                {
                                    sizpds = "合格";
                                }
                                else
                                {
                                    sizpds = "不合格";
                                }
                                //五轴
                                if (Convert.ToDouble(TextIsnulls(wdzzdv.Text)) >= 50)
                                {
                                    wzpds = "合格";
                                }
                                else
                                {
                                    wzpds = "不合格";
                                }
                                //六轴
                                if (Convert.ToDouble(TextIsnulls(ldzzdv.Text)) >= 50)
                                {
                                    lzpds = "合格";
                                }
                                else
                                {
                                    lzpds = "不合格";
                                }
                                #endregion
                            }
                            else
                            {
                                #region 单转向轴
                                yzxzs = "≥60";
                                //一轴
                                if (Convert.ToDouble(TextIsnulls(ydzzdv.Text)) >= 60)
                                {
                                    yzpds = "合格";
                                }
                                else
                                {
                                    yzpds = "不合格";
                                }
                                ezxzs = "≥50";
                                //二轴
                                if (Convert.ToDouble(TextIsnulls(edzzdv.Text)) >= 50)
                                {
                                    ezpds = "合格";
                                }
                                else
                                {
                                    ezpds = "不合格";
                                }
                                siwlxz = "≥50";
                                //三轴
                                if (Convert.ToDouble(TextIsnulls(sdzzdv.Text)) >= 50)
                                {
                                    szpds = "合格";
                                }
                                else
                                {
                                    szpds = "不合格";
                                }
                                //四轴
                                if (Convert.ToDouble(TextIsnulls(sidzzdv.Text)) >= 50)
                                {
                                    sizpds = "合格";
                                }
                                else
                                {
                                    sizpds = "不合格";
                                }
                                //五轴
                                if (Convert.ToDouble(TextIsnulls(wdzzdv.Text)) >= 50)
                                {
                                    wzpds = "合格";
                                }
                                else
                                {
                                    wzpds = "不合格";
                                }
                                //六轴
                                if (Convert.ToDouble(TextIsnulls(ldzzdv.Text)) >= 50)
                                {
                                    lzpds = "合格";
                                }
                                else
                                {
                                    lzpds = "不合格";
                                }
                                #endregion
                            }
                        }
                    }
                    //总质量多于5t(M3类车)
                    else if (zzl.Text != "" && zzl.Text != "-" && Convert.ToDouble(zzl.Text) > 5000)
                    {
                        if (Convert.ToDouble(zzl.Text) > 3500)
                        {
                            if (zxzs.Text == "2")
                            {
                                #region 双转向轴
                                yzxzs = "≥60";
                                //一轴
                                if (Convert.ToDouble(TextIsnulls(ydzzdv.Text)) >= 60)
                                {
                                    yzpds = "合格";
                                }
                                else
                                {
                                    yzpds = "不合格";
                                }
                                ezxzs = "≥60";
                                //二轴
                                if (Convert.ToDouble(TextIsnulls(edzzdv.Text)) >= 60)
                                {
                                    ezpds = "合格";
                                }
                                else
                                {
                                    ezpds = "不合格";
                                }
                                siwlxz = "≥40";
                                //三轴
                                if (Convert.ToDouble(TextIsnulls(sdzzdv.Text)) >= 40)
                                {
                                    szpds = "合格";
                                }
                                else
                                {
                                    szpds = "不合格";
                                }
                                //四轴
                                if (Convert.ToDouble(TextIsnulls(sidzzdv.Text)) >= 40)
                                {
                                    sizpds = "合格";
                                }
                                else
                                {
                                    sizpds = "不合格";
                                }
                                //五轴
                                if (Convert.ToDouble(TextIsnulls(wdzzdv.Text)) >= 40)
                                {
                                    wzpds = "合格";
                                }
                                else
                                {
                                    wzpds = "不合格";
                                }
                                //六轴
                                if (Convert.ToDouble(TextIsnulls(ldzzdv.Text)) >= 40)
                                {
                                    lzpds = "合格";
                                }
                                else
                                {
                                    lzpds = "不合格";
                                }
                                #endregion
                            }
                            else
                            {
                                #region 单转向轴
                                yzxzs = "≥60";
                                //一轴
                                if (Convert.ToDouble(TextIsnulls(ydzzdv.Text)) >= 60)
                                {
                                    yzpds = "合格";
                                }
                                else
                                {
                                    yzpds = "不合格";
                                }
                                ezxzs = "≥40";
                                //二轴
                                if (Convert.ToDouble(TextIsnulls(edzzdv.Text)) >= 40)
                                {
                                    ezpds = "合格";
                                }
                                else
                                {
                                    ezpds = "不合格";
                                }
                                siwlxz = "≥40";
                                //三轴
                                if (Convert.ToDouble(TextIsnulls(sdzzdv.Text)) >= 40)
                                {
                                    szpds = "合格";
                                }
                                else
                                {
                                    szpds = "不合格";
                                }
                                //四轴
                                if (Convert.ToDouble(TextIsnulls(sidzzdv.Text)) >= 40)
                                {
                                    sizpds = "合格";
                                }
                                else
                                {
                                    sizpds = "不合格";
                                }
                                //五轴
                                if (Convert.ToDouble(TextIsnulls(wdzzdv.Text)) >= 40)
                                {
                                    wzpds = "合格";
                                }
                                else
                                {
                                    wzpds = "不合格";
                                }
                                //六轴
                                if (Convert.ToDouble(TextIsnulls(ldzzdv.Text)) >= 40)
                                {
                                    lzpds = "合格";
                                }
                                else
                                {
                                    lzpds = "不合格";
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            if (zxzs.Text == "2")
                            {
                                #region 双转向轴
                                yzxzs = "≥60";
                                //一轴
                                if (Convert.ToDouble(TextIsnulls(ydzzdv.Text)) >= 60)
                                {
                                    yzpds = "合格";
                                }
                                else
                                {
                                    yzpds = "不合格";
                                }
                                ezxzs = "≥60";
                                //二轴
                                if (Convert.ToDouble(TextIsnulls(edzzdv.Text)) >= 60)
                                {
                                    ezpds = "合格";
                                }
                                else
                                {
                                    ezpds = "不合格";
                                }
                                siwlxz = "≥50";
                                //三轴
                                if (Convert.ToDouble(TextIsnulls(sdzzdv.Text)) >= 50)
                                {
                                    szpds = "合格";
                                }
                                else
                                {
                                    szpds = "不合格";
                                }
                                //四轴
                                if (Convert.ToDouble(TextIsnulls(sidzzdv.Text)) >= 50)
                                {
                                    sizpds = "合格";
                                }
                                else
                                {
                                    sizpds = "不合格";
                                }
                                //五轴
                                if (Convert.ToDouble(TextIsnulls(wdzzdv.Text)) >= 50)
                                {
                                    wzpds = "合格";
                                }
                                else
                                {
                                    wzpds = "不合格";
                                }
                                //六轴
                                if (Convert.ToDouble(TextIsnulls(ldzzdv.Text)) >= 50)
                                {
                                    lzpds = "合格";
                                }
                                else
                                {
                                    lzpds = "不合格";
                                }
                                #endregion
                            }
                            else
                            {
                                #region 单转向轴
                                yzxzs = "≥60";
                                //一轴
                                if (Convert.ToDouble(TextIsnulls(ydzzdv.Text)) >= 60)
                                {
                                    yzpds = "合格";
                                }
                                else
                                {
                                    yzpds = "不合格";
                                }
                                ezxzs = "≥50";
                                //二轴
                                if (Convert.ToDouble(TextIsnulls(edzzdv.Text)) >= 50)
                                {
                                    ezpds = "合格";
                                }
                                else
                                {
                                    ezpds = "不合格";
                                }
                                siwlxz = "≥50";
                                //三轴
                                if (Convert.ToDouble(TextIsnulls(sdzzdv.Text)) >= 50)
                                {
                                    szpds = "合格";
                                }
                                else
                                {
                                    szpds = "不合格";
                                }
                                //四轴
                                if (Convert.ToDouble(TextIsnulls(sidzzdv.Text)) >= 50)
                                {
                                    sizpds = "合格";
                                }
                                else
                                {
                                    sizpds = "不合格";
                                }
                                //五轴
                                if (Convert.ToDouble(TextIsnulls(wdzzdv.Text)) >= 50)
                                {
                                    wzpds = "合格";
                                }
                                else
                                {
                                    wzpds = "不合格";
                                }
                                //六轴
                                if (Convert.ToDouble(TextIsnulls(ldzzdv.Text)) >= 50)
                                {
                                    lzpds = "合格";
                                }
                                else
                                {
                                    lzpds = "不合格";
                                }
                                #endregion
                            }
                        }
                    }
                }
                //载货车辆
                else if (cllx.Text.Contains("货"))
                {
                    //总质量不大于3.5t(N1类车)
                    if (zzl.Text != "" && zzl.Text != "-" && Convert.ToDouble(zzl.Text) <= 3500)
                    {
                        if (zxzs.Text == "2")
                        {
                            #region 双转向轴
                            yzxzs = "≥60";
                            //一轴
                            if (Convert.ToDouble(TextIsnulls(ydzzdv.Text)) >= 60)
                            {
                                yzpds = "合格";
                            }
                            else
                            {
                                yzpds = "不合格";
                            }
                            ezxzs = "≥60";
                            //二轴
                            if (Convert.ToDouble(TextIsnulls(edzzdv.Text)) >= 60)
                            {
                                ezpds = "合格";
                            }
                            else
                            {
                                ezpds = "不合格";
                            }
                            siwlxz = "≥20";
                            //三轴
                            if (Convert.ToDouble(TextIsnulls(sdzzdv.Text)) >= 20)
                            {
                                szpds = "合格";
                            }
                            else
                            {
                                szpds = "不合格";
                            }
                            //四轴
                            if (Convert.ToDouble(TextIsnulls(sidzzdv.Text)) >= 20)
                            {
                                sizpds = "合格";
                            }
                            else
                            {
                                sizpds = "不合格";
                            }
                            //五轴
                            if (Convert.ToDouble(TextIsnulls(wdzzdv.Text)) >= 20)
                            {
                                wzpds = "合格";
                            }
                            else
                            {
                                wzpds = "不合格";
                            }
                            //六轴
                            if (Convert.ToDouble(TextIsnulls(ldzzdv.Text)) >= 20)
                            {
                                lzpds = "合格";
                            }
                            else
                            {
                                lzpds = "不合格";
                            }
                            #endregion
                        }
                        else
                        {
                            #region 单转向轴
                            yzxzs = "≥60";
                            //一轴
                            if (Convert.ToDouble(TextIsnulls(ydzzdv.Text)) >= 60)
                            {
                                yzpds = "合格";
                            }
                            else
                            {
                                yzpds = "不合格";
                            }
                            ezxzs = "≥20";
                            //二轴
                            if (Convert.ToDouble(TextIsnulls(edzzdv.Text)) >= 20)
                            {
                                ezpds = "合格";
                            }
                            else
                            {
                                ezpds = "不合格";
                            }
                            siwlxz = "≥20";
                            //三轴
                            if (Convert.ToDouble(TextIsnulls(sdzzdv.Text)) >= 20)
                            {
                                szpds = "合格";
                            }
                            else
                            {
                                szpds = "不合格";
                            }
                            //四轴
                            if (Convert.ToDouble(TextIsnulls(sidzzdv.Text)) >= 20)
                            {
                                sizpds = "合格";
                            }
                            else
                            {
                                sizpds = "不合格";
                            }
                            //五轴
                            if (Convert.ToDouble(TextIsnulls(wdzzdv.Text)) >= 20)
                            {
                                wzpds = "合格";
                            }
                            else
                            {
                                wzpds = "不合格";
                            }
                            //六轴
                            if (Convert.ToDouble(TextIsnulls(ldzzdv.Text)) >= 20)
                            {
                                lzpds = "合格";
                            }
                            else
                            {
                                lzpds = "不合格";
                            }
                            #endregion
                        }
                    }
                    //总质量在3.5t与12t之间(N2类车)
                    else if (zzl.Text != "" && zzl.Text != "-" && Convert.ToDouble(zzl.Text) > 3500 && Convert.ToDouble(zzl.Text) <= 12000)
                    {
                        if (zxzs.Text == "2")
                        {
                            #region 双转向轴
                            yzxzs = "≥60";
                            //一轴
                            if (Convert.ToDouble(TextIsnulls(ydzzdv.Text)) >= 60)
                            {
                                yzpds = "合格";
                            }
                            else
                            {
                                yzpds = "不合格";
                            }
                            ezxzs = "≥60";
                            //二轴
                            if (Convert.ToDouble(TextIsnulls(edzzdv.Text)) >= 60)
                            {
                                ezpds = "合格";
                            }
                            else
                            {
                                ezpds = "不合格";
                            }
                            siwlxz = "≥50";
                            //三轴
                            if (Convert.ToDouble(TextIsnulls(sdzzdv.Text)) >= 50)
                            {
                                szpds = "合格";
                            }
                            else
                            {
                                szpds = "不合格";
                            }
                            //四轴
                            if (Convert.ToDouble(TextIsnulls(sidzzdv.Text)) >= 50)
                            {
                                sizpds = "合格";
                            }
                            else
                            {
                                sizpds = "不合格";
                            }
                            //五轴
                            if (Convert.ToDouble(TextIsnulls(wdzzdv.Text)) >= 50)
                            {
                                wzpds = "合格";
                            }
                            else
                            {
                                wzpds = "不合格";
                            }
                            //六轴
                            if (Convert.ToDouble(TextIsnulls(ldzzdv.Text)) >= 50)
                            {
                                lzpds = "合格";
                            }
                            else
                            {
                                lzpds = "不合格";
                            }
                            #endregion
                        }
                        else
                        {
                            #region 单转向轴
                            yzxzs = "≥60";
                            //一轴
                            if (Convert.ToDouble(TextIsnulls(ydzzdv.Text)) >= 60)
                            {
                                yzpds = "合格";
                            }
                            else
                            {
                                yzpds = "不合格";
                            }
                            ezxzs = "≥50";
                            //二轴
                            if (Convert.ToDouble(TextIsnulls(edzzdv.Text)) >= 50)
                            {
                                ezpds = "合格";
                            }
                            else
                            {
                                ezpds = "不合格";
                            }
                            siwlxz = "≥50";
                            //三轴
                            if (Convert.ToDouble(TextIsnulls(sdzzdv.Text)) >= 50)
                            {
                                szpds = "合格";
                            }
                            else
                            {
                                szpds = "不合格";
                            }
                            //四轴
                            if (Convert.ToDouble(TextIsnulls(sidzzdv.Text)) >= 50)
                            {
                                sizpds = "合格";
                            }
                            else
                            {
                                sizpds = "不合格";
                            }
                            //五轴
                            if (Convert.ToDouble(TextIsnulls(wdzzdv.Text)) >= 50)
                            {
                                wzpds = "合格";
                            }
                            else
                            {
                                wzpds = "不合格";
                            }
                            //六轴
                            if (Convert.ToDouble(TextIsnulls(ldzzdv.Text)) >= 50)
                            {
                                lzpds = "合格";
                            }
                            else
                            {
                                lzpds = "不合格";
                            }
                            #endregion
                        }
                    }
                    //总质量大于12t(N3类车)
                    else if (zzl.Text != "" && zzl.Text != "-" && Convert.ToDouble(zzl.Text) > 12000)
                    {
                        if (zxzs.Text == "2")
                        {
                            #region 双转向轴
                            yzxzs = "≥60";
                            //一轴
                            if (Convert.ToDouble(TextIsnulls(ydzzdv.Text)) >= 60)
                            {
                                yzpds = "合格";
                            }
                            else
                            {
                                yzpds = "不合格";
                            }
                            ezxzs = "≥60";
                            //二轴
                            if (Convert.ToDouble(TextIsnulls(edzzdv.Text)) >= 60)
                            {
                                ezpds = "合格";
                            }
                            else
                            {
                                ezpds = "不合格";
                            }
                            siwlxz = "≥50";
                            //三轴
                            if (Convert.ToDouble(TextIsnulls(sdzzdv.Text)) >= 50)
                            {
                                szpds = "合格";
                            }
                            else
                            {
                                szpds = "不合格";
                            }
                            //四轴
                            if (Convert.ToDouble(TextIsnulls(sidzzdv.Text)) >= 50)
                            {
                                sizpds = "合格";
                            }
                            else
                            {
                                sizpds = "不合格";
                            }
                            //五轴
                            if (Convert.ToDouble(TextIsnulls(wdzzdv.Text)) >= 50)
                            {
                                wzpds = "合格";
                            }
                            else
                            {
                                wzpds = "不合格";
                            }
                            //六轴
                            if (Convert.ToDouble(TextIsnulls(ldzzdv.Text)) >= 50)
                            {
                                lzpds = "合格";
                            }
                            else
                            {
                                lzpds = "不合格";
                            }
                            #endregion
                        }
                        else
                        {
                            #region 单转向轴
                            yzxzs = "≥60";
                            //一轴
                            if (Convert.ToDouble(TextIsnulls(ydzzdv.Text)) >= 60)
                            {
                                yzpds = "合格";
                            }
                            else
                            {
                                yzpds = "不合格";
                            }
                            ezxzs = "≥50";
                            //二轴
                            if (Convert.ToDouble(TextIsnulls(edzzdv.Text)) >= 50)
                            {
                                ezpds = "合格";
                            }
                            else
                            {
                                ezpds = "不合格";
                            }
                            siwlxz = "≥50";
                            //三轴
                            if (Convert.ToDouble(TextIsnulls(sdzzdv.Text)) >= 50)
                            {
                                szpds = "合格";
                            }
                            else
                            {
                                szpds = "不合格";
                            }
                            //四轴
                            if (Convert.ToDouble(TextIsnulls(sidzzdv.Text)) >= 50)
                            {
                                sizpds = "合格";
                            }
                            else
                            {
                                sizpds = "不合格";
                            }
                            //五轴
                            if (Convert.ToDouble(TextIsnulls(wdzzdv.Text)) >= 50)
                            {
                                wzpds = "合格";
                            }
                            else
                            {
                                wzpds = "不合格";
                            }
                            //六轴
                            if (Convert.ToDouble(TextIsnulls(ldzzdv.Text)) >= 50)
                            {
                                lzpds = "合格";
                            }
                            else
                            {
                                lzpds = "不合格";
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        if (zxzs.Text == "2")
                        {
                            #region 双转向轴
                            yzxzs = "≥60";
                            //一轴
                            if (Convert.ToDouble(TextIsnulls(ydzzdv.Text)) >= 60)
                            {
                                yzpds = "合格";
                            }
                            else
                            {
                                yzpds = "不合格";
                            }
                            ezxzs = "≥60";
                            //二轴
                            if (Convert.ToDouble(TextIsnulls(edzzdv.Text)) >= 60)
                            {
                                ezpds = "合格";
                            }
                            else
                            {
                                ezpds = "不合格";
                            }
                            siwlxz = "≥20";
                            //三轴
                            if (Convert.ToDouble(TextIsnulls(sdzzdv.Text)) >= 20)
                            {
                                szpds = "合格";
                            }
                            else
                            {
                                szpds = "不合格";
                            }
                            //四轴
                            if (Convert.ToDouble(TextIsnulls(sidzzdv.Text)) >= 20)
                            {
                                sizpds = "合格";
                            }
                            else
                            {
                                sizpds = "不合格";
                            }
                            //五轴
                            if (Convert.ToDouble(TextIsnulls(wdzzdv.Text)) >= 20)
                            {
                                wzpds = "合格";
                            }
                            else
                            {
                                wzpds = "不合格";
                            }
                            //六轴
                            if (Convert.ToDouble(TextIsnulls(ldzzdv.Text)) >= 20)
                            {
                                lzpds = "合格";
                            }
                            else
                            {
                                lzpds = "不合格";
                            }
                            #endregion
                        }
                        else
                        {
                            #region 单转向轴
                            yzxzs = "≥60";
                            //一轴
                            if (Convert.ToDouble(TextIsnulls(ydzzdv.Text)) >= 60)
                            {
                                yzpds = "合格";
                            }
                            else
                            {
                                yzpds = "不合格";
                            }
                            ezxzs = "≥20";
                            //二轴
                            if (Convert.ToDouble(TextIsnulls(edzzdv.Text)) >= 20)
                            {
                                ezpds = "合格";
                            }
                            else
                            {
                                ezpds = "不合格";
                            }
                            siwlxz = "≥20";
                            //三轴
                            if (Convert.ToDouble(TextIsnulls(sdzzdv.Text)) >= 20)
                            {
                                szpds = "合格";
                            }
                            else
                            {
                                szpds = "不合格";
                            }
                            //四轴
                            if (Convert.ToDouble(TextIsnulls(sidzzdv.Text)) >= 20)
                            {
                                sizpds = "合格";
                            }
                            else
                            {
                                sizpds = "不合格";
                            }
                            //五轴
                            if (Convert.ToDouble(TextIsnulls(wdzzdv.Text)) >= 20)
                            {
                                wzpds = "合格";
                            }
                            else
                            {
                                wzpds = "不合格";
                            }
                            //六轴
                            if (Convert.ToDouble(TextIsnulls(ldzzdv.Text)) >= 20)
                            {
                                lzpds = "合格";
                            }
                            else
                            {
                                lzpds = "不合格";
                            }
                            #endregion
                        }
                    }
                }
                else
                {
                    if (cllx.Text.Replace(" ", "").Contains("牵引"))
                    {
                        //牵引车
                        if (zxzs.Text.Replace(" ", "").Contains("2"))
                        {
                            #region 双转向轴
                            yzxzs = "≥60";
                            //一轴
                            if (Convert.ToDouble(TextIsnulls(ydzzdv.Text.Replace(" ", ""))) >= 60)
                            {
                                yzpds = "合格";
                            }
                            else
                            {
                                yzpds = "不合格";
                            }
                            ezxzs = "≥60";
                            //二轴
                            if (Convert.ToDouble(TextIsnulls(edzzdv.Text.Replace(" ", ""))) >= 60)
                            {
                                ezpds = "合格";
                            }
                            else
                            {
                                ezpds = "不合格";
                            }
                            siwlxz = "≥50";
                            //三轴
                            if (Convert.ToDouble(TextIsnulls(sdzzdv.Text.Replace(" ", ""))) >= 50)
                            {
                                szpds = "合格";
                            }
                            else
                            {
                                szpds = "不合格";
                            }
                            //四轴
                            if (Convert.ToDouble(TextIsnulls(sidzzdv.Text.Replace(" ", ""))) >= 50)
                            {
                                sizpds = "合格";
                            }
                            else
                            {
                                sizpds = "不合格";
                            }
                            //五轴
                            if (Convert.ToDouble(TextIsnulls(wdzzdv.Text.Replace(" ", ""))) >= 50)
                            {
                                wzpds = "合格";
                            }
                            else
                            {
                                wzpds = "不合格";
                            }
                            //六轴
                            if (Convert.ToDouble(TextIsnulls(ldzzdv.Text.Replace(" ", ""))) >= 50)
                            {
                                lzpds = "合格";
                            }
                            else
                            {
                                lzpds = "不合格";
                            }
                            #endregion
                        }
                        else
                        {
                            #region 单转向轴
                            yzxzs = "≥60";
                            //一轴
                            if (Convert.ToDouble(TextIsnulls(ydzzdv.Text.Replace(" ", ""))) >= 60)
                            {
                                yzpds = "合格";
                            }
                            else
                            {
                                yzpds = "不合格";
                            }
                            ezxzs = "≥50";
                            //二轴
                            if (Convert.ToDouble(TextIsnulls(edzzdv.Text.Replace(" ", ""))) >= 50)
                            {
                                ezpds = "合格";
                            }
                            else
                            {
                                ezpds = "不合格";
                            }
                            siwlxz = "≥50";
                            //三轴
                            if (Convert.ToDouble(TextIsnulls(sdzzdv.Text.Replace(" ", ""))) >= 50)
                            {
                                szpds = "合格";
                            }
                            else
                            {
                                szpds = "不合格";
                            }
                            //四轴
                            if (Convert.ToDouble(TextIsnulls(sidzzdv.Text.Replace(" ", ""))) >= 50)
                            {
                                sizpds = "合格";
                            }
                            else
                            {
                                sizpds = "不合格";
                            }
                            //五轴
                            if (Convert.ToDouble(TextIsnulls(wdzzdv.Text.Replace(" ", ""))) >= 50)
                            {
                                wzpds = "合格";
                            }
                            else
                            {
                                wzpds = "不合格";
                            }
                            //六轴
                            if (Convert.ToDouble(TextIsnulls(ldzzdv.Text.Replace(" ", ""))) >= 50)
                            {
                                lzpds = "合格";
                            }
                            else
                            {
                                lzpds = "不合格";
                            }
                            #endregion
                        }
                    }
                    else if (cllx.Text.Replace(" ", "").Contains("半挂"))
                    {
                        //半挂车
                        #region 半挂车
                        yzxzs = "≥55";
                        //一轴
                        if (Convert.ToDouble(TextIsnulls(ydzzdv.Text.Replace(" ", ""))) >= 55)
                        {
                            yzpds = "合格";
                        }
                        else
                        {
                            yzpds = "不合格";
                        }
                        ezxzs = "≥55";
                        //二轴
                        if (Convert.ToDouble(TextIsnulls(edzzdv.Text.Replace(" ", ""))) >= 55)
                        {
                            ezpds = "合格";
                        }
                        else
                        {
                            ezpds = "不合格";
                        }
                        siwlxz = "≥55";
                        //三轴
                        if (Convert.ToDouble(TextIsnulls(sdzzdv.Text.Replace(" ", ""))) >= 55)
                        {
                            szpds = "合格";
                        }
                        else
                        {
                            szpds = "不合格";
                        }
                        //四轴
                        if (Convert.ToDouble(TextIsnulls(sidzzdv.Text.Replace(" ", ""))) >= 55)
                        {
                            sizpds = "合格";
                        }
                        else
                        {
                            sizpds = "不合格";
                        }
                        //五轴
                        if (Convert.ToDouble(TextIsnulls(wdzzdv.Text.Replace(" ", ""))) >= 55)
                        {
                            wzpds = "合格";
                        }
                        else
                        {
                            wzpds = "不合格";
                        }
                        //六轴
                        if (Convert.ToDouble(TextIsnulls(ldzzdv.Text.Replace(" ", ""))) >= 55)
                        {
                            lzpds = "合格";
                        }
                        else
                        {
                            lzpds = "不合格";
                        }
                        #endregion
                    }
                    else if (cllx.Text.Replace(" ", "").Contains("全挂"))
                    {
                        //全挂车
                        #region 全挂车
                        yzxzs = "≥55";
                        //一轴
                        if (Convert.ToDouble(TextIsnulls(ydzzdv.Text.Replace(" ", ""))) >= 55)
                        {
                            yzpds = "合格";
                        }
                        else
                        {
                            yzpds = "不合格";
                        }
                        ezxzs = "≥55";
                        //二轴
                        if (Convert.ToDouble(TextIsnulls(edzzdv.Text.Replace(" ", ""))) >= 55)
                        {
                            ezpds = "合格";
                        }
                        else
                        {
                            ezpds = "不合格";
                        }
                        siwlxz = "≥55";
                        //三轴
                        if (Convert.ToDouble(TextIsnulls(sdzzdv.Text.Replace(" ", ""))) >= 55)
                        {
                            szpds = "合格";
                        }
                        else
                        {
                            szpds = "不合格";
                        }
                        //四轴
                        if (Convert.ToDouble(TextIsnulls(sidzzdv.Text.Replace(" ", ""))) >= 55)
                        {
                            sizpds = "合格";
                        }
                        else
                        {
                            sizpds = "不合格";
                        }
                        //五轴
                        if (Convert.ToDouble(TextIsnulls(wdzzdv.Text.Replace(" ", ""))) >= 55)
                        {
                            wzpds = "合格";
                        }
                        else
                        {
                            wzpds = "不合格";
                        }
                        //六轴
                        if (Convert.ToDouble(TextIsnulls(ldzzdv.Text.Replace(" ", ""))) >= 55)
                        {
                            lzpds = "合格";
                        }
                        else
                        {
                            lzpds = "不合格";
                        }
                        #endregion
                    }
                    else
                    {
                        if (zxzs.Text == "2")
                        {
                            #region 双转向轴
                            yzxzs = "≥60";
                            //一轴
                            if (Convert.ToDouble(TextIsnulls(ydzzdv.Text)) >= 60)
                            {
                                yzpds = "合格";
                            }
                            else
                            {
                                yzpds = "不合格";
                            }
                            ezxzs = "≥60";
                            //二轴
                            if (Convert.ToDouble(TextIsnulls(edzzdv.Text)) >= 60)
                            {
                                ezpds = "合格";
                            }
                            else
                            {
                                ezpds = "不合格";
                            }
                            siwlxz = "≥20";
                            //三轴
                            if (Convert.ToDouble(TextIsnulls(sdzzdv.Text)) >= 20)
                            {
                                szpds = "合格";
                            }
                            else
                            {
                                szpds = "不合格";
                            }
                            //四轴
                            if (Convert.ToDouble(TextIsnulls(sidzzdv.Text)) >= 20)
                            {
                                sizpds = "合格";
                            }
                            else
                            {
                                sizpds = "不合格";
                            }
                            //五轴
                            if (Convert.ToDouble(TextIsnulls(wdzzdv.Text)) >= 20)
                            {
                                wzpds = "合格";
                            }
                            else
                            {
                                wzpds = "不合格";
                            }
                            //六轴
                            if (Convert.ToDouble(TextIsnulls(ldzzdv.Text)) >= 20)
                            {
                                lzpds = "合格";
                            }
                            else
                            {
                                lzpds = "不合格";
                            }
                            #endregion
                        }
                        else
                        {
                            #region 单转向轴
                            yzxzs = "≥60";
                            //一轴
                            if (Convert.ToDouble(TextIsnulls(ydzzdv.Text)) >= 60)
                            {
                                yzpds = "合格";
                            }
                            else
                            {
                                yzpds = "不合格";
                            }
                            ezxzs = "≥20";
                            //二轴
                            if (Convert.ToDouble(TextIsnulls(edzzdv.Text)) >= 20)
                            {
                                ezpds = "合格";
                            }
                            else
                            {
                                ezpds = "不合格";
                            }
                            siwlxz = "≥20";
                            //三轴
                            if (Convert.ToDouble(TextIsnulls(sdzzdv.Text)) >= 20)
                            {
                                szpds = "合格";
                            }
                            else
                            {
                                szpds = "不合格";
                            }
                            //四轴
                            if (Convert.ToDouble(TextIsnulls(sidzzdv.Text)) >= 20)
                            {
                                sizpds = "合格";
                            }
                            else
                            {
                                sizpds = "不合格";
                            }
                            //五轴
                            if (Convert.ToDouble(TextIsnulls(wdzzdv.Text)) >= 20)
                            {
                                wzpds = "合格";
                            }
                            else
                            {
                                wzpds = "不合格";
                            }
                            //六轴
                            if (Convert.ToDouble(TextIsnulls(ldzzdv.Text)) >= 20)
                            {
                                lzpds = "合格";
                            }
                            else
                            {
                                lzpds = "不合格";
                            }
                            #endregion
                        }
                    }
                }
                #endregion
                #region 检验结论
                string cldjxz = "";
                if (checkBox1.Checked)
                {
                    cldjxz = "一级";
                }
                if (checkBox2.Checked == true)
                {
                    cldjxz = "二级";
                }
                if (checkBox4.Checked == true)
                {
                    cldjxz = "不合格";
                }
                string jyjl = "";
                string swtr = "";
                if (jylb.Text == "等级评定")
                {
                    jyjl = cldjxz;
                    swtr = syr.Text;
                }
                else
                {
                    jyjl = "检验";
                    swtr = sjdw.Text;
                    if (cldjxz == "一级" || cldjxz == "二级")
                    {
                        cldjxz = "合格";
                    }
                    if (cldjxz == "不合格")
                    {
                        cldjxz = "不合格";
                    }
                }
                string sjylb = "";
                if (jylb.Text.Contains("等级评定"))
                {
                    sjylb = "技术等级评定";
                }
                else if (jylb.Text.Contains("二级维护"))
                {
                    sjylb = "二级维护竣工质量检验";
                }
                else if (jylb.Text.Contains("竣工委托"))
                {
                    sjylb = "汽车大修竣工质量检验";
                }
                else
                {
                    sjylb = jylb.Text;
                }
                #endregion
                #region 排放性
                string pszzl = zzl.Text;//总质量
                string scc = kccc.Text;//车长
                string sccrq = ccrq.Text;//出厂日期
                string scgx1 = cygxs1.Text;//光吸收系数1
                string scgx2 = cygxs2.Text;//光吸收系数2
                string scgx3 = cygxs3.Text;//光吸收系数3
                string scgxavg = cygxsavg.Text;//光吸收系数平均值
                double dpzzl;//总质量
                double dcc;//车长
                double dgxs1;//光吸收系数1
                double dgxs2;//光吸收系数2
                double dgxs3;//光吸收系数3
                double dgxsavg;//光吸收系数平均值
                string gxspd1 = "";
                string gxspd2 = "";
                string gxspd3 = "";
                string gxsxz = "";
                string gxspds = "";
                #region 判断变量是否有值
                //总质量
                if (pszzl == "" || pszzl == "-")
                {
                    dpzzl = 0;
                }
                else
                {
                    dpzzl = Convert.ToDouble(pszzl);
                }
                //车长
                if (scc == "" || scc == "-")
                {
                    dcc = 0;
                }
                else
                {
                    dcc = Convert.ToDouble(scc);
                }
                //光吸收系数1
                if (scgx1 == "" || scgx1 == "-")
                {
                    dgxs1 = 0;
                }
                else
                {
                    dgxs1 = Convert.ToDouble(scgx1);
                }
                //光吸收系数2
                if (scgx2 == "" || scgx2 == "-")
                {
                    dgxs2 = 0;
                }
                else
                {
                    dgxs2 = Convert.ToDouble(scgx2);
                }
                //光吸收系数3
                if (scgx3 == "" || scgx3 == "-")
                {
                    dgxs3 = 0;
                }
                else
                {
                    dgxs3 = Convert.ToDouble(scgx3);
                }
                //光吸收系数平均值
                if (scgxavg == "" || scgxavg == "-")
                {
                    dgxsavg = 0;
                }
                else
                {
                    dgxsavg = Convert.ToDouble(scgxavg);
                }
                #endregion

                if (ccrq.Text == "" || ccrq.Text == "-")
                {
                    sccrq = "2005-08-09";
                }
                if (zzl.Text == "" || zzl.Text == "-")
                {
                    dpzzl = 22000;
                }
                if (kccc.Text == "" || kccc.Text == "-")
                {
                    dcc = 13400;
                }
                //2001年5月1日与2005年7月1日之间生产的汽车
                if (sccrq != "" && sccrq != "-"&&cypd.Text.Replace(" ","")!="")
                {
                    gxsxz = "≤2.5";
                    #region 总质量不低于20吨
                    //总质量不低于20吨
                    if (dpzzl > 20000)
                    {
                        //光吸收系数1超过2.5
                        if (dgxs1 > 2.5)
                        {
                            gxspd1 = "不合格";
                        }
                        else
                        {
                            gxspd1 = "合格";
                        }
                        //光吸收系数2超过2.5
                        if (dgxs2 > 2.5)
                        {
                            gxspd2 = "不合格";
                        }
                        else
                        {
                            gxspd2 = "合格";
                        }
                        //光吸收系数3超过2.5
                        if (dgxs3 > 2.5)
                        {
                            gxspd3 = "不合格";
                        }
                        else
                        {
                            gxspd3 = "合格";
                        }
                        //光吸收系数平均值超过2.5
                        if (dgxsavg > 2.5)
                        {
                            gxspds = "不合格";
                        }
                        else
                        {
                            gxspds = "合格";
                        }
                    }
                    else
                    {
                        //光吸收系数1超过2.5
                        if (dgxs1 > 2.5)
                        {
                            gxspd1 = "不合格";
                        }
                        else
                        {
                            gxspd1 = "合格";
                        }
                        //光吸收系数2超过2.5
                        if (dgxs2 > 2.5)
                        {
                            gxspd2 = "不合格";
                        }
                        else
                        {
                            gxspd2 = "合格";
                        }
                        //光吸收系数3超过2.5
                        if (dgxs3 > 2.5)
                        {
                            gxspd3 = "不合格";
                        }
                        else
                        {
                            gxspd3 = "合格";
                        }
                        //光吸收系数平均值超过2.5
                        if (dgxsavg > 2.5)
                        {
                            gxspds = "不合格";
                        }
                        else
                        {
                            gxspds = "合格";
                        }
                    }
                    #endregion
                    #region 车长不少于12米
                    //车长不少于12米
                    if (dcc > 12000)
                    {
                        //光吸收系数1超过2.5
                        if (dgxs1 > 2.5)
                        {
                            gxspd1 = "不合格";
                        }
                        else
                        {
                            gxspd1 = "合格";
                        }
                        //光吸收系数2超过2.5
                        if (dgxs2 > 2.5)
                        {
                            gxspd2 = "不合格";
                        }
                        else
                        {
                            gxspd2 = "合格";
                        }
                        //光吸收系数3超过2.5
                        if (dgxs3 > 2.5)
                        {
                            gxspd3 = "不合格";
                        }
                        else
                        {
                            gxspd3 = "合格";
                        }
                        //光吸收系数平均值超过2.5
                        if (dgxsavg > 2.5)
                        {
                            gxspds = "不合格";
                        }
                        else
                        {
                            gxspds = "合格";
                        }
                    }
                    else
                    {
                        //光吸收系数1超过2.5
                        if (dgxs1 > 2.5)
                        {
                            gxspd1 = "不合格";
                        }
                        else
                        {
                            gxspd1 = "合格";
                        }
                        //光吸收系数2超过2.5
                        if (dgxs2 > 2.5)
                        {
                            gxspd2 = "不合格";
                        }
                        else
                        {
                            gxspd2 = "合格";
                        }
                        //光吸收系数3超过2.5
                        if (dgxs3 > 2.5)
                        {
                            gxspd3 = "不合格";
                        }
                        else
                        {
                            gxspd3 = "合格";
                        }
                        //光吸收系数平均值超过2.5
                        if (dgxsavg > 2.5)
                        {
                            gxspds = "不合格";
                        }
                        else
                        {
                            gxspds = "合格";
                        }
                    }
                    #endregion
                }
                string spcllx = cllx.Text;//车辆类型
                string szws = kczws.Text;//座位数
                double dzws;
                string sgdsco = qygdsCO.Text;//高怠速CO
                string sgdshc = qygdsHC.Text;//高怠速HC
                string sddsco = qyddsCO.Text;//低怠速CO
                string sddshc = qyddsHC.Text;//低怠速HC
                string sgdsλ = qygdsλ.Text;//高怠速λ
                double dgco;
                double dghc;
                double ddco;
                double ddhc;
                double dgλ;
                string gdscoxz = "";
                string gdshcxz = "";
                string ddscoxz = "";
                string ddshcxz = "";
                string gdscopd = "";
                string gdshcpd = "";
                string gdsλpd = "";
                string ddscopd = "";
                string ddshcpd = "";
                #region 判断变量是否存在
                //高怠速CO
                if (sgdsco == "" || sgdsco == "-")
                {
                    dgco = 0;
                }
                else
                {
                    dgco = Convert.ToDouble(sgdsco);
                }
                //高怠速HC
                if (sgdshc == "" || sgdshc == "-")
                {
                    dghc = 0;
                }
                else
                {
                    dghc = Convert.ToDouble(sgdshc);
                }
                //低怠速CO
                if (sddsco == "" || sddsco == "-")
                {
                    ddco = 0;
                }
                else
                {
                    ddco = Convert.ToDouble(sddsco);
                }
                //低怠速HC
                if (sddshc == "" || sddshc == "-")
                {
                    ddhc = 0;
                }
                else
                {
                    ddhc = Convert.ToDouble(sddshc);
                }
                //高怠速λ
                if (sgdsλ == "" || sgdsλ == "-")
                {
                    dgλ = 0;
                }
                else
                {
                    dgλ = Convert.ToDouble(sgdsλ);
                }
                //总质量
                if (pszzl == "" || pszzl == "-")
                {
                    dpzzl = 0;
                }
                else
                {
                    dpzzl = Convert.ToDouble(pszzl);
                }
                //座位数
                if (szws == "" || szws == "-")
                {
                    dzws = 0;
                }
                else
                {
                    if (szws == "3+2" || szws == "2+3")
                    {
                        szws = "5";
                    }
                    dzws = Convert.ToDouble(szws);
                }
                #endregion
                #region 判断双怠速排气污染物排放限值
                if (sgdsλ != "" && sgdsλ != "-")
                {
                    if (dgλ >= 0.97 && dgλ <= 1.03)
                    {
                        gdsλpd = "合格";
                    }
                    else
                    {
                        gdsλpd = "不合格";
                    }
                }
                if (kczws.Text == "" || kczws.Text == "-")
                {
                    dzws = 5;
                }
                if (zzl.Text == "" || zzl.Text == "-")
                {
                    dpzzl = 4600;
                }
                //1.M1类车
                if ((szws != "" && szws != "-" && pszzl != "" && pszzl != "-" && dzws <= 8 && dpzzl > 1000) && !spcllx.Contains("货"))
                {
                    //重型汽车
                    if (dpzzl > 3500)
                    {
                        try
                        {
                            if (DateTime.Parse(sccrq) < DateTime.Parse("1995-07-01"))
                            {
                                gdscoxz = "≤3.5";
                                gdshcxz = "≤1200";
                                ddscoxz = "≤5.0";
                                ddshcxz = "≤2000";
                                #region 1995年7月1日前生产的重型汽车
                                //高怠速CO
                                if (dgco > 3.5)
                                {
                                    gdscopd = "不合格";
                                }
                                else
                                {
                                    gdscopd = "合格";
                                }
                                //高怠速HC
                                if (dghc > 1200)
                                {
                                    gdshcpd = "不合格";
                                }
                                else
                                {
                                    gdshcpd = "合格";
                                }
                                //低怠速CO
                                if (ddco > 5.0)
                                {
                                    ddscopd = "不合格";
                                }
                                else
                                {
                                    ddscopd = "合格";
                                }
                                //低怠速HC
                                if (ddhc > 2000)
                                {
                                    ddshcpd = "不合格";
                                }
                                else
                                {
                                    ddshcpd = "合格";
                                }
                                #endregion
                            }
                            else
                            {
                                gdscoxz = "≤3.0";
                                gdshcxz = "≤900";
                                ddscoxz = "≤4.5";
                                ddshcxz = "≤1200";
                                #region 1995年7月1日起生产的重型汽车
                                //高怠速CO
                                if (dgco > 3.0)
                                {
                                    gdscopd = "不合格";
                                }
                                else
                                {
                                    gdscopd = "合格";
                                }
                                //高怠速HC
                                if (dghc > 900)
                                {
                                    gdshcpd = "不合格";
                                }
                                else
                                {
                                    gdshcpd = "合格";
                                }
                                //低怠速CO
                                if (ddco > 4.5)
                                {
                                    ddscopd = "不合格";
                                }
                                else
                                {
                                    ddscopd = "合格";
                                }
                                //低怠速HC
                                if (ddhc > 1200)
                                {
                                    ddshcpd = "不合格";
                                }
                                else
                                {
                                    ddshcpd = "合格";
                                }
                                #endregion
                            }
                            if (DateTime.Parse(sccrq) >= DateTime.Parse("2004-09-01"))
                            {
                                gdscoxz = "≤0.7";
                                gdshcxz = "≤200";
                                ddscoxz = "≤1.5";
                                ddshcxz = "≤250";
                                #region 2004年9月1日起生产的重型汽车
                                //高怠速CO
                                if (dgco > 0.7)
                                {
                                    gdscopd = "不合格";
                                }
                                else
                                {
                                    gdscopd = "合格";
                                }
                                //高怠速HC
                                if (dghc > 200)
                                {
                                    gdshcpd = "不合格";
                                }
                                else
                                {
                                    gdshcpd = "合格";
                                }
                                //低怠速CO
                                if (ddco > 1.5)
                                {
                                    ddscopd = "不合格";
                                }
                                else
                                {
                                    ddscopd = "合格";
                                }
                                //低怠速HC
                                if (ddhc > 250)
                                {
                                    ddshcpd = "不合格";
                                }
                                else
                                {
                                    ddshcpd = "合格";
                                }
                                #endregion
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex);
                        }
                    }
                }
                //2.M2类车
                if ((szws != "" && szws != "-" && pszzl != "" && pszzl != "-" && dzws > 8 && dpzzl <= 5000) && !spcllx.Contains("货"))
                {
                    //重型汽车
                    if (dpzzl > 3500)
                    {
                        try
                        {
                            if (DateTime.Parse(sccrq) < DateTime.Parse("1995-07-01"))
                            {
                                gdscoxz = "≤3.5";
                                gdshcxz = "≤1200";
                                ddscoxz = "≤5.0";
                                ddshcxz = "≤2000";
                                #region 1995年7月1日前生产的重型汽车
                                //高怠速CO
                                if (dgco > 3.5)
                                {
                                    gdscopd = "不合格";
                                }
                                else
                                {
                                    gdscopd = "合格";
                                }
                                //高怠速HC
                                if (dghc > 1200)
                                {
                                    gdshcpd = "不合格";
                                }
                                else
                                {
                                    gdshcpd = "合格";
                                }
                                //低怠速CO
                                if (ddco > 5.0)
                                {
                                    ddscopd = "不合格";
                                }
                                else
                                {
                                    ddscopd = "合格";
                                }
                                //低怠速HC
                                if (ddhc > 2000)
                                {
                                    ddshcpd = "不合格";
                                }
                                else
                                {
                                    ddshcpd = "合格";
                                }
                                #endregion
                            }
                            else
                            {
                                gdscoxz = "≤3.0";
                                gdshcxz = "≤900";
                                ddscoxz = "≤4.5";
                                ddshcxz = "≤1200";
                                #region 1995年7月1日起生产的重型汽车
                                //高怠速CO
                                if (dgco > 3.0)
                                {
                                    gdscopd = "不合格";
                                }
                                else
                                {
                                    gdscopd = "合格";
                                }
                                //高怠速HC
                                if (dghc > 900)
                                {
                                    gdshcpd = "不合格";
                                }
                                else
                                {
                                    gdshcpd = "合格";
                                }
                                //低怠速CO
                                if (ddco > 4.5)
                                {
                                    ddscopd = "不合格";
                                }
                                else
                                {
                                    ddscopd = "合格";
                                }
                                //低怠速HC
                                if (ddhc > 1200)
                                {
                                    ddshcpd = "不合格";
                                }
                                else
                                {
                                    ddshcpd = "合格";
                                }
                                #endregion
                            }
                            if (DateTime.Parse(sccrq) >= DateTime.Parse("2004-09-01"))
                            {
                                gdscoxz = "≤0.7";
                                gdshcxz = "≤200";
                                ddscoxz = "≤1.5";
                                ddshcxz = "≤250";
                                #region 2004年9月1日起生产的重型汽车
                                //高怠速CO
                                if (dgco > 0.7)
                                {
                                    gdscopd = "不合格";
                                }
                                else
                                {
                                    gdscopd = "合格";
                                }
                                //高怠速HC
                                if (dghc > 200)
                                {
                                    gdshcpd = "不合格";
                                }
                                else
                                {
                                    gdshcpd = "合格";
                                }
                                //低怠速CO
                                if (ddco > 1.5)
                                {
                                    ddscopd = "不合格";
                                }
                                else
                                {
                                    ddscopd = "合格";
                                }
                                //低怠速HC
                                if (ddhc > 250)
                                {
                                    ddshcpd = "不合格";
                                }
                                else
                                {
                                    ddshcpd = "合格";
                                }
                                #endregion
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex);
                        }
                    }
                }
                //3.M3类车(重型汽车)
                if (pszzl != "" && pszzl != "-" && dpzzl > 5000 && !spcllx.Contains("货"))
                {
                    try
                    {
                        if (DateTime.Parse(sccrq) < DateTime.Parse("1995-07-01"))
                        {
                            gdscoxz = "≤3.5";
                            gdshcxz = "≤1200";
                            ddscoxz = "≤5.0";
                            ddshcxz = "≤2000";
                            #region 1995年7月1日前生产的重型汽车
                            //高怠速CO
                            if (dgco > 3.5)
                            {
                                gdscopd = "不合格";
                            }
                            else
                            {
                                gdscopd = "合格";
                            }
                            //高怠速HC
                            if (dghc > 1200)
                            {
                                gdshcpd = "不合格";
                            }
                            else
                            {
                                gdshcpd = "合格";
                            }
                            //低怠速CO
                            if (ddco > 5.0)
                            {
                                ddscopd = "不合格";
                            }
                            else
                            {
                                ddscopd = "合格";
                            }
                            //低怠速HC
                            if (ddhc > 2000)
                            {
                                ddshcpd = "不合格";
                            }
                            else
                            {
                                ddshcpd = "合格";
                            }
                            #endregion
                        }
                        else
                        {
                            gdscoxz = "≤3.0";
                            gdshcxz = "≤900";
                            ddscoxz = "≤4.5";
                            ddshcxz = "≤1200";
                            #region 1995年7月1日起生产的重型汽车
                            //高怠速CO
                            if (dgco > 3.0)
                            {
                                gdscopd = "不合格";
                            }
                            else
                            {
                                gdscopd = "合格";
                            }
                            //高怠速HC
                            if (dghc > 900)
                            {
                                gdshcpd = "不合格";
                            }
                            else
                            {
                                gdshcpd = "合格";
                            }
                            //低怠速CO
                            if (ddco > 4.5)
                            {
                                ddscopd = "不合格";
                            }
                            else
                            {
                                ddscopd = "合格";
                            }
                            //低怠速HC
                            if (ddhc > 1200)
                            {
                                ddshcpd = "不合格";
                            }
                            else
                            {
                                ddshcpd = "合格";
                            }
                            #endregion
                        }
                        if (DateTime.Parse(sccrq) >= DateTime.Parse("2004-09-01"))
                        {
                            gdscoxz = "≤0.7";
                            gdshcxz = "≤200";
                            ddscoxz = "≤1.5";
                            ddshcxz = "≤250";
                            #region 2004年9月1日起生产的重型汽车
                            //高怠速CO
                            if (dgco > 0.7)
                            {
                                gdscopd = "不合格";
                            }
                            else
                            {
                                gdscopd = "合格";
                            }
                            //高怠速HC
                            if (dghc > 200)
                            {
                                gdshcpd = "不合格";
                            }
                            else
                            {
                                gdshcpd = "合格";
                            }
                            //低怠速CO
                            if (ddco > 1.5)
                            {
                                ddscopd = "不合格";
                            }
                            else
                            {
                                ddscopd = "合格";
                            }
                            //低怠速HC
                            if (ddhc > 250)
                            {
                                ddshcpd = "不合格";
                            }
                            else
                            {
                                ddshcpd = "合格";
                            }
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
                //4.N2类车(重型汽车)
                if ((pszzl != "" && pszzl != "-" && dpzzl > 3500 && dpzzl <= 12000) && spcllx.Contains("货"))
                {
                    try
                    {
                        if (DateTime.Parse(sccrq) < DateTime.Parse("1995-07-01"))
                        {
                            gdscoxz = "≤3.5";
                            gdshcxz = "≤1200";
                            ddscoxz = "≤5.0";
                            ddshcxz = "≤2000";
                            #region 1995年7月1日前生产的重型汽车
                            //高怠速CO
                            if (dgco > 3.5)
                            {
                                gdscopd = "不合格";
                            }
                            else
                            {
                                gdscopd = "合格";
                            }
                            //高怠速HC
                            if (dghc > 1200)
                            {
                                gdshcpd = "不合格";
                            }
                            else
                            {
                                gdshcpd = "合格";
                            }
                            //低怠速CO
                            if (ddco > 5.0)
                            {
                                ddscopd = "不合格";
                            }
                            else
                            {
                                ddscopd = "合格";
                            }
                            //低怠速HC
                            if (ddhc > 2000)
                            {
                                ddshcpd = "不合格";
                            }
                            else
                            {
                                ddshcpd = "合格";
                            }
                            #endregion
                        }
                        else
                        {
                            gdscoxz = "≤3.0";
                            gdshcxz = "≤900";
                            ddscoxz = "≤4.5";
                            ddshcxz = "≤1200";
                            #region 1995年7月1日起生产的重型汽车
                            //高怠速CO
                            if (dgco > 3.0)
                            {
                                gdscopd = "不合格";
                            }
                            else
                            {
                                gdscopd = "合格";
                            }
                            //高怠速HC
                            if (dghc > 900)
                            {
                                gdshcpd = "不合格";
                            }
                            else
                            {
                                gdshcpd = "合格";
                            }
                            //低怠速CO
                            if (ddco > 4.5)
                            {
                                ddscopd = "不合格";
                            }
                            else
                            {
                                ddscopd = "合格";
                            }
                            //低怠速HC
                            if (ddhc > 1200)
                            {
                                ddshcpd = "不合格";
                            }
                            else
                            {
                                ddshcpd = "合格";
                            }
                            #endregion
                        }
                        if (DateTime.Parse(sccrq) >= DateTime.Parse("2004-09-01"))
                        {
                            gdscoxz = "≤0.7";
                            gdshcxz = "≤200";
                            ddscoxz = "≤1.5";
                            ddshcxz = "≤250";
                            #region 2004年9月1日起生产的重型汽车
                            //高怠速CO
                            if (dgco > 0.7)
                            {
                                gdscopd = "不合格";
                            }
                            else
                            {
                                gdscopd = "合格";
                            }
                            //高怠速HC
                            if (dghc > 200)
                            {
                                gdshcpd = "不合格";
                            }
                            else
                            {
                                gdshcpd = "合格";
                            }
                            //低怠速CO
                            if (ddco > 1.5)
                            {
                                ddscopd = "不合格";
                            }
                            else
                            {
                                ddscopd = "合格";
                            }
                            //低怠速HC
                            if (ddhc > 250)
                            {
                                ddshcpd = "不合格";
                            }
                            else
                            {
                                ddshcpd = "合格";
                            }
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
                //5.N3类车(重型汽车)
                if (pszzl != "" && pszzl != "-" && dpzzl > 12000 && spcllx.Contains("货"))
                {
                    try
                    {
                        if (DateTime.Parse(sccrq) < DateTime.Parse("1995-07-01"))
                        {
                            gdscoxz = "≤3.5";
                            gdshcxz = "≤1200";
                            ddscoxz = "≤5.0";
                            ddshcxz = "≤2000";
                            #region 1995年7月1日前生产的重型汽车
                            //高怠速CO
                            if (dgco > 3.5)
                            {
                                gdscopd = "不合格";
                            }
                            else
                            {
                                gdscopd = "合格";
                            }
                            //高怠速HC
                            if (dghc > 1200)
                            {
                                gdshcpd = "不合格";
                            }
                            else
                            {
                                gdshcpd = "合格";
                            }
                            //低怠速CO
                            if (ddco > 5.0)
                            {
                                ddscopd = "不合格";
                            }
                            else
                            {
                                ddscopd = "合格";
                            }
                            //低怠速HC
                            if (ddhc > 2000)
                            {
                                ddshcpd = "不合格";
                            }
                            else
                            {
                                ddshcpd = "合格";
                            }
                            #endregion
                        }
                        else
                        {
                            gdscoxz = "≤3.0";
                            gdshcxz = "≤900";
                            ddscoxz = "≤4.5";
                            ddshcxz = "≤1200";
                            #region 1995年7月1日起生产的重型汽车
                            //高怠速CO
                            if (dgco > 3.0)
                            {
                                gdscopd = "不合格";
                            }
                            else
                            {
                                gdscopd = "合格";
                            }
                            //高怠速HC
                            if (dghc > 900)
                            {
                                gdshcpd = "不合格";
                            }
                            else
                            {
                                gdscopd = "合格";
                            }
                            //低怠速CO
                            if (ddco > 4.5)
                            {
                                ddscopd = "不合格";
                            }
                            else
                            {
                                ddscopd = "合格";
                            }
                            //低怠速HC
                            if (ddhc > 1200)
                            {
                                ddshcpd = "不合格";
                            }
                            else
                            {
                                ddscopd = "合格";
                            }
                            #endregion
                        }
                        if (DateTime.Parse(sccrq) >= DateTime.Parse("2004-09-01"))
                        {
                            gdscoxz = "≤0.7";
                            gdshcxz = "≤200";
                            ddscoxz = "≤1.5";
                            ddshcxz = "≤250";
                            #region 2004年9月1日起生产的重型汽车
                            //高怠速CO
                            if (dgco > 0.7)
                            {
                                gdscopd = "不合格";
                            }
                            else
                            {
                                gdscopd = "合格";
                            }
                            //高怠速HC
                            if (dghc > 200)
                            {
                                gdshcpd = "不合格";
                            }
                            else
                            {
                                gdshcpd = "合格";
                            }
                            //低怠速CO
                            if (ddco > 1.5)
                            {
                                ddscopd = "不合格";
                            }
                            else
                            {
                                ddscopd = "合格";
                            }
                            //低怠速HC
                            if (ddhc > 250)
                            {
                                ddshcpd = "不合格";
                            }
                            else
                            {
                                ddshcpd = "合格";
                            }
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
                //6.小于5座(含5座)的微型面包车
                if (szws != "" && szws != "-" && dzws <= 5 && spcllx.Contains("微型面包车"))
                {
                    try
                    {
                        if (DateTime.Parse(sccrq) < DateTime.Parse("2001-05-31"))
                        {
                            gdscoxz = "≤3.0";
                            gdshcxz = "≤900";
                            ddscoxz = "≤4.5";
                            ddshcxz = "≤1200";
                            #region 2001年5月31日前生产的5座(含5座)的微型面包车
                            //高怠速CO
                            if (dgco > 3.0)
                            {
                                gdscopd = "不合格";
                            }
                            else
                            {
                                gdscopd = "合格";
                            }
                            //高怠速HC
                            if (dghc > 900)
                            {
                                gdshcpd = "不合格";
                            }
                            else
                            {
                                gdshcpd = "合格";
                            }
                            //低怠速CO
                            if (ddco > 4.5)
                            {
                                ddscopd = "不合格";
                            }
                            else
                            {
                                ddscopd = "合格";
                            }
                            //低怠速HC
                            if (ddhc > 900)
                            {
                                ddshcpd = "不合格";
                            }
                            else
                            {
                                ddshcpd = "合格";
                            }
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
                else
                {
                    gdscoxz = "≤0.5";
                    gdshcxz = "≤150";
                    ddscoxz = "≤1.0";
                    ddshcxz = "≤200";
                    #region 总质量小于3.5吨的汽车
                    //高怠速CO
                    if (dgco > 0.5)
                    {
                        gdscopd = "不合格";
                    }
                    else
                    {
                        gdscopd = "合格";
                    }
                    //高怠速HC
                    if (dghc > 150)
                    {
                        gdshcpd = "不合格";
                    }
                    else
                    {
                        gdshcpd = "合格";
                    }
                    //低怠速CO
                    if (ddco > 1.0)
                    {
                        ddscopd = "不合格";
                    }
                    else
                    {
                        ddscopd = "合格";
                    }
                    //低怠速HC
                    if (ddhc > 200)
                    {
                        ddshcpd = "不合格";
                    }
                    else
                    {
                        ddshcpd = "合格";
                    }
                    #endregion
                }
                #endregion
                string lbgvxz = "";
                string slbgvpd = "";
                if (cylbgv.Text != "" && cylbgv.Text != "-" && yrsfdjedg.Text != "" && yrsfdjedg.Text != "-")
                {
                    if (Convert.ToDouble(cylbgv.Text) >= (Convert.ToDouble(yrsfdjedg.Text) / 2))
                    {
                        lbgvxz = (Convert.ToDouble(yrsfdjedg.Text) / 2).ToString("0.0");
                        slbgvpd = "合格";
                    }

                    else
                    {
                        lbgvxz = (Convert.ToDouble(yrsfdjedg.Text) / 2).ToString("0.0");
                        slbgvpd = "不合格";
                    }
                }
                #endregion
                #region 整车制动判定
                string zczdpd = "";
                if (dczdl.Text != "" && dczdl.Text != "-")
                {
                    if (Convert.ToDouble(dczdl.Text) >= 60)
                    {
                        zczdpd = "合格";
                    }
                    else
                    {
                        zczdpd = "不合格";
                    }
                }
                #endregion
                #region 驻车制动率的限值
                string szczds = dczczdl.Text;//驻车制动率
                string zcpd = "";
                if (szczds != "" && szczds != "-")
                {
                    if (Convert.ToDouble(szczds) >= 20)
                    {
                        zcpd = "合格";
                    }
                    else
                    {
                        zcpd = "不合格";
                    }
                }
                #endregion
                #region 阻滞率的限值
                string yzzzzvs = "";
                string yzyzzvs = "";
                string ezzzzvs = "";
                string ezyzzvs = "";
                string szzzzvs = "";
                string szyzzvs = "";
                string sizzzzvs = "";
                string sizyzzvs = "";
                string wzzzzvs = "";
                string wzyzzvs = "";
                string lzzzzvs = "";
                string lzyzzvs = "";
                //一轴左阻滞率
                if (ydzzzzv.Text != "" && ydzzzzv.Text != "-")
                {
                    if (Convert.ToDouble(ydzzzzv.Text) <= 3.5)
                    {
                        yzzzzvs = "合格";
                    }
                    else
                    {
                        yzzzzvs = "不合格";
                    }
                }
                //一轴右阻滞率
                if (ydzyzzv.Text != "" && ydzyzzv.Text != "-")
                {
                    if (Convert.ToDouble(ydzyzzv.Text) <= 3.5)
                    {
                        yzyzzvs = "合格";
                    }
                    else
                    {
                        yzyzzvs = "不合格";
                    }
                }
                //二轴左阻滞率
                if (edzzzzv.Text != "" && edzzzzv.Text != "-")
                {
                    if (Convert.ToDouble(edzzzzv.Text) <= 3.5)
                    {
                        ezzzzvs = "合格";
                    }
                    else
                    {
                        ezzzzvs = "不合格";
                    }
                }
                //二轴右阻滞率
                if (edzyzzv.Text != "" && edzyzzv.Text != "-")
                {
                    if (Convert.ToDouble(edzyzzv.Text) <= 3.5)
                    {
                        ezyzzvs = "合格";
                    }
                    else
                    {
                        ezyzzvs = "不合格";
                    }
                }
                //三轴左阻滞率
                if (sdzzzzv.Text != "" && sdzzzzv.Text != "-")
                {
                    if (Convert.ToDouble(sdzzzzv.Text) <= 3.5)
                    {
                        szzzzvs = "合格";
                    }
                    else
                    {
                        szzzzvs = "不合格";
                    }
                }
                //三轴右阻滞率
                if (sdzyzzv.Text != "" && sdzyzzv.Text != "-")
                {
                    if (Convert.ToDouble(sdzyzzv.Text) <= 3.5)
                    {
                        szyzzvs = "合格";
                    }
                    else
                    {
                        szyzzvs = "不合格";
                    }
                }
                //四轴左阻滞率
                if (sidzzzzv.Text != "" && sidzzzzv.Text != "-")
                {
                    if (Convert.ToDouble(sidzzzzv.Text) <= 3.5)
                    {
                        sizzzzvs = "合格";
                    }
                    else
                    {
                        sizzzzvs = "不合格";
                    }
                }
                //四轴右阻滞率
                if (sidzyzzv.Text != "" && sidzyzzv.Text != "-")
                {
                    if (Convert.ToDouble(sidzyzzv.Text) <= 3.5)
                    {
                        sizyzzvs = "合格";
                    }
                    else
                    {
                        sizyzzvs = "不合格";
                    }
                }
                //五轴左阻滞率
                if (wdzzzzv.Text != "" && wdzzzzv.Text != "-")
                {
                    if (Convert.ToDouble(wdzzzzv.Text) <= 3.5)
                    {
                        wzzzzvs = "合格";
                    }
                    else
                    {
                        wzzzzvs = "不合格";
                    }
                }
                //五轴右阻滞率
                if (wdzyzzv.Text != "" && wdzyzzv.Text != "-")
                {
                    if (Convert.ToDouble(wdzyzzv.Text) <= 3.5)
                    {
                        wzyzzvs = "合格";
                    }
                    else
                    {
                        wzyzzvs = "不合格";
                    }
                }
                //六轴左阻滞率
                if (ldzzzzv.Text != "" && ldzzzzv.Text != "-")
                {
                    if (Convert.ToDouble(ldzzzzv.Text) <= 3.5)
                    {
                        lzzzzvs = "合格";
                    }
                    else
                    {
                        lzzzzvs = "不合格";
                    }
                }
                //六轴右阻滞率
                if (ldzyzzv.Text != "" && ldzyzzv.Text != "-")
                {
                    if (Convert.ToDouble(ldzyzzv.Text) <= 3.5)
                    {
                        lzyzzvs = "合格";
                    }
                    else
                    {
                        lzyzzvs = "不合格";
                    }
                }
                #endregion
                #region 制动不平衡率的限值
                string sywlx = ywlx.Text;//业务类型          
                string ydzzdvs = ydzbphv.Text;//一轴不平衡率
                string edzzdvs = edzbphv.Text;//二轴不平衡率
                string sdzzdvs = sdzbphv.Text;//三轴不平衡率
                string sidzzdvs = sidzbphv.Text;//四轴不平衡率
                string wdzzdvs = wdzbphv.Text;//五轴不平衡率
                string ldzzdvs = ldzbphv.Text;//六轴不平衡率
                double yzbphv;
                double ezbphv;
                double szbphv;
                double sizbphv;
                double wzbphv;
                double lzbphv;
                string yzxz = "";
                string ezxz = "";
                string szxz = "";
                string sizxz = "";
                string wzxz = "";
                string lzxz = "";
                string ybpd = "";
                string ebpd = "";
                string sbpd = "";
                string sibpd = "";
                string wbpd = "";
                string lbpds = "";
                #region 判断一至六轴不平衡率是否有值
                //判断一轴不平衡率是否有值
                if (ydzzdvs == "" || ydzzdvs == "")
                {
                    yzbphv = 0;
                }
                else
                {
                    yzbphv = Convert.ToDouble(ydzzdvs);
                }
                //判断二轴不平衡率是否有值
                if (edzzdvs == "" || edzzdvs == "-")
                {
                    ezbphv = 0;
                }
                else
                {
                    ezbphv = Convert.ToDouble(edzzdvs);
                }
                //判断三轴不平衡率是否有值
                if (sdzzdvs == "" || sdzzdvs == "-")
                {
                    szbphv = 0;
                }
                else
                {
                    szbphv = Convert.ToDouble(sdzzdvs);
                }
                //判断四轴不平衡率是否有值
                if (sidzzdvs == "" || sidzzdvs == "-")
                {
                    sizbphv = 0;
                }
                else
                {
                    sizbphv = Convert.ToDouble(sidzzdvs);
                }
                //判断五轴不平衡率是否有值
                if (wdzzdvs == "" || wdzzdvs == "-")
                {
                    wzbphv = 0;
                }
                else
                {
                    wzbphv = Convert.ToDouble(wdzzdvs);
                }
                //判断六轴不平衡率
                if (ldzzdvs == "" || wdzzdvs == "-")
                {
                    lzbphv = 0;
                }
                else
                {
                    lzbphv = Convert.ToDouble(ldzzdvs);
                }
                #endregion
                
                #region 车辆判断标准
                #region 一轴
                if (yzbphv <= 20)
                {
                    yzxz = "≤20";
                    ybpd = "一级";
                }
                else if (yzbphv <= 24)
                {
                    yzxz = "≤24";
                    ybpd = "二级";
                }
                else
                {
                    yzxz = "≤24";
                    ybpd = "不合格";
                }
                #endregion
                #region 二轴
                if (zxzs.Text == "2")
                {
                    //二轴
                    if (ezbphv <= 20)
                    {
                        ezxz = "≤20";
                        ebpd = "一级";
                    }
                    else if (ezbphv <= 24)
                    {
                        ezxz = "≤24";
                        ebpd = "二级";
                    }
                    else
                    {
                        ezxz = "≤24";
                        ebpd = "不合格";
                    }
                }
                else
                {
                    //二轴
                    if (edzzdv.Text != "" && edzzdv.Text != "-")
                    {
                        if (Convert.ToDouble(edzzdv.Text) >= 60)
                        {
                            if (ezbphv <= 24)
                            {
                                ezxz = "≤24";
                                ebpd = "一级";
                            }
                            else if (ezbphv <= 30)
                            {
                                ezxz = "≤30";
                                ebpd = "二级";
                            }
                            else
                            {
                                ezxz = "≤30";
                                ebpd = "不合格";
                            }
                        }
                        else
                        {
                            if (ezbphv <= 8)
                            {
                                ezxz = "≤8";
                                ebpd = "一级";
                            }
                            else if (ezbphv <= 10)
                            {
                                ezxz = "≤10";
                                ebpd = "二级";
                            }
                            else
                            {
                                ezxz = "≤10";
                                ebpd = "不合格";
                            }
                        }
                    }
                }
                #endregion
                #region 三轴
                if (sdzzdv.Text != "" && sdzzdv.Text != "-")
                {
                    if (Convert.ToDouble(sdzzdv.Text) >= 60)
                    {
                        if (szbphv <= 24)
                        {
                            szxz = "≤24";
                            sbpd = "一级";
                        }
                        else if (szbphv <= 30)
                        {
                            szxz = "≤30";
                            sbpd = "二级";
                        }
                        else
                        {
                            szxz = "≤30";
                            sbpd = "不合格";
                        }
                    }
                    else
                    {
                        if (szbphv <= 8)
                        {
                            szxz = "≤8";
                            sbpd = "一级";
                        }
                        else if (szbphv <= 10)
                        {
                            szxz = "≤10";
                            sbpd = "二级";
                        }
                        else
                        {
                            szxz = "≤10";
                            sbpd = "不合格";
                        }
                    }
                }
                #endregion
                #region 四轴
                if (sidzzdv.Text != "" && sidzzdv.Text != "-")
                {
                    if (Convert.ToDouble(sidzzdv.Text) >= 60)
                    {
                        if (sizbphv <= 24)
                        {
                            sizxz = "≤24";
                            sibpd = "一级";
                        }
                        else if (sizbphv <= 30)
                        {
                            sizxz = "≤30";
                            sibpd = "二级";
                        }
                        else
                        {
                            sizxz = "≤30";
                            sibpd = "不合格";
                        }
                    }
                    else
                    {
                        if (sizbphv <= 8)
                        {
                            sizxz = "≤8";
                            sibpd = "一级";
                        }
                        else if (sizbphv <= 10)
                        {
                            sizxz = "≤10";
                            sibpd = "二级";
                        }
                        else
                        {
                            sizxz = "≤10";
                            sibpd = "不合格";
                        }
                    }
                }
                #endregion
                #region 五轴
                if (wdzzdv.Text != "" && wdzzdv.Text != "-")
                {
                    if (Convert.ToDouble(wdzzdv.Text) >= 60)
                    {
                        if (wzbphv <= 24)
                        {
                            wzxz = "≤24";
                            wbpd = "一级";
                        }
                        else if (wzbphv <= 30)
                        {
                            wzxz = "≤30";
                            wbpd = "二级";
                        }
                        else
                        {
                            wzxz = "≤30";
                            wbpd = "不合格";
                        }
                    }
                    else
                    {
                        if (wzbphv <= 8)
                        {
                            wzxz = "≤8";
                            wbpd = "一级";
                        }
                        else if (wzbphv <= 10)
                        {
                            wzxz = "≤10";
                            wbpd = "二级";
                        }
                        else
                        {
                            wzxz = "≤10";
                            wbpd = "不合格";
                        }
                    }
                }
                #endregion
                #region 六轴
                if (ldzzdv.Text != "" && ldzzdv.Text != "-")
                {
                    if (Convert.ToDouble(ldzzdv.Text) >= 60)
                    {
                        if (lzbphv <= 24)
                        {
                            lzxz = "≤24";
                            lbpds = "一级";
                        }
                        else if (lzbphv <= 30)
                        {
                            lzxz = "≤30";
                            lbpds = "二级";
                        }
                        else
                        {
                            lzxz = "≤30";
                            lbpds = "不合格";
                        }
                    }
                    else
                    {
                        if (lzbphv <= 8)
                        {
                            lzxz = "≤8";
                            lbpds = "一级";
                        }
                        else if (lzbphv <= 10)
                        {
                            lzxz = "≤10";
                            lbpds = "二级";
                        }
                        else
                        {
                            lzxz = "≤10";
                            lbpds = "不合格";
                        }
                    }
                }
                #endregion
                #endregion
                #endregion
                #region 车速喇叭与侧滑判定
                string cspds = "";
                string lbsjpd = "";
                string chpda = "";
                string ch2pds = "";
                if (csb.Text != "" && csb.Text != "-")
                {
                    if (Convert.ToDouble(csb.Text) >= 32.8 && Convert.ToDouble(csb.Text) <= 40.0)
                    {
                        cspds = "合格";
                    }
                    else
                    {
                        cspds = "不合格";
                    }
                }
                if (lbsjz.Text != "" && lbsjz.Text != "-")
                {
                    if (Convert.ToDouble(lbsjz.Text) >= 90 && Convert.ToDouble(lbsjz.Text) <= 115)
                    {
                        lbsjpd = "合格";
                    }
                    else
                    {
                        lbsjpd = "不合格";
                    }
                }
                if (dychl.Text != "" && dychl.Text != "-")
                {
                    if (Convert.ToDouble(dychl.Text) >= -5 && Convert.ToDouble(dychl.Text) <= 5)
                    {
                        chpda = "合格";
                    }
                    else
                    {
                        chpda = "不合格";
                    }
                }
                if (zxzs.Text == "2")
                {
                    if (dechl.Text != "" && dechl.Text != "-")
                    {
                        if (Convert.ToDouble(dechl.Text) >= -5 && Convert.ToDouble(dechl.Text) <= 5)
                        {
                            ch2pds = "合格";
                        }
                        else
                        {
                            ch2pds = "不合格";
                        }
                    }
                }
                else
                {
                    dechl.Text = "";
                    ch2pds = "";
                }
                #endregion
                #region 灯光判定
                #region 光强
                string zgqxz = "";
                string ygqxz = "";
                string zwgqpd = "";
                string ywgqpd = "";
                string zngqpd = "";
                string yngqpd = "";
                if (qzdz.Text == "四灯")
                {
                    if (zwyggq.Text != "" && zwyggq.Text != "-")
                    {
                        zgqxz = "≥15000";
                        if (Convert.ToDouble(zwyggq.Text) >= 15000)
                        {
                            zwgqpd = "合格";
                        }
                        else
                        {
                            zwgqpd = "不合格";
                        }
                    }
                    if (ywyggq.Text != "" && ywyggq.Text != "-")
                    {
                        ygqxz = "≥15000";
                        if (Convert.ToDouble(ywyggq.Text) >= 15000)
                        {
                            ywgqpd = "合格";
                        }
                        else
                        {
                            ywgqpd = "不合格";
                        }
                    }
                }
                else
                {
                    if (zwyggq.Text != "" && zwyggq.Text != "-")
                    {
                        zgqxz = "≥15000";
                        if (Convert.ToDouble(zwyggq.Text) >= 15000)
                        {
                            zwgqpd = "合格";
                        }
                        else
                        {
                            zwgqpd = "不合格";
                        }
                    }
                    if (ywyggq.Text != "" && ywyggq.Text != "-")
                    {
                        ygqxz = "≥15000";
                        if (Convert.ToDouble(ywyggq.Text) >= 15000)
                        {
                            ywgqpd = "合格";
                        }
                        else
                        {
                            ywgqpd = "不合格";
                        }
                    }
                }
                #endregion
                #region 变量
                string scllx = cllx.Text;//车辆类型
                string sqzdz = qzdz.Text;//前照灯制
                string szwgq = zwyggq.Text;//左外灯远光光强
                string szngq = znyggq.Text;//左内灯远光光强
                string syngq = ynyggq.Text;//右内灯远光光强
                string sywgq = ywyggq.Text;//右外灯远光光强
                string szwych = zwygczH.Text;//左外灯远光垂直H值
                string sznych = znygczH.Text;//左内灯远光垂直H值
                string synych = ynygczH.Text;//右内灯远光垂直H值
                string sywych = ywygczH.Text;//右外灯远光垂直H值
                string szwjch = zwjgczH.Text;//左外灯近光垂直H值
                string sznjch = znjgczH.Text;//左内灯近光垂直H值
                string synjch = ynjgczH.Text;//右内灯近光垂直H值
                string sywjch = ywjgczH.Text;//右外灯近光垂直H值
                double dzwygq;//左外灯远光光强
                double dznygq;//左内灯远光光强
                double dynygq;//右内灯远光光强
                double dywygq;//右外灯远光光强
                double dzwyh;//左外灯远光垂直H值
                double dznyh;//左内灯远光垂直H值
                double dynyh;//右内灯远光垂直H值
                double dywyh;//右外灯远光垂直H值
                double dzwjh;//左外灯近光垂直H值
                double dznjh;//左内灯近光垂直H值
                double dynjh;//右内灯近光垂直H值
                double dywjh;//右外灯近光垂直H值
                string czws = kczws.Text;//座位数
                string szzl = zzl.Text;//总质量
                double zws;//座位数
                double dzzl;//总质量
                #endregion
                #region 判断值是否存在            
                //判断左外灯远光光强是否有值
                if (szwgq == "" || szwgq == "-")
                {
                    dzwygq = 0;
                }
                else
                {
                    dzwygq = Convert.ToDouble(szwgq);
                }
                //判断左内灯远光光强
                if (szngq == "" || szngq == "-")
                {
                    dznygq = 0;
                }
                else
                {
                    dznygq = Convert.ToDouble(szngq);
                }
                //判断右内灯远光光强
                if (syngq == "" || syngq == "-")
                {
                    dynygq = 0;
                }
                else
                {
                    dynygq = Convert.ToDouble(syngq);
                }
                //判断右外灯远光光强
                if (sywgq == "" || sywgq == "-")
                {
                    dywygq = 0;
                }
                else
                {
                    dywygq = Convert.ToDouble(sywgq);
                }
                //判断左外灯远光垂直H值
                if (szwych == "" || szwych == "-")
                {
                    dzwyh = 0;
                }
                else
                {
                    dzwyh = Convert.ToDouble(szwych);
                }
                //判断左内灯远光垂直H值
                if (sznych == "" || sznych == "-")
                {
                    dznyh = 0;
                }
                else
                {
                    dznyh = Convert.ToDouble(sznych);
                }
                //判断右内灯远光垂直H值
                if (synych == "" || synych == "-")
                {
                    dynyh = 0;
                }
                else
                {
                    dynyh = Convert.ToDouble(synych);
                }
                //判断右外灯远光垂直H值
                if (sywych == "" || sywych == "-")
                {
                    dywyh = 0;
                }
                else
                {
                    dywyh = Convert.ToDouble(sywych);
                }
                //判断左外灯近光垂直H值
                if (szwjch == "" || szwjch == "-")
                {
                    dzwjh = 0;
                }
                else
                {
                    dzwjh = Convert.ToDouble(szwjch);
                }
                //判断左内灯近光垂直H值
                if (sznjch == "" || sznjch == "-")
                {
                    dznjh = 0;
                }
                else
                {
                    dznjh = Convert.ToDouble(sznjch);
                }
                //判断右内灯近光垂直H值
                if (synjch == "" || synjch == "-")
                {
                    dynjh = 0;
                }
                else
                {
                    dynjh = Convert.ToDouble(synjch);
                }
                //判断右外灯近光垂直H值
                if (sywjch == "" || sywjch == "-")
                {
                    dywjh = 0;
                }
                else
                {
                    dywjh = Convert.ToDouble(sywjch);
                }
                #endregion
                string zwypd = "";
                string znypd = "";
                string ynypd = "";
                string ywypd = "";
                string zwjpd = "";
                string ywjpd = "";
                string znjpd = "";
                string ynjpd = "";

                string ygxz = "";
                string jgxz = "";
                if (scllx.Contains("客"))
                {
                    //M1类车
                    if (szws != "" && szws != "-" && szzl != "" && szzl != "-" && Convert.ToDouble(szws) <= 8 && Convert.ToDouble(szzl) > 1000)
                    {
                        jgxz = "0.70～0.90";
                        ygxz = "0.85～0.95";
                        #region 近光范围判断
                        if (szwjch != "" && szwjch != "-")
                        {
                            //近光垂直H值是否在0.7~0.9范围内
                            if (Convert.ToDouble(dzwjh) >= 0.7 && Convert.ToDouble(dzwjh) <= 0.9)
                            {
                                zwjpd = "合格";
                            }
                            else
                            {
                                zwjpd = "不合格";
                            }
                        }
                        if (sznjch != "" && sznjch != "-")
                        {
                            //近光垂直H值是否在0.7~0.9范围内
                            if (Convert.ToDouble(dznjh) >= 0.7 && Convert.ToDouble(dznjh) <= 0.9)
                            {
                                znjpd = "合格";
                            }
                            else
                            {
                                znjpd = "不合格";
                            }
                        }
                        if (sywjch != "" && sywjch != "-")
                        {
                            //近光垂直H值是否在0.7~0.9范围内
                            if (Convert.ToDouble(dywjh) >= 0.7 && Convert.ToDouble(dywjh) <= 0.9)
                            {
                                ywjpd = "合格";
                            }
                            else
                            {
                                ywjpd = "不合格";
                            }
                        }
                        if (synjch != "" && synjch != "-")
                        {
                            //近光垂直H值是否在0.7~0.9范围内
                            if (Convert.ToDouble(dynjh) >= 0.7 && Convert.ToDouble(dynjh) <= 0.9)
                            {
                                ynjpd = "合格";
                            }
                            else
                            {
                                ynjpd = "不合格";
                            }
                        }
                        #endregion
                        #region 远光范围判断
                        if (szwych != "" && szwych != "-")
                        {
                            //左外远光垂直H值
                            if (Convert.ToDouble(dzwyh) >= 0.85 && Convert.ToDouble(dzwyh) <= 0.95)
                            {
                                zwypd = "合格";
                            }
                            else
                            {
                                zwypd = "不合格";
                            }
                            //左内远光垂直H值
                            if (Convert.ToDouble(dznyh) >= 0.85 && Convert.ToDouble(dznyh) <= 0.95)
                            {
                                znypd = "合格";
                            }
                            else
                            {
                                znypd = "不合格";
                            }
                            //右外远光垂直H值
                            if (Convert.ToDouble(dywyh) >= 0.85 && Convert.ToDouble(dywyh) <= 0.95)
                            {
                                ywypd = "合格";
                            }
                            else
                            {
                                ywypd = "不合格";
                            }
                            //右内远光垂直H值
                            if (Convert.ToDouble(dynyh) >= 0.85 && Convert.ToDouble(dynyh) <= 0.95)
                            {
                                ynypd = "合格";
                            }
                            else
                            {
                                ynypd = "不合格";
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        jgxz = "0.60～0.80";
                        ygxz = "0.80～0.95";
                        #region 近光范围判断
                        if (szwjch != "" && szwjch != "-")
                        {
                            //近光垂直H值是否在0.6~0.8范围内
                            if (Convert.ToDouble(dzwjh) >= 0.6 && Convert.ToDouble(dzwjh) <= 0.8)
                            {
                                zwjpd = "合格";
                            }
                            else
                            {
                                zwjpd = "不合格";
                            }
                        }
                        if (sznjch != "" && sznjch != "-")
                        {
                            //近光垂直H值是否在0.6~0.8范围内
                            if (Convert.ToDouble(dznjh) >= 0.6 && Convert.ToDouble(dznjh) <= 0.8)
                            {
                                znjpd = "合格";
                            }
                            else
                            {
                                znjpd = "不合格";
                            }
                        }
                        if (sywjch != "" && sywjch != "-")
                        {
                            //近光垂直H值是否在0.6~0.8范围内
                            if (Convert.ToDouble(dywjh) >= 0.6 && Convert.ToDouble(dywjh) <= 0.8)
                            {
                                ywjpd = "合格";
                            }
                            else
                            {
                                ywjpd = "不合格";
                            }
                        }
                        if (synjch != "" && synjch != "-")
                        {
                            //近光垂直H值是否在0.6~0.8范围内
                            if (Convert.ToDouble(dynjh) >= 0.6 && Convert.ToDouble(dynjh) <= 0.8)
                            {
                                ynjpd = "合格";
                            }
                            else
                            {
                                ynjpd = "不合格";
                            }
                        }
                        #endregion
                        #region 远光范围判断
                        if (szwych != "" && szwych != "-")
                        {
                            //左外远光垂直H值
                            if (Convert.ToDouble(dzwyh) >= 0.8 && Convert.ToDouble(dzwyh) <= 0.95)
                            {
                                zwypd = "合格";
                            }
                            else
                            {
                                zwypd = "不合格";
                            }
                            //左内远光垂直H值
                            if (Convert.ToDouble(dznyh) >= 0.8 && Convert.ToDouble(dznyh) <= 0.95)
                            {
                                znypd = "合格";
                            }
                            else
                            {
                                znypd = "不合格";
                            }
                            //右外远光垂直H值
                            if (Convert.ToDouble(dywyh) >= 0.8 && Convert.ToDouble(dywyh) <= 0.95)
                            {
                                ywypd = "合格";
                            }
                            else
                            {
                                ywypd = "不合格";
                            }
                            //右内远光垂直H值
                            if (Convert.ToDouble(dynyh) >= 0.8 && Convert.ToDouble(dynyh) <= 0.95)
                            {
                                ynypd = "合格";
                            }
                            else
                            {
                                ynypd = "不合格";
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    jgxz = "0.60～0.80";
                    ygxz = "0.80～0.95";
                    #region 近光范围判断
                    if (szwjch != "" && szwjch != "-")
                    {
                        //近光垂直H值是否在0.6~0.8范围内
                        if (Convert.ToDouble(dzwjh) >= 0.6 && Convert.ToDouble(dzwjh) <= 0.8)
                        {
                            zwjpd = "合格";
                        }
                        else
                        {
                            zwjpd = "不合格";
                        }
                    }
                    if (sznjch != "" && sznjch != "-")
                    {
                        //近光垂直H值是否在0.6~0.8范围内
                        if (Convert.ToDouble(dznjh) >= 0.6 && Convert.ToDouble(dznjh) <= 0.8)
                        {
                            znjpd = "合格";
                        }
                        else
                        {
                            znjpd = "不合格";
                        }
                    }
                    if (sywjch != "" && sywjch != "-")
                    {
                        //近光垂直H值是否在0.6~0.8范围内
                        if (Convert.ToDouble(dywjh) >= 0.6 && Convert.ToDouble(dywjh) <= 0.8)
                        {
                            ywjpd = "合格";
                        }
                        else
                        {
                            ywjpd = "不合格";
                        }
                    }
                    if (synjch != "" && synjch != "-")
                    {
                        //近光垂直H值是否在0.6~0.8范围内
                        if (Convert.ToDouble(dynjh) >= 0.6 && Convert.ToDouble(dynjh) <= 0.8)
                        {
                            ynjpd = "合格";
                        }
                        else
                        {
                            ynjpd = "不合格";
                        }
                    }
                    #endregion
                    #region 远光范围判断
                    if (szwych != "" && szwych != "-")
                    {
                        //左外远光垂直H值
                        if (Convert.ToDouble(dzwyh) >= 0.8 && Convert.ToDouble(dzwyh) <= 0.95)
                        {
                            zwypd = "合格";
                        }
                        else
                        {
                            zwypd = "不合格";
                        }
                        //左内远光垂直H值
                        if (Convert.ToDouble(dznyh) >= 0.8 && Convert.ToDouble(dznyh) <= 0.95)
                        {
                            znypd = "合格";
                        }
                        else
                        {
                            znypd = "不合格";
                        }
                        //右外远光垂直H值
                        if (Convert.ToDouble(dywyh) >= 0.8 && Convert.ToDouble(dywyh) <= 0.95)
                        {
                            ywypd = "合格";
                        }
                        else
                        {
                            ywypd = "不合格";
                        }
                        //右内远光垂直H值
                        if (Convert.ToDouble(dynyh) >= 0.8 && Convert.ToDouble(dynyh) <= 0.95)
                        {
                            ynypd = "合格";
                        }
                        else
                        {
                            ynypd = "不合格";
                        }
                    }
                    #endregion
                }
                #region 水平偏移量判断
                string zwygspd = "";
                string znygspd = "";
                string ywygspd = "";
                string ynygspd = "";
                string zwjgspd = "";
                string znjgspd = "";
                string ywjgspd = "";
                string ynjgspd = "";
                if (zwygsp.Text != "" && zwygsp.Text != "-")
                {
                    if (Convert.ToDouble(zwygsp.Text) >= -170 && Convert.ToDouble(zwygsp.Text) <= 350)
                    {
                        zwygspd = "合格";
                    }
                    else
                    {
                        zwygspd = "不合格";
                    }
                }
                if (znygsp.Text != "" && znygsp.Text != "-")
                {
                    if (Convert.ToDouble(znygsp.Text) >= -170 && Convert.ToDouble(znygsp.Text) <= 350)
                    {
                        znygspd = "合格";
                    }
                    else
                    {
                        znygspd = "不合格";
                    }
                }

                if (ywygsp.Text != "" && ywygsp.Text != "-")
                {
                    if (Convert.ToDouble(ywygsp.Text) >= -350 && Convert.ToDouble(ywygsp.Text) <= 350)
                    {
                        ywygspd = "合格";
                    }
                    else
                    {
                        ywygspd = "不合格";
                    }
                }
                if (ynygsp.Text != "" && ynygsp.Text != "-")
                {
                    if (Convert.ToDouble(ynygsp.Text) >= -350 && Convert.ToDouble(ynygsp.Text) <= 350)
                    {
                        ynygspd = "合格";
                    }
                    else
                    {
                        ynygspd = "不合格";
                    }
                }

                if (zwjgsp.Text != "" && zwjgsp.Text != "-")
                {
                    if (Convert.ToDouble(zwjgsp.Text) >= -170 && Convert.ToDouble(zwjgsp.Text) <= 350)
                    {
                        zwjgspd = "合格";
                    }
                    else
                    {
                        zwjgspd = "不合格";
                    }
                }
                if (znjgsp.Text != "" && znjgsp.Text != "-")
                {
                    if (Convert.ToDouble(znjgsp.Text) >= -170 && Convert.ToDouble(znjgsp.Text) <= 350)
                    {
                        znjgspd = "合格";
                    }
                    else
                    {
                        znjgspd = "不合格";
                    }
                }
                if (ywjgsp.Text != "" && ywjgsp.Text != "-")
                {
                    if (Convert.ToDouble(ywjgsp.Text) >= -170 && Convert.ToDouble(ywjgsp.Text) <= 350)
                    {
                        ywjgspd = "合格";
                    }
                    else
                    {
                        ywjgspd = "不合格";
                    }
                }
                if (ynjgsp.Text != "" && ynjgsp.Text != "-")
                {
                    if (Convert.ToDouble(ynjgsp.Text) >= -170 && Convert.ToDouble(ynjgsp.Text) <= 350)
                    {
                        ynjgspd = "合格";
                    }
                    else
                    {
                        ynjgspd = "不合格";
                    }
                }
                #endregion
                #endregion
                #endregion
                tabControl1.SelectedIndex = 1;
                #region 方向盘自由转动量
                string zdlpds = "";
                if(maxsd.Text.Replace(" ","")!=""&&maxsd.Text.Replace(" ","")!="-")
                {
                    if(Convert.ToDouble(maxsd.Text.Replace(" ", ""))>=100)
                    {
                        if(tb.Text.Replace(" ","")!=""&&tb.Text.Replace(" ","")!="-")
                        {
                            if(Convert.ToDouble(tb.Text.Replace(" ",""))<=15)
                            {
                                zdlpds = "○";
                            }
                            else
                            {
                                zdlpds = "×";
                            }
                        }
                    }
                    else
                    {
                        if (tb.Text.Replace(" ", "") != "" && tb.Text.Replace(" ", "") != "-")
                        {
                            if (Convert.ToDouble(tb.Text.Replace(" ", "")) <= 25)
                            {
                                zdlpds = "○";
                            }
                            else
                            {
                                zdlpds = "×";
                            }
                        }
                    }
                }
                else
                {
                    if (tb.Text.Replace(" ", "") != "" && tb.Text.Replace(" ", "") != "-")
                    {
                        if (Convert.ToDouble(tb.Text.Replace(" ", "")) <= 25)
                        {
                            zdlpds = "○";
                        }
                        else
                        {
                            zdlpds = "×";
                        }
                    }
                }
                #endregion
                string kcccs = "";
                string kczwss = "";
                if (cllx.Text.Contains("客"))
                {
                    kcccs = kccc.Text;
                    kczwss = kczws.Text;
                }
                else
                {
                    kcccs = "-";
                    kczwss = "-";
                }
                #region 根据检测类别判断数据的评定
                //整车判定
                string dcpdsa = "";
                dcpdsa = dcpd.Text;
                //灯光
                string zwpj = "";
                string znpj = "";
                string ywpj = "";
                string ynpj = "";
                if (zwpd.Text.Replace(" ", "").Contains("×"))
                {
                    if (zwpd.Text.Replace(" ", "").Substring(1, 1).Contains("×"))
                    {
                        if (zwpd.Text.Replace(" ", "").Substring(2, 1).Contains("×"))
                        {
                            zwpj = zwpd.Text.Replace(" ", "").Substring(0, 1) + "#" + "#" ;
                        }
                        else
                        {
                            zwpj = zwpd.Text.Replace(" ", "").Substring(0, 1) + "#" + "○";
                        }
                    }
                    else
                    {
                        if (zwpd.Text.Replace(" ", "").Substring(2, 1).Contains("×"))
                        {
                            zwpj = zwpd.Text.Replace(" ", "").Substring(0, 1) + "○" + "#";
                        }
                        else
                        {
                            zwpj = zwpd.Text.Replace(" ", "").Substring(0, 1) + "○" + "○";
                        }
                    }
                }
                else
                {
                    zwpj = zwpd.Text;
                }
                if (znpd.Text.Replace(" ", "").Contains("×"))
                {
                    if (znpd.Text.Replace(" ", "").Substring(1, 1).Contains("×"))
                    {
                        if (znpd.Text.Replace(" ", "").Substring(2, 1).Contains("×"))
                        {
                            znpj = znpd.Text.Replace(" ", "").Substring(0, 1) + "#" + "#";
                        }
                        else
                        {
                            znpj = znpd.Text.Replace(" ", "").Substring(0, 1) + "#" + "○";
                        }
                    }
                    else
                    {
                        if (znpd.Text.Replace(" ", "").Substring(2, 1).Contains("×"))
                        {
                            znpj = znpd.Text.Replace(" ", "").Substring(0, 1) + "○" + "#";
                        }
                        else
                        {
                            znpj = znpd.Text.Replace(" ", "").Substring(0, 1) + "○" + "○";
                        }
                    }
                }
                else
                {
                    znpj = znpd.Text;
                }
                if (ynpd.Text.Replace(" ", "").Contains("×"))
                {
                    if (ynpd.Text.Replace(" ", "").Substring(1, 1).Contains("×"))
                    {
                        if (ynpd.Text.Replace(" ", "").Substring(2, 1).Contains("×"))
                        {
                            ynpj = ynpd.Text.Replace(" ", "").Substring(0, 1) + "#" + "#";
                        }
                        else
                        {
                            ynpj = ynpd.Text.Replace(" ", "").Substring(0, 1) + "#" + "○";
                        }
                    }
                    else
                    {
                        if (ynpd.Text.Replace(" ", "").Substring(2, 1).Contains("×"))
                        {
                            ynpj = ynpd.Text.Replace(" ", "").Substring(0, 1) + "○" + "#";
                        }
                        else
                        {
                            ynpj = ynpd.Text.Replace(" ", "").Substring(0, 1) + "○" + "○";
                        }
                    }
                }
                else
                {
                    ynpj = ynpd.Text;
                }
                if (ywpd.Text.Replace(" ", "").Contains("×"))
                {
                    if (ywpd.Text.Replace(" ", "").Substring(1, 1).Contains("×"))
                    {
                        if (ywpd.Text.Replace(" ", "").Substring(2, 1).Contains("×"))
                        {
                            ywpj = ywpd.Text.Replace(" ", "").Substring(0, 1) + "#" +"#";
                        }
                        else
                        {
                            ywpj = ywpd.Text.Replace(" ", "").Substring(0, 1) + "#" + "○";
                        }
                    }
                    else
                    {
                        if (ywpd.Text.Replace(" ", "").Substring(2, 1).Contains("×"))
                        {
                            ywpj = ywpd.Text.Replace(" ", "").Substring(0, 1) + "○" + "#";
                        }
                        else
                        {
                            ywpj = ywpd.Text.Replace(" ", "").Substring(0, 1) + "○" + "○";
                        }
                    }
                }
                else
                {
                    ywpj = ywpd.Text;
                }
                //if (zwgq.Visible==false&&zwpd.Text!="")
                //{
                //    zwpj = "○";
                //}
                //if (zngq.Visible == false&&znpd.Text!="")
                //{
                //    znpj = "○";
                //}
                //if (yngq.Visible == false&&ynpd.Text!="")
                //{
                //    ynpj = "○";
                //}
                //if (ywgq.Visible == false&&ywpd.Text!="")
                //{
                //    ywpj = "○";
                //}
                string qpfpj = "";
                if (qypd.Text.Replace(" ", "").Contains("×"))
                {
                    qpfpj = "×";
                }
                else
                {
                    if (qypd.Text.Replace(" ", "").Contains("○"))
                    {
                        qpfpj = "○";
                    }
                }
                string cpfpj = "";
                if (cypd.Text.Replace(" ", "").Contains("×"))
                {
                    cpfpj = "×";
                }
                else
                {
                    if (cypd.Text.Replace(" ", "").Contains("○"))
                    {
                        cpfpj = "○";
                    }
                }
                string zwpjs = "";
                string znpjs = "";
                string ynpjs = "";
                string ywpjs = "";
                if (zwyggq.Text.Replace(" ", "") != "-")
                {
                    if (zwgq.Visible == false)
                    {
                        zwpjs = "○";
                    }
                    else
                    {
                        zwpjs = "×";
                    }
                }
                if (znyggq.Text.Replace(" ", "") != "")
                {
                    if (zngq.Visible == false)
                    {
                        znpjs = "○";
                    }
                    else
                    {
                        znpjs = "×";
                    }
                }
                if (ywyggq.Text.Replace(" ", "") != "")
                {
                    if (ywgq.Visible == false)
                    {
                        ywpjs = "○";
                    }
                    else
                    {
                        ywpjs = "×";
                    }
                }
                if (ynyggq.Text.Replace(" ", "") != "")
                {
                    if (yngq.Visible == false)
                    {
                        ynpjs = "○";
                    }
                    else
                    {
                        ynpjs = "×";
                    }
                }
                //单轴
                string yzpjs = "";
                string ezpjs = "";
                string szpjs = "";
                string sizpjs = "";
                string wzpjs = "";
                string lzpjs = "";
                if(lb1.Text.Replace(" ","").Contains("一"))
                {
                    yzpjs = yzpd.Text.Replace(" ","").Substring(0, 1) + "①" + yzpd.Text.Replace(" ", "").Substring(2, 2);
                }
               else if (lb1.Text.Replace(" ", "").Contains("二"))
                {
                    yzpjs = yzpd.Text.Replace(" ", "").Substring(0, 1) + "②" + yzpd.Text.Replace(" ", "").Substring(2, 2);
                }
                else
                {
                    yzpjs = yzpd.Text;
                }
                if (lb2.Text.Replace(" ", "").Contains("一"))
                {
                    ezpjs = ezpd.Text.Replace(" ", "").Substring(0, 1) + "①" +ezpd.Text.Replace(" ", "").Substring(2, 2);
                }
                else if (lb2.Text.Replace(" ", "").Contains("二"))
                {
                    ezpjs = ezpd.Text.Replace(" ", "").Substring(0, 1) + "②" + ezpd.Text.Replace(" ", "").Substring(2, 2);
                }
                else
                {
                    ezpjs = ezpd.Text;
                }
                if (lb3.Text.Replace(" ", "").Contains("一"))
                {
                    szpjs = szpd.Text.Replace(" ", "").Substring(0, 1) + "①" + szpd.Text.Replace(" ", "").Substring(2, 2);
                }
                else if (lb3.Text.Replace(" ", "").Contains("二"))
                {
                    szpjs = szpd.Text.Replace(" ", "").Substring(0, 1) + "②" + szpd.Text.Replace(" ", "").Substring(2, 2);
                }
                else
                {
                    szpjs = szpd.Text;
                }
                if (lb4.Text.Replace(" ", "").Contains("一"))
                {
                    sizpjs = sizpd.Text.Replace(" ", "").Substring(0, 1) + "①" + sizpd.Text.Replace(" ", "").Substring(2, 2);
                }
                else if (lb4.Text.Replace(" ", "").Contains("二"))
                {
                    sizpjs = sizpd.Text.Replace(" ", "").Substring(0, 1) + "②" + sizpd.Text.Replace(" ", "").Substring(2, 2);
                }
                else
                {
                    sizpjs = sizpd.Text;
                }
                if (lb5.Text.Replace(" ", "").Contains("一"))
                {
                    wzpjs = wzpd.Text.Replace(" ", "").Substring(0, 1) + "①" + wzpd.Text.Replace(" ", "").Substring(2, 2);
                }
                else if (lb5.Text.Replace(" ", "").Contains("二"))
                {
                    wzpjs = wzpd.Text.Replace(" ", "").Substring(0, 1) + "②" + wzpd.Text.Replace(" ", "").Substring(2, 2);
                }
                else
                {
                    wzpjs = wzpd.Text;
                }
                if (lb6.Text.Replace(" ", "").Contains("一"))
                {
                    lzpjs = lzpd.Text.Replace(" ", "").Substring(0, 1) + "①" + lzpd.Text.Replace(" ", "").Substring(2, 2);
                }
                else if (lb6.Text.Replace(" ", "").Contains("二"))
                {
                    lzpjs = lzpd.Text.Replace(" ", "").Substring(0, 1) + "②" + lzpd.Text.Replace(" ", "").Substring(2, 2);
                }
                else
                {
                    lzpjs = lzpd.Text;
                }
                #endregion
                #region 不合格项汇总
                ArrayList arraylists = new ArrayList();
                for(int k=0;k<al.Count;k++)
                {
                    arraylists.Add(al[k]);
                }
                if (dczd.Visible == true)
                {
                    arraylists.Add("整车制动率:"+dczdl.Text+"");
                }
                if (zczdbd.Visible == true)
                {
                    arraylists.Add("驻车制动率:"+dczczdl.Text+"");
                }
                if (lqz.Visible == true)
                {
                    arraylists.Add("悬架前轴左吸收率:"+qzzxsl.Text+"");
                }
                if (lqy.Visible == true)
                {
                    arraylists.Add("悬架前轴右吸收率:"+qzyxsl.Text+"");
                }
                if (lq.Visible == true)
                {
                    arraylists.Add("悬架前轴左右差:" + qzzyc.Text + "");
                }
                if (lhz.Visible == true)
                {
                    arraylists.Add("悬架后轴左吸收率:" + hzzxsl.Text + "");
                }
                if (lhy.Visible == true)
                {
                    arraylists.Add("悬架后轴右吸收率:" + hzyxsl.Text + "");
                }
                if (lh.Visible == true)
                {
                    arraylists.Add("悬架后轴左右差:" + hzzyc.Text + "");
                }
                if (ydzpd.Visible == true)
                {
                    arraylists.Add("一轴制动率:" + ydzzdv.Text + "");
                }
                if (edzpd.Visible == true)
                {
                    arraylists.Add("二轴制动率:" + edzzdv.Text + "");
                }
                if (sdzpd.Visible == true)
                {
                    arraylists.Add("三轴制动率:" + sdzzdv.Text + "");
                }
                if (sidzpd.Visible == true)
                {
                    arraylists.Add("四轴制动率:" + sidzzdv.Text + "");
                }
                if (wdzpd.Visible == true)
                {
                    arraylists.Add("五轴制动率:" + wdzzdv.Text + "");
                }
                if (ldzpd.Visible == true)
                {
                    arraylists.Add("六轴制动率:" + ldzzdv.Text + "");
                }
                if (ydzbpd.Visible == true)
                {
                    arraylists.Add("一轴制动不平衡率(分级项):" + ydzbphv.Text + "");
                }
                if (edzbpd.Visible == true)
                {
                    arraylists.Add("二轴制动不平衡率(分级项):" + edzbphv.Text + "");
                }
                if (sdzbpd.Visible == true)
                {
                    arraylists.Add("三轴制动不平衡率(分级项):" + sdzbphv.Text + "");
                }
                if (sidzbpd.Visible == true)
                {
                    arraylists.Add("四轴制动不平衡率(分级项):" + sidzbphv.Text + "");
                }
                if (wdzbpd.Visible == true)
                {
                    arraylists.Add("五轴制动不平衡率(分级项):" + wdzbphv.Text + "");
                }
                if (ldzbpd.Visible == true)
                {
                    arraylists.Add("六轴制动不平衡率(分级项):" + ldzbphv.Text + "");
                }
                if (yzzzv.Visible == true)
                {
                    arraylists.Add("一轴左轮阻滞率:" + ydzzzzv.Text + "");
                }
                if (ezzzv.Visible == true)
                {
                    arraylists.Add("二轴左轮阻滞率:" + edzzzzv.Text + "");
                }
                if (szzzv.Visible == true)
                {
                    arraylists.Add("三轴左轮阻滞率:" + sdzzzzv.Text + "");
                }
                if (sizzzv.Visible == true)
                {
                    arraylists.Add("四轴左轮阻滞率:" + sidzzzzv.Text + "");
                }
                if (wzzzv.Visible == true)
                {
                    arraylists.Add("五轴左轮阻滞率:" + wdzzzzv.Text + "");
                }
                if (lzzzv.Visible == true)
                {
                    arraylists.Add("六轴左轮阻滞率:" + ldzzzzv.Text + "");
                }
                if (yyzzv.Visible == true)
                {
                    arraylists.Add("一轴右轮阻滞率:" + ydzyzzv.Text + "");
                }
                if (eyzzv.Visible == true)
                {
                    arraylists.Add("二轴右轮阻滞率:" + edzyzzv.Text + "");
                }
                if (syzzv.Visible == true)
                {
                    arraylists.Add("三轴右轮阻滞率:" + sdzyzzv.Text + "");
                }
                if (siyzzv.Visible == true)
                {
                    arraylists.Add("四轴右轮阻滞率:" + sidzyzzv.Text + "");
                }
                if (wyzzv.Visible == true)
                {
                    arraylists.Add("五轴右轮阻滞率:" + wdzyzzv.Text + "");
                }
                if (lyzzv.Visible == true)
                {
                    arraylists.Add("六轴右轮阻滞率:" + ldzyzzv.Text + "");
                }
                if (qgco.Visible == true)
                {
                    arraylists.Add("高怠速CO:" + qygdsCO.Text + "");
                }
                if (qghc.Visible == true)
                {
                    arraylists.Add("高怠速HC:" + qygdsHC.Text + "");
                }
                if (qgλ.Visible == true)
                {
                    arraylists.Add("空气过量系数:" + qygdsλ.Text + "");
                }
                if (qdco.Visible == true)
                {
                    arraylists.Add("低怠速CO:" + qyddsCO.Text + "");
                }
                if (qdhc.Visible == true)
                {
                    arraylists.Add("低怠速HC:" + qyddsHC.Text + "");
                }
                if (cg1.Visible == true)
                {
                    arraylists.Add("光吸收系数:" + cygxs1.Text + "1");
                }
                if (cg2.Visible == true)
                {
                    arraylists.Add("光吸收系数2:" + cygxs2.Text + "");
                }
                if (cg3.Visible == true)
                {
                    arraylists.Add("光吸收系数3:" + cygxs3.Text + "");
                }
                if (zwgq.Visible == true)
                {
                    arraylists.Add("左外灯远光光强:" + zwyggq.Text + "");
                }
                if (zngq.Visible == true)
                {
                    arraylists.Add("左内灯远光光强:" + znyggq.Text + "");
                }
                if (ywgq.Visible == true)
                {
                    arraylists.Add("右外灯远光光强:" + ywyggq.Text + "");
                }
                if (yngq.Visible == true)
                {
                    arraylists.Add("右内灯远光光强:" + ynyggq.Text + "");
                }
                if (zwyc.Visible == true)
                {
                    arraylists.Add("左外灯远光垂直偏移量(一般项):" + zwygczH.Text + "");
                }
                if (znyc.Visible == true)
                {
                    arraylists.Add("左内灯远光垂直偏移量(一般项):" + znygczH.Text + "");
                }
                if (ywyc.Visible == true)
                {
                    arraylists.Add("右外灯远光垂直偏移量(一般项):" + ywygczH.Text + "");
                }
                if (ynyc.Visible == true)
                {
                    arraylists.Add("右内灯远光垂直偏移量(一般项):" + ynygczH.Text + "");
                }
                if (zwjc.Visible == true)
                {
                    arraylists.Add("左外灯近光垂直偏移量(一般项):" + zwjgczH.Text + "");
                }
                if (znjc.Visible == true)
                {
                    arraylists.Add("左内灯近光垂直偏移量(一般项):" + znjgczH.Text + "");
                }
                if (ywjc.Visible == true)
                {
                    arraylists.Add("右外灯近光垂直偏移量(一般项):" + ywjgczH.Text + "");
                }
                if (ynjc.Visible == true)
                {
                    arraylists.Add("右内灯近光垂直偏移量(一般项):" + ynjgczH.Text + "");
                }
                if(zwysp.Visible==true)
                {
                    arraylists.Add("左外灯远光水平偏移量(不作判定):" + zwygsp.Text + "");
                }
                if (zwjsp.Visible == true)
                {
                    arraylists.Add("左外灯近光水平偏移量(不作判定):" + zwjgsp.Text + "");
                }
                if (ywysp.Visible == true)
                {
                    arraylists.Add("右外灯远光水平偏移量(不作判定):" + ywygsp.Text + "");
                }
                if (ywjsp.Visible == true)
                {
                    arraylists.Add("右外灯近光水平偏移量(不作判定):" + ywjgsp.Text + "");
                }
                if (scsz.Visible == true)
                {
                    arraylists.Add("车速(一般项):" + csb.Text + "");
                }
                if (ch1pd.Visible == true)
                {
                    arraylists.Add("第一转向轮侧滑量:" + dychl.Text + "");
                }
                if (ch2pd.Visible == true)
                {
                    arraylists.Add("第二转向轮侧滑量:" + dechl.Text + "");
                }
                if (slbs.Visible == true)
                {
                    arraylists.Add("喇叭声压级:" + lbsjz.Text + "");
                }
                if (wdcspd.Visible == true)
                {
                    arraylists.Add("动力性(分级项):" + wdcs.Text + "");
                }
                if(jjxpd.Text== "×")
                {
                    arraylists.Add("经济性:" + yhscz.Text + "");
                }
                #endregion
                #region
                string jls = "";
                if(checkBox4.Checked)
                {
                    jls = "不";
                }
                else
                {
                    jls = "";
                }
                string sywlbs1 = "";
                if (ywlx.Text.Contains("在用"))
                {
                    sywlbs1 = "在用";
                }
                else
                {
                    sywlbs1 = "申请";
                }
                string sdz = "";
                if (qzdz.Text.Contains("四"))
                {
                    sdz = "四";
                }
                else
                {
                    sdz = "二";
                }
                string sar = "";
                if (arraylists.Count == 0)
                {
                    sar = "无";
                }
                else
                {
                    sar = string.Join("、", (string[])arraylists.ToArray(typeof(string)));
                }
                string swtrs = "";
                if (jylb.Text == "等级评定")
                {
                    swtrs = syr.Text;
                }
                else
                {
                    swtrs = sjdw.Text;
                }
                string sjylbs = "";
                if (jylb.Text.Contains("等级评定"))
                {
                    sjylbs = "技术等级评定";
                }
                else if (jylb.Text.Contains("二级维护"))
                {
                    sjylbs = "二级维护竣工质量检验";
                }
                else if (jylb.Text.Contains("竣工委托"))
                {
                    sjylbs = "汽车大修竣工质量检验";
                }
                else
                {
                    sjylbs = jylb.Text;
                }
                string qzd = "";
                if (qzdygsnfddtz.Text.Contains("是"))
                {
                    qzd = "能";
                }
                else
                {
                    qzd = qzdygsnfddtz.Text;
                }
                #endregion
                DataTable dt = new DataTable();
                #region 车辆基本信息
                dt.Columns.Add("检测次数");
                dt.Columns.Add("车牌号码");
                dt.Columns.Add("车牌颜色");
                dt.Columns.Add("登记日期");
                dt.Columns.Add("整备质量");
                dt.Columns.Add("底盘号码");
                dt.Columns.Add("行驶里程");
                dt.Columns.Add("燃油类型");
                dt.Columns.Add("发动机号码");
                dt.Columns.Add("车辆类型");
                dt.Columns.Add("车轴数");
                dt.Columns.Add("车身颜色");
                dt.Columns.Add("灯制");
                dt.Columns.Add("发动机额定功率");
                dt.Columns.Add("登录员");
                dt.Columns.Add("检测项目");
                dt.Columns.Add("检测类别");
                dt.Columns.Add("引车员");
                dt.Columns.Add("检测日期");
                dt.Columns.Add("检测时间");
                dt.Columns.Add("厂牌型号");
                dt.Columns.Add("车高");
                dt.Columns.Add("车长");
                dt.Columns.Add("客车车长");
                dt.Columns.Add("车宽");
                dt.Columns.Add("VIN");
                dt.Columns.Add("总质量");
                dt.Columns.Add("座位数");
                dt.Columns.Add("发动机额定转速");
                dt.Columns.Add("出厂日期");
                dt.Columns.Add("检测编号");
                dt.Columns.Add("远光光束单独调整");
                dt.Columns.Add("车主单位");
                dt.Columns.Add("双转向轴");
                dt.Columns.Add("前轮距");
                dt.Columns.Add("客车等级");
                dt.Columns.Add("发动机额定扭矩");
                dt.Columns.Add("轮胎规格");
                dt.Columns.Add("底盘类型");
                dt.Columns.Add("手刹起始轴位");
                dt.Columns.Add("型号");
                dt.Columns.Add("驱动轴数");
                dt.Columns.Add("驱动形式");
                dt.Columns.Add("业务类型");
                dt.Columns.Add("货车车身型式");
                dt.Columns.Add("转向轴悬架形式");
                dt.Columns.Add("驱动轴空载质量");
                dt.Columns.Add("牵引车满载总质量");
                dt.Columns.Add("并装轴形式");
                #endregion
                #region 原始数据
                dt.Columns.Add("一轴左轴重值");
                dt.Columns.Add("一轴右轴重值");
                dt.Columns.Add("一轴轴重值");
                dt.Columns.Add("一轴左轴重动态值");
                dt.Columns.Add("一轴右轴重动态值");
                dt.Columns.Add("一轴求和时左制动力值");
                dt.Columns.Add("一轴求和时右制动力值");
                dt.Columns.Add("二轴左轴重值");
                dt.Columns.Add("二轴右轴重值");
                dt.Columns.Add("二轴轴重值");
                dt.Columns.Add("二轴左轴重动态值");
                dt.Columns.Add("二轴右轴重动态值");
                dt.Columns.Add("二轴求和时左制动力值");
                dt.Columns.Add("二轴求和时右制动力值");
                dt.Columns.Add("三轴左轴重值");
                dt.Columns.Add("三轴右轴重值");
                dt.Columns.Add("三轴轴重值");
                dt.Columns.Add("三轴左轴重动态值");
                dt.Columns.Add("三轴右轴重动态值");
                dt.Columns.Add("三轴求和时左制动力值");
                dt.Columns.Add("三轴求和时右制动力值");
                dt.Columns.Add("四轴左轴重值");
                dt.Columns.Add("四轴右轴重值");
                dt.Columns.Add("四轴轴重值");
                dt.Columns.Add("四轴左轴重动态值");
                dt.Columns.Add("四轴右轴重动态值");
                dt.Columns.Add("四轴求和时左制动力值");
                dt.Columns.Add("四轴求和时右制动力值");
                dt.Columns.Add("五轴左轴重值");
                dt.Columns.Add("五轴右轴重值");
                dt.Columns.Add("五轴轴重值");
                dt.Columns.Add("五轴求和时左制动力值");
                dt.Columns.Add("五轴求和时右制动力值");
                dt.Columns.Add("六轴轴重值");
                dt.Columns.Add("六轴求和时左制动力值");
                dt.Columns.Add("六轴求和时右制动力值");
                dt.Columns.Add("一轴左制动力值");
                dt.Columns.Add("一轴右制动力值");
                dt.Columns.Add("二轴左制动力值");
                dt.Columns.Add("二轴右制动力值");
                dt.Columns.Add("三轴左制动力值");
                dt.Columns.Add("三轴右制动力值");
                dt.Columns.Add("四轴左制动力值");
                dt.Columns.Add("四轴右制动力值");
                dt.Columns.Add("五轴左制动力值");
                dt.Columns.Add("五轴右制动力值");
                dt.Columns.Add("六轴左制动力值");
                dt.Columns.Add("六轴右制动力值");
                #endregion
                #region 悬架
                dt.Columns.Add("悬架前左吸收率值");
                dt.Columns.Add("悬架前右吸收率值");
                dt.Columns.Add("悬架前轴吸收率差值");
                dt.Columns.Add("悬架后左吸收率值");
                dt.Columns.Add("悬架后右吸收率值");
                dt.Columns.Add("悬架后轴吸收率差值");
                dt.Columns.Add("悬架前轴吸收率评价");
                dt.Columns.Add("悬架后轴吸收率评价");
                #endregion
                #region 车速喇叭与侧滑
                dt.Columns.Add("车速值");
                dt.Columns.Add("侧滑值1");
                dt.Columns.Add("侧滑值2");
                dt.Columns.Add("侧滑值1评定");
                dt.Columns.Add("侧滑值2评定");
                dt.Columns.Add("喇叭声级值");
                dt.Columns.Add("车速评价");
                dt.Columns.Add("车速值评价");
                dt.Columns.Add("侧滑评价1");
                dt.Columns.Add("侧滑评价2");
                dt.Columns.Add("喇叭声级评价");
                dt.Columns.Add("喇叭评价");
                #endregion
                #region 单轴
                dt.Columns.Add("一轴制动和值");
                dt.Columns.Add("一轴制动差值");
                dt.Columns.Add("一轴求差时左制动力值");
                dt.Columns.Add("一轴求差时右制动力值");
                dt.Columns.Add("一轴左拖滞比值");
                dt.Columns.Add("一轴右拖滞比值");
                dt.Columns.Add("二轴制动和值");
                dt.Columns.Add("二轴制动差值");
                dt.Columns.Add("二轴求差时左制动力值");
                dt.Columns.Add("二轴求差时右制动力值");
                dt.Columns.Add("二轴左拖滞比值");
                dt.Columns.Add("二轴右拖滞比值");
                dt.Columns.Add("三轴制动和值");
                dt.Columns.Add("三轴制动差值");
                dt.Columns.Add("三轴求差时左制动力值");
                dt.Columns.Add("三轴求差时右制动力值");
                dt.Columns.Add("三轴左拖滞比值");
                dt.Columns.Add("三轴右拖滞比值");
                dt.Columns.Add("四轴制动和值");
                dt.Columns.Add("四轴制动差值");
                dt.Columns.Add("四轴求差时左制动力值");
                dt.Columns.Add("四轴求差时右制动力值");
                dt.Columns.Add("四轴左拖滞比值");
                dt.Columns.Add("四轴右拖滞比值");
                dt.Columns.Add("五轴制动和值");
                dt.Columns.Add("五轴制动差值");
                dt.Columns.Add("五轴求差时左制动力值");
                dt.Columns.Add("五轴求差时右制动力值");
                dt.Columns.Add("五轴左拖滞比值");
                dt.Columns.Add("五轴右拖滞比值");
                dt.Columns.Add("六轴制动和值");
                dt.Columns.Add("六轴制动差值");
                dt.Columns.Add("六轴求差时左制动力值");
                dt.Columns.Add("六轴求差时右制动力值");
                dt.Columns.Add("六轴左拖滞比值");
                dt.Columns.Add("六轴右拖滞比值");
                dt.Columns.Add("一轴制动和评价");
                dt.Columns.Add("二轴制动和评价");
                dt.Columns.Add("三轴制动和评价");
                dt.Columns.Add("四轴制动和评价");
                dt.Columns.Add("五轴制动和评价");
                dt.Columns.Add("六轴制动和评价");
                #endregion
                #region 排放性
                dt.Columns.Add("双怠速CO值");
                dt.Columns.Add("双怠速HC值");
                dt.Columns.Add("空气过量系数值");
                dt.Columns.Add("怠速CO值");
                dt.Columns.Add("怠速HC值");
                dt.Columns.Add("VmasCO");
                dt.Columns.Add("VmasHC");
                dt.Columns.Add("VmasNO");
                dt.Columns.Add("VmasHCNO");
                dt.Columns.Add("工况法5025CO值");
                dt.Columns.Add("工况法5025HC值");
                dt.Columns.Add("工况法5025NO值");
                dt.Columns.Add("工况法2540CO值");
                dt.Columns.Add("工况法2540HC值");
                dt.Columns.Add("工况法2540NO值");
                dt.Columns.Add("光吸收率值1");
                dt.Columns.Add("光吸收率值2");
                dt.Columns.Add("光吸收率值3");
                dt.Columns.Add("光吸收平均值");
                dt.Columns.Add("烟度值1");
                dt.Columns.Add("烟度值2");
                dt.Columns.Add("烟度值3");
                dt.Columns.Add("烟度平均值");
                dt.Columns.Add("Lugdown100K");
                dt.Columns.Add("Lugdown90K");
                dt.Columns.Add("Lugdown80K");
                dt.Columns.Add("柴油评定");
                dt.Columns.Add("柴油评价");
                dt.Columns.Add("汽油评价");
                dt.Columns.Add("光吸收率评价");
                dt.Columns.Add("怠速CO评价");
                #endregion
                #region 前照灯
                dt.Columns.Add("左近灯高值");
                dt.Columns.Add("左灯高值");
                dt.Columns.Add("右近灯高值");
                dt.Columns.Add("右灯高值");

                dt.Columns.Add("左主远光强度值");
                dt.Columns.Add("左副远光强度值");
                dt.Columns.Add("右副远光强度值");
                dt.Columns.Add("右主远光强度值");

                dt.Columns.Add("左主远光上下偏差H值");
                dt.Columns.Add("左副远光上下偏差H值");
                dt.Columns.Add("右副远光上下偏差H值");
                dt.Columns.Add("右主远光上下偏差H值");

                dt.Columns.Add("左主远光左右偏差值");
                dt.Columns.Add("左副远光左右偏差值");
                dt.Columns.Add("右副远光左右偏差值");
                dt.Columns.Add("右主远光左右偏差值");

                dt.Columns.Add("左主近光上下偏差H值");
                dt.Columns.Add("左副近光上下偏差H值");
                dt.Columns.Add("右副近光上下偏差H值");
                dt.Columns.Add("右主近光上下偏差H值");

                dt.Columns.Add("左主近光左右偏差值");
                dt.Columns.Add("左副近光左右偏差值");
                dt.Columns.Add("右副近光左右偏差值");
                dt.Columns.Add("右主近光左右偏差值");

                dt.Columns.Add("左主远光上下偏差值");
                dt.Columns.Add("左副远光上下偏差值");
                dt.Columns.Add("右副远光上下偏差值");
                dt.Columns.Add("右主远光上下偏差值");

                dt.Columns.Add("左主近光上下偏差值");
                dt.Columns.Add("左副近光上下偏差值");
                dt.Columns.Add("右副近光上下偏差值");
                dt.Columns.Add("右主近光上下偏差值");

                dt.Columns.Add("左主远光强度评价");
                dt.Columns.Add("左副远光强度评价");
                dt.Columns.Add("右主远光强度评价");
                dt.Columns.Add("右副远光强度评价");
                #endregion
                #region 整车
                dt.Columns.Add("整车制动和值");
                dt.Columns.Add("手制动和值");
                dt.Columns.Add("整车轴重值");
                dt.Columns.Add("整车评定");
                dt.Columns.Add("驻车评定");
                #endregion
                #region 路试
                dt.Columns.Add("制动初速度值");
                dt.Columns.Add("制动距离值");
                dt.Columns.Add("制动稳定性值");
                dt.Columns.Add("制动协调时间");
                dt.Columns.Add("制动减速度值");
                #endregion
                #region 限值
                dt.Columns.Add("动力性限值");
                dt.Columns.Add("经济性限值");
                dt.Columns.Add("一轴制动率限值");
                dt.Columns.Add("一轴不平衡率限值");
                dt.Columns.Add("一轴左阻滞率限值");
                dt.Columns.Add("一轴右阻滞率限值");
                dt.Columns.Add("二轴制动率限值");
                dt.Columns.Add("二轴不平衡率限值");
                dt.Columns.Add("二轴左阻滞率限值");
                dt.Columns.Add("二轴右阻滞率限值");
                dt.Columns.Add("三轴制动率限值");
                dt.Columns.Add("三轴不平衡率限值");
                dt.Columns.Add("三轴左阻滞率限值");
                dt.Columns.Add("三轴右阻滞率限值");
                dt.Columns.Add("四轴制动率限值");
                dt.Columns.Add("四轴不平衡率限值");
                dt.Columns.Add("四轴左阻滞率限值");
                dt.Columns.Add("四轴右阻滞率限值");
                dt.Columns.Add("五轴制动率限值");
                dt.Columns.Add("五轴不平衡率限值");
                dt.Columns.Add("五轴左阻滞率限值");
                dt.Columns.Add("五轴右阻滞率限值");
                dt.Columns.Add("六轴制动率限值");
                dt.Columns.Add("六轴不平衡率限值");
                dt.Columns.Add("六轴左阻滞率限值");
                dt.Columns.Add("六轴右阻滞率限值");
                dt.Columns.Add("整车制动率限值");
                dt.Columns.Add("整车驻车制动率限值");
                dt.Columns.Add("侧滑1限值");
                dt.Columns.Add("侧滑2限值");
                dt.Columns.Add("高怠速HC限值");
                dt.Columns.Add("高怠速CO限值");
                dt.Columns.Add("高怠速λ限值");
                dt.Columns.Add("低怠速HC限值");
                dt.Columns.Add("低怠速CO限值");
                dt.Columns.Add("光吸收率限值");
                dt.Columns.Add("左外光强限值");
                dt.Columns.Add("左外远光垂直偏移量H限值");
                dt.Columns.Add("左外远光水平偏移量限值");
                dt.Columns.Add("左外近光垂直偏移量H限值");
                dt.Columns.Add("左外近光水平偏移量限值");
                dt.Columns.Add("左内光强限值");
                dt.Columns.Add("左内远光垂直偏移量H限值");
                dt.Columns.Add("左内远光水平偏移量限值");
                dt.Columns.Add("右外光强限值");
                dt.Columns.Add("右外远光垂直偏移量H限值");
                dt.Columns.Add("右外远光水平偏移量限值");
                dt.Columns.Add("右外近光垂直偏移量H限值");
                dt.Columns.Add("右外近光水平偏移量限值");
                dt.Columns.Add("右内光强限值");
                dt.Columns.Add("右内远光垂直偏移量H限值");
                dt.Columns.Add("右内远光水平偏移量限值");
                dt.Columns.Add("车速限值");
                dt.Columns.Add("喇叭限值");
                #endregion
                #region 评价
                dt.Columns.Add("一轴制动率评价");
                dt.Columns.Add("一轴不平衡率评价");
                dt.Columns.Add("一轴左阻滞率评价");
                dt.Columns.Add("一轴右阻滞率评价");
                dt.Columns.Add("二轴制动率评价");
                dt.Columns.Add("二轴不平衡率评价");
                dt.Columns.Add("二轴左阻滞率评价");
                dt.Columns.Add("二轴右阻滞率评价");
                dt.Columns.Add("三轴制动率评价");
                dt.Columns.Add("三轴不平衡率评价");
                dt.Columns.Add("三轴左阻滞率评价");
                dt.Columns.Add("三轴右阻滞率评价");
                dt.Columns.Add("四轴制动率评价");
                dt.Columns.Add("四轴不平衡率评价");
                dt.Columns.Add("四轴左阻滞率评价");
                dt.Columns.Add("四轴右阻滞率评价");
                dt.Columns.Add("五轴制动率评价");
                dt.Columns.Add("五轴不平衡率评价");
                dt.Columns.Add("五轴左阻滞率评价");
                dt.Columns.Add("五轴右阻滞率评价");
                dt.Columns.Add("六轴制动率评价");
                dt.Columns.Add("六轴不平衡率评价");
                dt.Columns.Add("六轴左阻滞率评价");
                dt.Columns.Add("六轴右阻滞率评价");
                dt.Columns.Add("整车制动率评价");
                dt.Columns.Add("整车驻车制动率评价");
                dt.Columns.Add("高怠速HC评价");
                dt.Columns.Add("高怠速CO评价");
                dt.Columns.Add("高怠速λ评价");
                dt.Columns.Add("低怠速HC评价");
                dt.Columns.Add("低怠速CO评价");
                dt.Columns.Add("左外灯远光光强评价");
                dt.Columns.Add("左外灯远光垂直偏移量H评价");
                dt.Columns.Add("左外灯远光水平偏移量评价");
                dt.Columns.Add("左外灯近光垂直偏移量H评价");
                dt.Columns.Add("左外灯近光水平偏移量评价");
                dt.Columns.Add("左内灯远光光强评价");
                dt.Columns.Add("左内灯远光垂直偏移量H评价");
                dt.Columns.Add("左内灯远光水平偏移量评价");
                dt.Columns.Add("右外灯远光光强评价");
                dt.Columns.Add("右外灯远光垂直偏移量H评价");
                dt.Columns.Add("右外灯远光水平偏移量评价");
                dt.Columns.Add("右外灯近光垂直偏移量H评价");
                dt.Columns.Add("右外灯近光水平偏移量评价");
                dt.Columns.Add("右内灯远光光强评价");
                dt.Columns.Add("右内灯远光垂直偏移量H评价");
                dt.Columns.Add("右内灯远光水平偏移量评价");
                #endregion
                #region
                dt.Columns.Add("等级评定结论");
                dt.Columns.Add("人工评论");
                dt.Columns.Add("线号标识");
                dt.Columns.Add("营运证号");
                dt.Columns.Add("油耗标准");
                dt.Columns.Add("不合格项汇总");
                dt.Columns.Add("百公里油耗值");
                dt.Columns.Add("稳定车速");
                dt.Columns.Add("额定车速");
                dt.Columns.Add("动力性达标功率");
                dt.Columns.Add("动力性加载力");
                dt.Columns.Add("动力性评定");
                dt.Columns.Add("稳定车速评价");
                dt.Columns.Add("经济性评定");
                dt.Columns.Add("油耗评定");
                dt.Columns.Add("轮边功率");
                dt.Columns.Add("档案编号");
                dt.Columns.Add("制动工位照片", typeof(byte[]));
                dt.Columns.Add("灯光工位照片", typeof(byte[]));
                dt.Columns.Add("动力工位照片", typeof(byte[]));
                dt.Columns.Add("方向盘自由转动量值");
                dt.Columns.Add("方向盘自由转动量判定");
                dt.Columns.Add("结论");
                dt.Columns.Add("二维外检项");
                dt.Columns.Add("检验结论");
                dt.Columns.Add("地址");
                dt.Columns.Add("公司名称");
                dt.Columns.Add("联系方式");
                dt.Columns.Add("唯一性认定");
                dt.Columns.Add("故障信息诊断");
                dt.Columns.Add("外观检查");
                dt.Columns.Add("运行检查");
                dt.Columns.Add("底盘检查");
                dt.Columns.Add("核查评定");
                dt.Columns.Add("唯一性认定判定");
                dt.Columns.Add("故障信息诊断判定");
                dt.Columns.Add("外观检查判定");
                dt.Columns.Add("运行检查判定");
                dt.Columns.Add("底盘检查判定");
                dt.Columns.Add("核查评定判定");
                #endregion

                DataRow dr = dt.NewRow();
                #region 车辆基本信息
                dr["检测次数"]= dataGridView1.SelectedRows[0].Cells["检测次数"].Value.ToString();
                dr["车牌号码"] = hphm.Text;
                dr["车牌颜色"] = hpzl.Text;
                dr["登记日期"] = djrq.Text;
                dr["整备质量"] = zbzl.Text;
                dr["底盘号码"] = clsbdm.Text;
                dr["行驶里程"] = xslc.Text;
                dr["燃油类型"] = ryxs.Text;
                dr["发动机号码"] = fdjhm.Text;
                dr["车辆类型"] = cllx.Text;
                dr["车轴数"] = dczs.Text;
                dr["车身颜色"] = csys.Text;
                dr["灯制"] = sdz;
                if(ryxs.Text.Contains("汽油"))
                {
                    dr["发动机额定扭矩"] = fdjednj.Text;
                    dr["发动机额定转速"] = drsednjzs.Text;
                    dr["发动机额定功率"] = "-";
                }
                else
                {
                    dr["发动机额定扭矩"] = "-";
                    dr["发动机额定转速"] = "-";
                    dr["发动机额定功率"] = yrsfdjedg.Text;
                }
                dr["登录员"] = dly.Text;
                dr["检测项目"] = jyxm.Text;
                dr["检测类别"] = sjylb;
                dr["引车员"] = ycy.Text;
                dr["检测日期"] = jyrq.Text;
                dr["检测时间"] = dataGridView1.SelectedRows[0].Cells["检测时间"].Value.ToString();
                dr["厂牌型号"] = ppxh.Text;
                dr["车高"] = cg.Text;
                dr["车长"] = kccc.Text;
                dr["客车车长"] = kcccs;
                dr["车宽"] = ck.Text;
                dr["VIN"] = vin.Text;
                dr["总质量"] = zzl.Text;
                dr["座位数"] = kczwss;
                dr["出厂日期"] = ccrq.Text;
                dr["检测编号"] =dqbh+jczbh+lsh.Text;
                dr["远光光束单独调整"] = qzd;
                dr["车主单位"] = swtr;
                dr["双转向轴"] = zxzs.Text;
                dr["轮胎规格"] = qdlltggxh.Text;
                if (kclxdj.Text.Replace(" ", "") == "")
                {
                    dr["客车等级"] = "-";
                }
                else
                {
                    dr["客车等级"] = kclxdj.Text;
                }
                dr["前轮距"] = qlj.Text;
                dr["底盘类型"] = qdxs.Text;
                dr["手刹起始轴位"] = zcz.Text;
                dr["型号"] = clxh.Text;
                dr["驱动轴数"] = qdzs.Text;
                dr["驱动形式"] = qdxs.Text;
                dr["业务类型"] = sywlbs;
                dr["额定车速"] = edcs.Text;
                dr["稳定车速"] = wdcs.Text;
                dr["货车车身型式"] = hccsxs.Text;
                if (hccsxs.Text.Replace(" ", "") == "")
                {
                    dr["货车车身型式"] = "-";
                }
                else
                {
                    dr["货车车身型式"] = hccsxs.Text;
                }
                if (zxzxjxs.Text.Replace(" ", "") == "")
                {
                    dr["转向轴悬架形式"] = "-";
                }
                else
                {
                    dr["转向轴悬架形式"] = zxzxjxs.Text;
                }
                dr["驱动轴空载质量"] = qdzkazl.Text;
                if (qycmzzzl.Text.Replace(" ", "") == "")
                {
                    dr["牵引车满载总质量"] = "-";
                }
                else
                {
                    dr["牵引车满载总质量"] = qycmzzzl.Text;
                }
                if (bzzxs.Text.Replace(" ", "") == "")
                {
                    dr["并装轴形式"] = "无";
                }
                else
                {
                    dr["并装轴形式"] = bzzxs.Text;
                }
                #endregion
                #region 原始数据
                dr["一轴左轴重值"] = yzzlh.Text;
                dr["一轴右轴重值"] = yzylh.Text;
                dr["一轴轴重值"] = yzzz.Text;
                dr["一轴左轴重动态值"] = yzzdt.Text;
                dr["一轴右轴重动态值"] = yzydt.Text;
                dr["一轴求和时左制动力值"] = yzzxczd.Text;
                dr["一轴求和时右制动力值"] = yzyxczd.Text;
                dr["二轴左轴重值"] = ezzlh.Text;
                dr["二轴右轴重值"] = ezylh.Text;
                dr["二轴轴重值"] = ezzz.Text;
                dr["二轴左轴重动态值"] = ezzdt.Text;
                dr["二轴右轴重动态值"] = ezydt.Text;
                dr["二轴求和时左制动力值"] = ezzxczd.Text;
                dr["二轴求和时右制动力值"] = ezyxczd.Text;
                dr["三轴左轴重值"] = szzlh.Text;
                dr["三轴右轴重值"] = szylh.Text;
                dr["三轴轴重值"] = szzz.Text;
                dr["三轴左轴重动态值"] = szzdt.Text;
                dr["三轴右轴重动态值"] = szydt.Text;
                dr["三轴求和时左制动力值"] = szzxczd.Text;
                dr["三轴求和时右制动力值"] = szyxczd.Text;
                dr["四轴左轴重值"] = sizzlh.Text;
                dr["四轴右轴重值"] = sizylh.Text;
                dr["四轴轴重值"] = sizzz.Text;
                dr["四轴左轴重动态值"] = sizzdt.Text;
                dr["四轴右轴重动态值"] = sizydt.Text;
                dr["四轴求和时左制动力值"] = sizzxczd.Text;
                dr["四轴求和时右制动力值"] = sizyxczd.Text;
                dr["五轴左轴重值"] = wzzlh.Text;
                dr["五轴右轴重值"] = wzylh.Text;
                dr["五轴轴重值"] = wzzz.Text;
                dr["五轴求和时左制动力值"] = wzzxczd.Text;
                dr["五轴求和时右制动力值"] = wzyxczd.Text;
                dr["六轴轴重值"] = lzzz.Text;
                dr["六轴求和时左制动力值"] = lzzxczd.Text;
                dr["六轴求和时右制动力值"] = lzyxczd.Text;
                dr["一轴左制动力值"] = yzzzczd.Text;
                dr["一轴右制动力值"] = yzyzczd.Text;
                dr["二轴左制动力值"] = ezzzczd.Text;
                dr["二轴右制动力值"] = ezyzczd.Text;
                dr["三轴左制动力值"] = szzzczd.Text;
                dr["三轴右制动力值"] = szyzczd.Text;
                dr["四轴左制动力值"] = sizzzczd.Text;
                dr["四轴右制动力值"] = sizyzczd.Text;
                dr["五轴左制动力值"] = wzzzczd.Text;
                dr["五轴右制动力值"] = wzyzczd.Text;
                dr["六轴左制动力值"] = lzzzczd.Text;
                dr["六轴右制动力值"] = lzyzczd.Text;
                #endregion
                #region 悬架
                dr["悬架前左吸收率值"] = qzzxsl.Text;
                dr["悬架前右吸收率值"] = qzyxsl.Text;
                dr["悬架前轴吸收率差值"] = qzzyc.Text;
                dr["悬架后左吸收率值"] = hzzxsl.Text;
                dr["悬架后右吸收率值"] = hzyxsl.Text;
                dr["悬架后轴吸收率差值"] = hzzyc.Text;
                dr["悬架前轴吸收率评价"] = xjqzpd.Text;
                dr["悬架后轴吸收率评价"] = xjhzpd.Text;
                #endregion
                #region 车速喇叭与侧滑
                dr["车速值"] = csb.Text;
                if (dychl.Text.Replace(" ","")!="")
                {
                    if (dychl.Text.Replace(" ", "").Contains("-"))
                    {
                        dr["侧滑值1"] = dychl.Text;
                    }
                    else
                    {
                        dr["侧滑值1"] ="+"+dychl.Text;
                    }
                }
                if (zxzs.Text.Contains("2"))
                {
                    if (dechl.Text.Replace(" ", "") != "")
                    {
                        if (dechl.Text.Replace(" ", "").Contains("-"))
                        {
                            dr["侧滑值2"] = dechl.Text;
                        }
                        else
                        {
                            dr["侧滑值2"] = "+" + dechl.Text;
                        }
                    }
                    dr["侧滑值2评定"] = ch2pds;
                }
                else
                {
                    dr["侧滑值2"] = "";
                    dr["侧滑值2评定"] = "";
                }
                dr["侧滑值1评定"] = chpda;
                dr["喇叭声级值"] = lbsjz.Text;
                dr["车速值评价"] = cspd.Text;
                dr["车速评价"] = cspds;
                dr["侧滑评价1"] = chpd.Text;
                dr["侧滑评价2"] = chpd2.Text;
                dr["喇叭声级评价"] = lbsjpd;
                dr["喇叭评价"] = lbpd.Text ;
                #endregion. 
                #region 单轴
                dr["一轴制动和值"] = ydzzdv.Text;
                dr["一轴制动差值"] = ydzbphv.Text;
                dr["一轴求差时左制动力值"] = ydzzgcc.Text;
                dr["一轴求差时右制动力值"] = ydzygcc.Text;
                dr["一轴左拖滞比值"] = ydzzzzv.Text;
                dr["一轴右拖滞比值"] = ydzyzzv.Text;
                dr["二轴制动和值"] = edzzdv.Text;
                dr["二轴制动差值"] = edzbphv.Text;
                dr["二轴求差时左制动力值"] = edzzgcc.Text;
                dr["二轴求差时右制动力值"] = edzygcc.Text;
                dr["二轴左拖滞比值"] = edzzzzv.Text;
                dr["二轴右拖滞比值"] = edzyzzv.Text;
                dr["三轴制动和值"] = sdzzdv.Text;
                dr["三轴制动差值"] = sdzbphv.Text;
                dr["三轴求差时左制动力值"] = sdzzgcc.Text;
                dr["三轴求差时右制动力值"] = sdzygcc.Text;
                dr["三轴左拖滞比值"] = sdzzzzv.Text;
                dr["三轴右拖滞比值"] = sdzyzzv.Text;
                dr["四轴制动和值"] = sidzzdv.Text;
                dr["四轴制动差值"] = sidzbphv.Text;
                dr["四轴求差时左制动力值"] = sidzzgcc.Text;
                dr["四轴求差时右制动力值"] = sidzygcc.Text;
                dr["四轴左拖滞比值"] = sidzzzzv.Text;
                dr["四轴右拖滞比值"] = sidzyzzv.Text;
                dr["五轴制动和值"] = wdzzdv.Text;
                dr["五轴制动差值"] = wdzbphv.Text;
                dr["五轴求差时左制动力值"] = wdzzgcc.Text;
                dr["五轴求差时右制动力值"] = wdzygcc.Text;
                dr["五轴左拖滞比值"] = wdzzzzv.Text;
                dr["五轴右拖滞比值"] = wdzyzzv.Text;
                dr["六轴制动和值"] = ldzzdv.Text;
                dr["六轴制动差值"] = ldzbphv.Text;
                dr["六轴求差时左制动力值"] = ldzzgcc.Text;
                dr["六轴求差时右制动力值"] = ldzygcc.Text;
                dr["六轴左拖滞比值"] = ldzzzzv.Text;
                dr["六轴右拖滞比值"] = ldzyzzv.Text;
                #endregion
                #region 排放性
                if (ryxs.Text.Contains("汽油")|| ryxs.Text.Contains("天然气"))
                {
                    dr["双怠速CO值"] = qygdsCO.Text;
                    dr["双怠速HC值"] = qygdsHC.Text;
                    dr["空气过量系数值"] = qygdsλ.Text;
                    dr["怠速CO值"] = qyddsCO.Text;
                    dr["怠速HC值"] = qyddsHC.Text;
                }
                else
                {
                    dr["双怠速CO值"] = "";
                    dr["双怠速HC值"] = "";
                    dr["空气过量系数值"] = "";
                    dr["怠速CO值"] = "";
                    dr["怠速HC值"] = "";
                }
                dr["VmasCO"] = "";
                dr["VmasHC"] = "";
                dr["VmasNO"] = "";
                dr["VmasHCNO"] = "";
                dr["工况法5025CO值"] = "";
                dr["工况法5025HC值"] = "";
                dr["工况法5025NO值"] = "";
                dr["工况法2540CO值"] = "";
                dr["工况法2540HC值"] = "";
                dr["工况法2540NO值"] = "";
                if (ryxs.Text.Contains("柴油"))
                {
                    dr["光吸收率值1"] = cygxs1.Text;
                    dr["光吸收率值2"] = cygxs2.Text;
                    dr["光吸收率值3"] = cygxs3.Text;
                    dr["光吸收平均值"] = cygxsavg.Text;
                    dr["柴油评定"] = gxspds;
                }
                else
                {
                    dr["光吸收率值1"] = "";
                    dr["光吸收率值2"] = "";
                    dr["光吸收率值3"] = "";
                    dr["光吸收平均值"] = "";
                    dr["柴油评定"] = "";
                }
                dr["烟度值1"] = "";
                dr["烟度值2"] = "";
                dr["烟度值3"] = "";
                dr["烟度平均值"] = "";
                dr["Lugdown100K"] = "";
                dr["Lugdown90K"] = "";
                dr["Lugdown80K"] = "";
                dr["汽油评价"] = qypd.Text;
                dr["光吸收率评价"] = cpfpj;
                #endregion
                #region 前照灯
                dr["左近灯高值"] = zwjgdg.Text;
                dr["左灯高值"] = zwygdg.Text;
                dr["右近灯高值"] = ywjgdg.Text;
                dr["右灯高值"] = ywygdg.Text;

                dr["左主远光强度值"] = zwyggq.Text;
                dr["左副远光强度值"] = znyggq.Text;
                dr["右副远光强度值"] = ynyggq.Text;
                dr["右主远光强度值"] = ywyggq.Text;

                dr["左主远光上下偏差H值"] = zwygczH.Text;
                dr["左副远光上下偏差H值"] = znygczH.Text;
                dr["右副远光上下偏差H值"] = ynygczH.Text;
                dr["右主远光上下偏差H值"] = ywygczH.Text;
                if (zwygsp.Text.Contains("-"))
                {
                    dr["左主远光左右偏差值"] ="左"+zwygsp.Text.Substring(1).ToString();
                }
                else
                {
                    if (zwygsp.Text.Replace(" ", "") == "")
                    {
                        dr["左主远光左右偏差值"] = zwygsp.Text;
                    }
                    else
                    {
                        dr["左主远光左右偏差值"] = "右" + zwygsp.Text;
                    }
                }
                if (ywygsp.Text.Contains("-"))
                {
                    dr["右主远光左右偏差值"] = "左" + ywygsp.Text.Substring(1).ToString();
                }
                else
                {
                    if (ywygsp.Text.Replace(" ", "") == "")
                    {
                        dr["右主远光左右偏差值"] = ywygsp.Text;
                    }
                    else
                    {
                        dr["右主远光左右偏差值"] = "右" + ywygsp.Text;
                    }
                }
                dr["左副远光左右偏差值"] = znygsp.Text;
                dr["右副远光左右偏差值"] = ynygsp.Text;

                dr["左主近光上下偏差H值"] = zwjgczH.Text;
                dr["左副近光上下偏差H值"] = znjgczH.Text;
                dr["右主近光上下偏差H值"] = ywjgczH.Text;
                dr["右副近光上下偏差H值"] = ynjgczH.Text;
                if (zwjgsp.Text.Contains("-"))
                {
                    dr["左主近光左右偏差值"] = "左" + zwjgsp.Text.Substring(1).ToString();
                }
                else
                {
                    if (zwjgsp.Text.Replace(" ", "") == "")
                    {
                        dr["左主近光左右偏差值"] = zwjgsp.Text;
                    }
                    else
                    {
                        dr["左主近光左右偏差值"] = "右" + zwjgsp.Text;
                    }
                }
                if (ywjgsp.Text.Contains("-"))
                {
                    dr["右主近光左右偏差值"] = "左" + ywjgsp.Text.Substring(1).ToString();
                }
                else
                {
                    if (ywjgsp.Text.Replace(" ", "") == "")
                    {
                        dr["右主近光左右偏差值"] = ywjgsp.Text;
                    }
                    else
                    {
                        dr["右主近光左右偏差值"] = "右" + ywjgsp.Text;
                    }
                }
                dr["左副近光左右偏差值"] = znjgsp.Text;
                dr["右副近光左右偏差值"] = ynjgsp.Text;
                
                dr["左主远光上下偏差值"] = lbzwyc.Text;
                dr["左副远光上下偏差值"] = lbznyc.Text;
                dr["右副远光上下偏差值"] = lbynyc.Text;
                dr["右主远光上下偏差值"] = lbywyc.Text;
                
                dr["左主近光上下偏差值"] = lbzjc.Text;
                dr["左副近光上下偏差值"] = lbzjc.Text;
                dr["右副近光上下偏差值"] = lbyjc.Text;
                dr["右主近光上下偏差值"] = lbyjc.Text;
                if (jylb.Text.Contains("等级评定"))
                {
                    dr["左主远光强度评价"] = zwpj;
                    dr["左副远光强度评价"] = znpj;
                    dr["右主远光强度评价"] = ywpj;
                    dr["右副远光强度评价"] = ynpj;
                    dr["整车评定"] = dcpdsa;
                    dr["一轴制动和评价"] = yzpjs;
                    dr["二轴制动和评价"] = ezpjs;
                    dr["三轴制动和评价"] = szpjs;
                    dr["四轴制动和评价"] = sizpjs;
                    dr["五轴制动和评价"] = wzpjs;
                    dr["六轴制动和评价"] = lzpjs;
                    dr["柴油评价"] = gxspds;
                }
                else
                {
                    dr["左主远光强度评价"] = zwpd.Text;
                    dr["左副远光强度评价"] = znpd.Text;
                    dr["右主远光强度评价"] = ywpd.Text;
                    dr["右副远光强度评价"] = ynpd.Text;
                    dr["整车评定"] = dcpd.Text;
                    dr["一轴制动和评价"] = yzpd.Text;
                    dr["二轴制动和评价"] = ezpd.Text;
                    dr["三轴制动和评价"] = szpd.Text;
                    dr["四轴制动和评价"] = sizpd.Text;
                    dr["五轴制动和评价"] = wzpd.Text;
                    dr["六轴制动和评价"] = lzpd.Text;
                    dr["怠速CO评价"] = qypd.Text;
                    dr["光吸收率评价"] = cypd.Text;
                }
                #endregion
                #region 整车
                dr["整车制动和值"] = dczdl.Text;
                dr["手制动和值"] = dczczdl.Text;
                dr["整车轴重值"] = dcspcz.Text;
                dr["驻车评定"] = zcpd;
                #endregion
                #region 路试
                dr["制动初速度值"] = lszdcsd.Text;
                dr["制动距离值"] = lszdjl.Text;
                dr["制动稳定性值"] = lszdwdx.Text;
                dr["制动协调时间"] = lszdxtsj.Text;
                dr["制动减速度值"] = lszdmfdd.Text;
                #endregion
                #region
                if (ywlx.Text.Contains("在用"))
                {
                    dr["等级评定结论"] = "";
                }
                else
                {
                    dr["等级评定结论"] = djpdjl;
                }
                dr["人工评论"] = jls;
                if (yyzh.Text.Replace(" ", "") == "")
                {
                    dr["营运证号"] = "-";
                }
                else
                {
                    dr["营运证号"] = yyzh.Text;
                }
                dr["线号标识"] = jcxb.Text;
                dr["油耗标准"] = yhbzz.Text;
                dr["额定车速"] = edcs.Text;
                dr["稳定车速"] = wdcs.Text;
                if(arraylists.Count>0)
                {
                    dr["不合格项汇总"] = sar;
                }
                else
                {
                    dr["不合格项汇总"] = "";
                }
                dr["百公里油耗值"] = yhscz.Text;
                dr["动力性达标功率"] = dbgl.Text;
                dr["动力性加载力"] = jzl.Text;
                dr["动力性评定"] = edcspd;
                dr["稳定车速评价"] = dlpj;
                dr["经济性评定"] = yhpd;
                dr["油耗评定"] = jjxpd.Text;
                dr["轮边功率"] = cylbgv.Text;
                dr["档案编号"] = dabh.Text;
                dr["方向盘自由转动量值"] = tb.Text;
                dr["方向盘自由转动量判定"] = zdlpds;
                if (wjx.Text.Replace(" ", "").Length == 35)
                {
                    dr["二维外检项"] = wjx.Text.Replace(" ", "");
                }
                else
                {
                    dr["二维外检项"] = "○,○,○,○,○,○,○,○,○,○,○,○,○,○,○,○,○,○";
                }
                if (checkBox1.Checked||checkBox2.Checked)
                {
                    dr["结论"] = "合格";
                }
                else
                {
                    dr["结论"] = "不合格";
                }
                if(checkBox1.Checked)
                {
                    dr["检验结论"] = "一级";
                }
                else if(checkBox2.Checked)
                {
                    dr["检验结论"] = "二级";
                }
                else
                {
                    dr["检验结论"] = "不合格";
                }
                dr["地址"] = jdz;
                dr["公司名称"] = jgsmc;
                dr["联系方式"] = jlxfs;
                dr["唯一性认定"] = rgwyx;
                dr["唯一性认定判定"] = rgwyxpd;
                dr["故障信息诊断"] = rggzxx;
                dr["故障信息诊断判定"] = rggzxxpd;
                dr["外观检查"] = rgwg;
                dr["外观检查判定"] = rgwgpd;
                dr["运行检查"] = rgyx;
                dr["运行检查判定"] = rgyxpd;
                dr["底盘检查"] = rgdp;
                dr["底盘检查判定"] = rgdppd;
                dr["核查评定"] = rghc;
                dr["核查评定判定"] = rghcpd;
                #endregion
                #region 限值与评价
                if (wdcs.Text.Replace(" ","")=="")
                {
                    dr["动力性限值"] = "";
                }
                else
                {
                    dr["动力性限值"] = edcs.Text;
                }
                if(yhscz.Text.Replace(" ","")=="")
                {
                    dr["经济性限值"] = "";
                }
                else
                {
                    dr["经济性限值"] = yhbzz.Text;
                }
                #region 制动限值
                if(ydzzdv.Text.Replace(" ","")=="")
                {
                    dr["一轴制动率限值"] = "";
                    dr["一轴制动率评价"] = "";
                }
               else
                {
                    dr["一轴制动率限值"] = yzxzs;
                    dr["一轴制动率评价"] = yzpds;
                }
                if (ydzbphv.Text.Replace(" ", "") == "")
                {
                    dr["一轴不平衡率限值"] = "";
                    dr["一轴不平衡率评价"] = "";
                }
                else
                {
                    dr["一轴不平衡率限值"] = yzxz;
                    dr["一轴不平衡率评价"] = ybpd;
                }
                if(ydzzzzv.Text.Replace(" ","")=="")
                {
                    dr["一轴左阻滞率限值"] = "";
                    dr["一轴左阻滞率评价"] = "";
                }
                else
                {
                    dr["一轴左阻滞率限值"] = "≤3.5";
                    dr["一轴左阻滞率评价"] = yzzzzvs;
                }
                if (ydzyzzv.Text.Replace(" ", "") == "")
                {
                    dr["一轴右阻滞率限值"] = "";
                    dr["一轴右阻滞率评价"] = "";
                }
                else
                {
                    dr["一轴右阻滞率限值"] = "≤3.5";
                    dr["一轴右阻滞率评价"] = yzyzzvs;
                }
                if (edzzdv.Text.Replace(" ", "") == "")
                {
                    dr["二轴制动率限值"] = "";
                    dr["二轴制动率评价"] = "";
                }
                else
                {
                    dr["二轴制动率限值"] = ezxzs;
                    dr["二轴制动率评价"] = ezpds;
                }
                if (edzbphv.Text.Replace(" ", "") == "")
                {
                    dr["二轴不平衡率限值"] = "";
                    dr["二轴不平衡率评价"] = "";
                }
                else
                {
                    dr["二轴不平衡率限值"] = ezxz;
                    dr["二轴不平衡率评价"] = ebpd;
                }
                if (edzzzzv.Text.Replace(" ", "") == "")
                {
                    dr["二轴左阻滞率限值"] = "";
                    dr["二轴左阻滞率评价"] = "";
                }
                else
                {
                    dr["二轴左阻滞率限值"] = "≤3.5";
                    dr["二轴左阻滞率评价"] = ezzzzvs;
                }
                if (edzyzzv.Text.Replace(" ", "") == "")
                {
                    dr["二轴右阻滞率限值"] = "";
                    dr["二轴右阻滞率评价"] = "";
                }
                else
                {
                    dr["二轴右阻滞率限值"] = "≤3.5";
                    dr["二轴右阻滞率评价"] = ezyzzvs;
                }
                if (sdzzdv.Text.Replace(" ", "") == "")
                {
                    dr["三轴制动率限值"] = "";
                    dr["三轴制动率评价"] = "";
                }
                else
                {
                    dr["三轴制动率限值"] = siwlxz;
                    dr["三轴制动率评价"] = szpds;
                }
                if (sdzbphv.Text.Replace(" ", "") == "")
                {
                    dr["三轴不平衡率限值"] = "";
                    dr["三轴不平衡率评价"] = "";
                }
                else
                {
                    dr["三轴不平衡率限值"] = szxz;
                    dr["三轴不平衡率评价"] = sbpd;
                }
                if (sdzzzzv.Text.Replace(" ", "") == "")
                {
                    dr["三轴左阻滞率限值"] = "";
                    dr["三轴左阻滞率评价"] = "";
                }
                else
                {
                    dr["三轴左阻滞率限值"] = "≤3.5";
                    dr["三轴左阻滞率评价"] = szzzzvs;
                }
                if (sdzyzzv.Text.Replace(" ", "") == "")
                {
                    dr["三轴右阻滞率限值"] = "";
                    dr["三轴右阻滞率评价"] = "";
                }
                else
                {
                    dr["三轴右阻滞率限值"] = "≤3.5";
                    dr["三轴右阻滞率评价"] = szyzzvs;
                }
                if (sidzzdv.Text.Replace(" ", "") == "")
                {
                    dr["四轴制动率限值"] = "";
                    dr["四轴制动率评价"] = "";
                }
                else
                {
                    dr["四轴制动率限值"] = siwlxz;
                    dr["四轴制动率评价"] = sizpds;
                }
                if (sidzbphv.Text.Replace(" ", "") == "")
                {
                    dr["四轴不平衡率限值"] = "";
                    dr["四轴不平衡率评价"] = "";
                }
                else
                {
                    dr["四轴不平衡率限值"] = sizxz;
                    dr["四轴不平衡率评价"] = sibpd;
                }
                if (sidzzzzv.Text.Replace(" ", "") == "")
                {
                    dr["四轴左阻滞率限值"] = "";
                    dr["四轴左阻滞率评价"] = "";
                }
                else
                {
                    dr["四轴左阻滞率限值"] = "≤3.5";
                    dr["四轴左阻滞率评价"] = sizzzzvs;
                }
                if (sidzyzzv.Text.Replace(" ", "") == "")
                {
                    dr["四轴右阻滞率限值"] = "";
                    dr["四轴右阻滞率评价"] = "";
                }
                else
                {
                    dr["四轴右阻滞率限值"] = "≤3.5";
                    dr["四轴右阻滞率评价"] = sizyzzvs;
                }
                if (wdzzdv.Text.Replace(" ", "") == "")
                {
                    dr["五轴制动率限值"] = "";
                    dr["五轴制动率评价"] = "";
                }
                else
                {
                    dr["五轴制动率限值"] = siwlxz;
                    dr["五轴制动率评价"] = wzpds;
                }
                if (wdzbphv.Text.Replace(" ", "") == "")
                {
                    dr["五轴不平衡率限值"] = "";
                    dr["五轴不平衡率评价"] = "";
                }
                else
                {
                    dr["五轴不平衡率限值"] = wzxz;
                    dr["五轴不平衡率评价"] = wbpd;
                }
                if (wdzzzzv.Text.Replace(" ", "") == "")
                {
                    dr["五轴左阻滞率限值"] = "";
                    dr["五轴左阻滞率评价"] = "";
                }
                else
                {
                    dr["五轴左阻滞率限值"] = "≤3.5";
                    dr["五轴左阻滞率评价"] = wzzzzvs;
                }
                if (wdzyzzv.Text.Replace(" ", "") == "")
                {
                    dr["五轴右阻滞率限值"] = "";
                    dr["五轴右阻滞率评价"] = "";
                }
                else
                {
                    dr["五轴右阻滞率限值"] = "≤3.5";
                    dr["五轴右阻滞率评价"] = wzyzzvs;
                }
                if (ldzzdv.Text.Replace(" ", "") == "")
                {
                    dr["六轴制动率限值"] = "";
                    dr["六轴制动率评价"] = "";
                }
                else
                {
                    dr["六轴制动率限值"] = siwlxz;
                    dr["六轴制动率评价"] = lzpds;
                }
                if (ldzbphv.Text.Replace(" ", "") == "")
                {
                    dr["六轴不平衡率限值"] = "";
                    dr["六轴不平衡率评价"] = "";
                }
                else
                {
                    dr["六轴不平衡率限值"] = lzxz;
                    dr["六轴不平衡率评价"] = lbpds;
                }
                if (ldzzzzv.Text.Replace(" ", "") == "")
                {
                    dr["六轴左阻滞率限值"] = "";
                    dr["六轴左阻滞率评价"] = "";
                }
                else
                {
                    dr["六轴左阻滞率限值"] = "≤3.5";
                    dr["六轴左阻滞率评价"] = lzzzzvs;
                }
                if (ldzyzzv.Text.Replace(" ", "") == "")
                {
                    dr["六轴右阻滞率限值"] = "";
                    dr["六轴右阻滞率评价"] = "";
                }
                else
                {
                    dr["六轴右阻滞率限值"] = "≤3.5";
                    dr["六轴右阻滞率评价"] = lzyzzvs;
                }
                if(dczdl.Text.Replace(" ","")=="")
                {
                    dr["整车制动率限值"] = "";
                    dr["整车制动率评价"] = "";
                }
                else
                {
                    dr["整车制动率限值"] = "≥60";
                    dr["整车制动率评价"] = zczdpd;
                }
                if(dczczdl.Text.Replace(" ","")=="")
                {
                    dr["整车驻车制动率限值"] = "";
                    dr["整车驻车制动率评价"] = "";
                }
                else
                {
                    dr["整车驻车制动率限值"] = "≥20";
                    dr["整车驻车制动率评价"] = zcpd;
                }
                #endregion
                if(dychl.Text.Replace(" ","")=="")
                {
                    dr["侧滑1限值"] = "";
                }
                else
                {
                    dr["侧滑1限值"] = "-5~+5";
                }
                if(zxzs.Text.Contains("2"))
                {
                    if (dechl.Text.Replace(" ", "") == "")
                    {
                        dr["侧滑2限值"] = "";
                    }
                    else
                    {
                        dr["侧滑2限值"] = "-5~+5";
                    }
                }
                else
                {
                    dr["侧滑2限值"] = "";
                }
                #region 排放限值
                if(qygdsHC.Text.Replace(" ","")=="")
                {
                    dr["高怠速HC限值"] = "";
                    dr["高怠速HC评价"] = "";
                }
                else
                {
                    dr["高怠速HC限值"] = gdshcxz;
                    dr["高怠速HC评价"] = gdshcpd;
                }
                if(qygdsCO.Text.Replace(" ","")=="")
                {
                    dr["高怠速CO限值"] = "";
                    dr["高怠速CO评价"] = "";
                }
                else
                {
                    dr["高怠速CO限值"] = gdscoxz;
                    dr["高怠速CO评价"] = gdscopd;
                }
                if(qygdsλ.Text.Replace(" ","")=="")
                {
                    dr["高怠速λ限值"] = "";
                    dr["高怠速λ评价"] = "";
                }
                else
                {
                    dr["高怠速λ限值"] = "0.97~1.03";
                    dr["高怠速λ评价"] = gdsλpd;
                }
                if(qyddsHC.Text.Replace(" ","")=="")
                {
                    dr["低怠速HC限值"] = "";
                    dr["低怠速HC评价"] = "";
                }
                else
                {
                    dr["低怠速HC限值"] = ddshcxz;
                    dr["低怠速HC评价"] = ddshcpd;
                }
                if(qyddsCO.Text.Replace(" ","")=="")
                {
                    dr["低怠速CO限值"] = "";
                    dr["低怠速CO评价"] = "";
                }
                else
                {
                    dr["低怠速CO限值"] = ddscoxz;
                    dr["低怠速CO评价"] = ddscopd;
                }
                if(cygxsavg.Text.Replace(" ","")=="")
                {
                    dr["光吸收率限值"] = "";
                }
                else
                {
                    dr["光吸收率限值"] = gxsxz;
                }
                #endregion
                #region 左外灯限值
                if(zwyggq.Text.Replace(" ","")=="")
                {
                    dr["左外光强限值"] = "";
                    dr["左外灯远光光强评价"] = "";
                }
                else
                {
                    dr["左外光强限值"] = zgqxz;
                    dr["左外灯远光光强评价"] = zwgqpd;
                }
                if(zwygczH.Text.Replace(" ","")=="")
                {
                    dr["左外远光垂直偏移量H限值"] = "";
                    dr["左外灯远光垂直偏移量H评价"] = "";
                }
                else
                {
                    dr["左外远光垂直偏移量H限值"] = ygxz;
                    dr["左外灯远光垂直偏移量H评价"] = zwypd;
                }
                if(zwygsp.Text.Replace(" ","")=="")
                {
                    dr["左外远光水平偏移量限值"] = "";
                    dr["左外灯远光水平偏移量评价"] = "";
                }
                else
                {
                    dr["左外远光水平偏移量限值"] = "左170~右350";
                    dr["左外灯远光水平偏移量评价"] = "-";
                }
                if(zwjgczH.Text.Replace(" ","")=="")
                {
                    dr["左外近光垂直偏移量H限值"] = "";
                    dr["左外灯近光垂直偏移量H评价"] = "";
                }
                else
                {
                    dr["左外近光垂直偏移量H限值"] = jgxz;
                    dr["左外灯近光垂直偏移量H评价"] = zwjpd;
                }
                if (zwjgsp.Text.Replace(" ", "") == "")
                {
                    dr["左外近光水平偏移量限值"] = "";
                    dr["左外灯近光水平偏移量评价"] = "";
                }
                else
                {
                    dr["左外近光水平偏移量限值"] = "左170~右350";
                    dr["左外灯近光水平偏移量评价"] = "-";
                }
                #endregion
                #region 左内灯限值
                if (znyggq.Text.Replace(" ", "") == "")
                {
                    dr["左内光强限值"] = "";
                    dr["左内灯远光光强评价"] = "";
                }
                else
                {
                    dr["左内光强限值"] = zgqxz;
                    dr["左内灯远光光强评价"] = zngqpd;
                }
                if (znygczH.Text.Replace(" ", "") == "")
                {
                    dr["左内远光垂直偏移量H限值"] = "";
                    dr["左内灯远光垂直偏移量H评价"] = "";
                }
                else
                {
                    dr["左内远光垂直偏移量H限值"] = ygxz;
                    dr["左内灯远光垂直偏移量H评价"] = znypd;
                }
                if (znygsp.Text.Replace(" ", "") == "")
                {
                    dr["左内远光水平偏移量限值"] = "";
                    dr["左内灯远光水平偏移量评价"] = "";
                }
                else
                {
                    dr["左内远光水平偏移量限值"] = "左170~右350";
                    dr["左内灯远光水平偏移量评价"] = "-";
                }
                #endregion
                #region 右外灯限值
                if (ywyggq.Text.Replace(" ", "") == "")
                {
                    dr["右外光强限值"] = "";
                    dr["右外灯远光光强评价"] = "";
                }
                else
                {
                    dr["右外光强限值"] = zgqxz;
                    dr["右外灯远光光强评价"] = ywgqpd;
                }
                if (ywygczH.Text.Replace(" ", "") == "")
                {
                    dr["右外远光垂直偏移量H限值"] = "";
                    dr["右外灯远光垂直偏移量H评价"] = "";
                }
                else
                {
                    dr["右外远光垂直偏移量H限值"] = ygxz;
                    dr["右外灯远光垂直偏移量H评价"] = ywypd;
                }
                if (ywygsp.Text.Replace(" ", "") == "")
                {
                    dr["右外远光水平偏移量限值"] = "";
                    dr["右外灯远光水平偏移量评价"] = "";
                }
                else
                {
                    dr["右外远光水平偏移量限值"] = "左350~右350";
                    dr["右外灯远光水平偏移量评价"] = "-";
                }
                if (ywjgczH.Text.Replace(" ", "") == "")
                {
                    dr["右外近光垂直偏移量H限值"] = "";
                    dr["右外灯近光垂直偏移量H评价"] = "";
                }
                else
                {
                    dr["右外近光垂直偏移量H限值"] = jgxz;
                    dr["右外灯近光垂直偏移量H评价"] = ywjpd;
                }
                if (ywjgsp.Text.Replace(" ", "") == "")
                {
                    dr["右外近光水平偏移量限值"] = "";
                    dr["右外灯近光水平偏移量评价"] = "";
                }
                else
                {
                    dr["右外近光水平偏移量限值"] = "左170~右350";
                    dr["右外灯近光水平偏移量评价"] = "-";
                }
                #endregion
                #region 右内灯限值
                if (ynyggq.Text.Replace(" ", "") == "")
                {
                    dr["右内光强限值"] = "";
                    dr["右内灯远光光强评价"] = "";
                }
                else
                {
                    dr["右内光强限值"] = zgqxz;
                    dr["右内灯远光光强评价"] = yngqpd;
                }
                if (ynygczH.Text.Replace(" ", "") == "")
                {
                    dr["右内远光垂直偏移量H限值"] = "";
                    dr["右内灯远光垂直偏移量H评价"] = "";
                }
                else
                {
                    dr["右内远光垂直偏移量H限值"] = ygxz;
                    dr["右内灯远光垂直偏移量H评价"] = ynypd;
                }
                if (ynygsp.Text.Replace(" ", "") == "")
                {
                    dr["右内远光水平偏移量限值"] = "";
                    dr["右内灯远光水平偏移量评价"] = "";
                }
                else
                {
                    dr["右内远光水平偏移量限值"] = "左170~右350";
                    dr["右内灯远光水平偏移量评价"] = "-";
                }
                #endregion
                if(csb.Text.Replace(" ","")=="")
                {
                    dr["车速限值"] = "";
                }
                else
                {
                    dr["车速限值"] = "32.8~40";
                }
                if(lbsjz.Text.Replace(" ","")=="")
                {
                    dr["喇叭限值"] = "";
                }
                else
                {
                    dr["喇叭限值"] = "90~115";
                }
                #endregion
                #region 工位照片
                if (checkBox3.Checked == true)
                {
                    string strfiles = photoserver + @"" + photoadress + @"\" + lsh.Text + @"\";
                    bool brets  = File.Exists(strfiles);
                    if (brets)
                    {
                        string strFileName = photoserver + @"" + photoadress + @"\" + lsh.Text + @"\" + lsh.Text + "_B.jpeg";
                        string strFileName1 = photoserver + @"" + photoadress + @"\" + lsh.Text + @"\" + lsh.Text + "_H.jpeg";
                        string strFileName2 = photoserver + @"" + photoadress + @"\" + lsh.Text + @"\" + lsh.Text + "_S.jpeg";
                        string strFileName3 = photoserver + @"" + photoadress + @"\" + lsh.Text + @"\" + lsh.Text + "_P.jpeg";
                        bool bRet = File.Exists(strFileName);
                        if (bRet)
                        {
                            FileStream fs = new FileStream(strFileName, FileMode.Open);
                            byte[] byteData = new byte[fs.Length];
                            fs.Read(byteData, 0, byteData.Length);
                            fs.Close();
                            dr["制动工位照片"] = byteData;
                        }
                        bool bRet1 = File.Exists(strFileName1);
                        if (bRet1)
                        {
                            FileStream fs1 = new FileStream(strFileName1, FileMode.Open);
                            byte[] byteData1 = new byte[fs1.Length];
                            fs1.Read(byteData1, 0, byteData1.Length);
                            fs1.Close();
                            dr["灯光工位照片"] = byteData1;
                        }
                        bool bRet2 = File.Exists(strFileName2);
                        if (bRet2)
                        {
                            FileStream fs2 = new FileStream(strFileName2, FileMode.Open);
                            byte[] byteData2 = new byte[fs2.Length];
                            fs2.Read(byteData2, 0, byteData2.Length);
                            fs2.Close();
                            dr["动力工位照片"] = byteData2;
                        }
                        else
                        {
                            bool bRet3 = File.Exists(strFileName3);
                            if (bRet3)
                            {
                                FileStream fs3 = new FileStream(strFileName3, FileMode.Open);
                                byte[] byteData3 = new byte[fs3.Length];
                                fs3.Read(byteData3, 0, byteData3.Length);
                                fs3.Close();
                                dr["动力工位照片"] = byteData3;
                            }
                        }
                    }
                    else
                    {
                        if (dataGridView1.SelectedRows[0].Cells["检测次数"].Value.ToString() != "1")
                        {
                            string str = string.Format("select fid from Data_Modification where 检测次数=1 and 检测编号='{0}'", lsh.Text);
                            SqlDataAdapter sda = new SqlDataAdapter(str, conn);
                            DataTable dta = new DataTable();
                            sda.Fill(dta);
                            string fids = dta.Rows[0]["fid"].ToString();
                            string strFileName = photoserver + @"" + photoadress + @"\" + fids + @"\" + fids + "_B.jpeg";
                            string strFileName1 = photoserver + @"" + photoadress + @"\" + fids + @"\" + fids + "_H.jpeg";
                            string strFileName2 = photoserver + @"" + photoadress + @"\" + fids + @"\" + fids + "_S.jpeg";
                            string strFileName3 = photoserver + @"" + photoadress + @"\" + fids + @"\" + fids + "_P.jpeg";

                            string strFileName4 = photoserver + @"" + photoadress + @"\" + fid.Text + @"\" + fid.Text + "_B.jpeg";
                            string strFileName5 = photoserver + @"" + photoadress + @"\" + fid.Text + @"\" + fid.Text + "_H.jpeg";
                            string strFileName6 = photoserver + @"" + photoadress + @"\" + fid.Text + @"\" + fid.Text + "_S.jpeg";
                            string strFileName7 = photoserver + @"" + photoadress + @"\" + fid.Text + @"\" + fid.Text + "_P.jpeg";
                            bool bRet = File.Exists(strFileName4);
                            bool bRetb = File.Exists(strFileName);
                            if (bRet)
                            {
                                FileStream fs = new FileStream(strFileName4, FileMode.Open);
                                byte[] byteData = new byte[fs.Length];
                                fs.Read(byteData, 0, byteData.Length);
                                fs.Close();
                                dr["制动工位照片"] = byteData;
                            }
                            if(bRetb)
                            {
                                FileStream fs = new FileStream(strFileName, FileMode.Open);
                                byte[] byteData = new byte[fs.Length];
                                fs.Read(byteData, 0, byteData.Length);
                                fs.Close();
                                dr["制动工位照片"] = byteData;
                            }
                            bool bRet1 = File.Exists(strFileName5);
                            bool bReth = File.Exists(strFileName1);
                            if (bRet1)
                            {
                                FileStream fs1 = new FileStream(strFileName5, FileMode.Open);
                                byte[] byteData1 = new byte[fs1.Length];
                                fs1.Read(byteData1, 0, byteData1.Length);
                                fs1.Close();
                                dr["灯光工位照片"] = byteData1;
                            }
                            if(bReth)
                            {
                                FileStream fs1 = new FileStream(strFileName1, FileMode.Open);
                                byte[] byteData1 = new byte[fs1.Length];
                                fs1.Read(byteData1, 0, byteData1.Length);
                                fs1.Close();
                                dr["灯光工位照片"] = byteData1;
                            }
                            bool bRet2 = File.Exists(strFileName6);
                            if (bRet2)
                            {
                                FileStream fs2 = new FileStream(strFileName6, FileMode.Open);
                                byte[] byteData2 = new byte[fs2.Length];
                                fs2.Read(byteData2, 0, byteData2.Length);
                                fs2.Close();
                                dr["动力工位照片"] = byteData2;
                            }
                            else
                            {
                                bool bRet3 = File.Exists(strFileName2);
                                if (bRet3)
                                {
                                    FileStream fs2 = new FileStream(strFileName2, FileMode.Open);
                                    byte[] byteData2 = new byte[fs2.Length];
                                    fs2.Read(byteData2, 0, byteData2.Length);
                                    fs2.Close();
                                    dr["动力工位照片"] = byteData2;
                                }
                                else
                                {
                                    bool bRet9 = File.Exists(strFileName7);
                                    bool bRetp = File.Exists(strFileName3);
                                    if (bRet9)
                                    {
                                        FileStream fs3 = new FileStream(strFileName7, FileMode.Open);
                                        byte[] byteData3 = new byte[fs3.Length];
                                        fs3.Read(byteData3, 0, byteData3.Length);
                                        fs3.Close();
                                        dr["动力工位照片"] = byteData3;
                                    }
                                    if(bRetp)
                                    {
                                        FileStream fs3 = new FileStream(strFileName3, FileMode.Open);
                                        byte[] byteData3 = new byte[fs3.Length];
                                        fs3.Read(byteData3, 0, byteData3.Length);
                                        fs3.Close();
                                        dr["动力工位照片"] = byteData3;
                                    }
                                }
                            }
                        }
                        else
                        {
                            string strFileName = photoserver + @"" + photoadress + @"\" + fid.Text + @"\" + fid.Text + "_B.jpeg";
                            string strFileName1 = photoserver + @"" + photoadress + @"\" + fid.Text + @"\" + fid.Text + "_H.jpeg";
                            string strFileName2 = photoserver + @"" + photoadress + @"\" + fid.Text + @"\" + fid.Text + "_S.jpeg";
                            string strFileName3 = photoserver + @"" + photoadress + @"\" + fid.Text + @"\" + fid.Text + "_P.jpeg";
                            bool bRet = File.Exists(strFileName);
                            if (bRet)
                            {
                                FileStream fs = new FileStream(strFileName, FileMode.Open);
                                byte[] byteData = new byte[fs.Length];
                                fs.Read(byteData, 0, byteData.Length);
                                fs.Close();
                                dr["制动工位照片"] = byteData;
                            }
                            bool bRet1 = File.Exists(strFileName1);
                            if (bRet1)
                            {
                                FileStream fs1 = new FileStream(strFileName1, FileMode.Open);
                                byte[] byteData1 = new byte[fs1.Length];
                                fs1.Read(byteData1, 0, byteData1.Length);
                                fs1.Close();
                                dr["灯光工位照片"] = byteData1;
                            }
                            bool bRet2 = File.Exists(strFileName2);
                            if (bRet2)
                            {
                                FileStream fs2 = new FileStream(strFileName2, FileMode.Open);
                                byte[] byteData2 = new byte[fs2.Length];
                                fs2.Read(byteData2, 0, byteData2.Length);
                                fs2.Close();
                                dr["动力工位照片"] = byteData2;
                            }
                            else
                            {
                                bool bRet3 = File.Exists(strFileName3);
                                if (bRet3)
                                {
                                    FileStream fs3 = new FileStream(strFileName3, FileMode.Open);
                                    byte[] byteData3 = new byte[fs3.Length];
                                    fs3.Read(byteData3, 0, byteData3.Length);
                                    fs3.Close();
                                    dr["动力工位照片"] = byteData3;
                                }
                            }
                        }
                    }
                }
                #endregion
                dt.Rows.Add(dr);
                FastReport.Report report = new FastReport.Report();
                if (sender == "0")
                {
                    if (cllx.Text.Contains("客"))
                    {
                        report.Load(@".\fh.frx");
                        report.RegisterData(dt, "Data_Modification");
                    }
                    else
                    {
                        report.Load(@".\ysfh.frx");
                        report.RegisterData(dt, "Data_Modification");
                    }
                }
                #region 报告单类别
                if (sender == "1")
                {
                    if (formats == "套打")
                    {
                        //出小票
                        if (pd == "1")
                        {
                            if (checkBox1.Checked || checkBox2.Checked)
                            {
                                if (jylb.Text.Replace(" ", "").Contains("二级维护"))
                                {
                                    report.Load(@".\erwei.frx");
                                    report.RegisterData(dt, "Data_Modification");
                                }
                                else
                                {
                                    //A3模板套打整车评定
                                    report.Load(@".\wkcc.frx");
                                    report.RegisterData(dt, "Data_Modification");
                                }
                            }
                            else
                            {
                                report.Load(@".\final_inspection.frx");
                                report.RegisterData(dt, "Data_Modification");
                            }
                        }
                        else
                        {
                            if (jylb.Text.Replace(" ", "").Contains("二级维护"))
                            {
                                report.Load(@".\erwei.frx");
                                report.RegisterData(dt, "Data_Modification");
                            }
                            else
                            {
                                //A3模板套打整车评定
                                report.Load(@".\wkcc.frx");
                                report.RegisterData(dt, "Data_Modification");
                            }
                        }
                    }
                    else
                    {
                        if (pd == "1")
                        {
                            //A3模板非套打
                            if (checkBox1.Checked || checkBox2.Checked)
                            {
                                if (jylb.Text.Replace(" ", "").Contains("二级维护"))
                                {
                                    report.Load(@".\erwei.frx");
                                    report.RegisterData(dt, "Data_Modification");
                                }
                                else
                                {
                                    report.Load(@".\ZJ_JL.frx");
                                    report.RegisterData(dt, "Data_Modification");
                                }
                            }
                            else
                            {
                                report.Load(@".\final_inspection.frx");
                                report.RegisterData(dt, "Data_Modification");
                            }
                        }
                        else
                        {
                            if (jylb.Text.Replace(" ", "").Contains("二级维护"))
                            {
                                report.Load(@".\erwei.frx");
                                report.RegisterData(dt, "Data_Modification");
                            }
                            else
                            {
                                report.Load(@".\ZJ_JL.frx");
                                report.RegisterData(dt, "Data_Modification");
                            }
                        }
                    }
                }
                #endregion
                if(sender=="2")
                {
                    report.Load(@".\zjbg.frx");
                    report.RegisterData(dt, "Data_Modification");
                }
                if(sender=="3")
                {
                    report.Load(@".\djpd.frx");
                    report.RegisterData(dt, "Data_Modification");
                }
                if (!bPrint)
                {
                    report.Design();
                }
                else
                {
                    report.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private static bool JudgeFileExist(string url)
        {
            try
            {
                //创建根据网络地址的请求对象
                System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.CreateDefault(new Uri(url));
                httpWebRequest.Method = "HEAD";
                httpWebRequest.Timeout = 1000;
                //返回响应状态是否是成功比较的布尔值
                return (((System.Net.HttpWebResponse)httpWebRequest.GetResponse()).StatusCode == System.Net.HttpStatusCode.OK);
            }
            catch
            {
                return false;
            }
        }
        public void DownloadImage(string url, string path)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.ServicePoint.Expect100Continue = false;
            req.Method = "GET";
            req.KeepAlive = true;
            req.ContentType = "image/*";
            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            System.IO.Stream stream = null;
            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                Image.FromStream(stream).Save(path);
            }
            finally
            {
                // 释放资源
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }
        //设计综检记录单
        private void button5_Click(object sender, EventArgs e)
        {
            Print(false,"1");
        }   
        //打印综检记录单
        private void button4_Click(object sender, EventArgs e)
        {
            //conn.Open();
            //string str = string.Format("select 用户等级 from DM_hUsers_操作权限 where 用户名='{0}'", uname);
            //SqlDataAdapter sda = new SqlDataAdapter(str, conn);
            //DataTable dt = new DataTable();
            //sda.Fill(dt);
            //if (dt.Rows[0]["用户等级"].ToString().Replace(" ", "").Contains("A"))
            //{
            Exit_Modification();
            Print(true,"1");
            Commitpic();
                conn.Open();
                string str1 = string.Format("update Data_Modification set 是否通过='Y' where 检测编号='{0}' and 检测次数='{1}'", dataGridView1.SelectedRows[0].Cells["检测编号"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["检测次数"].Value.ToString());
                SqlCommand cmd1 = new SqlCommand(str1, conn);
                cmd1.ExecuteNonQuery();
                conn.Close();
            //}
            //else
            //{
            //    Exit_Modification();
            //    Print(true, "1");
            //    conn.Open();
            //    string str1 = string.Format("update Data_Modification set 是否通过='Y' where 检测编号='{0}' and 检测次数='{1}'", dataGridView1.SelectedRows[0].Cells["检测编号"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["检测次数"].Value.ToString());
            //    SqlCommand cmd1 = new SqlCommand(str1, conn);
            //    cmd1.ExecuteNonQuery();
            //    conn.Close();
            //    button4.Enabled = false;
            //}
            //choice.Text = "1";
            //conn.Open();
            //string str2 = string.Format("update Data_Modification set 是否结束='1' where 检测编号='{0}' and 检测次数='{1}'", dataGridView1.SelectedRows[0].Cells["检测编号"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["检测次数"].Value.ToString());
            //SqlCommand cmd2 = new SqlCommand(str2, conn);
            //cmd2.ExecuteNonQuery();
            //conn.Close();
        }
        //设计综检报告单
        private void button6_Click(object sender, EventArgs e)
        {
            Print(false, "2");
        }
        //打印综检报告单
        private void button7_Click(object sender, EventArgs e)
        {
            //conn.Open();
            //string str = string.Format("select 用户等级 from DM_hUsers_操作权限 where 用户名='{0}'", uname);
            //SqlDataAdapter sda = new SqlDataAdapter(str, conn);
            //DataTable dt = new DataTable();
            //sda.Fill(dt);
            //if (dt.Rows[0]["用户等级"].ToString().Replace(" ", "").Contains("A"))
            //{
                Exit_Modification();
                Print(true, "2");
            //    conn.Open();
            //    string str1 = string.Format("update Data_Modification set 是否通过='Y' where 检测编号='{0}' and 检测次数='{1}'", dataGridView1.SelectedRows[0].Cells["检测编号"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["检测次数"].Value.ToString());
            //    SqlCommand cmd1 = new SqlCommand(str1, conn);
            //    cmd1.ExecuteNonQuery();
            //    conn.Close();
            //}
            //else
            //{
            //    Print(true, "2");
            //    button7.Enabled = false;
            //}
        }
        //设计复核表
        private void button18_Click(object sender, EventArgs e)
        {
            Print(false,"0");
        }
        //打印复核表
        private void button16_Click(object sender, EventArgs e)
        {
            Print(true,"0");
        }
        //设计等级评定书
        private void button14_Click_1(object sender, EventArgs e)
        {
            Print(false,"3");
        }
        //打印等级评定书
        private void button13_Click_1(object sender, EventArgs e)
        {
            //conn.Open();
            //string str = string.Format("select 用户等级 from DM_hUsers_操作权限 where 用户名='{0}'", uname);
            //SqlDataAdapter sda = new SqlDataAdapter(str, conn);
            //DataTable dt = new DataTable();
            //sda.Fill(dt);
            //if (dt.Rows[0]["用户等级"].ToString().Replace(" ", "").Contains("A"))
            //{
            //    Print(true, "3");
            //}
            //else
            //{
            Exit_Modification();
            Print(true, "3");
            //    button13.Enabled = false;
            //    conn.Open();
            //    string str1 = string.Format("select 车牌号码,车牌颜色,检测次数,检测编号,检测日期,检测时间 from Data_Modification where 检测次数!='' and 检测次数!='' and  (车牌号码 like '%'+'{0}'+'%' or 车牌号码='') and (检测日期 between '{1}' and '{2}' or '{1}'='' or '{2}'='') and 检测时间!='' and 是否通过='Y' order by FID desc,检测时间 desc", zhphm.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"), dateTimePicker2.Value.ToString("yyyy-MM-dd"));
            //    SqlDataAdapter sda1 = new SqlDataAdapter(str1, conn);
            //    DataTable dt1 = new DataTable();
            //    sda1.Fill(dt1);
            //    dataGridView1.DataSource = dt1;
            //    conn.Close();
            //    button4.Enabled = true;
            //    button7.Enabled = true;
            //    button13.Enabled = true;
            //}
        }

        #region 值改变事件
        //车速值改变事件
        private void csb_TextChanged(object sender, EventArgs e)
        {
            CSZjudge();
            Exitpd();
            CLDEPD();
        }
        //驻车制动改变事件
        private void dczczdl_TextChanged(object sender, EventArgs e)
        {
            ZCZDjudge();
            Exitpd();
            CLDEPD();
        }
        //整车制动率改变事件
        private void dczdl_TextChanged(object sender, EventArgs e)
        {
            DCZDjudge();
            Exitpd();
            CLDEPD();
        }
        //高怠速CO值改变事件
        private void qygdsCO_TextChanged(object sender, EventArgs e)
        {
            SDSjudge();
            Exitpd();
            CLDEPD();
        }
        //高怠速HC值改变事件
        private void qygdsHC_TextChanged(object sender, EventArgs e)
        {
            SDSjudge();
            Exitpd();
            CLDEPD();
        }
        //高怠速λ值改变事件
        private void qygdsλ_TextChanged(object sender, EventArgs e)
        {
            SDSjudge();
            Exitpd();
            CLDEPD();
        }
        //低怠速CO值改变事件
        private void qyddsCO_TextChanged(object sender, EventArgs e)
        {
            SDSjudge();
            Exitpd();
            CLDEPD();
        }
        //低怠速HC值改变事件
        private void qyddsHC_TextChanged(object sender, EventArgs e)
        {
            SDSjudge();
            Exitpd();
            CLDEPD();
        }
        //光吸收系数1值改变事件
        private void cygxs1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (cygxs1.Text != "-" && cygxs1.Text != "" && cygxs2.Text != "-" && cygxs2.Text != "" && cygxs3.Text != "" && cygxs3.Text != "-")
                {
                    cygxsavg.Text = ((Convert.ToDouble(cygxs1.Text) + Convert.ToDouble(cygxs2.Text) + Convert.ToDouble(cygxs3.Text)) / 3).ToString("0.0");
                }
                ZYJSjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //光吸收系数2值改变事件
        private void cygxs2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (cygxs1.Text != "-" && cygxs1.Text != "" && cygxs2.Text != "-" && cygxs2.Text != "" && cygxs3.Text != "" && cygxs3.Text != "-")
                {
                    cygxsavg.Text = ((Convert.ToDouble(cygxs1.Text) + Convert.ToDouble(cygxs2.Text) + Convert.ToDouble(cygxs3.Text)) / 3).ToString("0.0");
                }
                ZYJSjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //光吸收系数3值改变事件
        private void cygxs3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (cygxs1.Text != "-" && cygxs1.Text != "" && cygxs2.Text != "-" && cygxs2.Text != "" && cygxs3.Text != "" && cygxs3.Text != "-")
                {
                    cygxsavg.Text = ((Convert.ToDouble(cygxs1.Text) + Convert.ToDouble(cygxs2.Text) + Convert.ToDouble(cygxs3.Text)) / 3).ToString("0.0");
                }
                ZYJSjudge();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //第一测滑量值改变事件
        private void dychl_TextChanged(object sender, EventArgs e)
        {
            CHjudge();
            Exitpd();
            CLDEPD();
        }
        //第二测滑量值改变事件
        private void dechl_TextChanged(object sender, EventArgs e)
        {
            CHjudge();
            Exitpd();
            CLDEPD();
        }
        //喇叭声级值改变事件
        private void lbsjz_TextChanged(object sender, EventArgs e)
        {
            LBjudge();
            Exitpd();
            CLDEPD();
        }
        //左外远光光强值改变事件
        private void zwyggq_TextChanged(object sender, EventArgs e)
        {
            DZjudge();
            Exitpd();
            CLDEPD();
        }
        //右外远光光强值改变事件
        private void ywyggq_TextChanged(object sender, EventArgs e)
        {
            DZjudge();
            Exitpd();
            CLDEPD();
        }
        //左内远光光强值改变事件
        private void znyggq_TextChanged(object sender, EventArgs e)
        {
            DZjudge();
            Exitpd();
            CLDEPD();
        }
        //右内远光光强值改变事件
        private void ynyggq_TextChanged(object sender, EventArgs e)
        {
            DZjudge();
            Exitpd();
            CLDEPD();
        }
        //左外远光垂直偏移量H值改变事件
        private void zwygczH_TextChanged(object sender, EventArgs e)
        {
            DZjudge();
            Exitpd();
            CLDEPD();
        }
        //左内远光垂直偏移量H值改变事件
        private void znygczH_TextChanged(object sender, EventArgs e)
        {
            DZjudge();
            Exitpd();
            CLDEPD();
        }
        //右内远光垂直偏移量H值改变事件
        private void ynygczH_TextChanged(object sender, EventArgs e)
        {
            DZjudge();
            Exitpd();
            CLDEPD();
        }
        //右外远光垂直偏移量H值改变事件
        private void ywygczH_TextChanged(object sender, EventArgs e)
        {
            DZjudge();
            Exitpd();
            CLDEPD();
        }
        //左外近光垂直偏移量H值改变事件
        private void zwjgczH_TextChanged(object sender, EventArgs e)
        {
            DZjudge();
            Exitpd();
            CLDEPD();
        }
        //左内近光垂直偏移量H值改变事件
        private void znjgczH_TextChanged(object sender, EventArgs e)
        {
            DZjudge();
            Exitpd();
            CLDEPD();
        }
        //右内近光垂直偏移量H值改变事件
        private void ynjgczH_TextChanged(object sender, EventArgs e)
        {
            DZjudge();
            Exitpd();
            CLDEPD();
        }
        //右外近光垂直偏移量H值改变事件
        private void ywjgczH_TextChanged(object sender, EventArgs e)
        {
            DZjudge();
            Exitpd();
            CLDEPD();
        }
        //悬架前轴左吸收率值改变事件
        private void qzzxsl_TextChanged(object sender, EventArgs e)
        {
            XJjudge();
            Exitpd();
            CLDEPD();
        }
        //悬架前轴右吸收率值改变事件
        private void qzyxsl_TextChanged(object sender, EventArgs e)
        {
            XJjudge();
            Exitpd();
            CLDEPD();
        }
        //悬架前轴左右吸收率差值改变事件
        private void qzzyc_TextChanged(object sender, EventArgs e)
        {
            XJjudge();
            Exitpd();
            CLDEPD();
        }
        //悬架后轴左吸收率值改变事件
        private void hzzxsl_TextChanged(object sender, EventArgs e)
        {
            XJjudge();
            Exitpd();
            CLDEPD();
        }
        //悬架后轴右吸收率值改变事件
        private void hzyxsl_TextChanged(object sender, EventArgs e)
        {
            XJjudge();
            Exitpd();
            CLDEPD();
        }
        //悬架后轴左右吸收率差值改变事件
        private void hzzyc_TextChanged(object sender, EventArgs e)
        {
            XJjudge();
            Exitpd();
            CLDEPD();
        }
        //一轴制动率值改变事件
        private void ydzzdv_TextChanged(object sender, EventArgs e)
        {
            M1judge();
            Exitpd();
            CLDEPD();
        }
        //二轴制动率值改变事件
        private void edzzdv_TextChanged(object sender, EventArgs e)
        {
            M1judge();
            Exitpd();
            CLDEPD();
        }
        //三轴制动率值改变事件
        private void sdzzdv_TextChanged(object sender, EventArgs e)
        {
            M1judge();
            Exitpd();
            CLDEPD();
        }
        //四轴制动率值改变事件
        private void sidzzdv_TextChanged(object sender, EventArgs e)
        {
            M1judge();
            Exitpd();
            CLDEPD();
        }
        //五轴制动率值改变事件
        private void wdzzdv_TextChanged(object sender, EventArgs e)
        {
            M1judge();
            Exitpd();
            CLDEPD();
        }
        //六轴制动率值改变事件
        private void ldzzdv_TextChanged(object sender, EventArgs e)
        {
            M1judge();
            Exitpd();
            CLDEPD();
        }
        //一轴制动不平衡率值改变事件
        private void ydzbphv_TextChanged(object sender, EventArgs e)
        {
            SZXZjudge();
            Exitpd();
            CLDEPD();
        }
        //二轴制动不平衡率值改变事件
        private void edzbphv_TextChanged(object sender, EventArgs e)
        {
            SZXZjudge();
            Exitpd();
            CLDEPD();
        }
        //三轴制动不平衡率值改变事件
        private void sdzbphv_TextChanged(object sender, EventArgs e)
        {
            SZXZjudge();
            Exitpd();
            CLDEPD();
        }
        //四轴制动不平衡率值改变事件
        private void sidzbphv_TextChanged(object sender, EventArgs e)
        {
            SZXZjudge();
            Exitpd();
            CLDEPD();
        }
        //五轴制动不平衡率值改变事件
        private void wdzbphv_TextChanged(object sender, EventArgs e)
        {
            SZXZjudge();
            Exitpd();
            CLDEPD();
        }
        //六轴制动不平衡率值改变事件
        private void ldzbphv_TextChanged(object sender, EventArgs e)
        {
            SZXZjudge();
            Exitpd();
            CLDEPD();
        }
        //一轴左阻滞率改变事件
        private void ydzzzzv_TextChanged(object sender, EventArgs e)
        {
            ZZVjudge();
            Exitpd();
            CLDEPD();
        }
        //一轴右阻滞率改变事件
        private void ydzyzzv_TextChanged(object sender, EventArgs e)
        {
            ZZVjudge();
            Exitpd();
            CLDEPD();
        }
        //二轴左阻滞率改变事件
        private void edzzzzv_TextChanged(object sender, EventArgs e)
        {
            ZZVjudge();
            Exitpd();
            CLDEPD();
        }
        //二轴右阻滞率改变事件
        private void edzyzzv_TextChanged(object sender, EventArgs e)
        {
            ZZVjudge();
            Exitpd();
            CLDEPD();
        }
        //三轴左阻滞率改变事件
        private void sdzzzzv_TextChanged(object sender, EventArgs e)
        {
            ZZVjudge();
            Exitpd();
            CLDEPD();
        }
        //三轴右阻滞率改变事件
        private void sdzyzzv_TextChanged(object sender, EventArgs e)
        {
            ZZVjudge();
            Exitpd();
            CLDEPD();
        }
        //四轴左阻滞率改变事件
        private void sidzzzzv_TextChanged(object sender, EventArgs e)
        {
            ZZVjudge();
            Exitpd();
            CLDEPD();
        }
        //四轴右阻滞率改变事件
        private void sidzyzzv_TextChanged(object sender, EventArgs e)
        {
            ZZVjudge();
            Exitpd();
            CLDEPD();
        }
        //五轴左阻滞率改变事件
        private void wdzzzzv_TextChanged(object sender, EventArgs e)
        {
            ZZVjudge();
            Exitpd();
            CLDEPD();
        }
        //五轴右阻滞率改变事件
        private void wdzyzzv_TextChanged(object sender, EventArgs e)
        {
            ZZVjudge();
            Exitpd();
            CLDEPD();
        }
        //六轴左阻滞率改变事件
        private void ldzzzzv_TextChanged(object sender, EventArgs e)
        {
            ZZVjudge();
            Exitpd();
            CLDEPD();
        }
        //六轴右阻滞率改变事件
        private void ldzyzzv_TextChanged(object sender, EventArgs e)
        {
            ZZVjudge();
            Exitpd();
            CLDEPD();
        }

        #endregion

        #region 热键
        class HotKey
        {
            //申明API函数
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern bool RegisterHotKey(
             IntPtr hWnd, // handle to window
             int id, // hot key identifier
             uint fsModifiers, // key-modifier options
             Keys vk // virtual-key code
            );
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern bool UnregisterHotKey(
             IntPtr hWnd, // handle to window
             int id // hot key identifier
            );
        }
        //重载WndProc函数
        protected override void WndProc(ref Message m)
        {

            switch (m.WParam.ToInt32())
            {
                case 101:
                    if (this.button5.Visible)
                        this.button5.Visible = false;
                    else
                        this.button5.Visible = true;
                    break;
                case 102:
                    if (this.button6.Visible)
                        this.button6.Visible = false;
                    else
                        this.button6.Visible = true;
                    break;
                case 103:
                    if (this.button14.Visible)
                        this.button14.Visible = false;
                    else
                        this.button14.Visible = true;
                    break;
                case 104:
                    if (this.button18.Visible)
                        this.button18.Visible = false;
                    else
                        this.button18.Visible = true;
                    break;
            }
            base.WndProc(ref m);
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            HotKey.RegisterHotKey(Handle, 101, 2, Keys.Y);
            HotKey.RegisterHotKey(Handle, 102, 2, Keys.X);
            HotKey.RegisterHotKey(Handle, 103, 2, Keys.M);
            HotKey.RegisterHotKey(Handle, 104, 2, Keys.K);
        }

        private void Form1_Leave(object sender, EventArgs e)
        {
            HotKey.UnregisterHotKey(Handle, 101);
            HotKey.UnregisterHotKey(Handle, 102);
            HotKey.UnregisterHotKey(Handle, 103);
            HotKey.UnregisterHotKey(Handle, 104);
        }
        #endregion
        #region 驻车制动率计算
        //一轴左驻车制动力发生改变
        private void yzzzczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double dc1 = 0;
                double dc2 = 0;
                double dc3 = 0;
                double dc4 = 0;
                double dc5 = 0;
                double dc6 = 0;
                double dc7 = 0;
                double dc8 = 0;
                double dc9 = 0;
                double dc10 = 0;
                double dc11 = 0;
                double dc12 = 0;
                double dc13 = 0;
                if (yzzzczd.Text == "" || yzzzczd.Text == "-") { dc1 = 0; }
                else { dc1 = Convert.ToDouble(yzzzczd.Text); }
                if (yzyzczd.Text == "" || yzyzczd.Text == "-") { dc2 = 0; }
                else { dc2 = Convert.ToDouble(yzyzczd.Text); }
                if (ezzzczd.Text == "" || ezzzczd.Text == "-") { dc3 = 0; }
                else { dc3 = Convert.ToDouble(ezzzczd.Text); }
                if (ezyzczd.Text == "" || ezyzczd.Text == "-") { dc4 = 0; }
                else { dc4 = Convert.ToDouble(ezyzczd.Text); }
                if (szzzczd.Text == "" || szzzczd.Text == "-") { dc5 = 0; }
                else { dc5 = Convert.ToDouble(szzzczd.Text); }
                if (szyzczd.Text == "" || szyzczd.Text == "-") { dc6 = 0; }
                else { dc6 = Convert.ToDouble(szyzczd.Text); }
                if (sizzzczd.Text == "" || sizzzczd.Text == "-") { dc7 = 0; }
                else { dc7 = Convert.ToDouble(sizzzczd.Text); }
                if (sizyzczd.Text == "" || sizyzczd.Text == "-") { dc8 = 0; }
                else { dc8 = Convert.ToDouble(sizyzczd.Text); }
                if (wzzzczd.Text == "" || wzzzczd.Text == "-") { dc9 = 0; }
                else { dc9 = Convert.ToDouble(wzzzczd.Text); }
                if (wzyzczd.Text == "" || wzyzczd.Text == "-") { dc10 = 0; }
                else { dc10 = Convert.ToDouble(wzyzczd.Text); }
                if (lzzzczd.Text == "" || lzzzczd.Text == "-") { dc11 = 0; }
                else { dc11 = Convert.ToDouble(lzzzczd.Text); }
                if (lzyzczd.Text == "" || lzyzczd.Text == "-") { dc12 = 0; }
                else { dc12 = Convert.ToDouble(lzyzczd.Text); }
                //if (dcspcz.Text == "" || dcspcz.Text == "-") { dc13 = 0; }
                //else { dc13 = Convert.ToDouble(dcspcz.Text); }
                dc13 = Convert.ToDouble(TextsIsnull(yzzlh.Text)) + Convert.ToDouble(TextsIsnull(yzylh.Text)) + Convert.ToDouble(TextsIsnull(ezzlh.Text)) + Convert.ToDouble(TextsIsnull(ezylh.Text)) + Convert.ToDouble(TextsIsnull(szzlh.Text)) + Convert.ToDouble(TextsIsnull(szylh.Text)) + Convert.ToDouble(TextsIsnull(sizzlh.Text)) + Convert.ToDouble(TextsIsnull(sizylh.Text)) + Convert.ToDouble(TextsIsnull(wzzlh.Text)) + Convert.ToDouble(TextsIsnull(wzylh.Text)) + Convert.ToDouble(TextsIsnull(lzzlh.Text)) + Convert.ToDouble(TextsIsnull(lzylh.Text));
                double n = dc1 + dc2 + dc3 + dc4 + dc5 + dc6 + dc7 + dc8 + dc9 + dc10 + dc11 + dc12;//所有驻车制动力之和
                double dszs = dc13;
                if (dszs != 0 && dcspcz.Text != "0")
                {
                    dczczdl.Text = ((n / dszs) * 100).ToString("0.0");//整车驻车制动率
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //一轴右驻车制动力发生改变
        private void yzyzczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double dc1 = 0;
                double dc2 = 0;
                double dc3 = 0;
                double dc4 = 0;
                double dc5 = 0;
                double dc6 = 0;
                double dc7 = 0;
                double dc8 = 0;
                double dc9 = 0;
                double dc10 = 0;
                double dc11 = 0;
                double dc12 = 0;
                double dc13 = 0;
                if (yzzzczd.Text == "" || yzzzczd.Text == "-") { dc1 = 0; }
                else { dc1 = Convert.ToDouble(yzzzczd.Text); }
                if (yzyzczd.Text == "" || yzyzczd.Text == "-") { dc2 = 0; }
                else { dc2 = Convert.ToDouble(yzyzczd.Text); }
                if (ezzzczd.Text == "" || ezzzczd.Text == "-") { dc3 = 0; }
                else { dc3 = Convert.ToDouble(ezzzczd.Text); }
                if (ezyzczd.Text == "" || ezyzczd.Text == "-") { dc4 = 0; }
                else { dc4 = Convert.ToDouble(ezyzczd.Text); }
                if (szzzczd.Text == "" || szzzczd.Text == "-") { dc5 = 0; }
                else { dc5 = Convert.ToDouble(szzzczd.Text); }
                if (szyzczd.Text == "" || szyzczd.Text == "-") { dc6 = 0; }
                else { dc6 = Convert.ToDouble(szyzczd.Text); }
                if (sizzzczd.Text == "" || sizzzczd.Text == "-") { dc7 = 0; }
                else { dc7 = Convert.ToDouble(sizzzczd.Text); }
                if (sizyzczd.Text == "" || sizyzczd.Text == "-") { dc8 = 0; }
                else { dc8 = Convert.ToDouble(sizyzczd.Text); }
                if (wzzzczd.Text == "" || wzzzczd.Text == "-") { dc9 = 0; }
                else { dc9 = Convert.ToDouble(wzzzczd.Text); }
                if (wzyzczd.Text == "" || wzyzczd.Text == "-") { dc10 = 0; }
                else { dc10 = Convert.ToDouble(wzyzczd.Text); }
                if (lzzzczd.Text == "" || lzzzczd.Text == "-") { dc11 = 0; }
                else { dc11 = Convert.ToDouble(lzzzczd.Text); }
                if (lzyzczd.Text == "" || lzyzczd.Text == "-") { dc12 = 0; }
                else { dc12 = Convert.ToDouble(lzyzczd.Text); }
                dc13 = Convert.ToDouble(TextsIsnull(yzzlh.Text)) + Convert.ToDouble(TextsIsnull(yzylh.Text)) + Convert.ToDouble(TextsIsnull(ezzlh.Text)) + Convert.ToDouble(TextsIsnull(ezylh.Text)) + Convert.ToDouble(TextsIsnull(szzlh.Text)) + Convert.ToDouble(TextsIsnull(szylh.Text)) + Convert.ToDouble(TextsIsnull(sizzlh.Text)) + Convert.ToDouble(TextsIsnull(sizylh.Text)) + Convert.ToDouble(TextsIsnull(wzzlh.Text)) + Convert.ToDouble(TextsIsnull(wzylh.Text)) + Convert.ToDouble(TextsIsnull(lzzlh.Text)) + Convert.ToDouble(TextsIsnull(lzylh.Text));
                double n = dc1 + dc2 + dc3 + dc4 + dc5 + dc6 + dc7 + dc8 + dc9 + dc10 + dc11 + dc12;//所有驻车制动力之和
                double dszs = dc13 ;
                if (dszs != 0 && dcspcz.Text != "0")
                {
                    dczczdl.Text = ((n / dszs) * 100).ToString("0.0");//整车驻车制动率
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //二轴左驻车制动力发生改变
        private void ezzzczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double dc1 = 0;
                double dc2 = 0;
                double dc3 = 0;
                double dc4 = 0;
                double dc5 = 0;
                double dc6 = 0;
                double dc7 = 0;
                double dc8 = 0;
                double dc9 = 0;
                double dc10 = 0;
                double dc11 = 0;
                double dc12 = 0;
                double dc13 = 0;
                if (yzzzczd.Text == "" || yzzzczd.Text == "-") { dc1 = 0; }
                else { dc1 = Convert.ToDouble(yzzzczd.Text); }
                if (yzyzczd.Text == "" || yzyzczd.Text == "-") { dc2 = 0; }
                else { dc2 = Convert.ToDouble(yzyzczd.Text); }
                if (ezzzczd.Text == "" || ezzzczd.Text == "-") { dc3 = 0; }
                else { dc3 = Convert.ToDouble(ezzzczd.Text); }
                if (ezyzczd.Text == "" || ezyzczd.Text == "-") { dc4 = 0; }
                else { dc4 = Convert.ToDouble(ezyzczd.Text); }
                if (szzzczd.Text == "" || szzzczd.Text == "-") { dc5 = 0; }
                else { dc5 = Convert.ToDouble(szzzczd.Text); }
                if (szyzczd.Text == "" || szyzczd.Text == "-") { dc6 = 0; }
                else { dc6 = Convert.ToDouble(szyzczd.Text); }
                if (sizzzczd.Text == "" || sizzzczd.Text == "-") { dc7 = 0; }
                else { dc7 = Convert.ToDouble(sizzzczd.Text); }
                if (sizyzczd.Text == "" || sizyzczd.Text == "-") { dc8 = 0; }
                else { dc8 = Convert.ToDouble(sizyzczd.Text); }
                if (wzzzczd.Text == "" || wzzzczd.Text == "-") { dc9 = 0; }
                else { dc9 = Convert.ToDouble(wzzzczd.Text); }
                if (wzyzczd.Text == "" || wzyzczd.Text == "-") { dc10 = 0; }
                else { dc10 = Convert.ToDouble(wzyzczd.Text); }
                if (lzzzczd.Text == "" || lzzzczd.Text == "-") { dc11 = 0; }
                else { dc11 = Convert.ToDouble(lzzzczd.Text); }
                if (lzyzczd.Text == "" || lzyzczd.Text == "-") { dc12 = 0; }
                else { dc12 = Convert.ToDouble(lzyzczd.Text); }
                dc13 = Convert.ToDouble(TextsIsnull(yzzlh.Text)) + Convert.ToDouble(TextsIsnull(yzylh.Text)) + Convert.ToDouble(TextsIsnull(ezzlh.Text)) + Convert.ToDouble(TextsIsnull(ezylh.Text)) + Convert.ToDouble(TextsIsnull(szzlh.Text)) + Convert.ToDouble(TextsIsnull(szylh.Text)) + Convert.ToDouble(TextsIsnull(sizzlh.Text)) + Convert.ToDouble(TextsIsnull(sizylh.Text)) + Convert.ToDouble(TextsIsnull(wzzlh.Text)) + Convert.ToDouble(TextsIsnull(wzylh.Text)) + Convert.ToDouble(TextsIsnull(lzzlh.Text)) + Convert.ToDouble(TextsIsnull(lzylh.Text));
                double n = dc1 + dc2 + dc3 + dc4 + dc5 + dc6 + dc7 + dc8 + dc9 + dc10 + dc11 + dc12;//所有驻车制动力之和
                double dszs = dc13 ;
                if (dszs != 0 && dcspcz.Text != "0")
                {
                    dczczdl.Text = ((n / dszs) * 100).ToString("0.0");//整车驻车制动率
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //二轴右驻车制动力发生改变
        private void ezyzczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double dc1 = 0;
                double dc2 = 0;
                double dc3 = 0;
                double dc4 = 0;
                double dc5 = 0;
                double dc6 = 0;
                double dc7 = 0;
                double dc8 = 0;
                double dc9 = 0;
                double dc10 = 0;
                double dc11 = 0;
                double dc12 = 0;
                double dc13 = 0;
                if (yzzzczd.Text == "" || yzzzczd.Text == "-") { dc1 = 0; }
                else { dc1 = Convert.ToDouble(yzzzczd.Text); }
                if (yzyzczd.Text == "" || yzyzczd.Text == "-") { dc2 = 0; }
                else { dc2 = Convert.ToDouble(yzyzczd.Text); }
                if (ezzzczd.Text == "" || ezzzczd.Text == "-") { dc3 = 0; }
                else { dc3 = Convert.ToDouble(ezzzczd.Text); }
                if (ezyzczd.Text == "" || ezyzczd.Text == "-") { dc4 = 0; }
                else { dc4 = Convert.ToDouble(ezyzczd.Text); }
                if (szzzczd.Text == "" || szzzczd.Text == "-") { dc5 = 0; }
                else { dc5 = Convert.ToDouble(szzzczd.Text); }
                if (szyzczd.Text == "" || szyzczd.Text == "-") { dc6 = 0; }
                else { dc6 = Convert.ToDouble(szyzczd.Text); }
                if (sizzzczd.Text == "" || sizzzczd.Text == "-") { dc7 = 0; }
                else { dc7 = Convert.ToDouble(sizzzczd.Text); }
                if (sizyzczd.Text == "" || sizyzczd.Text == "-") { dc8 = 0; }
                else { dc8 = Convert.ToDouble(sizyzczd.Text); }
                if (wzzzczd.Text == "" || wzzzczd.Text == "-") { dc9 = 0; }
                else { dc9 = Convert.ToDouble(wzzzczd.Text); }
                if (wzyzczd.Text == "" || wzyzczd.Text == "-") { dc10 = 0; }
                else { dc10 = Convert.ToDouble(wzyzczd.Text); }
                if (lzzzczd.Text == "" || lzzzczd.Text == "-") { dc11 = 0; }
                else { dc11 = Convert.ToDouble(lzzzczd.Text); }
                if (lzyzczd.Text == "" || lzyzczd.Text == "-") { dc12 = 0; }
                else { dc12 = Convert.ToDouble(lzyzczd.Text); }
                dc13 = Convert.ToDouble(TextsIsnull(yzzlh.Text)) + Convert.ToDouble(TextsIsnull(yzylh.Text)) + Convert.ToDouble(TextsIsnull(ezzlh.Text)) + Convert.ToDouble(TextsIsnull(ezylh.Text)) + Convert.ToDouble(TextsIsnull(szzlh.Text)) + Convert.ToDouble(TextsIsnull(szylh.Text)) + Convert.ToDouble(TextsIsnull(sizzlh.Text)) + Convert.ToDouble(TextsIsnull(sizylh.Text)) + Convert.ToDouble(TextsIsnull(wzzlh.Text)) + Convert.ToDouble(TextsIsnull(wzylh.Text)) + Convert.ToDouble(TextsIsnull(lzzlh.Text)) + Convert.ToDouble(TextsIsnull(lzylh.Text));
                double n = dc1 + dc2 + dc3 + dc4 + dc5 + dc6 + dc7 + dc8 + dc9 + dc10 + dc11 + dc12;//所有驻车制动力之和
                double dszs = dc13 ;
                if (dszs != 0 && dcspcz.Text != "0")
                {
                    dczczdl.Text = ((n / dszs) * 100).ToString("0.0");//整车驻车制动率
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //三轴左驻车制动力发生改变
        private void szzzczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double dc1 = 0;
                double dc2 = 0;
                double dc3 = 0;
                double dc4 = 0;
                double dc5 = 0;
                double dc6 = 0;
                double dc7 = 0;
                double dc8 = 0;
                double dc9 = 0;
                double dc10 = 0;
                double dc11 = 0;
                double dc12 = 0;
                double dc13 = 0;
                if (yzzzczd.Text == "" || yzzzczd.Text == "-") { dc1 = 0; }
                else { dc1 = Convert.ToDouble(yzzzczd.Text); }
                if (yzyzczd.Text == "" || yzyzczd.Text == "-") { dc2 = 0; }
                else { dc2 = Convert.ToDouble(yzyzczd.Text); }
                if (ezzzczd.Text == "" || ezzzczd.Text == "-") { dc3 = 0; }
                else { dc3 = Convert.ToDouble(ezzzczd.Text); }
                if (ezyzczd.Text == "" || ezyzczd.Text == "-") { dc4 = 0; }
                else { dc4 = Convert.ToDouble(ezyzczd.Text); }
                if (szzzczd.Text == "" || szzzczd.Text == "-") { dc5 = 0; }
                else { dc5 = Convert.ToDouble(szzzczd.Text); }
                if (szyzczd.Text == "" || szyzczd.Text == "-") { dc6 = 0; }
                else { dc6 = Convert.ToDouble(szyzczd.Text); }
                if (sizzzczd.Text == "" || sizzzczd.Text == "-") { dc7 = 0; }
                else { dc7 = Convert.ToDouble(sizzzczd.Text); }
                if (sizyzczd.Text == "" || sizyzczd.Text == "-") { dc8 = 0; }
                else { dc8 = Convert.ToDouble(sizyzczd.Text); }
                if (wzzzczd.Text == "" || wzzzczd.Text == "-") { dc9 = 0; }
                else { dc9 = Convert.ToDouble(wzzzczd.Text); }
                if (wzyzczd.Text == "" || wzyzczd.Text == "-") { dc10 = 0; }
                else { dc10 = Convert.ToDouble(wzyzczd.Text); }
                if (lzzzczd.Text == "" || lzzzczd.Text == "-") { dc11 = 0; }
                else { dc11 = Convert.ToDouble(lzzzczd.Text); }
                if (lzyzczd.Text == "" || lzyzczd.Text == "-") { dc12 = 0; }
                else { dc12 = Convert.ToDouble(lzyzczd.Text); }
                dc13 = Convert.ToDouble(TextsIsnull(yzzlh.Text)) + Convert.ToDouble(TextsIsnull(yzylh.Text)) + Convert.ToDouble(TextsIsnull(ezzlh.Text)) + Convert.ToDouble(TextsIsnull(ezylh.Text)) + Convert.ToDouble(TextsIsnull(szzlh.Text)) + Convert.ToDouble(TextsIsnull(szylh.Text)) + Convert.ToDouble(TextsIsnull(sizzlh.Text)) + Convert.ToDouble(TextsIsnull(sizylh.Text)) + Convert.ToDouble(TextsIsnull(wzzlh.Text)) + Convert.ToDouble(TextsIsnull(wzylh.Text)) + Convert.ToDouble(TextsIsnull(lzzlh.Text)) + Convert.ToDouble(TextsIsnull(lzylh.Text));
                double n = dc1 + dc2 + dc3 + dc4 + dc5 + dc6 + dc7 + dc8 + dc9 + dc10 + dc11 + dc12;//所有驻车制动力之和
                double dszs = dc13 ;
                if (dszs != 0 && dcspcz.Text != "0")
                {
                    dczczdl.Text = ((n / dszs) * 100).ToString("0.0");//整车驻车制动率
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //三轴右驻车制动力发生改变
        private void szyzczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double dc1 = 0;
                double dc2 = 0;
                double dc3 = 0;
                double dc4 = 0;
                double dc5 = 0;
                double dc6 = 0;
                double dc7 = 0;
                double dc8 = 0;
                double dc9 = 0;
                double dc10 = 0;
                double dc11 = 0;
                double dc12 = 0;
                double dc13 = 0;
                if (yzzzczd.Text == "" || yzzzczd.Text == "-") { dc1 = 0; }
                else { dc1 = Convert.ToDouble(yzzzczd.Text); }
                if (yzyzczd.Text == "" || yzyzczd.Text == "-") { dc2 = 0; }
                else { dc2 = Convert.ToDouble(yzyzczd.Text); }
                if (ezzzczd.Text == "" || ezzzczd.Text == "-") { dc3 = 0; }
                else { dc3 = Convert.ToDouble(ezzzczd.Text); }
                if (ezyzczd.Text == "" || ezyzczd.Text == "-") { dc4 = 0; }
                else { dc4 = Convert.ToDouble(ezyzczd.Text); }
                if (szzzczd.Text == "" || szzzczd.Text == "-") { dc5 = 0; }
                else { dc5 = Convert.ToDouble(szzzczd.Text); }
                if (szyzczd.Text == "" || szyzczd.Text == "-") { dc6 = 0; }
                else { dc6 = Convert.ToDouble(szyzczd.Text); }
                if (sizzzczd.Text == "" || sizzzczd.Text == "-") { dc7 = 0; }
                else { dc7 = Convert.ToDouble(sizzzczd.Text); }
                if (sizyzczd.Text == "" || sizyzczd.Text == "-") { dc8 = 0; }
                else { dc8 = Convert.ToDouble(sizyzczd.Text); }
                if (wzzzczd.Text == "" || wzzzczd.Text == "-") { dc9 = 0; }
                else { dc9 = Convert.ToDouble(wzzzczd.Text); }
                if (wzyzczd.Text == "" || wzyzczd.Text == "-") { dc10 = 0; }
                else { dc10 = Convert.ToDouble(wzyzczd.Text); }
                if (lzzzczd.Text == "" || lzzzczd.Text == "-") { dc11 = 0; }
                else { dc11 = Convert.ToDouble(lzzzczd.Text); }
                if (lzyzczd.Text == "" || lzyzczd.Text == "-") { dc12 = 0; }
                else { dc12 = Convert.ToDouble(lzyzczd.Text); }
                dc13 = Convert.ToDouble(TextsIsnull(yzzlh.Text)) + Convert.ToDouble(TextsIsnull(yzylh.Text)) + Convert.ToDouble(TextsIsnull(ezzlh.Text)) + Convert.ToDouble(TextsIsnull(ezylh.Text)) + Convert.ToDouble(TextsIsnull(szzlh.Text)) + Convert.ToDouble(TextsIsnull(szylh.Text)) + Convert.ToDouble(TextsIsnull(sizzlh.Text)) + Convert.ToDouble(TextsIsnull(sizylh.Text)) + Convert.ToDouble(TextsIsnull(wzzlh.Text)) + Convert.ToDouble(TextsIsnull(wzylh.Text)) + Convert.ToDouble(TextsIsnull(lzzlh.Text)) + Convert.ToDouble(TextsIsnull(lzylh.Text));
                double n = dc1 + dc2 + dc3 + dc4 + dc5 + dc6 + dc7 + dc8 + dc9 + dc10 + dc11 + dc12;//所有驻车制动力之和
                double dszs = dc13 ;
                if (dszs != 0 && dcspcz.Text != "0")
                {
                    dczczdl.Text = ((n / dszs) * 100).ToString("0.0");//整车驻车制动率
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //四轴左驻车制动力发生改变
        private void sizzzczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double dc1 = 0;
                double dc2 = 0;
                double dc3 = 0;
                double dc4 = 0;
                double dc5 = 0;
                double dc6 = 0;
                double dc7 = 0;
                double dc8 = 0;
                double dc9 = 0;
                double dc10 = 0;
                double dc11 = 0;
                double dc12 = 0;
                double dc13 = 0;
                if (yzzzczd.Text == "" || yzzzczd.Text == "-") { dc1 = 0; }
                else { dc1 = Convert.ToDouble(yzzzczd.Text); }
                if (yzyzczd.Text == "" || yzyzczd.Text == "-") { dc2 = 0; }
                else { dc2 = Convert.ToDouble(yzyzczd.Text); }
                if (ezzzczd.Text == "" || ezzzczd.Text == "-") { dc3 = 0; }
                else { dc3 = Convert.ToDouble(ezzzczd.Text); }
                if (ezyzczd.Text == "" || ezyzczd.Text == "-") { dc4 = 0; }
                else { dc4 = Convert.ToDouble(ezyzczd.Text); }
                if (szzzczd.Text == "" || szzzczd.Text == "-") { dc5 = 0; }
                else { dc5 = Convert.ToDouble(szzzczd.Text); }
                if (szyzczd.Text == "" || szyzczd.Text == "-") { dc6 = 0; }
                else { dc6 = Convert.ToDouble(szyzczd.Text); }
                if (sizzzczd.Text == "" || sizzzczd.Text == "-") { dc7 = 0; }
                else { dc7 = Convert.ToDouble(sizzzczd.Text); }
                if (sizyzczd.Text == "" || sizyzczd.Text == "-") { dc8 = 0; }
                else { dc8 = Convert.ToDouble(sizyzczd.Text); }
                if (wzzzczd.Text == "" || wzzzczd.Text == "-") { dc9 = 0; }
                else { dc9 = Convert.ToDouble(wzzzczd.Text); }
                if (wzyzczd.Text == "" || wzyzczd.Text == "-") { dc10 = 0; }
                else { dc10 = Convert.ToDouble(wzyzczd.Text); }
                if (lzzzczd.Text == "" || lzzzczd.Text == "-") { dc11 = 0; }
                else { dc11 = Convert.ToDouble(lzzzczd.Text); }
                if (lzyzczd.Text == "" || lzyzczd.Text == "-") { dc12 = 0; }
                else { dc12 = Convert.ToDouble(lzyzczd.Text); }
                dc13 = Convert.ToDouble(TextsIsnull(yzzlh.Text)) + Convert.ToDouble(TextsIsnull(yzylh.Text)) + Convert.ToDouble(TextsIsnull(ezzlh.Text)) + Convert.ToDouble(TextsIsnull(ezylh.Text)) + Convert.ToDouble(TextsIsnull(szzlh.Text)) + Convert.ToDouble(TextsIsnull(szylh.Text)) + Convert.ToDouble(TextsIsnull(sizzlh.Text)) + Convert.ToDouble(TextsIsnull(sizylh.Text)) + Convert.ToDouble(TextsIsnull(wzzlh.Text)) + Convert.ToDouble(TextsIsnull(wzylh.Text)) + Convert.ToDouble(TextsIsnull(lzzlh.Text)) + Convert.ToDouble(TextsIsnull(lzylh.Text));
                double n = dc1 + dc2 + dc3 + dc4 + dc5 + dc6 + dc7 + dc8 + dc9 + dc10 + dc11 + dc12;//所有驻车制动力之和
                double dszs = dc13 ;
                if (dszs != 0 && dcspcz.Text != "0")
                {
                    dczczdl.Text = ((n / dszs) * 100).ToString("0.0");//整车驻车制动率
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //四轴右驻车制动力发生改变
        private void sizyzczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double dc1 = 0;
                double dc2 = 0;
                double dc3 = 0;
                double dc4 = 0;
                double dc5 = 0;
                double dc6 = 0;
                double dc7 = 0;
                double dc8 = 0;
                double dc9 = 0;
                double dc10 = 0;
                double dc11 = 0;
                double dc12 = 0;
                double dc13 = 0;
                if (yzzzczd.Text == "" || yzzzczd.Text == "-") { dc1 = 0; }
                else { dc1 = Convert.ToDouble(yzzzczd.Text); }
                if (yzyzczd.Text == "" || yzyzczd.Text == "-") { dc2 = 0; }
                else { dc2 = Convert.ToDouble(yzyzczd.Text); }
                if (ezzzczd.Text == "" || ezzzczd.Text == "-") { dc3 = 0; }
                else { dc3 = Convert.ToDouble(ezzzczd.Text); }
                if (ezyzczd.Text == "" || ezyzczd.Text == "-") { dc4 = 0; }
                else { dc4 = Convert.ToDouble(ezyzczd.Text); }
                if (szzzczd.Text == "" || szzzczd.Text == "-") { dc5 = 0; }
                else { dc5 = Convert.ToDouble(szzzczd.Text); }
                if (szyzczd.Text == "" || szyzczd.Text == "-") { dc6 = 0; }
                else { dc6 = Convert.ToDouble(szyzczd.Text); }
                if (sizzzczd.Text == "" || sizzzczd.Text == "-") { dc7 = 0; }
                else { dc7 = Convert.ToDouble(sizzzczd.Text); }
                if (sizyzczd.Text == "" || sizyzczd.Text == "-") { dc8 = 0; }
                else { dc8 = Convert.ToDouble(sizyzczd.Text); }
                if (wzzzczd.Text == "" || wzzzczd.Text == "-") { dc9 = 0; }
                else { dc9 = Convert.ToDouble(wzzzczd.Text); }
                if (wzyzczd.Text == "" || wzyzczd.Text == "-") { dc10 = 0; }
                else { dc10 = Convert.ToDouble(wzyzczd.Text); }
                if (lzzzczd.Text == "" || lzzzczd.Text == "-") { dc11 = 0; }
                else { dc11 = Convert.ToDouble(lzzzczd.Text); }
                if (lzyzczd.Text == "" || lzyzczd.Text == "-") { dc12 = 0; }
                else { dc12 = Convert.ToDouble(lzyzczd.Text); }
                dc13 = Convert.ToDouble(TextsIsnull(yzzlh.Text)) + Convert.ToDouble(TextsIsnull(yzylh.Text)) + Convert.ToDouble(TextsIsnull(ezzlh.Text)) + Convert.ToDouble(TextsIsnull(ezylh.Text)) + Convert.ToDouble(TextsIsnull(szzlh.Text)) + Convert.ToDouble(TextsIsnull(szylh.Text)) + Convert.ToDouble(TextsIsnull(sizzlh.Text)) + Convert.ToDouble(TextsIsnull(sizylh.Text)) + Convert.ToDouble(TextsIsnull(wzzlh.Text)) + Convert.ToDouble(TextsIsnull(wzylh.Text)) + Convert.ToDouble(TextsIsnull(lzzlh.Text)) + Convert.ToDouble(TextsIsnull(lzylh.Text));
                double n = dc1 + dc2 + dc3 + dc4 + dc5 + dc6 + dc7 + dc8 + dc9 + dc10 + dc11 + dc12;//所有驻车制动力之和
                double dszs = dc13;
                if (dszs != 0 && dcspcz.Text != "0")
                {
                    dczczdl.Text = ((n / dszs) * 100).ToString("0.0");//整车驻车制动率
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //五轴左驻车制动力发生改变
        private void wzzzczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double dc1 = 0;
                double dc2 = 0;
                double dc3 = 0;
                double dc4 = 0;
                double dc5 = 0;
                double dc6 = 0;
                double dc7 = 0;
                double dc8 = 0;
                double dc9 = 0;
                double dc10 = 0;
                double dc11 = 0;
                double dc12 = 0;
                double dc13 = 0;
                if (yzzzczd.Text == "" || yzzzczd.Text == "-") { dc1 = 0; }
                else { dc1 = Convert.ToDouble(yzzzczd.Text); }
                if (yzyzczd.Text == "" || yzyzczd.Text == "-") { dc2 = 0; }
                else { dc2 = Convert.ToDouble(yzyzczd.Text); }
                if (ezzzczd.Text == "" || ezzzczd.Text == "-") { dc3 = 0; }
                else { dc3 = Convert.ToDouble(ezzzczd.Text); }
                if (ezyzczd.Text == "" || ezyzczd.Text == "-") { dc4 = 0; }
                else { dc4 = Convert.ToDouble(ezyzczd.Text); }
                if (szzzczd.Text == "" || szzzczd.Text == "-") { dc5 = 0; }
                else { dc5 = Convert.ToDouble(szzzczd.Text); }
                if (szyzczd.Text == "" || szyzczd.Text == "-") { dc6 = 0; }
                else { dc6 = Convert.ToDouble(szyzczd.Text); }
                if (sizzzczd.Text == "" || sizzzczd.Text == "-") { dc7 = 0; }
                else { dc7 = Convert.ToDouble(sizzzczd.Text); }
                if (sizyzczd.Text == "" || sizyzczd.Text == "-") { dc8 = 0; }
                else { dc8 = Convert.ToDouble(sizyzczd.Text); }
                if (wzzzczd.Text == "" || wzzzczd.Text == "-") { dc9 = 0; }
                else { dc9 = Convert.ToDouble(wzzzczd.Text); }
                if (wzyzczd.Text == "" || wzyzczd.Text == "-") { dc10 = 0; }
                else { dc10 = Convert.ToDouble(wzyzczd.Text); }
                if (lzzzczd.Text == "" || lzzzczd.Text == "-") { dc11 = 0; }
                else { dc11 = Convert.ToDouble(lzzzczd.Text); }
                if (lzyzczd.Text == "" || lzyzczd.Text == "-") { dc12 = 0; }
                else { dc12 = Convert.ToDouble(lzyzczd.Text); }
                dc13 = Convert.ToDouble(TextsIsnull(yzzlh.Text)) + Convert.ToDouble(TextsIsnull(yzylh.Text)) + Convert.ToDouble(TextsIsnull(ezzlh.Text)) + Convert.ToDouble(TextsIsnull(ezylh.Text)) + Convert.ToDouble(TextsIsnull(szzlh.Text)) + Convert.ToDouble(TextsIsnull(szylh.Text)) + Convert.ToDouble(TextsIsnull(sizzlh.Text)) + Convert.ToDouble(TextsIsnull(sizylh.Text)) + Convert.ToDouble(TextsIsnull(wzzlh.Text)) + Convert.ToDouble(TextsIsnull(wzylh.Text)) + Convert.ToDouble(TextsIsnull(lzzlh.Text)) + Convert.ToDouble(TextsIsnull(lzylh.Text));
                double n = dc1 + dc2 + dc3 + dc4 + dc5 + dc6 + dc7 + dc8 + dc9 + dc10 + dc11 + dc12;//所有驻车制动力之和
                double dszs = dc13;
                if (dszs != 0 && dcspcz.Text != "0")
                {
                    dczczdl.Text = ((n / dszs) * 100).ToString("0.0");//整车驻车制动率
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //五轴右驻车制动力发生改变
        private void wzyzczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double dc1 = 0;
                double dc2 = 0;
                double dc3 = 0;
                double dc4 = 0;
                double dc5 = 0;
                double dc6 = 0;
                double dc7 = 0;
                double dc8 = 0;
                double dc9 = 0;
                double dc10 = 0;
                double dc11 = 0;
                double dc12 = 0;
                double dc13 = 0;
                if (yzzzczd.Text == "" || yzzzczd.Text == "-") { dc1 = 0; }
                else { dc1 = Convert.ToDouble(yzzzczd.Text); }
                if (yzyzczd.Text == "" || yzyzczd.Text == "-") { dc2 = 0; }
                else { dc2 = Convert.ToDouble(yzyzczd.Text); }
                if (ezzzczd.Text == "" || ezzzczd.Text == "-") { dc3 = 0; }
                else { dc3 = Convert.ToDouble(ezzzczd.Text); }
                if (ezyzczd.Text == "" || ezyzczd.Text == "-") { dc4 = 0; }
                else { dc4 = Convert.ToDouble(ezyzczd.Text); }
                if (szzzczd.Text == "" || szzzczd.Text == "-") { dc5 = 0; }
                else { dc5 = Convert.ToDouble(szzzczd.Text); }
                if (szyzczd.Text == "" || szyzczd.Text == "-") { dc6 = 0; }
                else { dc6 = Convert.ToDouble(szyzczd.Text); }
                if (sizzzczd.Text == "" || sizzzczd.Text == "-") { dc7 = 0; }
                else { dc7 = Convert.ToDouble(sizzzczd.Text); }
                if (sizyzczd.Text == "" || sizyzczd.Text == "-") { dc8 = 0; }
                else { dc8 = Convert.ToDouble(sizyzczd.Text); }
                if (wzzzczd.Text == "" || wzzzczd.Text == "-") { dc9 = 0; }
                else { dc9 = Convert.ToDouble(wzzzczd.Text); }
                if (wzyzczd.Text == "" || wzyzczd.Text == "-") { dc10 = 0; }
                else { dc10 = Convert.ToDouble(wzyzczd.Text); }
                if (lzzzczd.Text == "" || lzzzczd.Text == "-") { dc11 = 0; }
                else { dc11 = Convert.ToDouble(lzzzczd.Text); }
                if (lzyzczd.Text == "" || lzyzczd.Text == "-") { dc12 = 0; }
                else { dc12 = Convert.ToDouble(lzyzczd.Text); }
                dc13 = Convert.ToDouble(TextsIsnull(yzzlh.Text)) + Convert.ToDouble(TextsIsnull(yzylh.Text)) + Convert.ToDouble(TextsIsnull(ezzlh.Text)) + Convert.ToDouble(TextsIsnull(ezylh.Text)) + Convert.ToDouble(TextsIsnull(szzlh.Text)) + Convert.ToDouble(TextsIsnull(szylh.Text)) + Convert.ToDouble(TextsIsnull(sizzlh.Text)) + Convert.ToDouble(TextsIsnull(sizylh.Text)) + Convert.ToDouble(TextsIsnull(wzzlh.Text)) + Convert.ToDouble(TextsIsnull(wzylh.Text)) + Convert.ToDouble(TextsIsnull(lzzlh.Text)) + Convert.ToDouble(TextsIsnull(lzylh.Text));
                double n = dc1 + dc2 + dc3 + dc4 + dc5 + dc6 + dc7 + dc8 + dc9 + dc10 + dc11 + dc12;//所有驻车制动力之和
                double dszs = dc13;
                if (dszs != 0 && dcspcz.Text != "0")
                {
                    dczczdl.Text = ((n / dszs) * 100).ToString("0.0");//整车驻车制动率
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //六轴左驻车制动力发生改变
        private void lzzzczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double dc1 = 0;
                double dc2 = 0;
                double dc3 = 0;
                double dc4 = 0;
                double dc5 = 0;
                double dc6 = 0;
                double dc7 = 0;
                double dc8 = 0;
                double dc9 = 0;
                double dc10 = 0;
                double dc11 = 0;
                double dc12 = 0;
                double dc13 = 0;
                if (yzzzczd.Text == "" || yzzzczd.Text == "-") { dc1 = 0; }
                else { dc1 = Convert.ToDouble(yzzzczd.Text); }
                if (yzyzczd.Text == "" || yzyzczd.Text == "-") { dc2 = 0; }
                else { dc2 = Convert.ToDouble(yzyzczd.Text); }
                if (ezzzczd.Text == "" || ezzzczd.Text == "-") { dc3 = 0; }
                else { dc3 = Convert.ToDouble(ezzzczd.Text); }
                if (ezyzczd.Text == "" || ezyzczd.Text == "-") { dc4 = 0; }
                else { dc4 = Convert.ToDouble(ezyzczd.Text); }
                if (szzzczd.Text == "" || szzzczd.Text == "-") { dc5 = 0; }
                else { dc5 = Convert.ToDouble(szzzczd.Text); }
                if (szyzczd.Text == "" || szyzczd.Text == "-") { dc6 = 0; }
                else { dc6 = Convert.ToDouble(szyzczd.Text); }
                if (sizzzczd.Text == "" || sizzzczd.Text == "-") { dc7 = 0; }
                else { dc7 = Convert.ToDouble(sizzzczd.Text); }
                if (sizyzczd.Text == "" || sizyzczd.Text == "-") { dc8 = 0; }
                else { dc8 = Convert.ToDouble(sizyzczd.Text); }
                if (wzzzczd.Text == "" || wzzzczd.Text == "-") { dc9 = 0; }
                else { dc9 = Convert.ToDouble(wzzzczd.Text); }
                if (wzyzczd.Text == "" || wzyzczd.Text == "-") { dc10 = 0; }
                else { dc10 = Convert.ToDouble(wzyzczd.Text); }
                if (lzzzczd.Text == "" || lzzzczd.Text == "-") { dc11 = 0; }
                else { dc11 = Convert.ToDouble(lzzzczd.Text); }
                if (lzyzczd.Text == "" || lzyzczd.Text == "-") { dc12 = 0; }
                else { dc12 = Convert.ToDouble(lzyzczd.Text); }
                dc13 = Convert.ToDouble(TextsIsnull(yzzlh.Text)) + Convert.ToDouble(TextsIsnull(yzylh.Text)) + Convert.ToDouble(TextsIsnull(ezzlh.Text)) + Convert.ToDouble(TextsIsnull(ezylh.Text)) + Convert.ToDouble(TextsIsnull(szzlh.Text)) + Convert.ToDouble(TextsIsnull(szylh.Text)) + Convert.ToDouble(TextsIsnull(sizzlh.Text)) + Convert.ToDouble(TextsIsnull(sizylh.Text)) + Convert.ToDouble(TextsIsnull(wzzlh.Text)) + Convert.ToDouble(TextsIsnull(wzylh.Text)) + Convert.ToDouble(TextsIsnull(lzzlh.Text)) + Convert.ToDouble(TextsIsnull(lzylh.Text));
                double n = dc1 + dc2 + dc3 + dc4 + dc5 + dc6 + dc7 + dc8 + dc9 + dc10 + dc11 + dc12;//所有驻车制动力之和
                double dszs = dc13 ;
                if (dszs != 0 && dcspcz.Text != "0")
                {
                    dczczdl.Text = ((n / dszs) * 100).ToString("0.0");//整车驻车制动率
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //六轴右驻车制动力发生改变
        private void lzyzczd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double dc1 = 0;
                double dc2 = 0;
                double dc3 = 0;
                double dc4 = 0;
                double dc5 = 0;
                double dc6 = 0;
                double dc7 = 0;
                double dc8 = 0;
                double dc9 = 0;
                double dc10 = 0;
                double dc11 = 0;
                double dc12 = 0;
                double dc13 = 0;
                if (yzzzczd.Text == "" || yzzzczd.Text == "-") { dc1 = 0; }
                else { dc1 = Convert.ToDouble(yzzzczd.Text); }
                if (yzyzczd.Text == "" || yzyzczd.Text == "-") { dc2 = 0; }
                else { dc2 = Convert.ToDouble(yzyzczd.Text); }
                if (ezzzczd.Text == "" || ezzzczd.Text == "-") { dc3 = 0; }
                else { dc3 = Convert.ToDouble(ezzzczd.Text); }
                if (ezyzczd.Text == "" || ezyzczd.Text == "-") { dc4 = 0; }
                else { dc4 = Convert.ToDouble(ezyzczd.Text); }
                if (szzzczd.Text == "" || szzzczd.Text == "-") { dc5 = 0; }
                else { dc5 = Convert.ToDouble(szzzczd.Text); }
                if (szyzczd.Text == "" || szyzczd.Text == "-") { dc6 = 0; }
                else { dc6 = Convert.ToDouble(szyzczd.Text); }
                if (sizzzczd.Text == "" || sizzzczd.Text == "-") { dc7 = 0; }
                else { dc7 = Convert.ToDouble(sizzzczd.Text); }
                if (sizyzczd.Text == "" || sizyzczd.Text == "-") { dc8 = 0; }
                else { dc8 = Convert.ToDouble(sizyzczd.Text); }
                if (wzzzczd.Text == "" || wzzzczd.Text == "-") { dc9 = 0; }
                else { dc9 = Convert.ToDouble(wzzzczd.Text); }
                if (wzyzczd.Text == "" || wzyzczd.Text == "-") { dc10 = 0; }
                else { dc10 = Convert.ToDouble(wzyzczd.Text); }
                if (lzzzczd.Text == "" || lzzzczd.Text == "-") { dc11 = 0; }
                else { dc11 = Convert.ToDouble(lzzzczd.Text); }
                if (lzyzczd.Text == "" || lzyzczd.Text == "-") { dc12 = 0; }
                else { dc12 = Convert.ToDouble(lzyzczd.Text); }
                dc13 = Convert.ToDouble(TextsIsnull(yzzlh.Text)) + Convert.ToDouble(TextsIsnull(yzylh.Text)) + Convert.ToDouble(TextsIsnull(ezzlh.Text)) + Convert.ToDouble(TextsIsnull(ezylh.Text)) + Convert.ToDouble(TextsIsnull(szzlh.Text)) + Convert.ToDouble(TextsIsnull(szylh.Text)) + Convert.ToDouble(TextsIsnull(sizzlh.Text)) + Convert.ToDouble(TextsIsnull(sizylh.Text)) + Convert.ToDouble(TextsIsnull(wzzlh.Text)) + Convert.ToDouble(TextsIsnull(wzylh.Text)) + Convert.ToDouble(TextsIsnull(lzzlh.Text)) + Convert.ToDouble(TextsIsnull(lzylh.Text));
                double n = dc1 + dc2 + dc3 + dc4 + dc5 + dc6 + dc7 + dc8 + dc9 + dc10 + dc11 + dc12;//所有驻车制动力之和
                double dszs = dc13 ;
                if (dszs != 0 && dcspcz.Text != "0")
                {
                    dczczdl.Text = ((n / dszs) * 100).ToString("0.0");//整车驻车制动率
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
        
        public static object Issf1(string aa)
        {
            if (aa == "1")
            {
                aa = "合格";
            }
            if (aa == "0")
            {
                aa = "不合格";
            }
            return aa;
        }
        //一级车的选择
        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2.Checked = false;
            }
        }
        //二级车的选择
        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox1.Checked = false;
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            CLDEPD();
        } 
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CLDEPD();
        }

        #region 每个轴轴重的计算
        //一轴左轮荷发生改变
        private void yzzlh_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (yzzxczd.Text != "" && yzzxczd.Text != "-" && yzyxczd.Text != "" && yzyxczd.Text != "-")
                {
                    if (yzzlh.Text != "" && yzzlh.Text != "-" && yzzlh.Text != "0" && yzylh.Text != "" && yzylh.Text != "-" && yzylh.Text != "0")
                    {
                        //一轴制动率
                        if (Convert.ToDouble(TextIsnulls(dczs.Text))<3)
                        {
                            ydzzdv.Text = ((Convert.ToDouble(yzzxczd.Text) + Convert.ToDouble(yzyxczd.Text)) / (Convert.ToDouble(yzzlh.Text) + Convert.ToDouble(yzylh.Text)) * 100).ToString("0.0");
                        }
                    }
                }
                //整车静态轴重
                dcspcz.Text = (Convert.ToDouble(TextIsnulls(yzzlh.Text)) + Convert.ToDouble(TextIsnulls(yzylh.Text)) + Convert.ToDouble(TextIsnulls(ezzlh.Text)) + Convert.ToDouble(TextIsnulls(ezylh.Text)) + Convert.ToDouble(TextIsnulls(szzlh.Text)) + Convert.ToDouble(TextIsnulls(szylh.Text)) + Convert.ToDouble(TextIsnulls(sizzlh.Text)) + Convert.ToDouble(TextIsnulls(sizylh.Text)) + Convert.ToDouble(TextIsnulls(wzzlh.Text)) + Convert.ToDouble(TextIsnulls(wzylh.Text)) + Convert.ToDouble(TextIsnulls(lzzlh.Text)) + Convert.ToDouble(TextIsnulls(lzylh.Text))).ToString("0");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //一轴右轮荷发生改变
        private void yzylh_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (yzzxczd.Text != "" && yzzxczd.Text != "-" && yzyxczd.Text != "" && yzyxczd.Text != "-")
                {
                    if (yzzlh.Text != "" && yzzlh.Text != "-" && yzzlh.Text != "0" && yzylh.Text != "" && yzylh.Text != "-" && yzylh.Text != "0")
                    {
                        //一轴制动率
                        if (Convert.ToDouble(TextIsnulls(dczs.Text)) < 3)
                        {
                            ydzzdv.Text = ((Convert.ToDouble(yzzxczd.Text) + Convert.ToDouble(yzyxczd.Text)) / (Convert.ToDouble(yzzlh.Text) + Convert.ToDouble(yzylh.Text)) * 100).ToString("0.0");
                        }
                    }
                }
                //整车静态轴重
                dcspcz.Text = (Convert.ToDouble(TextIsnulls(yzzlh.Text)) + Convert.ToDouble(TextIsnulls(yzylh.Text)) + Convert.ToDouble(TextIsnulls(ezzlh.Text)) + Convert.ToDouble(TextIsnulls(ezylh.Text)) + Convert.ToDouble(TextIsnulls(szzlh.Text)) + Convert.ToDouble(TextIsnulls(szylh.Text)) + Convert.ToDouble(TextIsnulls(sizzlh.Text)) + Convert.ToDouble(TextIsnulls(sizylh.Text)) + Convert.ToDouble(TextIsnulls(wzzlh.Text)) + Convert.ToDouble(TextIsnulls(wzylh.Text)) + Convert.ToDouble(TextIsnulls(lzzlh.Text)) + Convert.ToDouble(TextIsnulls(lzylh.Text))).ToString("0");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //二轴左轮荷发生改变
        private void ezzlh_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (ezzxczd.Text != "" && ezzxczd.Text != "-" && ezyxczd.Text != "" && ezyxczd.Text != "-")
                {
                    if (ezzlh.Text != "" && ezzlh.Text != "-" && ezzlh.Text != "0" && ezylh.Text != "" && ezylh.Text != "-" && ezylh.Text != "0")
                    {
                        //二轴制动率
                        if (Convert.ToDouble(TextIsnulls(dczs.Text)) < 3)
                        { 
                        edzzdv.Text = ((Convert.ToDouble(ezzxczd.Text) + Convert.ToDouble(ezyxczd.Text)) / (Convert.ToDouble(ezzlh.Text) + Convert.ToDouble(ezylh.Text))  * 100).ToString("0.0");
                        }
                    }
                }
                //整车静态轴重
                dcspcz.Text = (Convert.ToDouble(TextIsnulls(yzzlh.Text)) + Convert.ToDouble(TextIsnulls(yzylh.Text)) + Convert.ToDouble(TextIsnulls(ezzlh.Text)) + Convert.ToDouble(TextIsnulls(ezylh.Text)) + Convert.ToDouble(TextIsnulls(szzlh.Text)) + Convert.ToDouble(TextIsnulls(szylh.Text)) + Convert.ToDouble(TextIsnulls(sizzlh.Text)) + Convert.ToDouble(TextIsnulls(sizylh.Text)) + Convert.ToDouble(TextIsnulls(wzzlh.Text)) + Convert.ToDouble(TextIsnulls(wzylh.Text)) + Convert.ToDouble(TextIsnulls(lzzlh.Text)) + Convert.ToDouble(TextIsnulls(lzylh.Text))).ToString("0");
                if (zxzs.Text != "2")
                {
                    //判断二轴制动率为不平衡率赋值
                    if (edzzdv.Text != "" && edzzdv.Text != "-")
                    {
                        //如果二轴制动率大于60
                        if (Convert.ToDouble(edzzdv.Text) < 60)
                        {
                            if (edzzgcc.Text != "" && edzzgcc.Text != "-" && edzygcc.Text != "" && edzygcc.Text != "-")
                            {
                                double i = System.Math.Abs(Convert.ToDouble(edzzgcc.Text) - Convert.ToDouble(edzygcc.Text));//过程差最大点之差
                                if (ezzxczd.Text != "" && ezzxczd.Text != "-" && ezzxczd.Text != "0" && ezyxczd.Text != "" && ezyxczd.Text != "-" && ezyxczd.Text != "0")
                                {
                                    if (ezzlh.Text != "" && ezzlh.Text != "-" && ezzlh.Text != "0" && ezylh.Text != "" && ezylh.Text != "-" && ezylh.Text != "0")
                                    {
                                        //为二轴不平衡率赋值
                                        edzbphv.Text = ((i / (Convert.ToDouble(ezzlh.Text) + Convert.ToDouble(ezylh.Text))) * 100).ToString("0.0");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //二轴右轮荷发生改变
        private void ezylh_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (ezzxczd.Text != "" && ezzxczd.Text != "-" && ezyxczd.Text != "" && ezyxczd.Text != "-")
                {
                    if (ezzlh.Text != "" && ezzlh.Text != "-" && ezzlh.Text != "0" && ezylh.Text != "" && ezylh.Text != "-" && ezylh.Text != "0")
                    {
                        //二轴制动率
                        if (Convert.ToDouble(TextIsnulls(dczs.Text)) < 3)
                        {
                            edzzdv.Text = ((Convert.ToDouble(ezzxczd.Text) + Convert.ToDouble(ezyxczd.Text)) / (Convert.ToDouble(ezzlh.Text) + Convert.ToDouble(ezylh.Text)) * 100).ToString("0.0");
                        }
                    }
                }
                //整车静态轴重
                dcspcz.Text = (Convert.ToDouble(TextIsnulls(yzzlh.Text)) + Convert.ToDouble(TextIsnulls(yzylh.Text)) + Convert.ToDouble(TextIsnulls(ezzlh.Text)) + Convert.ToDouble(TextIsnulls(ezylh.Text)) + Convert.ToDouble(TextIsnulls(szzlh.Text)) + Convert.ToDouble(TextIsnulls(szylh.Text)) + Convert.ToDouble(TextIsnulls(sizzlh.Text)) + Convert.ToDouble(TextIsnulls(sizylh.Text)) + Convert.ToDouble(TextIsnulls(wzzlh.Text)) + Convert.ToDouble(TextIsnulls(wzylh.Text)) + Convert.ToDouble(TextIsnulls(lzzlh.Text)) + Convert.ToDouble(TextIsnulls(lzylh.Text))).ToString("0");
                if (zxzs.Text != "2")
                {
                    //判断二轴制动率给不平衡率赋值
                    if (edzzdv.Text != "" && edzzdv.Text != "-")
                    {
                        //如果二轴制动率大于60
                        if (Convert.ToDouble(edzzdv.Text) < 60)
                        {
                            if (edzzgcc.Text != "" && edzzgcc.Text != "-" && edzygcc.Text != "" && edzygcc.Text != "-")
                            {
                                double i = System.Math.Abs(Convert.ToDouble(edzzgcc.Text) - Convert.ToDouble(edzygcc.Text));//过程差最大点之差
                                if (ezzxczd.Text != "" && ezzxczd.Text != "-" && ezzxczd.Text != "0" && ezyxczd.Text != "" && ezyxczd.Text != "-" && ezyxczd.Text != "0")
                                {
                                    if (ezzlh.Text != "" && ezzlh.Text != "-" && ezzlh.Text != "0" && ezylh.Text != "" && ezylh.Text != "-" && ezylh.Text != "0")
                                    {
                                        //为二轴不平衡率赋值
                                        edzbphv.Text = ((i / (Convert.ToDouble(ezzlh.Text) + Convert.ToDouble(ezylh.Text))) * 100).ToString("0.0");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //三轴左轮荷发生改变
        private void szzlh_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (szzxczd.Text != "" && szzxczd.Text != "-" && szyxczd.Text != "" && szyxczd.Text != "-")
                {
                    if (szzlh.Text != "" && szzlh.Text != "-" && szzlh.Text != "0" && szylh.Text != "" && szylh.Text != "-" && szylh.Text != "0")
                    {
                        //三轴制动率
                        if (Convert.ToDouble(TextIsnulls(dczs.Text)) < 3)
                        {
                            sdzzdv.Text = ((Convert.ToDouble(szzxczd.Text) + Convert.ToDouble(szyxczd.Text)) / (Convert.ToDouble(szzlh.Text) + Convert.ToDouble(szylh.Text)) * 100).ToString("0.0");
                        }
                    }
                }
                //整车静态轴重
                dcspcz.Text = (Convert.ToDouble(TextIsnulls(yzzlh.Text)) + Convert.ToDouble(TextIsnulls(yzylh.Text)) + Convert.ToDouble(TextIsnulls(ezzlh.Text)) + Convert.ToDouble(TextIsnulls(ezylh.Text)) + Convert.ToDouble(TextIsnulls(szzlh.Text)) + Convert.ToDouble(TextIsnulls(szylh.Text)) + Convert.ToDouble(TextIsnulls(sizzlh.Text)) + Convert.ToDouble(TextIsnulls(sizylh.Text)) + Convert.ToDouble(TextIsnulls(wzzlh.Text)) + Convert.ToDouble(TextIsnulls(wzylh.Text)) + Convert.ToDouble(TextIsnulls(lzzlh.Text)) + Convert.ToDouble(TextIsnulls(lzylh.Text))).ToString("0");
                //判断三轴制动率为不平衡率赋值
                if (sdzzdv.Text != "" && sdzzdv.Text != "-")
                {
                    //如果三轴制动率大于60
                    if (Convert.ToDouble(sdzzdv.Text) < 60)
                    {
                        if (sdzzgcc.Text != "" && sdzzgcc.Text != "-" && sdzygcc.Text != "" && sdzygcc.Text != "-")
                        {
                            double i = System.Math.Abs(Convert.ToDouble(sdzzgcc.Text) - Convert.ToDouble(sdzygcc.Text));//过程差最大点之差
                            if (szzxczd.Text != "" && szzxczd.Text != "-" && szzxczd.Text != "0" && szyxczd.Text != "" && szyxczd.Text != "-" && szyxczd.Text != "0")
                            {
                                if (szzlh.Text != "" && szzlh.Text != "-" && szzlh.Text != "0" && szylh.Text != "" && szylh.Text != "-" && szylh.Text != "0")
                                {
                                    //为三轴不平衡率赋值
                                    sdzbphv.Text = ((i / (Convert.ToDouble(szzlh.Text) + Convert.ToDouble(szylh.Text))) * 100).ToString("0.0");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //三轴右轮荷发生改变
        private void szylh_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (szzxczd.Text != "" && szzxczd.Text != "-" && szyxczd.Text != "" && szyxczd.Text != "-")
                {
                    if (szzlh.Text != "" && szzlh.Text != "-" && szzlh.Text != "0" && szylh.Text != "" && szylh.Text != "-" && szylh.Text != "0")
                    {
                        //三轴制动率
                        if (Convert.ToDouble(TextIsnulls(dczs.Text)) < 3)
                        {
                            sdzzdv.Text = ((Convert.ToDouble(szzxczd.Text) + Convert.ToDouble(szyxczd.Text)) / (Convert.ToDouble(szzlh.Text) + Convert.ToDouble(szylh.Text)) * 100).ToString("0.0");
                        }
                    }
                }
                //整车静态轴重
                dcspcz.Text = (Convert.ToDouble(TextIsnulls(yzzlh.Text)) + Convert.ToDouble(TextIsnulls(yzylh.Text)) + Convert.ToDouble(TextIsnulls(ezzlh.Text)) + Convert.ToDouble(TextIsnulls(ezylh.Text)) + Convert.ToDouble(TextIsnulls(szzlh.Text)) + Convert.ToDouble(TextIsnulls(szylh.Text)) + Convert.ToDouble(TextIsnulls(sizzlh.Text)) + Convert.ToDouble(TextIsnulls(sizylh.Text)) + Convert.ToDouble(TextIsnulls(wzzlh.Text)) + Convert.ToDouble(TextIsnulls(wzylh.Text)) + Convert.ToDouble(TextIsnulls(lzzlh.Text)) + Convert.ToDouble(TextIsnulls(lzylh.Text))).ToString("0");
                //判断三轴制动率给不平衡率赋值
                if (sdzzdv.Text != "" && sdzzdv.Text != "-")
                {
                    //如果三轴制动率大于60
                    if (Convert.ToDouble(sdzzdv.Text) < 60)
                    {
                        if (sdzzgcc.Text != "" && sdzzgcc.Text != "-" && sdzygcc.Text != "" && sdzygcc.Text != "-")
                        {
                            double i = System.Math.Abs(Convert.ToDouble(sdzzgcc.Text) - Convert.ToDouble(sdzygcc.Text));//过程差最大点之差
                            if (szzxczd.Text != "" && szzxczd.Text != "-" && szzxczd.Text != "0" && szyxczd.Text != "" && szyxczd.Text != "-" && szyxczd.Text != "0")
                            {
                                if (szzlh.Text != "" && szzlh.Text != "-" && szzlh.Text != "0" && szylh.Text != "" && szylh.Text != "-" && szylh.Text != "0")
                                {
                                    //为三轴不平衡率赋值
                                    sdzbphv.Text = ((i / (Convert.ToDouble(szzlh.Text) + Convert.ToDouble(szylh.Text))) * 100).ToString("0.0");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //四轴左轮荷发生改变,
        private void sizzlh_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (sizzxczd.Text != "" && sizzxczd.Text != "-" && sizyxczd.Text != "" && sizyxczd.Text != "-")
                {
                    if (sizzlh.Text != "" && sizzlh.Text != "-" && sizzlh.Text != "0" && sizylh.Text != "" && sizylh.Text != "-" && sizylh.Text != "0")
                    {
                        //四轴制动率
                        if (Convert.ToDouble(TextIsnulls(dczs.Text)) < 3)
                        {
                            sidzzdv.Text = ((Convert.ToDouble(sizzxczd.Text) + Convert.ToDouble(sizyxczd.Text)) / (Convert.ToDouble(sizzlh.Text) + Convert.ToDouble(sizylh.Text)) * 100).ToString("0.0");
                        }
                    }
                }
                //整车静态轴重
                dcspcz.Text = (Convert.ToDouble(TextIsnulls(yzzlh.Text)) + Convert.ToDouble(TextIsnulls(yzylh.Text)) + Convert.ToDouble(TextIsnulls(ezzlh.Text)) + Convert.ToDouble(TextIsnulls(ezylh.Text)) + Convert.ToDouble(TextIsnulls(szzlh.Text)) + Convert.ToDouble(TextIsnulls(szylh.Text)) + Convert.ToDouble(TextIsnulls(sizzlh.Text)) + Convert.ToDouble(TextIsnulls(sizylh.Text)) + Convert.ToDouble(TextIsnulls(wzzlh.Text)) + Convert.ToDouble(TextIsnulls(wzylh.Text)) + Convert.ToDouble(TextIsnulls(lzzlh.Text)) + Convert.ToDouble(TextIsnulls(lzylh.Text))).ToString("0");
                //判断四轴制动率给不平衡率赋值
                if (sidzzdv.Text != "" && sidzzdv.Text != "-")
                {
                    //如果四轴制动率大于60
                    if (Convert.ToDouble(sidzzdv.Text) < 60)
                    {
                        if (sidzzgcc.Text != "" && sidzzgcc.Text != "-" && sidzygcc.Text != "" && sidzygcc.Text != "-")
                        {
                            double i = System.Math.Abs(Convert.ToDouble(sidzzgcc.Text) - Convert.ToDouble(sidzygcc.Text));//过程差最大点之差
                            if (sizzxczd.Text != "" && sizzxczd.Text != "-" && sizzxczd.Text != "0" && sizyxczd.Text != "" && sizyxczd.Text != "-" && sizyxczd.Text != "0")
                            {
                                if (sizzlh.Text != "" && sizzlh.Text != "-" && sizzlh.Text != "0" && sizylh.Text != "" && sizylh.Text != "-" && sizylh.Text != "0")
                                {
                                    //为四轴不平衡率赋值
                                    sidzbphv.Text = ((i / (Convert.ToDouble(sizzlh.Text) + Convert.ToDouble(sizylh.Text))) * 100).ToString("0.0");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //四轴右轮荷发生改变
        private void sizylh_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (sizzxczd.Text != "" && sizzxczd.Text != "-" && sizyxczd.Text != "" && sizyxczd.Text != "-")
                {
                    if (sizzlh.Text != "" && sizzlh.Text != "-" && sizzlh.Text != "0" && sizylh.Text != "" && sizylh.Text != "-" && sizylh.Text != "0")
                    {
                        //四轴制动率
                        if (Convert.ToDouble(TextIsnulls(dczs.Text)) < 3)
                        {
                            sidzzdv.Text = ((Convert.ToDouble(sizzxczd.Text) + Convert.ToDouble(sizyxczd.Text)) / (Convert.ToDouble(sizzlh.Text) + Convert.ToDouble(sizylh.Text)) * 100).ToString("0.0");
                        }
                    }
                }
                //整车静态轴重
                dcspcz.Text = (Convert.ToDouble(TextIsnulls(yzzlh.Text)) + Convert.ToDouble(TextIsnulls(yzylh.Text)) + Convert.ToDouble(TextIsnulls(ezzlh.Text)) + Convert.ToDouble(TextIsnulls(ezylh.Text)) + Convert.ToDouble(TextIsnulls(szzlh.Text)) + Convert.ToDouble(TextIsnulls(szylh.Text)) + Convert.ToDouble(TextIsnulls(sizzlh.Text)) + Convert.ToDouble(TextIsnulls(sizylh.Text)) + Convert.ToDouble(TextIsnulls(wzzlh.Text)) + Convert.ToDouble(TextIsnulls(wzylh.Text)) + Convert.ToDouble(TextIsnulls(lzzlh.Text)) + Convert.ToDouble(TextIsnulls(lzylh.Text))).ToString("0");
                //判断四轴制动率为不平衡率赋值
                if (sidzzdv.Text != "" && sidzzdv.Text != "-")
                {
                    //如果四轴制动率大于60
                    if (Convert.ToDouble(sidzzdv.Text) < 60)
                    {
                        if (sidzzgcc.Text != "" && sidzzgcc.Text != "-" && sidzygcc.Text != "" && sidzygcc.Text != "-")
                        {
                            double i = System.Math.Abs(Convert.ToDouble(sidzzgcc.Text) - Convert.ToDouble(sidzygcc.Text));//过程差最大点之差
                            if (sizzxczd.Text != "" && sizzxczd.Text != "-" && sizzxczd.Text != "0" && sizyxczd.Text != "" && sizyxczd.Text != "-" && sizyxczd.Text != "0")
                            {
                                if (sizzlh.Text != "" && sizzlh.Text != "-" && sizzlh.Text != "0" && sizylh.Text != "" && sizylh.Text != "-" && sizylh.Text != "0")
                                {
                                    //为四轴不平衡率赋值
                                    sidzbphv.Text = ((i / (Convert.ToDouble(sizzlh.Text) + Convert.ToDouble(sizylh.Text))) * 100).ToString("0.0");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //五轴左轮荷发生改变
        private void wzzlh_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (wzzxczd.Text != "" && wzzxczd.Text != "-" && wzyxczd.Text != "" && wzyxczd.Text != "-")
                {
                    if (wzzlh.Text != "" && wzzlh.Text != "-" && wzzlh.Text != "0" && wzylh.Text != "" && wzylh.Text != "-" && wzylh.Text != "0")
                    {
                        //五轴制动率
                        if (Convert.ToDouble(TextIsnulls(dczs.Text)) < 3)
                        {
                            wdzzdv.Text = ((Convert.ToDouble(wzzxczd.Text) + Convert.ToDouble(wzyxczd.Text)) / (Convert.ToDouble(wzzlh.Text) + Convert.ToDouble(wzylh.Text)) * 100).ToString("0.0");
                        }
                    }
                }
                //整车静态轴重
                dcspcz.Text = (Convert.ToDouble(TextIsnulls(yzzlh.Text)) + Convert.ToDouble(TextIsnulls(yzylh.Text)) + Convert.ToDouble(TextIsnulls(ezzlh.Text)) + Convert.ToDouble(TextIsnulls(ezylh.Text)) + Convert.ToDouble(TextIsnulls(szzlh.Text)) + Convert.ToDouble(TextIsnulls(szylh.Text)) + Convert.ToDouble(TextIsnulls(sizzlh.Text)) + Convert.ToDouble(TextIsnulls(sizylh.Text)) + Convert.ToDouble(TextIsnulls(wzzlh.Text)) + Convert.ToDouble(TextIsnulls(wzylh.Text)) + Convert.ToDouble(TextIsnulls(lzzlh.Text)) + Convert.ToDouble(TextIsnulls(lzylh.Text))).ToString("0");
                //判断五轴制动率为不平衡率赋值
                if (wdzzdv.Text != "" && wdzzdv.Text != "-")
                {
                    //如果五轴制动率大于60
                    if (Convert.ToDouble(wdzzdv.Text) < 60)
                    {
                        if (wdzzgcc.Text != "" && wdzzgcc.Text != "-" && wdzygcc.Text != "" && wdzygcc.Text != "-")
                        {
                            double i = System.Math.Abs(Convert.ToDouble(wdzzgcc.Text) - Convert.ToDouble(wdzygcc.Text));//过程差最大点之差
                            if (wzzxczd.Text != "" && wzzxczd.Text != "-" && wzzxczd.Text != "0" && wzyxczd.Text != "" && wzyxczd.Text != "" && wzyxczd.Text != "0")
                            {
                                if (wzzlh.Text != "" && wzzlh.Text != "-" && wzzlh.Text != "0" && wzylh.Text != "" && wzylh.Text != "-" && wzylh.Text != "0")
                                {
                                    //为五轴不平衡率赋值
                                    wdzbphv.Text = ((i / (Convert.ToDouble(wzzlh.Text) + Convert.ToDouble(wzylh.Text))) * 100).ToString("0.0");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //五轴右轮荷发生改变
        private void wzylh_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (wzzxczd.Text != "" && wzzxczd.Text != "-" && wzyxczd.Text != "" && wzyxczd.Text != "-")
                {
                    if (wzzlh.Text != "" && wzzlh.Text != "-" && wzzlh.Text != "0" && wzylh.Text != "" && wzylh.Text != "-" && wzylh.Text != "0")
                    {
                        //五轴制动率
                        if (Convert.ToDouble(TextIsnulls(dczs.Text)) < 3)
                        {
                            wdzzdv.Text = ((Convert.ToDouble(wzzxczd.Text) + Convert.ToDouble(wzyxczd.Text)) / (Convert.ToDouble(wzzlh.Text) + Convert.ToDouble(wzylh.Text)) * 100).ToString("0.0");
                        }
                    }
                }
                //整车静态轴重
                dcspcz.Text = (Convert.ToDouble(TextIsnulls(yzzlh.Text)) + Convert.ToDouble(TextIsnulls(yzylh.Text)) + Convert.ToDouble(TextIsnulls(ezzlh.Text)) + Convert.ToDouble(TextIsnulls(ezylh.Text)) + Convert.ToDouble(TextIsnulls(szzlh.Text)) + Convert.ToDouble(TextIsnulls(szylh.Text)) + Convert.ToDouble(TextIsnulls(sizzlh.Text)) + Convert.ToDouble(TextIsnulls(sizylh.Text)) + Convert.ToDouble(TextIsnulls(wzzlh.Text)) + Convert.ToDouble(TextIsnulls(wzylh.Text)) + Convert.ToDouble(TextIsnulls(lzzlh.Text)) + Convert.ToDouble(TextIsnulls(lzylh.Text))).ToString("0");
                //判断五轴制动率为不平衡率赋值
                if (wdzzdv.Text != "" && wdzzdv.Text != "-")
                {
                    //如果五轴制动率大于60
                    if (Convert.ToDouble(wdzzdv.Text) < 60)
                    {
                        if (wdzzgcc.Text != "" && wdzzgcc.Text != "-" && wdzygcc.Text != "" && wdzygcc.Text != "-")
                        {
                            double i = System.Math.Abs(Convert.ToDouble(wdzzgcc.Text) - Convert.ToDouble(wdzygcc.Text));//过程差最大点之差
                            if (wzzxczd.Text != "" && wzzxczd.Text != "-" && wzzxczd.Text != "0" && wzyxczd.Text != "" && wzyxczd.Text != "-" && wzyxczd.Text != "0")
                            {
                                if (wzzlh.Text != "" && wzzlh.Text != "-" && wzzlh.Text != "0" && wzylh.Text != "" && wzylh.Text != "-" && wzylh.Text != "0")
                                {
                                    //为五轴不平衡率赋值
                                    wdzbphv.Text = ((i / (Convert.ToDouble(wzzlh.Text) + Convert.ToDouble(wzylh.Text))) * 100).ToString("0.0");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //六轴左轮荷发生改变
        private void lzzlh_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (lzzxczd.Text != "" && lzzxczd.Text != "-" && lzyxczd.Text != "" && lzyxczd.Text != "-")
                {
                    if (lzzlh.Text != "" && lzzlh.Text != "-" && lzzlh.Text != "0" && lzylh.Text != "" && lzylh.Text != "-" && lzylh.Text != "0")
                    {
                        //六轴制动率
                        if (Convert.ToDouble(TextIsnulls(dczs.Text)) < 3)
                        {
                            ldzzdv.Text = ((Convert.ToDouble(lzzxczd.Text) + Convert.ToDouble(lzyxczd.Text)) / (Convert.ToDouble(lzzlh.Text) + Convert.ToDouble(lzylh.Text)) * 100).ToString("0.0");
                        }
                    }
                }
                //整车静态轴重
                dcspcz.Text = (Convert.ToDouble(TextIsnulls(yzzlh.Text)) + Convert.ToDouble(TextIsnulls(yzylh.Text)) + Convert.ToDouble(TextIsnulls(ezzlh.Text)) + Convert.ToDouble(TextIsnulls(ezylh.Text)) + Convert.ToDouble(TextIsnulls(szzlh.Text)) + Convert.ToDouble(TextIsnulls(szylh.Text)) + Convert.ToDouble(TextIsnulls(sizzlh.Text)) + Convert.ToDouble(TextIsnulls(sizylh.Text)) + Convert.ToDouble(TextIsnulls(wzzlh.Text)) + Convert.ToDouble(TextIsnulls(wzylh.Text)) + Convert.ToDouble(TextIsnulls(lzzlh.Text)) + Convert.ToDouble(TextIsnulls(lzylh.Text))).ToString("0");
                //判断六轴制动率为不平衡率赋值
                if (ldzzdv.Text != "" && ldzzdv.Text != "-")
                {
                    //如果六轴制动率大于60
                    if (Convert.ToDouble(ldzzdv.Text) < 60)
                    {
                        if (ldzzgcc.Text != "" && ldzzgcc.Text != "-" && ldzygcc.Text != "" && ldzygcc.Text != "-")
                        {
                            double i = System.Math.Abs(Convert.ToDouble(ldzzgcc.Text) - Convert.ToDouble(ldzygcc.Text));//过程差最大点之差
                            if (lzzxczd.Text != "" && lzzxczd.Text != "-" && lzzxczd.Text != "0" && lzyxczd.Text != "" && lzyxczd.Text != "-" && lzyxczd.Text != "0")
                            {
                                if (lzzlh.Text != "" && lzzlh.Text != "-" && lzzlh.Text != "0" && lzylh.Text != "" && lzylh.Text != "-" && lzylh.Text != "0")
                                {
                                    //为六轴不平衡率赋值
                                    ldzbphv.Text = ((i / (Convert.ToDouble(lzzlh.Text) + Convert.ToDouble(lzylh.Text))) * 100).ToString("0.0");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //六轴右轮荷发生改变
        private void lzylh_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (lzzxczd.Text != "" && lzzxczd.Text != "-" && lzyxczd.Text != "" && lzyxczd.Text != "-")
                {
                    if (lzzlh.Text != "" && lzzlh.Text != "-" && lzzlh.Text != "0" && lzylh.Text != "" && lzylh.Text != "-" && lzylh.Text != "0")
                    {
                        //六轴制动率
                        if (Convert.ToDouble(TextIsnulls(dczs.Text)) < 3)
                        {
                            ldzzdv.Text = ((Convert.ToDouble(lzzxczd.Text) + Convert.ToDouble(lzyxczd.Text)) / (Convert.ToDouble(lzzlh.Text) + Convert.ToDouble(lzylh.Text)) * 100).ToString("0.0");
                        }
                    }
                }
                //整车静态轴重
                dcspcz.Text = (Convert.ToDouble(TextIsnulls(yzzlh.Text)) + Convert.ToDouble(TextIsnulls(yzylh.Text)) + Convert.ToDouble(TextIsnulls(ezzlh.Text)) + Convert.ToDouble(TextIsnulls(ezylh.Text)) + Convert.ToDouble(TextIsnulls(szzlh.Text)) + Convert.ToDouble(TextIsnulls(szylh.Text)) + Convert.ToDouble(TextIsnulls(sizzlh.Text)) + Convert.ToDouble(TextIsnulls(sizylh.Text)) + Convert.ToDouble(TextIsnulls(wzzlh.Text)) + Convert.ToDouble(TextIsnulls(wzylh.Text)) + Convert.ToDouble(TextIsnulls(lzzlh.Text)) + Convert.ToDouble(TextIsnulls(lzylh.Text))).ToString("0");
                //判断六轴制动率为不平衡率赋值
                if (ldzzdv.Text != "" && ldzzdv.Text != "-")
                {
                    //如果六轴制动率大于60
                    if (Convert.ToDouble(ldzzdv.Text) < 60)
                    {
                        if (ldzzgcc.Text != "" && ldzzgcc.Text != "-" && ldzygcc.Text != "" && ldzygcc.Text != "-")
                        {
                            double i = System.Math.Abs(Convert.ToDouble(ldzzgcc.Text) - Convert.ToDouble(ldzygcc.Text));//过程差最大点之差
                            if (lzzxczd.Text != "" && lzzxczd.Text != "-" && lzzxczd.Text != "0" && lzyxczd.Text != "" && lzyxczd.Text != "-" && lzyxczd.Text != "0")
                            {
                                if (lzzlh.Text != "" && lzzlh.Text != "-" && lzzlh.Text != "0" && lzylh.Text != "" && lzylh.Text != "-" && lzylh.Text != "0")
                                {
                                    //为六轴不平衡率赋值
                                    ldzbphv.Text = ((i / (Convert.ToDouble(lzzlh.Text) + Convert.ToDouble(lzylh.Text))) * 100).ToString("0.0");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        //当整车轴重发生改变时整车的行车与驻车制动率也发生改变
        private void dcspcz_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double dc1 = 0;
                double dc2 = 0;
                double dc3 = 0;
                double dc4 = 0;
                double dc5 = 0;
                double dc6 = 0;
                double dc7 = 0;
                double dc8 = 0;
                double dc9 = 0;
                double dc10 = 0;
                double dc11 = 0;
                double dc12 = 0;
                double dc13 = 0;
                if (yzzzczd.Text == "" || yzzzczd.Text == "-") { dc1 = 0; }
                else { dc1 = Convert.ToDouble(yzzzczd.Text); }
                if (yzyzczd.Text == "" || yzyzczd.Text == "-") { dc2 = 0; }
                else { dc2 = Convert.ToDouble(yzyzczd.Text); }
                if (ezzzczd.Text == "" || ezzzczd.Text == "-") { dc3 = 0; }
                else { dc3 = Convert.ToDouble(ezzzczd.Text); }
                if (ezyzczd.Text == "" || ezyzczd.Text == "-") { dc4 = 0; }
                else { dc4 = Convert.ToDouble(ezyzczd.Text); }
                if (szzzczd.Text == "" || szzzczd.Text == "-") { dc5 = 0; }
                else { dc5 = Convert.ToDouble(szzzczd.Text); }
                if (szyzczd.Text == "" || szyzczd.Text == "-") { dc6 = 0; }
                else { dc6 = Convert.ToDouble(szyzczd.Text); }
                if (sizzzczd.Text == "" || sizzzczd.Text == "-") { dc7 = 0; }
                else { dc7 = Convert.ToDouble(sizzzczd.Text); }
                if (sizyzczd.Text == "" || sizyzczd.Text == "-") { dc8 = 0; }
                else { dc8 = Convert.ToDouble(sizyzczd.Text); }
                if (wzzzczd.Text == "" || wzzzczd.Text == "-") { dc9 = 0; }
                else { dc9 = Convert.ToDouble(wzzzczd.Text); }
                if (wzyzczd.Text == "" || wzyzczd.Text == "-") { dc10 = 0; }
                else { dc10 = Convert.ToDouble(wzyzczd.Text); }
                if (lzzzczd.Text == "" || lzzzczd.Text == "-") { dc11 = 0; }
                else { dc11 = Convert.ToDouble(lzzzczd.Text); }
                if (lzyzczd.Text == "" || lzyzczd.Text == "-") { dc12 = 0; }
                else { dc12 = Convert.ToDouble(lzyzczd.Text); }
                if (dcspcz.Text == "" || dcspcz.Text == "-") { dc13 = 0; }
                else { dc13 = Convert.ToDouble(dcspcz.Text); }
                double n = dc1 + dc2 + dc3 + dc4 + dc5 + dc6 + dc7 + dc8 + dc9 + dc10 + dc11 + dc12;//所有驻车制动力之和
                double dszs = dc13 ;
                if (dszs != 0 && dcspcz.Text == "0")
                {
                    dczczdl.Text = ((n / dszs) * 100).ToString("0.0");//整车驻车制动率
                }
                double d1 = 0;
                double d2 = 0;
                double d3 = 0;
                double d4 = 0;
                double d5 = 0;
                double d6 = 0;
                double d7 = 0;
                double d8 = 0;
                double d9 = 0;
                double d10 = 0;
                double d11 = 0;
                double d12 = 0;
                if (yzzxczd.Text == "" || yzzxczd.Text == "-") { d1 = 0; }
                else { d1 = Convert.ToDouble(yzzxczd.Text); }
                if (yzyxczd.Text == "" || yzyxczd.Text == "-") { d2 = 0; }
                else { d2 = Convert.ToDouble(yzyxczd.Text); }
                if (ezzxczd.Text == "" || ezzxczd.Text == "-") { d3 = 0; }
                else { d3 = Convert.ToDouble(ezzxczd.Text); }
                if (ezyxczd.Text == "" || ezyxczd.Text == "-") { d4 = 0; }
                else { d4 = Convert.ToDouble(ezyxczd.Text); }
                if (szzxczd.Text == "" || szzxczd.Text == "-") { d5 = 0; }
                else { d5 = Convert.ToDouble(szzxczd.Text); }
                if (szyxczd.Text == "" || szyxczd.Text == "-") { d6 = 0; }
                else { d6 = Convert.ToDouble(szyxczd.Text); }
                if (sizzxczd.Text == "" || sizzxczd.Text == "-") { d7 = 0; }
                else { d7 = Convert.ToDouble(sizzxczd.Text); }
                if (sizyxczd.Text == "" || sizyxczd.Text == "-") { d8 = 0; }
                else { d8 = Convert.ToDouble(sizyxczd.Text); }
                if (wzzxczd.Text == "" || wzzxczd.Text == "-") { d9 = 0; }
                else { d9 = Convert.ToDouble(wzzxczd.Text); }
                if (wzyxczd.Text == "" || wzyxczd.Text == "-") { d10 = 0; }
                else { d10 = Convert.ToDouble(wzyxczd.Text); }
                if (lzzxczd.Text == "" || lzzxczd.Text == "-") { d11 = 0; }
                else { d11 = Convert.ToDouble(lzzxczd.Text); }
                if (lzyxczd.Text == "" || lzyxczd.Text == "-") { d12 = 0; }
                else { d12 = Convert.ToDouble(lzyxczd.Text); }
                double m = d1 + d2 + d3 + d4 + d5 + d6 + d7 + d8 + d9 + d10 + d11 + d12;//所有制动力之和
                if (dc13 != 0 && dcspcz.Text == "0")
                {
                    dczdl.Text = (m / dc13 * 100).ToString("0.0");//整车制动率
                }
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //车长发生改变
        private void kccc_TextChanged(object sender, EventArgs e)
        {
            dcwkcc.Text = kccc.Text + "*" + ck.Text + "*" + cg.Text;
        }
        //车宽发生改变
        private void ck_TextChanged(object sender, EventArgs e)
        {
            dcwkcc.Text = kccc.Text + "*" + ck.Text + "*" + cg.Text;
        }
        //车高发生改变
        private void cg_TextChanged(object sender, EventArgs e)
        {
            dcwkcc.Text = kccc.Text + "*" + ck.Text + "*" + cg.Text;
        }
        //右灯高值发生改变
        private void ywygdg_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (lbywyc.Text != "" && lbywyc.Text != "-" && ywygdg.Text != "" && ywygdg.Text != "-" && ywygdg.Text != "0")
                {
                    double dywyh = (Convert.ToDouble(lbywyc.Text) + Convert.ToDouble(ywygdg.Text)) / Convert.ToDouble(ywygdg.Text);
                    ywygczH.Text = dywyh.ToString("0.00");
                    if (ynyggq.Text != "" && ynyggq.Text != "-")
                    {
                        double dynyh = (Convert.ToDouble(lbynyc.Text) + Convert.ToDouble(ywygdg.Text)) / Convert.ToDouble(ywygdg.Text);
                        ynygczH.Text = dynyh.ToString("0.00");
                    }
                }
                if (lbyjc.Text != "" && lbyjc.Text != "-" && ywygdg.Text != "" && ywygdg.Text != "-" && ywygdg.Text != "0")
                {
                    double dywjh = (Convert.ToDouble(lbyjc.Text) + Convert.ToDouble(ywygdg.Text)) / Convert.ToDouble(ywygdg.Text);
                    ywjgczH.Text = dywjh.ToString("0.00");
                    if (ynyggq.Text != "" && ynyggq.Text != "-")
                    {
                        double dynjh = (Convert.ToDouble(lbyjc.Text) + Convert.ToDouble(ywygdg.Text)) / Convert.ToDouble(ywygdg.Text);
                        ynjgczH.Text = dynjh.ToString("0.00");
                    }
                }
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //左灯高值发生改变
        private void zwygdg_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (lbzwyc.Text != "" && lbzwyc.Text != "-" && zwygdg.Text != "" && zwygdg.Text != "0" && zwygdg.Text != "-")
                {
                    double dzwyh = (Convert.ToDouble(lbzwyc.Text) + Convert.ToDouble(zwygdg.Text)) / Convert.ToDouble(zwygdg.Text);
                    zwygczH.Text = dzwyh.ToString("0.00");
                    if (znyggq.Text != "" && znyggq.Text != "-")
                    {
                        double dznyh = (Convert.ToDouble(lbznyc.Text) + Convert.ToDouble(zwygdg.Text)) / Convert.ToDouble(zwygdg.Text);
                        znygczH.Text = dznyh.ToString("0.00");
                    }
                }
                if (lbzjc.Text != "" && lbzjc.Text != "-" && zwygdg.Text != "" && zwygdg.Text != "0" && zwygdg.Text != "-")
                {
                    double dzwjh = (Convert.ToDouble(lbzjc.Text) + Convert.ToDouble(zwygdg.Text)) / Convert.ToDouble(zwygdg.Text);
                    zwjgczH.Text = dzwjh.ToString("0.00");
                    if (znyggq.Text != "" && znyggq.Text != "-")
                    {
                        double dznjh = (Convert.ToDouble(lbzjc.Text) + Convert.ToDouble(zwygdg.Text)) / Convert.ToDouble(zwygdg.Text);
                        znjgczH.Text = dznjh.ToString("0.00");
                    }
                }
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //动力性判断
        public void DLXPD()
        {
            try
            {
                if (wdcs.Text != "" && wdcs.Text != "-" && edcs.Text != "" && edcs.Text != "-")
                {
                    if (Convert.ToDouble(wdcs.Text) >= Convert.ToDouble(edcs.Text))
                    {
                        wdcspd.Visible = false;
                    }
                    else
                    {
                        wdcspd.Visible = true;
                    }
                }
                else
                {
                    wdcspd.Visible = false;
                }
                CLDEPD();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //动力性稳定车速超出标准限值范围鼠标经过事件
        private void wdcspd_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("动力性稳定车速超出标准限值范围", wdcspd, 10000);
        }
        //动力性稳定车速超出标准限值范围鼠标离开事件
        private void wdcspd_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(wdcspd);
        }
        //稳定车速的值改变事件
        private void wdcs_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (wdcs.Text != "" && wdcs.Text != "-" && edcs.Text != "" && edcs.Text != "-")
                {
                    if (Convert.ToDouble(wdcs.Text) >= Convert.ToDouble(edcs.Text))
                    {
                        wdcspd.Visible = false;
                    }
                    else
                    {
                        wdcspd.Visible = true;
                    }
                }
                DLXPD();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //额定车速的值改变事件
        private void edcs_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (wdcs.Text != "" && wdcs.Text != "-" && edcs.Text != "" && edcs.Text != "-")
                {
                    if (Convert.ToDouble(wdcs.Text) >= Convert.ToDouble(edcs.Text))
                    {
                        wdcspd.Visible = false;
                    }
                    else
                    {
                        wdcspd.Visible = true;
                    }
                }
                DLXPD();
                Exitpd();
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //左外灯远光水平偏移量的值改变事件
        private void zwygsp_TextChanged(object sender, EventArgs e)
        {
            DZjudge();
            Exitpd();
            CLDEPD();
        }
        //左内灯远光水平偏移量的值改变事件
        private void znygsp_TextChanged(object sender, EventArgs e)
        {
            DZjudge();
            Exitpd();
            CLDEPD();
        }
        //右内灯远光水平偏移量的值改变事件
        private void ynygsp_TextChanged(object sender, EventArgs e)
        {
            DZjudge();
            Exitpd();
            CLDEPD();
        }
        //右外灯远光水平偏移量的值改变事件
        private void ywygsp_TextChanged(object sender, EventArgs e)
        {
            DZjudge();
            Exitpd();
            CLDEPD();
        }
        //左外灯近光水平偏移量的值改变事件
        private void zwjgsp_TextChanged(object sender, EventArgs e)
        {
            DZjudge();
            Exitpd();
            CLDEPD();
        }
        //左内灯近光水平偏移量的值改变事件
        private void znjgsp_TextChanged(object sender, EventArgs e)
        {
            DZjudge();
            Exitpd();
            CLDEPD();
        }
        //右内灯近光水平偏移量的值改变事件
        private void ynjgsp_TextChanged(object sender, EventArgs e)
        {
            DZjudge();
            Exitpd();
            CLDEPD();
        }
        //右外灯近光水平偏移量的值改变事件
        private void ywjgsp_TextChanged(object sender, EventArgs e)
        {
            DZjudge();
            Exitpd();
            CLDEPD();
        }
        //百公里油耗值发生改变事件
        private void yhscz_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (yhbzz.Text.Replace(" ", "") != "" && yhbzz.Text.Replace(" ", "") != "-" && yhscz.Text.Replace(" ", "") != "" && yhscz.Text.Replace(" ", "") != "-")
                {
                    if (Convert.ToDouble(yhscz.Text) <= Convert.ToDouble(yhbzz.Text)*1.14)
                    {
                        jjxpd.Text = "○";
                    }
                    else
                    {
                        jjxpd.Text = "×";
                    }
                }
                CLDEPD();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //查看照片
        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                string lshs = dataGridView1.SelectedRows[0].Cells["检测编号"].Value.ToString();
                string strfiles = photoserver + @"" + photoadress + @"\" + lshs;
                bool brets = Directory.Exists(strfiles);
                if (brets)
                {
                    System.Diagnostics.Process.Start(strfiles);
                }
                else
                {
                    if (dataGridView1.SelectedRows[0].Cells["检测次数"].Value.ToString() != "1")
                    {
                        string str = string.Format("select fid from Data_Modification where 检测次数=1 and 检测编号='{0}'", lshs);
                        SqlDataAdapter sda = new SqlDataAdapter(str, conn);
                        DataTable dta = new DataTable();
                        sda.Fill(dta);
                        string fids = dta.Rows[0]["fid"].ToString();
                        string strfilename = photoserver + @"" + photoadress + @"\" + fids;
                        bool bRet = Directory.Exists(strfilename);
                        if (bRet)
                        {
                            System.Diagnostics.Process.Start(strfilename);
                        }
                        else
                        {
                            MessageBox.Show("此车没有照片");
                        }
                    }
                    else
                    {
                        string strfilename = photoserver + @"" + photoadress + @"\" + fid.Text;
                        bool bret = Directory.Exists(strfilename);
                        if (bret)
                        {
                            System.Diagnostics.Process.Start(strfilename);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //自动刷新
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                ClearText();
                //string str = string.Format("select 车牌号码,车牌颜色,检测日期,检测时间,检测次数,检测编号,是否结束 from dbo.Data_Modification where 是否结束!=1 and 检测时间!='' order by 检测时间 desc");
                //string str = string.Format("select 车牌号码,车牌颜色,检测日期,检测时间,检测次数,检测编号,是否结束 from dbo.Data_Modification where 是否结束!=1 and 检测时间!='' order by 检测时间 desc");
                //SqlDataAdapter sda = new SqlDataAdapter(str, conn);
                //DataTable dt = new DataTable();
                //sda.Fill(dt);
                //dataGridView1.DataSource = dt;
                //string str1 = string.Format("SELECT c.* FROM DM_iRecordTested_记录索引 a INNER JOIN DM_vRegister_变化信息 b ON a.[车辆变化信息(ID)] = b.[车辆变化信息(ID)] INNER JOIN dbo.DM_vRegister_检测项目 c ON a.[检测项目(ID)] = c.[检测项目(ID)] where b.是否结束 != 1");
                //SqlDataAdapter sda1 = new SqlDataAdapter(str1, conn);
                //DataTable dt1 = new DataTable();
                //sda1.Fill(dt1);
                //ArrayList list = new ArrayList();

                //Exit_Modification();
                //Commitpic();
                //Print(true, "1");
                //Print(true, "2");
                //Print(true, "3");
                //conn.Open();
                //string str2 = string.Format("update Data_Modification set 是否通过='Y' where 检测编号='{0}' and 检测次数='{1}'", dataGridView1.SelectedRows[0].Cells["检测编号"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["检测次数"].Value.ToString());
                //SqlCommand cmd1 = new SqlCommand(str2, conn);
                //cmd1.ExecuteNonQuery();
                //conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void wjx_TextChanged(object sender, EventArgs e)
        {
            CLDEPD();
        }

        private void wg_TextChanged(object sender, EventArgs e)
        {
            CLDEPD();
        }

        private void dj_TextChanged(object sender, EventArgs e)
        {
            CLDEPD();
        }

        private void wjx_MouseEnter(object sender, EventArgs e)
        {
            tp.Show("填写规则:只能填写○和×,中间用','隔开,获取顺序为纵向获取(共18项)", wjx, 10000);
        }

        private void wjx_MouseLeave(object sender, EventArgs e)
        {
            tp.Hide(wjx);
        }

        public double GetAbsoluteTime()
        {
            long time = 0;
            IntPtr threadId = GetCurrentThread();
            IntPtr previous = SetThreadAffinityMask(threadId, new IntPtr(1));
            QueryPerformanceCounter(ref time);
            SetThreadAffinityMask(threadId, previous);
            return (double)time / (double)ticksPerSecond * 1000;
        }

        private void QueryPerformanceCounter(ref long time)
        {
            throw new NotImplementedException();
        }

        private IntPtr SetThreadAffinityMask(IntPtr threadId, IntPtr intPtr)
        {
            throw new NotImplementedException();
        }

        private IntPtr GetCurrentThread()
        {
            throw new NotImplementedException();
        }

        public int SendCommand(byte[] SendData, ref byte[] ReceiveData, byte byAckFirst, double timeOut, ref string msg)
        {
            if (ReceiveData == null)
            {
                msg = "接收数据缓冲区不能为空";
                return -1;
            }
            int outBuffLen = ReceiveData.Length;

            System.Threading.Mutex mt = new System.Threading.Mutex(false, "SendCommand0");
            mt.WaitOne();
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }
            if (_serialPort.IsOpen)
            {
                try
                {
                    ReceiveEventFlag = true;        //关闭接收事件
                    _serialPort.DiscardInBuffer();  //清空接收缓冲区                

                    _serialPort.DiscardOutBuffer();//丢弃接收缓冲区数据
                    _serialPort.Write(SendData, 0, SendData.Length);//发送命令

                    if (msg == "NORTN")
                    {
                        System.Threading.Thread.Sleep(20);
                        _serialPort.DiscardOutBuffer();
                        mt.ReleaseMutex();
                        return 0;
                    }
                    double timBgn =GetAbsoluteTime();
                    while (true)
                    {
                        if (this._serialPort.BytesToRead >= 1)
                        {
                            byte by = Convert.ToByte(this._serialPort.ReadByte());
                            if (by == byAckFirst)
                            {
                                ReceiveData[0] = by;
                                break;
                            }
                        }
                        if ((GetAbsoluteTime() - timBgn) / 1000 > timeOut)
                        {
                            msg = "超时!";
                            mt.ReleaseMutex();
                            return -1;
                        }
                    }
                    //注释掉下面这行
                    //目的：两次循环总计超时时间为timeOut
                    //timBgn = timMeter.GetAbsoluteTime();
                    while (true)
                    {
                        if (this._serialPort.BytesToRead >= outBuffLen - 1)
                        {
                            break;
                        }
                        if ((GetAbsoluteTime() - timBgn) / 1000 > timeOut)//超时
                        {
                            msg = "超时!";
                            mt.ReleaseMutex();
                            return -1;
                        }
                    }
                    int ret = this._serialPort.Read(ReceiveData, 1, outBuffLen - 1);
                    mt.ReleaseMutex();
                    return ret;
                }
                catch (Exception ex)
                {
                    mt.ReleaseMutex();
                    msg = ex.Message.ToString();
                    return -1;
                }
            }
            else
            {
                msg = "串口已经关闭，请查明原因";
                mt.ReleaseMutex();
                return -1;
            }
        }
        
        private void tb_TextChanged(object sender, EventArgs e)
        {
            CLDEPD();
        }
        //取路试数据
        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                tabControl1.SelectedIndex = 1;
                if (datas1 == "")
                {
                    MessageBox.Show("数据库mbk 不存在");
                }
                else
                {
                    conn1.Open();
                    string str = string.Format("select top 1 * from mbk where CarNo='{0}' order by Num desc", hphm.Text);
                    SqlDataAdapter sda = new SqlDataAdapter(str, sqlcon1);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count < 1)
                    {
                        MessageBox.Show("取值失败");
                    }
                    else
                    {
                        lszdcsd.Text = Convert.ToDouble(dt.Rows[0]["BrakeFirstSpeed"].ToString()).ToString("0.00");
                        lszdmfdd.Text = Convert.ToDouble(dt.Rows[0]["MFDD"].ToString()).ToString("0.00");
                        lszdjl.Text = Convert.ToDouble(dt.Rows[0]["BrakeDistance"].ToString()).ToString("0.00");
                        lszdwdx.Text = dt.Rows[0]["paopian"].ToString();
                        Random r = new Random();
                        if (qzdz.Text.Contains("四"))
                        {
                            zwyggq.Text = 12000 + r.NextDouble().ToString("0");
                            ywyggq.Text = 13000 + r.NextDouble().ToString("0");
                        }
                        else
                        {
                            zwyggq.Text = 15000 + r.NextDouble().ToString("0");
                            ywyggq.Text = 16000 + r.NextDouble().ToString("0");
                        }
                        zwygczH.Text = (0.85 + r.NextDouble() * (0.95 - 0.85)).ToString("0.00");
                        ywygczH.Text = (0.85 + r.NextDouble() * (0.95 - 0.85)).ToString("0.00");
                        zwjgczH.Text = (0.7 + r.NextDouble() * (0.8 - 0.7)).ToString("0.00");
                        ywjgczH.Text = (0.7 + r.NextDouble() * (0.8 - 0.7)).ToString("0.00");
                        zwygsp.Text = (-170 + r.NextDouble() * (350 + 170)).ToString("0.00");
                        ywygsp.Text = (-160 + r.NextDouble() * (350 + 170)).ToString("0.00");
                        zwjgsp.Text = (-170 + r.NextDouble() * (350 + 350)).ToString("0.00");
                        ywjgsp.Text = (-150 + r.NextDouble() * (350 + 350)).ToString("0.00");
                        if (ryxs.Text.Contains("柴油"))
                        {
                            cygxs1.Text = (2.5 - r.NextDouble() * (2.5 - 0.1)).ToString("0.0");
                            cygxs2.Text = (2.5 - r.NextDouble() * (2.5 - 0.1)).ToString("0.0");
                            cygxs3.Text = (2.5 - r.NextDouble() * (2.5 - 0.1)).ToString("0.0");
                        }
                        else
                        {
                            qygdsCO.Text = (3.0 - r.NextDouble() * (3.0 - 0.1)).ToString("0.0");
                            qygdsHC.Text = (200 - r.NextDouble() * (200 - 1)).ToString("0.0");
                            qygdsλ.Text = (0.97 + r.NextDouble() * (1.03 - 0.97)).ToString("0.00");
                            qyddsCO.Text = (1.5 - r.NextDouble() * (1.5 - 0.1)).ToString("0.0");
                            qyddsHC.Text = (250 - r.NextDouble() * (250 - 1)).ToString("0.0");
                        }
                    }
                    conn1.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //一轴轴重值发生改变事件
        private void yzzz_TextChanged(object sender, EventArgs e)
        {
           if(Convert.ToDouble(TextIsnulls(dczs.Text)) >= 3)
            {
                ydzzdv.Text = ((Convert.ToDouble(TextIsnulls(yzzxczd.Text)) + Convert.ToDouble(TextIsnulls(yzyxczd.Text))) / Convert.ToDouble(TextIsnulls((yzzz.Text))) * 100).ToString("0.0");
            }
           else
            {
                return;
            }
        }
        //二轴轴重值发生改变事件
        private void ezzz_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDouble(TextIsnulls(dczs.Text)) >= 3)
            {
                edzzdv.Text = ((Convert.ToDouble(TextIsnulls(ezzxczd.Text)) + Convert.ToDouble(TextIsnulls(ezyxczd.Text))) / Convert.ToDouble(TextIsnulls((ezzz.Text))) * 100).ToString("0.0");
            }
            else
            {
                return;
            }
        }
        //三轴轴重值发生改变事件
        private void szzz_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDouble(TextIsnulls(dczs.Text)) >= 3)
            {
                sdzzdv.Text = ((Convert.ToDouble(TextIsnulls(szzxczd.Text)) + Convert.ToDouble(TextIsnulls(szyxczd.Text))) / Convert.ToDouble(TextIsnulls((szzz.Text))) * 100).ToString("0.0");
            }
            else
            {
                return;
            }
        }
        //四轴轴重值发生改变事件
        private void sizzz_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDouble(TextIsnulls(dczs.Text)) >= 3)
            {
                sidzzdv.Text = ((Convert.ToDouble(TextIsnulls(sizzxczd.Text)) + Convert.ToDouble(TextIsnulls(sizyxczd.Text))) / Convert.ToDouble(TextIsnulls((sizzz.Text))) * 100).ToString("0.0");
            }
            else
            {
                return;
            }
        }
        //五轴轴重值发生改变事件
        private void wzzz_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDouble(TextIsnulls(dczs.Text)) >= 3)
            {
                wdzzdv.Text = ((Convert.ToDouble(TextIsnulls(wzzxczd.Text)) + Convert.ToDouble(TextIsnulls(wzyxczd.Text))) / Convert.ToDouble(TextIsnulls((wzzz.Text))) * 100).ToString("0.0");
            }
            else
            {
                return;
            }
        }
        //六轴轴重值发生改变事件
        private void lzzz_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDouble(TextIsnulls(dczs.Text)) >= 3)
            {
                ldzzdv.Text = ((Convert.ToDouble(TextIsnulls(lzzxczd.Text)) + Convert.ToDouble(TextIsnulls(lzyxczd.Text))) / Convert.ToDouble(TextIsnulls((lzzz.Text))) * 100).ToString("0.0");
            }
            else
            {
                return;
            }
        }
        //统计查询
        private void button8_Click(object sender, EventArgs e)
        {
            string str = string.Format("select distinct 车牌号码,车牌颜色,检测日期,车辆类型,车主单位,送检单位,检测类别 from Data_Modification where (车牌号码 like '%'+'{0}'+'%' or 车牌号码='') and (检测日期 between '{1}' and '{2}' or '{1}'='' or '{2}'='') and (车主单位 like '%'+'{3}'+'%' or 车主单位='') and (车辆类型 like '%'+'{4}'+'%' or 车辆类型='') and (送检单位 like '%'+'{5}'+'%' or 送检单位='') and 检测时间!='' order by 车主单位 desc,车牌号码 desc,车辆类型 desc,送检单位 desc", tcphm.Text, dateTimePicker3.Value.ToString("yyyy-MM-dd"), dateTimePicker4.Value.ToString("yyyy-MM-dd"), tczdw.Text,tcllx.Text,tsjdw.Text);
            SqlDataAdapter sda = new SqlDataAdapter(str, conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView2.DataSource = dt;
            AddTotal(dataGridView2);
            dataGridView2.Columns["车主单位"].Width = 200;
            dataGridView2.Columns["车辆类型"].Width = 200;
            dataGridView2.AllowUserToAddRows = false;
            int j = 0;
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                j = i + 1;
                dataGridView2.Rows[i].HeaderCell.Value = j.ToString();
            }
            dataGridView2.RowHeadersWidth = 50;
        }
        //统计
        public void AddTotal(DataGridView dg)
        {
            #region
            if (dataGridView2.Columns.Contains("等级评定"))
            {
                dataGridView2.Columns.Remove("等级评定");
            }
            if (dataGridView2.Columns.Contains("二级维护"))
            {
                dataGridView2.Columns.Remove("二级维护");
            }
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.ReadOnly = false;
            //MessageBox.Show(dg.Rows.Count.ToString());
            int count4 = 0;
            int count5 = 0;
            for (int i = 0; i < dg.Rows.Count; i++)
            {
                if (i <= dg.Rows.Count - 2)
                {
                    #region 判断
                    if (dg.Rows[i].Cells["车牌号码"].Value.ToString() != dg.Rows[i + 1].Cells["车牌号码"].Value.ToString())
                    {
                        if (dg.Rows[i].Cells["检测类别"].Value.ToString() == "等级评定")
                        {
                            count4++;
                        }
                        if (dg.Rows[i].Cells["检测类别"].Value.ToString() == "二级维护")
                        {
                            count5++;
                        }
                    }
                    else
                    {
                        if(dg.Rows[i].Cells["检测日期"].Value.ToString() != dg.Rows[i + 1].Cells["检测日期"].Value.ToString())
                        {
                            if (dg.Rows[i].Cells["检测类别"].Value.ToString() == "等级评定")
                            {
                                count4++;
                            }
                            if (dg.Rows[i].Cells["检测类别"].Value.ToString() == "二级维护")
                            {
                                count5++;
                            }
                        }
                    }
                    #endregion
                }
                if (i == dg.Rows.Count - 1)
                {
                    #region 与倒数第二个不同
                    if (dg.Rows[i].Cells["车牌号码"].Value.ToString() != dg.Rows[i - 1].Cells["车牌号码"].Value.ToString())
                    {
                        if (dg.Rows[i].Cells["检测类别"].Value.ToString() == "等级评定")
                        {
                            count4++;
                        }
                        if (dg.Rows[i].Cells["检测类别"].Value.ToString() == "二级维护")
                        {
                            count5++;
                        }
                    }
                    else
                    {
                        if (dg.Rows[i].Cells["检测日期"].Value.ToString() != dg.Rows[i - 1].Cells["检测日期"].Value.ToString())
                        {
                            if (dg.Rows[i].Cells["检测类别"].Value.ToString() == "等级评定")
                            {
                                count4++;
                            }
                            if (dg.Rows[i].Cells["检测类别"].Value.ToString() == "二级维护")
                            {
                                count5++;
                            }
                        }
                    }
                    #endregion
                    #region 最后一个与倒数第二个相同
                    if (dg.Rows[i].Cells["车牌号码"].Value.ToString() == dg.Rows[i - 1].Cells["车牌号码"].Value.ToString())
                    {
                        if (dg.Rows[i].Cells["检测类别"].Value.ToString() == "等级评定")
                        {
                            count4++;
                        }
                        if (dg.Rows[i].Cells["检测类别"].Value.ToString() == "二级维护")
                        {
                            count5++;
                        }
                    }
                    else
                    {
                        if (dg.Rows[i].Cells["检测日期"].Value.ToString() != dg.Rows[i - 1].Cells["检测日期"].Value.ToString())
                        {
                            if (dg.Rows[i].Cells["检测类别"].Value.ToString() == "等级评定")
                            {
                                count4++;
                            }
                            if (dg.Rows[i].Cells["检测类别"].Value.ToString() == "二级维护")
                            {
                                count5++;
                            }
                        }
                    }
                    #endregion
                }
            }
            if (dataGridView2.Rows.Count > 0)
            {
                DataGridViewTextBoxColumn acCode = new DataGridViewTextBoxColumn();
                acCode.Name = "等级评定";
                acCode.DataPropertyName = "等级评定";
                acCode.HeaderText = "等级评定";
                dg.Columns.Add(acCode);
                dataGridView2.Rows[0].Cells["等级评定"].Value = count4;

                DataGridViewTextBoxColumn acCodes = new DataGridViewTextBoxColumn();
                acCodes.Name = "二级维护";
                acCodes.DataPropertyName = "二级维护";
                acCodes.HeaderText = "二级维护";
                dg.Columns.Add(acCodes);
                dataGridView2.Rows[0].Cells["二级维护"].Value = count5;
            }
            #endregion
        }
        //导出数据
        private void button9_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            // 列强制转换
            for (int count = 0; count < dataGridView2.Columns.Count; count++)
            {
                DataColumn dc = new DataColumn(dataGridView2.Columns[count].Name.ToString());
                dt.Columns.Add(dc);
            }
            // 循环行
            for (int count = 0; count < dataGridView2.Rows.Count; count++)
            {
                DataRow dr = dt.NewRow();
                for (int countsub = 0; countsub < dataGridView2.Columns.Count; countsub++)
                {
                    dr[countsub] = Convert.ToString(dataGridView2.Rows[count].Cells[countsub].Value);
                }
                dt.Rows.Add(dr);
            }
            //DataTable dt = (dataGridView1.DataSource as DataTable);//数据绑定过的数据转换成datable
            if (dt == null) return;
            System.Windows.Forms.SaveFileDialog saveFileDlg = new System.Windows.Forms.SaveFileDialog();
            saveFileDlg.Title = "导出过程数据";
            saveFileDlg.Filter = "过程数据(*.xls)|*.xls";
            saveFileDlg.RestoreDirectory = true;
            if (saveFileDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string execltext = "日志文件";
                ExcelHelper hp = new ExcelHelper(saveFileDlg.FileName);
                int rst = hp.DataTableToExcel(dt, execltext, true);
                if (rst == -1)
                {
                    MessageBox.Show("导出失败");
                }
                else
                {
                    MessageBox.Show("导出成功");
                }
            }
        }
        //合并行
        private void dataGridView2_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex <= 6 && e.Value.ToString() != string.Empty)
                {
                    #region
                    int UpRows = 0;//上面相同的行数
                    int DownRows = 0;//下面相同的行数
                    int count = 0;//总行数
                    int cellwidth = e.CellBounds.Width;//列宽
                    //获取下面的行数
                    for (int i = e.RowIndex; i < this.dataGridView2.Rows.Count; i++)
                    {
                        if (this.dataGridView2.Rows[i].Cells[e.ColumnIndex].Value.ToString().Equals(e.Value.ToString()))
                        {
                            DownRows++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    //获取上面的行数
                    for (int i = e.RowIndex; i >= 0; i--)
                    {
                        if (this.dataGridView2.Rows[i].Cells[e.ColumnIndex].Value.ToString().Equals(e.Value.ToString()))
                        {
                            UpRows++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    count = UpRows + DownRows - 1;//总行数
                    using (Brush gridBrush = new SolidBrush(this.dataGridView2.GridColor), backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                    {
                        using (Pen gridLinePen = new Pen(gridBrush))
                        {
                            //清除单元格
                            e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                            if (e.Value != null)
                            {
                                int cellheight = e.CellBounds.Height;
                                SizeF size = e.Graphics.MeasureString(e.Value.ToString(), e.CellStyle.Font);
                                e.Graphics.DrawString((e.Value).ToString(), e.CellStyle.Font, Brushes.Black, e.CellBounds.X + (cellwidth - size.Width) / 2, e.CellBounds.Y - cellheight * (UpRows - 1) + (cellheight * count - size.Height) / 2, StringFormat.GenericDefault);
                            }
                            //如果下一行数据不等于当前行数据，则画当前单元格底边线
                            if (e.RowIndex < this.dataGridView2.Rows.Count - 1 && this.dataGridView2.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value.ToString() != e.Value.ToString())
                            {
                                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                            }
                            if (e.RowIndex == this.dataGridView2.Rows.Count - 1)
                            {
                                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left + 2, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                                count = 0;
                            }
                            //画grid右边线
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);
                            e.Handled = true;
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }
        //关闭进程
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
            Application.Exit();
        }

        /// <summary>
        /// 向服务器post图片
        /// </summary>
        ///<param name="url">url
        ///<param name="jpegPath">头像地址
        /// <returns>返回服务器返回值</returns>
        public string postest(string url, byte[] jpegPath, string photoname, string year, string jctype)
        {

            ////将图片转化为byte[]再转化为string
            string array = Convert.ToBase64String(jpegPath);
            ////构造post提交字段
            string para = "jctype=" + jctype + "&safesn=zzxgcjpsc&photoname=" + photoname + "&year=" + year + "&headimg=" + HttpUtility.UrlEncode(array);
            #region HttpWebRequest写法

            HttpWebRequest httpWeb = (HttpWebRequest)WebRequest.Create(url);
            httpWeb.Timeout = 120000;
            httpWeb.Method = "POST";
            httpWeb.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            byte[] bytePara = Encoding.ASCII.GetBytes(para);
            using (Stream reqStream = httpWeb.GetRequestStream())
            {
                //提交数据
                reqStream.Write(bytePara, 0, para.Length);
            }
            //获取服务器返回值
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWeb.GetResponse();
            Stream stream = httpWebResponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(stream, Encoding.GetEncoding("utf-8"));
            //获得返回值
            string result = streamReader.ReadToEnd();
            stream.Close();

            #endregion
            //将服务器返回值返回
            return result;
        }
    
        public void Commitpic()
        {
            string dates = Convert.ToDateTime(jyrq.Text).ToString("yyyyMM");
            string cphm = hphm.Text.Substring(1);
            string cpxx = "";
            if (hpzl.Text.Contains("黄"))
            {
                cpxx = cphm + "1";
            }
            else if (hpzl.Text.Contains("蓝"))
            {
                cpxx = cphm + "2";
            }
            else
            {
                cpxx = cphm + "3";
            }
            string jclb = "";
            if (jylb.Text.Contains("等级评定"))
            {
                jclb = "1";
            }
            if (jylb.Text.Contains("二级维护"))
            {
                jclb = "2";
            }
            string lshs = dataGridView1.SelectedRows[0].Cells["检测编号"].Value.ToString();
            string strfiles = photoserver + @"" + photoadress + @"\" + lshs;
            bool brets = Directory.Exists(strfiles);
            if (brets)
            {
                #region 流水号
                string strFileName = photoserver + @"" + photoadress + @"\" + lsh.Text + @"\" + lsh.Text + "_B.jpg";
                string strFileName1 = photoserver + @"" + photoadress + @"\" + lsh.Text + @"\" + lsh.Text + "_H.jpg";
                string strFileName2 = photoserver + @"" + photoadress + @"\" + lsh.Text + @"\" + lsh.Text + "_S.jpg";
                string strFileName3 = photoserver + @"" + photoadress + @"\" + lsh.Text + @"\" + lsh.Text + "_P.jpg";
                ArrayList arr = new ArrayList();
                bool bret = File.Exists(strFileName);
                if (bret)
                {
                    arr.Add(strFileName);
                }
                bool bret1 = File.Exists(strFileName1);
                if (bret1)
                {
                    arr.Add(strFileName1);
                }
                bool bret2 = File.Exists(strFileName2);
                if (bret2)
                {
                    arr.Add(strFileName2);
                }
                else
                {
                    bool bret3 = File.Exists(strFileName3);
                    if (bret3)
                    {
                        arr.Add(strFileName3);
                    }
                }
                try
                {
                    for (int i = 0; i < arr.Count; i++)
                    {
                        string strfilename = arr[i].ToString();
                        byte[] Jpeg = System.IO.File.ReadAllBytes(strfilename);
                        string imgresult = postest("http://114.215.102.130/jiping.ashx", Jpeg, cpxx, dates, jclb);
                        if (imgresult.Contains("上传成功"))
                        {
                            MessageBox.Show("成功");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                #endregion
            }
            else
            {
                #region 根据fid查询路径
                if (dataGridView1.SelectedRows[0].Cells["检测次数"].Value.ToString() != "1")
                {
                    #region 检测次数不为1
                    string str = string.Format("select fid from Data_Modification where 检测次数=1 and 检测编号='{0}'", lshs);
                    SqlDataAdapter sda = new SqlDataAdapter(str, conn);
                    DataTable dta = new DataTable();
                    sda.Fill(dta);
                    string fids = dta.Rows[0]["fid"].ToString();
                    string strfileName = photoserver + @"" + photoadress + @"\" + fid.Text + @"\" + fid.Text + "_B.jpg";
                    string strfileName1 = photoserver + @"" + photoadress + @"\" + fid.Text + @"\" + fid.Text + "_H.jpg";
                    string strfileName2 = photoserver + @"" + photoadress + @"\" + fid.Text + @"\" + fid.Text + "_S.jpg";
                    string strFileName3 = photoserver + @"" + photoadress + @"\" + fid.Text + @"\" + fid.Text + "_P.jpg";

                    //string strFileName4 = photoserver + @"" + photoadress + @"\" + fids + @"\" + fids + "_B.jpeg";
                    //string strFileName5 = photoserver + @"" + photoadress + @"\" + fids + @"\" + fids + "_H.jpeg";
                    //string strFileName6 = photoserver + @"" + photoadress + @"\" + fids + @"\" + fids + "_S.jpeg";
                    //string strFileName7 = photoserver + @"" + photoadress + @"\" + fids + @"\" + fids + "_P.jpeg";
                    ArrayList arr = new ArrayList();
                    bool bret = File.Exists(strfileName);
                    if (bret)
                    {
                        arr.Add(strfileName);
                    }
                    bool bret1 = File.Exists(strfileName1);
                    if (bret1)
                    {
                        arr.Add(strfileName1);
                    }
                    bool bret2 = File.Exists(strfileName2);
                    if (bret2)
                    {
                        arr.Add(strfileName2);
                    }
                    else
                    {
                        bool bret3 = File.Exists(strFileName3);
                        if (bret3)
                        {
                            arr.Add(strFileName3);
                        }
                    }
                    try
                    {
                        for (int i = 0; i < arr.Count; i++)
                        {
                            string strfilename = arr[i].ToString();
                            byte[] Jpeg = System.IO.File.ReadAllBytes(strfilename);
                            string imgresult = postest("http://114.215.102.130/jiping.ashx", Jpeg, cpxx, dates, jclb);
                            if (imgresult.Contains("上传成功"))
                            {
                                MessageBox.Show("成功");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    #endregion
                }
                else
                {
                    #region 检测次数为1
                    string strfileName = photoserver + @"" + photoadress + @"\" + fid.Text + @"\" + fid.Text + "_B.jpg";
                    string strfileName1 = photoserver + @"" + photoadress + @"\" + fid.Text + @"\" + fid.Text + "_H.jpg";
                    string strfileName2 = photoserver + @"" + photoadress + @"\" + fid.Text + @"\" + fid.Text + "_S.jpg";
                    string strFileName3 = photoserver + @"" + photoadress + @"\" + fid.Text + @"\" + fid.Text + "_P.jpg";
                    ArrayList arr = new ArrayList();
                    int counts = 0;
                    bool bret1 = File.Exists(strfileName1);
                    if (bret1)
                    {
                        arr.Add(strfileName1);
                    }
                    bool bret = File.Exists(strfileName);
                    if (bret)
                    {
                        arr.Add(strfileName);
                    }
                    bool bret2 = File.Exists(strfileName2);
                    if (bret2)
                    {
                        arr.Add(strfileName2);
                    }
                    else
                    {
                        bool bret3 = File.Exists(strFileName3);
                        if (bret3)
                        {
                            arr.Add(strFileName3);
                        }
                    }
                    try
                    {
                        for (int i = 0; i < arr.Count; i++)
                        {
                            string strfilename = arr[i].ToString();
                            byte[] Jpeg = System.IO.File.ReadAllBytes(strfilename);
                            string imgresult = postest("http://114.215.102.130/jiping.ashx", Jpeg, cpxx, dates, jclb);
                            if (imgresult.Contains("上传成功"))
                            {
                                counts++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    #endregion
                }
                #endregion
            }

        }
    }
}