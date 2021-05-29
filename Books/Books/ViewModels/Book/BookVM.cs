using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Books.ViewModels.Book
{
	public class BookVM
	{
		[Required]
		public string Title { get; set; }

		[Required]
		public string Author { get; set; }

		[Required]
		public int Pages { get; set; }
	}
}
