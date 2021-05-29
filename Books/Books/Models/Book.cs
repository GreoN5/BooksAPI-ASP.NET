using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Books.Models
{
	public class Book
	{
		[Key]
		public int ID { get; set; }

		[Required]
		[Column(TypeName = "varchar(250)")]
		public string Title { get; set; }
		
		[Required]
		[Column(TypeName = "int")]
		public int Pages { get; set; }

		[Required]
		[Column(TypeName = "varchar(50)")]
		public BookStatus Status { get; set; }

		[Column(TypeName = "int")]
		public int PagesRead { get; set; }
	}
}
