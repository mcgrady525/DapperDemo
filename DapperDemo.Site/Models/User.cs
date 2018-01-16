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
    //[Table("t_sys_rights_user")]
    public partial class User
    {
        #region 基本属性
        public virtual int Id { get; set; }

        //[Column("user_id")]
        public virtual string UserId { get; set; }

        //[Column("user_name")]
        public virtual string UserName { get; set; }

        public virtual string Email { get; set; }

        public virtual string Address { get; set; }

        //[Column("enable_flag")]
        public virtual bool? EnableFlag { get; set; } 
        #endregion

        /// <summary>
        /// 用户拥有的角色名称
        /// </summary>
        //[IgnoreSelect]
        //[NotMapped]
        public string RoleName { get; set; }

    }
}