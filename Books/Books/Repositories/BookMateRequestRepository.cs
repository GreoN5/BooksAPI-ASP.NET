using Books.Data;
using Books.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Repositories
{
	public class BookMateRequestRepository
	{
		private readonly BookContext _context;

		public BookMateRequestRepository(BookContext context)
		{
			_context = context;
		}

		public async Task<bool> SendRequestAsync(string usernameSender, string usernameReceiver)
		{
			User sender = await GetUserAsync(usernameSender);
			User receiver = await GetUserAsync(usernameReceiver);

			if (sender != null && receiver != null)
			{
				BookMateRequest request = new BookMateRequest
				{
					UsernameSender = sender.Username,
					UsernameReceiver = receiver.Username
				};

				sender.BookMateRequests.Add(request);
				receiver.BookMateRequests.Add(request);

				await this._context.BookMateRequests.AddAsync(request);
				await this._context.SaveChangesAsync();

				return true;
			}

			return false;
		}

		public async Task<bool> AcceptRequestAsync(string usernameSender, string usernameReceiver)
		{
			User sender = await GetUserAsync(usernameSender);
			User receiver = await GetUserAsync(usernameReceiver);

			if (sender != null && receiver != null)
			{
				var request = await this._context.BookMateRequests.FirstOrDefaultAsync(u => u.UsernameSender == usernameSender && u.UsernameReceiver == usernameReceiver);

				if (request != null)
				{
					request.IsAccepted = true;

					sender.BookMates.Add(receiver);
					sender.BookMateRequests.Remove(request);

					receiver.BookMates.Add(sender);
					receiver.BookMateRequests.Remove(request);

					this._context.BookMateRequests.Remove(request);
					await this._context.SaveChangesAsync();

					return true;
				}
			}

			return false;
		}

		public async Task<bool> CancelRequestSendAsync(string usernameSender, string usernameReceiver)
		{
			User sender = await GetUserAsync(usernameSender);
			User receiver = await GetUserAsync(usernameReceiver);

			if (sender != null && receiver != null)
			{
				var request = await this._context.BookMateRequests.FirstOrDefaultAsync(u => u.UsernameSender == usernameSender && u.UsernameReceiver == usernameReceiver);

				if (request != null)
				{
					sender.BookMateRequests.Remove(request);
					receiver.BookMateRequests.Remove(request);

					this._context.BookMateRequests.Remove(request);
					await this._context.SaveChangesAsync();

					return true;
				}
			}

			return false;
		}

		private async Task<User> GetUserAsync(string username)
		{
			return await this._context.Users.FindAsync(username);
		}
	}
}
