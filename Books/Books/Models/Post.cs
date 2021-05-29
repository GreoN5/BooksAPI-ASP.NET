using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Books.Models
{
	public class Post
	{
		[Key]
		public int ID { get; set; }

		[Column(TypeName = "varchar(250)")]
		[Required]
		public string Title { get; set; }

		[Column(TypeName = "varchar(250)")]
		[Required]
		public string AuthorUsername { get; set; }

		[Column(TypeName = "datetime2")]
		[Required]
		public DateTime TimePosted { get; set; }

		[Column(TypeName = "datetime2")]
		public DateTime LastTimeEdited { get; set; } = DateTime.MinValue;

		[Column(TypeName = "varchar(MAX)")]
		[Required]
		public string Content { get; set; }

		[Column(TypeName = "int")]
		public int PostRating { get; set; } = 0;

		[Column(TypeName = "int")]
		public int PostShares { get; set; } = 0;

		[Column(TypeName = "varchar(50)")]
		public PostStatus Status { get; set; }

		public List<Hashtag> Hashtags { get; private set; } = new List<Hashtag>();
		public List<Comment> Comments { get; private set; } = new List<Comment>();
	}
}
