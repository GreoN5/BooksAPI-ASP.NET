using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
	public class Hashtag
	{
		[Key]
		public int ID { get; set; }

		[Required]
		[Column(TypeName = "varchar(1)")]
		public string Prefix { get; private set; } = "#";

		[Required]
		[Column(TypeName = "varchar(150)")]
		public string Content { get; set; }
	}
}
