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

namespace DapperDemo.Site.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 单条插入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertSingle()
        {
            var p = new DynamicParameters(new
            {
                UserId = "zhangsan",
                UserName = "张三111",
                Email = "zhangsan@qq.com",
                Address = "中国北京111",
                EnableFlag = true
            });
            using (var conn= DapperHelper.CreateConnection())
            {
                conn.Execute(@"INSERT INTO dbo.t_sys_rights_user VALUES (@UserId, @UserName, @Email, @Address, @EnableFlag)", p);
            }

            return Content("OK!");
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertBulk()
        {
            var users = new List<User> { };
            for (int i = 0; i < 5; i++)
            {
                users.Add(new User
                {
                    UserId = "userId" + i,
                    UserName = "userName" + i,
                    Email = "email" + i,
                    Address = "address" + i,
                    EnableFlag = true
                });
            }

            using (var conn = DapperHelper.CreateConnection())
            {
                var result = conn.Execute("INSERT INTO dbo.t_sys_rights_user VALUES (@UserId, @UserName, @Email, @Address, @EnableFlag)", users);
            }

            return Content("OK!");
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Query()
        {
            using (var conn = DapperHelper.CreateConnection())
            {
                var list = conn.Query<User>("SELECT id, user_id AS UserId, user_name AS UserName,email,address, enable_flag AS EnableFlag FROM dbo.t_sys_rights_user WHERE user_name= @UserName;", new { @UserName = "麦迪" });
            }

            return Content("OK!");
        }

        /// <summary>
        /// 多表查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueryJoin()
        {
            using (var conn = DapperHelper.CreateConnection())
            {
                var sql = @"SELECT rightsUser.id,rightsUser.user_id AS UserId,rightsUser.user_name AS UserName, role.role_name AS RoleName FROM dbo.t_sys_rights_user AS rightsUser
                                    LEFT JOIN dbo.t_sys_rights_user_role AS userRole ON rightsUser.id = userRole.user_id
                                    LEFT JOIN dbo.t_sys_rights_role AS role ON userRole.role_id = role.id
                                    WHERE rightsUser.id= 1;";
                var r = conn.Query<DapperDemo.Site.Models.User, DapperDemo.Site.Models.UserRole, DapperDemo.Site.Models.Role, DapperDemo.Site.Models.User>(sql, (user, userRole, role) =>
                {
                    user.RoleName = role.RoleName;
                    return user;
                }, splitOn: "id, id").ToList();
            }

            return Content("OK!");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update()
        {
            IDbConnection connection = new SqlConnection("Data Source=.;Initial Catalog=DapperStudyDB;Integrated Security=True;MultipleActiveResultSets=True");
            var result = connection.Execute("UPDATE dbo.t_sys_rights_user SET email= N'mcgrady@qq.com' WHERE id= @Id;", new { Id = 2 });

            return Content("OK!");
        }

        /// <summary>
        /// 单条删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteSingle()
        {
            IDbConnection connection = new SqlConnection("Data Source=.;Initial Catalog=DapperStudyDB;Integrated Security=True;MultipleActiveResultSets=True");
            var result = connection.Execute("DELETE FROM dbo.t_sys_rights_user WHERE id= @Id;", new { Id = 11 });

            return Content("OK!");
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteBulk()
        {
            IDbConnection connection = new SqlConnection("Data Source=.;Initial Catalog=DapperStudyDB;Integrated Security=True;MultipleActiveResultSets=True");
            var ids = new List<int> { 7, 6 };
            var result = connection.Execute("DELETE FROM dbo.t_sys_rights_user WHERE id IN @Ids;", new { Ids = ids });

            return Content("OK!");
        }

        /// <summary>
        /// 事务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TestTransaction()
        {
            using (var conn = DapperHelper.CreateConnection())
            {
                var trans = conn.BeginTransaction();
                try
                {
                    var r1 = conn.Execute("DELETE FROM dbo.t_sys_rights_user WHERE id= @Id;", new { Id = 6 }, trans);

                    throw new Exception("事务测试!");

                    var r2 = conn.Execute("DELETE FROM dbo.t_sys_rights_user WHERE id= @Id;", new { Id = 7 }, trans);

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
            }

            return Content("OK!");
        }

        /// <summary>
        /// 动态参数查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TestDynamicParametersQuery()
        {
            var request = new DapperDemo.Site.Models.User
            {
                UserId = "userId"
            };

            var paras = new DynamicParameters();
            StringBuilder sbSql = new StringBuilder("SELECT users.user_id AS UserId, users.user_name AS UserName, users.enable_flag AS EnableFlag, * FROM dbo.t_sys_rights_user AS users where 1=1");

            if (!string.IsNullOrEmpty(request.UserId))
            {
                sbSql.Append(" and users.user_id LIKE @UserId");
                paras.Add("UserId", "%"+request.UserId+"%");
            }

            using (var conn= DapperHelper.CreateConnection())
            {
                var query = conn.Query<DapperDemo.Site.Models.User>(sbSql.ToString(), paras);
                var result = query.ToList();
            }

            return Content("OK!");
        }



    }
}