using Books.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Books.ViewModels.Post
{
	public class PostVM
	{
		[Required]
		public string Title { get; set; }

		[Required]
		public string Content { get; set; }

		public List<Hashtag> Hashtags { get; private set; } = new List<Hashtag>();
	}
}
