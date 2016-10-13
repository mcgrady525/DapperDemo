using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using DapperDemo.Site.Models;

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
            IDbConnection connection = new SqlConnection("Data Source=.;Initial Catalog=DapperStudyDB;Integrated Security=True;MultipleActiveResultSets=True");
            var result = connection.Execute("INSERT INTO dbo.t_sys_rights_user VALUES (@UserId, @UserName, @Email, @Address, @EnableFlag)", new
            {
                UserId = "zhangsan",
                UserName = "张三",
                Email = "zhangsan@qq.com",
                Address = "中国北京",
                EnableFlag = true
            });

            return Content("OK!");
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertBulk()
        {
            IDbConnection connection = new SqlConnection("Data Source=.;Initial Catalog=DapperStudyDB;Integrated Security=True;MultipleActiveResultSets=True");
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

            var result = connection.Execute("INSERT INTO dbo.t_sys_rights_user VALUES (@UserId, @UserName, @Email, @Address, @EnableFlag)", users);

            return Content("OK!");
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Query()
        {
            IDbConnection connection = new SqlConnection("Data Source=.;Initial Catalog=DapperStudyDB;Integrated Security=True;MultipleActiveResultSets=True");
            var list = connection.Query<User>("SELECT * FROM dbo.t_sys_rights_user WHERE user_name= @UserName", new { UserName = "麦迪" });

            return Content("OK!");
        }

        /// <summary>
        /// 联表查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueryJoin()
        {
            IDbConnection connection = new SqlConnection("Data Source=.;Initial Catalog=DapperStudyDB;Integrated Security=True;MultipleActiveResultSets=True");
            var sql = @"SELECT rightsUser.id,rightsUser.user_id,rightsUser.user_name, role.role_name FROM dbo.t_sys_rights_user AS rightsUser
                    LEFT JOIN dbo.t_sys_rights_user_role AS userRole ON rightsUser.id = userRole.user_id
                    LEFT JOIN dbo.t_sys_rights_role AS role ON userRole.role_id = role.id
                    WHERE rightsUser.id= 1;";
            var list = connection.Query<dynamic>(sql);
            var result = new List<User>();
            foreach (var row in list)
            {
                //这里是否有办法优化?
                var fields = row as IDictionary<string, object>;
                result.Add(new User 
                {
                    Id= (int)fields["id"],
                    UserId= (string)fields["user_id"],
                    UserName = (string)fields["user_name"],
                    RoleName = (string)fields["role_name"]
                });
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
            var ids = new List<int> {7,6 };
            var result = connection.Execute("DELETE FROM dbo.t_sys_rights_user WHERE id IN @Ids;", new { Ids = ids });

            return Content("OK!");
        }



    }
}