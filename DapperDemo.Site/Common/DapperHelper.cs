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
            IDbConnection connection = new SqlConnection(ConfigHelper.GetConnectionString("DapperDemoDB"));
            if (connection.State != ConnectionState.Open)
            {
                connection.Close();
                connection.Open();
            }

            return connection;
        }

    }
}