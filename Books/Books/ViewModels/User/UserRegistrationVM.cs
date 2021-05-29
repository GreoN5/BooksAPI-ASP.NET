using System.ComponentModel.DataAnnotations;

namespace Books.ViewModels
{
	public class UserRegistrationVM
	{
		[Required(ErrorMessage = "Username is required!")]
		public string Username { get; set; }

		[RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{6,}$",
		ErrorMessage = "The password should be at least 6 symbols, containing lowercase letter(s), uppercase letter(s) and digits without spaces!")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Email is required!")]
		[EmailAddress(ErrorMessage = "Invalid email address!")]
		public string EmailAddress { get; set; }
	}
}
