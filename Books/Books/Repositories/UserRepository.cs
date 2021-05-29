using Books.Data;
using Books.Models;
using Books.ViewModels;
using Books.ViewModels.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Books.Repositories
{
	public class UserRepository
	{
		private readonly IConfiguration _config;
		private readonly BookContext _context;

		public UserRepository(IConfiguration config, BookContext context)
		{
			_config = config;
			_context = context;
		}

		public async Task<User> RegistrationAsync(UserRegistrationVM userRegistration)
		{
			bool usernameExists = await CheckIfUsernameExistsAsync(userRegistration.Username);
			bool emailExists = await CheckIfEmailExistsAsync(userRegistration.EmailAddress);

			if (usernameExists)
			{
				return GetUserWithoutUsername(userRegistration);
			}

			if (emailExists)
			{
				return GetUserWithoutEmail(userRegistration);
			}

			return await AddUserAsync(userRegistration);
		}

		public async Task<User> LoginAsync(UserLoginVM userLogin)
		{
			User user = await this._context.Users.FirstOrDefaultAsync(u => u.Username == userLogin.Username && u.Password == userLogin.Password);

			if (user != null)
			{
				return user;
			}

			return null;
		}

		public async Task<User> GetUserProfileAsync(string username)
		{
			if (await CheckIfUsernameExistsAsync(username))
			{
				return await GetUserAsync(username);
			}

			return null;
		}

		public async Task<bool> ChangeUserProfileInfoAsync(string username, UserEditVM userEdit)
		{
			if (await CheckIfUsernameExistsAsync(username))
			{
				User user = await GetUserAsync(username);

				user.FirstName = userEdit.FirstName;
				user.LastName = userEdit.LastName;
				user.Age = userEdit.Age;

				await this._context.SaveChangesAsync();

				return true;
			}

			return false;
		}

		public async Task<bool> ChangeUsernameAsync(string username, string newUsername)
		{
			if (await CheckIfUsernameExistsAsync(username) && !await CheckIfUsernameExistsAsync(newUsername))
			{
				User user = await GetUserAsync(username);

				user.Username = newUsername;
				await this._context.SaveChangesAsync();

				return true;
			}

			return false;
		}

		public async Task<bool> ChangePasswordAsync(string username, UserPasswordEdit passwordEdit)
		{
			User user = await GetUserAsync(username);

			if (user != null && 
				CheckIfPasswordsMatch(user.Password, passwordEdit.OldPassword) &&
				CheckIfPasswordsMatch(passwordEdit.NewPassword, passwordEdit.ConfirmNewPassword))
			{
				user.Password = passwordEdit.NewPassword;
				await this._context.SaveChangesAsync();

				return true;
			}

			return false;
		}

		public async Task<bool> ChangeEmailAddressAsync(string username, string newEmail)
		{
			if (await CheckIfUsernameExistsAsync(username) && !await CheckIfEmailExistsAsync(newEmail))
			{
				User user = await GetUserAsync(username);

				user.EmailAddress = newEmail;
				await this._context.SaveChangesAsync();

				return true;
			}

			return false;
		}

		public async Task<bool> FollowAuthor(string authorUsername, string username)
		{
			if (await CheckIfUsernameExistsAsync(authorUsername) && await CheckIfUsernameExistsAsync(username))
			{
				Author author = (Author)await GetUserAsync(authorUsername);
				User user = await GetUserAsync(username);

				author.Followers.Add(user);
				this._context.SaveChanges();

				return true;
			}

			return false;
		}

		public string GenerateJWTToken(User user)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
			var creditentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim("username", user.Username),
				new Claim("role", user.Role),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var token = new JwtSecurityToken(issuer: _config["Jwt:Issuer"], 
				audience: _config["Jwt:Audience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(30),
				signingCredentials: creditentials);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		private async Task<bool> CheckIfUsernameExistsAsync(string username)
		{
			return await this._context.Users.AnyAsync(u => u.Username == username);
		}

		private async Task<bool> CheckIfEmailExistsAsync(string email)
		{
			return await this._context.Users.AnyAsync(u => u.EmailAddress == email);
		}

		private async Task<User> GetUserAsync(string username)
		{
			return await this._context.Users.FirstAsync(u => u.Username == username);
		}

		private async Task<User> AddUserAsync(UserRegistrationVM user)
		{
			BasicUser newUser = new BasicUser()
			{
				Username = user.Username,
				Password = user.Password,
				EmailAddress = user.EmailAddress
			};

			await this._context.Users.AddAsync(newUser);
			await this._context.SaveChangesAsync();

			return newUser;
		}

		private User GetUserWithoutUsername(UserRegistrationVM user)
		{
			return new BasicUser()
			{
				Username = null,
				Password = user.Password,
				EmailAddress = user.EmailAddress
			};
		}

		private User GetUserWithoutEmail(UserRegistrationVM user)
		{
			return new BasicUser()
			{
				Username = user.Username,
				Password = user.Password,
				EmailAddress = null
			};
		}

		private bool CheckIfPasswordsMatch(string password, string matchPassword)
		{
			if (password == matchPassword)
			{
				return true;
			}

			return false;
		}
	}
}
