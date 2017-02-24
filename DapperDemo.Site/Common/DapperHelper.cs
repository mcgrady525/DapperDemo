using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DapperDemo.Site.Common
{
    /// <summary>
    /// Dapper扩展
    /// </summary>
    public partial class DapperHelper
    {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        public static IDbConnection CreateConnection()
        {
            //依据配置是否开启MiniProfiler
            IDbConnection conn = null;
            var connStr = ConfigHelper.GetConnectionString("DapperDemoDB");
            var isMiniProfilerEnabled = Convert.ToBoolean(ConfigHelper.GetAppSetting("IsMiniProfilerEnabled"));
            if (isMiniProfilerEnabled)
            {
                conn = new ProfiledDbConnection(new SqlConnection(connStr), MiniProfiler.Current);
            }
            else
            {
                conn = new SqlConnection(connStr);
            }
            if (conn.State != ConnectionState.Open)
            {
                conn.Close();
                conn.Open();
            }

            return conn;
        }

    }
}