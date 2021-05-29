using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Books.Models
{
	public class Comment
	{
		[Key]
		public int ID { get; set; }

		[Column(TypeName = "varchar(250)")]
		[Required]
		public string AuthorUsername { get; set; }

		[Column(TypeName = "datetime2")]
		[Required]
		public DateTime TimePosted { get; set; }

		[Column(TypeName = "varchar(MAX)")]
		[Required]
		public string Content { get; set; }

		[Column(TypeName = "int")]
		public int CommentRating { get; set; } = 0;

		public virtual List<CommentReply> CommentReplies { get; set; } = new List<CommentReply>();
	}
}
