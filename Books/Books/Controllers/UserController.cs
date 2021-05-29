using Books.Models;
using Books.Repositories;
using Books.ViewModels;
using Books.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Books.Controllers
{
	[ApiController]
	[AllowAnonymous]
	[Route("[controller]")]
	public class UserController : Controller
	{
		private readonly UserRepository _userRepository;

		public UserController(UserRepository repository)
		{
			this._userRepository = repository;
		}

		[HttpGet]
		[Authorize(Roles = "User")]
		[Route("MyProfile/{username}")]
		public async Task<IActionResult> GetProfile(string username)
		{
			User user = await _userRepository.GetUserProfileAsync(username);

			if (user != null)
			{
				return Ok(user);
			}

			return NotFound("User not found!");
		}

		[HttpPost]
		[AllowAnonymous]
		[Route("Registration")]
		public async Task<IActionResult> RegisterAsync([FromBody] UserRegistrationVM registrationUser)
		{
			User user = await this._userRepository.RegistrationAsync(registrationUser);

			if (user == null)
			{
				return NotFound("User not found!");
			}

			if (user.Username == null)
			{
				return StatusCode(409, $"Already existing user with username \"{registrationUser.Username}\"!");
			}

			if (user.EmailAddress == null)
			{
				return StatusCode(410, $"Already existing user with email \"{registrationUser.EmailAddress}\"!");
			}

			return Ok(user);
		}

		[HttpPost]
		[AllowAnonymous]
		[Route("Login")]
		public async Task<IActionResult> LoginAsync([FromBody] UserLoginVM loginUser)
		{
			User user = await this._userRepository.LoginAsync(loginUser);

			if (user != null)
			{
				var token = this._userRepository.GenerateJWTToken(user);

				return Ok(new { Token = token, User = user, Role = user.Role });
			}

			return NotFound("User not found!");
		}

		[HttpPut]
		[Authorize(Roles = "User")]
		[Route("MyProfile/{username}/ChangeInformation")]
		public async Task<IActionResult> ChangeProfileInfoAsync(string username, [FromBody] UserEditVM userEdit)
		{
			User user = await this._userRepository.GetUserProfileAsync(username);

			if (user == null)
			{
				return NotFound("User not found!");
			}

			await this._userRepository.ChangeUserProfileInfoAsync(username, userEdit);

			return Ok(new { message = "User information changed successfully!", userProfileInfo = user } );
		}

		[HttpPut]
		[Authorize(Roles = "User")]
		[Route("MyProfile/{username}/ChangeUsername")]
		public async Task<IActionResult> ChangeUsernameAsync(string username, [FromBody] string newUsername)
		{
			User user = await this._userRepository.GetUserProfileAsync(username);

			if (user == null)
			{
				return NotFound("User not found!");
			}

			if (await this._userRepository.ChangeUsernameAsync(username, newUsername))
			{
				return Ok($"Username successfully changed to \"{newUsername}\"!");
			}

			return BadRequest($"Username \"{newUsername}\" already in use!");
		}

		[HttpPut]
		[Authorize(Roles = "User")]
		[Route("MyProfile/{username}/ChangePassword")]
		public async Task<IActionResult> ChangePasswordAsync(string username, [FromBody] UserPasswordEdit passwordEdit)
		{
			User user = await this._userRepository.GetUserProfileAsync(username);

			if (user == null)
			{
				return NotFound("User not found!");
			}

			if (await this._userRepository.ChangePasswordAsync(username, passwordEdit))
			{
				return Ok("Your password has been successfully changed!");
			}

			return BadRequest("Password mismatch!");
		}

		[HttpPut]
		[Authorize(Roles = "User")]
		[Route("MyProfile/{username}/ChangeEmailAddress")]
		public async Task<IActionResult> ChangeEmailAddressAsync(string username, [FromBody] string newEmailAddress)
		{
			User user = await this._userRepository.GetUserProfileAsync(username);

			if (user == null)
			{
				return NotFound("User not found!");
			}

			if (await this._userRepository.ChangeEmailAddressAsync(username, newEmailAddress))
			{
				return Ok($"Email successfully changed to \"{newEmailAddress}\"!");
			}

			return BadRequest($"Email \"{newEmailAddress}\" already in use!");
		}
	}
}
