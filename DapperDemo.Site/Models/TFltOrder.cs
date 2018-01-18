using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DapperDemo.Site.Attributes;

/// <summary>
/// 
/// </summary>
namespace DapperDemo.Site.Models
{
	[Table("dbo.t_flt_order")]
	public partial class TFltOrder
	{
		public TFltOrder()
		{
			
		}

		/// <summary>
		/// id
		/// </summary>
		[Column("id", ColumnCategory=Category.IdentityKey)]
		public long Id { get; set; }
		
		/// <summary>
		/// order_no
		/// </summary>
		[Column("order_no")] 
		public string OrderNo { get; set; }
		
		/// <summary>
		/// status
		/// </summary>
		[Column("status")] 
		public string Status { get; set; }
		
		/// <summary>
		/// payment_amt
		/// </summary>
		[Column("payment_amt")] 
		public decimal? PaymentAmt { get; set; }
		
		/// <summary>
		/// created_time
		/// </summary>
		[Column("created_time")] 
		public DateTime? CreatedTime { get; set; }

        public string Remark { get; set; }
		
	}
}