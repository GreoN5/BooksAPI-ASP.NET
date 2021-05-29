using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Books.ViewModels.Post
{
	public class CommentVM
	{
		[Required]
		public string Content { get; set; }
	}
}
