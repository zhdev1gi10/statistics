using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using PubOp;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Modification
{
    class DBHelper
    {
        private static SqlCommand cmd = null;
        private static SqlDataReader sdr = null;

        static string strFilePath = Application.StartupPath + @"\Appli.ini";
        OperateIniFile op = new OperateIniFile();
        static string servers = OperateIniFile.ReadIniData("数据库连接", "server", "", strFilePath);
        static string datas = OperateIniFile.ReadIniData("数据库连接", "database", "", strFilePath);
        static string names = OperateIniFile.ReadIniData("数据库连接", "uid", "", strFilePath);
        static string pwds = OperateIniFile.ReadIniData("数据库连接", "pwd", "", strFilePath);
        static SqlConnection sqlcon =new SqlConnection("Data Source=" + servers + ";Initial Catalog=" + datas + ";User ID=" + names + ";pwd=" + pwds + ";");
        //static SqlConnection sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
        
        private static SqlConnection GetConn()
        {
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
            }
            return sqlcon;
        }

        /// <summary>  
        ///  执行不带参数的增删改SQL语句或存储过程  
        /// </summary>  
        /// <param name="cmdText">增删改SQL语句或存储过程</param>  
        /// <param name="ct">命令类型</param>  
        /// <returns></returns>  
        public static int ExecuteNonQuery(string cmdText, CommandType ct)
        {
            int res;
            try
            {
                cmd = new SqlCommand(cmdText, GetConn());
                cmd.CommandType = ct;
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                {
                    sqlcon.Close();
                }
            }
            return res;
        }

        /// <summary>  
        ///  执行带参数的增删改SQL语句或存储过程  
        /// </summary>  
        /// <param name="cmdText">增删改SQL语句或存储过程</param>  
        /// <param name="ct">命令类型</param>  
        /// <returns>int值</returns>  
        public static int ExecuteNonQuery(string cmdText, SqlParameter[] paras, CommandType ct)
        {
            int res;
            using (cmd = new SqlCommand(cmdText, GetConn()))
            {
                cmd.CommandType = ct;
                cmd.Parameters.AddRange(paras);
                res = cmd.ExecuteNonQuery();
            }
            return res;
        }

        /// <summary>  
        ///  执行查询SQL语句或存储过程  
        /// </summary>  
        /// <param name="cmdText">查询SQL语句或存储过程</param>  
        /// <param name="ct">命令类型</param>  
        /// <returns>Table值</returns>  
        public static DataTable ExecuteQuery(string cmdText, CommandType ct)
        {
            DataTable dt = new DataTable();
            cmd = new SqlCommand(cmdText, GetConn());
            cmd.CommandType = ct;
            using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(sdr);
            }
            return dt;
        }

        /// <summary>  
        ///  执行带参数的查询SQL语句或存储过程  
        /// </summary>  
        /// <param name="cmdText">查询SQL语句或存储过程</param>  
        /// <param name="paras">参数集合</param>  
        /// <param name="ct">命令类型</param>  
        /// <returns>Table值</returns>  
        public static DataTable ExecuteQuery(string cmdText, SqlParameter[] paras, CommandType ct)
        {
                DataTable dt = new DataTable();
                cmd = new SqlCommand(cmdText, GetConn());
                cmd.CommandType = ct;
                cmd.Parameters.AddRange(paras);
            using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(sdr);
            }
            return dt;
        }
        
        /// <summary>  
        /// 执行带参数的Scalar查询  
        /// </summary>  
        /// <param name="cmdText">查询SQL语句或存储过程</param>  
        /// <param name="paras">参数集合</param>  
        /// <param name="ct">命令类型</param>  
        /// <returns>一个int型值</returns>  
        public static int ExecuteCheck(string cmdText, SqlParameter[] paras, CommandType ct)
        {
            int result;
            using (cmd = new SqlCommand(cmdText, GetConn()))
            {
                cmd.CommandType = ct;
                cmd.Parameters.AddRange(paras);
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return result;
        }
    }
}
