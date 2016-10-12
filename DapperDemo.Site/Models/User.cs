using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DapperDemo.Site.Models
{
    /// <summary>
    /// 用户
    /// </summary>
    [Table("")]
    public partial class User
    {
        #region 基本属性
        [Column("")]
        public int Id { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public bool? EnableFlag { get; set; } 
        #endregion

        /// <summary>
        /// 用户拥有的角色名称
        /// </summary>
        public string RoleName { get; set; }

        public string user_id { get; set; }

    }
}