using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using DapperDemo.Site.Models;
using DapperDemo.Site.Common;
using System.Text;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using SSharing.Frameworks.Common.Extends;

namespace DapperDemo.Site.Controllers
{
    /// <summary>
    /// dapper原生示例
    /// *单条插入
    /// *批量插入，两种方法(dapper自带和SqlBulkCopy)
    /// *查询，单表和多表
    /// *更新
    /// *单条删除
    /// *批量删除
    /// *事务
    /// *调用存储过程
    /// *多结果集
    /// *动态参数
    /// *多数据库查询
    /// </summary>
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        #region 插入
        /// <summary>
        /// 单条插入
        /// insert列要跟值对应
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertSingle()
        {
            var fltOrder = new TFltOrder
            {
                OrderNo = "201801170002",
                Status = "TicketIssued",
                PaymentAmt = 100
            };

            using (var conn = DapperHelper.CreateDbConnection("DapperDemo1DB"))
            {
                conn.Execute(@"INSERT INTO dbo.t_flt_order
                    (   order_no,
                        status,
                        payment_amt
                    )
                    VALUES
                    (   @OrderNo,
                        @Status,
                        @PaymentAmt
                    );", fltOrder);
            }

            return Content("OK!");
        }

        /// <summary>
        /// 批量插入(dapper自带)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertBulkByDefault()
        {
            var fltOrders = new List<TFltOrder>();
            for (int i = 2; i < 10; i++)
            {
                fltOrders.Add(new TFltOrder
                {
                    OrderNo = "20180117000" + i,
                    Status = "Changed",
                    PaymentAmt = 1000
                });
            }

            using (var conn = DapperHelper.CreateDbConnection("DapperDemo1DB"))
            {
                conn.Execute(@"INSERT INTO dbo.t_flt_order(order_no,status,payment_amt) VALUES ( @OrderNo,@Status ,@PaymentAmt);", fltOrders);
            }

            return Content("OK!");
        }

        /// <summary>
        /// 批量插入(使用SqlBulkCopy)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertBulkBySqlBulkCopy()
        {
            var fltOrders = new List<TFltOrder>();
            for (int i = 10; i < 20; i++)
            {
                fltOrders.Add(new TFltOrder
                {
                    OrderNo = "2018011700" + i,
                    Status = "TicketIssued",
                    PaymentAmt = 1100
                });
            }

            using (var conn = DapperHelper.CreateDbConnection("DapperDemo1DB"))
            {
                //list转DataTable
                var dt = DapperHelper.ConvertToDataTable(fltOrders);

                using (var sqlbulkcopy = new SqlBulkCopy((SqlConnection)conn))
                {
                    sqlbulkcopy.BatchSize = fltOrders.Count;
                    sqlbulkcopy.DestinationTableName = dt.TableName;//tableName
                    for (var i = 0; i < dt.Columns.Count; i++)
                    {
                        sqlbulkcopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                    }
                    sqlbulkcopy.WriteToServer(dt);
                }
            }

            return Content("OK!");
        }
        #endregion

        #region 查询
        /// <summary>
        /// 查询(单表)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QuerySingleTable()
        {
            using (var conn = DapperHelper.CreateDbConnection("DapperDemo1DB"))
            {
                var fltOrder = conn.Query<TFltOrder>(@"SELECT order_no AS OrderNo,payment_amt AS PaymentAmt, created_time AS CreatedTime, * FROM dbo.t_flt_order WHERE id= @Id;", new { @Id = 10 });
            }

            return Content("OK!");
        }

        /// <summary>
        /// 查询(多表)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueryMultiTable()
        {
            //查询订单id为10和11的订单信息，包括：订单号，订单状态，乘机人姓名和乘机人性别
            using (var conn = DapperHelper.CreateDbConnection("DapperDemo1DB"))
            {
                var query = conn.Query<TFltOrderPassenger, TFltOrder, TFltOrderPassenger>(@"SELECT orderPassenger.passenger_name AS PassengerName, orderPassenger.passenger_gender AS PassengerGender,fltOrder.order_no AS OrderNo, fltOrder.status
                    FROM dbo.t_flt_order_passenger (NOLOCK) AS orderPassenger
                        LEFT JOIN dbo.t_flt_order (NOLOCK) AS fltOrder
                            ON fltOrder.id = orderPassenger.order_id
                    WHERE fltOrder.order_no IN ( '201801170009', '201801170010' );", (orderPassenger, fltOrder) =>
                {
                    orderPassenger.FltOrder = fltOrder;
                    return orderPassenger;
                }, splitOn: "OrderNo");
                var rs = query.ToList();
            }

            return Content("OK!");
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update()
        {
            using (var conn = DapperHelper.CreateDbConnection("DapperDemo1DB"))
            {
                var rs = conn.Execute(@"UPDATE dbo.t_flt_order SET payment_amt= 2000, created_time= GETDATE() WHERE id= @Id;", new { @Id = 2 });
            }

            return Content("OK!");
        }
        #endregion

        #region 删除
        /// <summary>
        /// 单条删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteSingle()
        {
            using (var conn = DapperHelper.CreateDbConnection("DapperDemo1DB"))
            {
                var rs = conn.Execute(@"DELETE FROM dbo.t_flt_order WHERE order_no= @OrderNo;", new { @OrderNo = "201801170019" });
            }

            return Content("OK!");
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteBatch()
        {
            var orderNos = new List<string> { "201801170018", "201801170017" };
            using (var conn = DapperHelper.CreateDbConnection("DapperDemo1DB"))
            {
                var rs = conn.Execute(@"DELETE FROM dbo.t_flt_order WHERE order_no in @OrderNos;", new { @OrderNos = orderNos });
            }

            return Content("OK!");
        }
        #endregion

        #region 事务
        /// <summary>
        /// 事务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DoTransaction()
        {
            using (var conn = DapperHelper.CreateDbConnection("DapperDemo1DB"))
            {
                var trans = conn.BeginTransaction();
                try
                {
                    var r1 = conn.Execute("DELETE FROM dbo.t_flt_order WHERE order_no= @OrderNo;", new { @OrderNo = "201801170001" }, trans);

                    throw new Exception("测试事务!");

                    var r2 = conn.Execute("DELETE FROM dbo.t_flt_order WHERE order_no= @OrderNo;", new { @OrderNo = "201801170002" }, trans);

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
            }

            return Content("OK!");
        }
        #endregion

        #region 调用存储过程
        /// <summary>
        /// 存储过程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DoStoredProcs()
        {
            using (var conn = DapperHelper.CreateDbConnection("DapperDemo1DB"))
            {
                var rs = conn.Query<TFltOrder>("usp_GetFltOrderByOrderNo", new { @OrderNo = "201801170001" }, commandType: CommandType.StoredProcedure);
            }

            return Content("OK!");
        }
        #endregion

        /// <summary>
        /// 多结果集
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MultiResults()
        {
            using (var conn = DapperHelper.CreateDbConnection("DapperDemo1DB"))
            {
                using (var multi = conn.QueryMultiple(@"SELECT order_no AS OrderNo, payment_amt AS PaymentAmt, created_time AS CreatedTime, * FROM dbo.t_flt_order;
SELECT order_id AS OrderId, passenger_name AS PassengerName, passenger_gender AS PassengerGender, created_time AS CreatedTime, * FROM dbo.t_flt_order_passenger;"))
                {
                    var fltOrders = multi.Read<TFltOrder>().ToList();
                    var orderPassengers = multi.Read<TFltOrderPassenger>().ToList();
                }
            }

            return Content("OK!");
        }

        /// <summary>
        /// 动态参数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DoDynamicParameters()
        {
            // 主要用于sql拼接
            var fltOrder = new TFltOrder { OrderNo = "201801170002" };

            var paras = new DynamicParameters();
            var sbSql = new StringBuilder();
            sbSql.Append("SELECT order_no AS OrderNo, payment_amt AS PaymentAmt, created_time AS CreatedTime, * FROM dbo.t_flt_order WHERE 1=1");

            if (!fltOrder.OrderNo.IsNullOrEmpty())
            {
                sbSql.Append(" AND order_no= @OrderNo");
                paras.Add("OrderNo", fltOrder.OrderNo, DbType.String);
            }

            using (var conn = DapperHelper.CreateDbConnection("DapperDemo1DB"))
            {
                var query = conn.Query<TFltOrder>(sbSql.ToString(), paras);
                var rs = query.ToList();
            }

            return Content("OK!");
        }

        /// <summary>
        /// 多数据库查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DoMultiDbQuery()
        {
            using (var conn = DapperHelper.CreateDbConnection("DapperDemo2DB"))
            {
                var query = conn.Query<TFltOrder>(@"SELECT order_no AS OrderNo,payment_amt AS PaymentAmt, created_time AS CreatedTime, * FROM dbo.t_flt_order WHERE order_no= @OrderNo;", new { @OrderNo = "201801170002" });
                var fltOrder = query.FirstOrDefault();
            }

            return Content("OK!");
        }

    }
}