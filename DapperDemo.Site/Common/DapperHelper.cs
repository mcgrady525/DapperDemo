using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SSharing.Frameworks.Common.Helpers;
using DapperDemo.Site.Attributes;
using System.ComponentModel;

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
        public static IDbConnection CreateDbConnection(string dbName)
        {
            var connStr = ConfigHelper.GetConnectionString(dbName);
            IDbConnection conn = new SqlConnection(connStr);
            if (conn.State != ConnectionState.Open)
            {
                conn.Close();
                conn.Open();
            }

            return conn;
        }

        /// <summary>
        /// list转DataTable
        /// 1，仅针对加了ColumnAttribute的公共属性
        /// 2，columnName处理(SystemCode->system_code)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable<T>(List<T> list)
        {
            if (list == null)
            {
                return null;
            }

            Type type = typeof(T);

            //只获取加了ColumnAttribute的公共属性
            var ps = type.GetProperties().Where(p => (ColumnAttribute)Attribute.GetCustomAttribute(p, typeof(ColumnAttribute)) != null).ToList();

            NullableConverter nullableConvert;
            List<DataColumn> cols = new List<DataColumn>();
            var columnName = string.Empty;
            Type targetType;
            foreach (var p in ps)
            {
                if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    nullableConvert = new NullableConverter(p.PropertyType);
                    targetType = nullableConvert.UnderlyingType;
                }
                else
                {
                    targetType = p.PropertyType;
                }

                //处理columnName
                columnName = p.Name;
                var columnAttribute = Attribute.GetCustomAttribute(p, typeof(ColumnAttribute));
                if (columnAttribute != null)
                {
                    var t = columnAttribute as ColumnAttribute;
                    columnName = t.ColumnName;
                }
                cols.Add(new DataColumn(columnName, targetType));
            }

            var tableName = type.Name;
            var tableAttribute= Attribute.GetCustomAttribute(type, typeof(TableAttribute));
            if (tableAttribute!= null)
            {
                var t = tableAttribute as TableAttribute;
                tableName = t.TableName;
            }
            DataTable dt = new DataTable(tableName);
            dt.Columns.AddRange(cols.ToArray());

            list.ForEach((l) =>
            {
                List<object> objs = new List<object>();
                objs.AddRange(ps.Select(p => p.GetValue(l, null)));
                dt.Rows.Add(objs.ToArray());
            });

            return dt;
        }
    }
}