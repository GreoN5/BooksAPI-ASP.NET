using System.ComponentModel.DataAnnotations;

namespace Books.ViewModels.User
{
	public class UserPasswordEdit
	{
		[Required(ErrorMessage = "Old password is required!")]
		public string OldPassword { get; set; }

		[Required(ErrorMessage = "New password is required!")]
		[RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{6,}$",
		ErrorMessage = "The password should be at least 6 symbols, containing lowercase letter(s), uppercase letter(s) and digits without spaces!")]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "New password confirmation is required!")]
		[RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{6,}$",
		ErrorMessage = "The password should be at least 6 symbols, containing lowercase letter(s), uppercase letter(s) and digits without spaces!")]
		public string ConfirmNewPassword { get; set; }
	}
}
