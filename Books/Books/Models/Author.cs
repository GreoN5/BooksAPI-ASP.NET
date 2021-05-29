using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Books.Models
{
	public class Author : User
	{
		public override string Role
		{
			get
			{
				return "Author";
			}
		}

		[Required]
		[Column(TypeName = "bit")]
		public bool IsVerified { get; set; }

		public List<User> Followers { get; private set; } = new List<User>();
		public List<Book> PublishedBooks { get; private set; } = new List<Book>();
	}
}
