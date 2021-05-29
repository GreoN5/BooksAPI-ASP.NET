using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Books.ViewModels.User
{
	public class UserEditVM
	{
		[Required(ErrorMessage = "First name is required!")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last name is required!")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Age is required!")]
		public byte Age { get; set; }
	}
}
