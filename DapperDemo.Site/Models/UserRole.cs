using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DapperDemo.Site.Models
{
    /// <summary>
    /// 用户-角色
    /// </summary>
    public partial class UserRole
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int RoleId { get; set; }

    }
}