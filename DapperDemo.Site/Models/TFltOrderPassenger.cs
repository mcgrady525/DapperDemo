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
	[Table("dbo.t_flt_order_passenger")]
	public partial class TFltOrderPassenger
	{
		public TFltOrderPassenger()
		{
			
		}

		/// <summary>
		/// id
		/// </summary>
		[Column("id", ColumnCategory=Category.IdentityKey)]
		public long Id { get; set; }
		
		/// <summary>
		/// order_id
		/// </summary>
		[Column("order_id")] 
		public long? OrderId { get; set; }
		
		/// <summary>
		/// passenger_name
		/// </summary>
		[Column("passenger_name")] 
		public string PassengerName { get; set; }
		
		/// <summary>
		/// passenger_gender
		/// </summary>
		[Column("passenger_gender")] 
		public string PassengerGender { get; set; }
		
		/// <summary>
		/// created_time
		/// </summary>
		[Column("created_time")] 
		public DateTime? CreatedTime { get; set; }

        #region ¿©’π Ù–‘
        public TFltOrder FltOrder { get; set; }

        #endregion

    }
}