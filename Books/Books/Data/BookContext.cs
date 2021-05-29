using Books.Models;
using Microsoft.EntityFrameworkCore;

namespace Books.Data
{
	public class BookContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		//public DbSet<BasicUser> BasicUsers { get; set; }
		//public DbSet<Author> Authors { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<CommentReply> CommentReplies { get; set; }
		public DbSet<BookMateRequest> BookMateRequests { get; set; }

		public BookContext(DbContextOptions<BookContext> options): base(options) { }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<BasicUser>();
			builder.Entity<Author>();

			builder.Entity<User>().HasKey(u => u.Username);
			builder.Entity<User>().Property(u => u.Username).ValueGeneratedNever();
		}
	}
}
