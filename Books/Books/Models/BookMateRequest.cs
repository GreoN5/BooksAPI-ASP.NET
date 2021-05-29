using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
	public class BookMateRequest
	{
		[Key]
		public int ID { get; set; }

		[Required]
		[Column(TypeName = "varchar(250)")]
		public string UsernameSender { get; set; }

		[Required]
		[Column(TypeName = "varchar(250)")]
		public string UsernameReceiver { get; set; }

		[Column(TypeName = "bit")]
		public bool IsAccepted { get; set; }
	}
}
