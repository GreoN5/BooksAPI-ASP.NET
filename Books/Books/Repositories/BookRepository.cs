using Books.Data;
using Books.Models;
using Books.ViewModels.Book;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Books.Repositories
{
	public class BookRepository
	{
		private readonly BookContext _context;

		public BookRepository(BookContext context)
		{
			_context = context;
		}

		public List<Book> GetAllBooks()
		{
			return _context.Books.ToList();
		}

		public List<Book> GetAllWantBooks()
		{
			return _context.Books.Where(books => books.Status == BookStatus.Want).ToList();
		}

		public List<Book> GetAllHaveBooks()
		{
			return _context.Books.Where(books => books.Status == BookStatus.Have).ToList();
		}

		public List<Book> GetAllReadBooks()
		{
			return _context.Books.Where(books => books.Status == BookStatus.Read).ToList();
		}

		public List<Book> GetAllReadingBooks()
		{
			return _context.Books.Where(books => books.Status == BookStatus.Reading).ToList();
		}

		public bool ChangePagesReadOnBook(string bookTitle, int pages)
		{
			Book changedBook = GetBookByTitle(bookTitle);

			if (changedBook != null)
			{
				changedBook.PagesRead = pages;

				return true;
			}

			return false;
		}

		public bool ChangeBookStatus(string bookTitle, string status)
		{
			Book changedBook = GetBookByTitle(bookTitle);

			if (changedBook != null)
			{
				switch (status)
				{
					case "Want": changedBook.Status = BookStatus.Want;
						break;
					case "Have": changedBook.Status = BookStatus.Have;
						break;
					case "Read": changedBook.Status = BookStatus.Read;
						break;
					case "Reading": changedBook.Status = BookStatus.Reading;
						break;
					default: changedBook.Status = BookStatus.None;
						break;
				}

				return true;
			}

			return false;
		}

		public bool AddBookToShelf(string username, BookVM newBook)
		{
			if (!CheckIfBookAlreadyExistsInShelf(username, newBook)) 
			{

			}

			return false;
		}

		public bool RemoveBookFromShelf(string username, string bookTitle)
		{
			User bookUser = GetUserByUsername(username);
			Book removedBook = GetBookByTitle(bookTitle);

			if (bookUser != null && removedBook != null)
			{
				bookUser.BookShelf.Remove(removedBook);
				_context.Books.Remove(removedBook);

				_context.SaveChanges();

				return true;
			}

			return false;
		}

		private bool CheckIfBookAlreadyExistsInShelf(string username, BookVM existingBook)
		{
			var user = GetUserByUsername(username);
			var book = user.BookShelf.Find(book => book.Title == existingBook.Title);

			if (book != null)
			{
				return true;
			}

			return false;
		}

		private Book GetBookByTitle(string bookTitle)
		{
			return this._context.Books.Find(bookTitle);
		}

		private User GetUserByUsername(string username)
		{
			return this._context.Users.Find(username);
		}
	}
}
