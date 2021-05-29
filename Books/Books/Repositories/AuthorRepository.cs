using Books.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Repositories
{
	public class AuthorRepository
	{
		private readonly BookContext _context;

		public AuthorRepository(BookContext context)
		{
			this._context = context;
		}
	}
}
