using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Books.Models
{
	public abstract class User
	{
		[Key]
		[Column(TypeName = "varchar(250)")]
		public string Username { get; set; }

		[Required]
		[Column(TypeName = "varchar(250)")]
		public string FirstName { get; set; }

		[Required]
		[Column(TypeName = "varchar(250)")]
		public string LastName { get; set; }

		[Required]
		[Column(TypeName = "varchar(50)")]
		public string Password { get; set; }

		[Required]
		[Column(TypeName = "varchar(150)")]
		public string EmailAddress { get; set; }

		[Required]
		[Column(TypeName = "varchar(500)")]
		public string UserProfileImageURL { get; set; }

		[Required]
		[Column(TypeName = "tinyint")]
		public byte Age { get; set; }

		[Required]
		[Column(TypeName = "varchar(10)")]
		public abstract string Role { get; }

		public List<Post> Posts { get; private set; } = new List<Post>();
		public List<Book> BookShelf { get; private set; } = new List<Book>();
		public List<User> BookMates { get; private set; } = new List<User>();
		public List<BookMateRequest> BookMateRequests { get; private set; } = new List<BookMateRequest>();
	}
}
