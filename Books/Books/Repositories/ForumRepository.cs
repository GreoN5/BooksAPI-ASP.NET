using Books.Data;
using Books.Models;
using Books.ViewModels.Post;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Books.Repositories
{
	public class ForumRepository
	{
		private readonly BookContext _context;

		public ForumRepository(BookContext context)
		{
			_context = context;
		}

		public async Task<Post> SeePostAsync(int postID)
		{
			return await GetPostAsync(postID);
		}

		public async Task<List<Post>> GetPublishedPostsAsync(string username)
		{
			User user = await GetUserAsync(username);

			if (user != null)
			{
				return user.Posts.FindAll(p => p.Status == PostStatus.Published);
			}

			return null;
		}

		public async Task<List<Post>> GetSavedForLaterPostsAsync(string username)
		{
			User user = await GetUserAsync(username);

			if (user != null)
			{
				return user.Posts.FindAll(p => p.Status == PostStatus.SavedForLater);
			}

			return null;
		}

		public async Task<List<Post>> GetSharedPostsAsync(string username)
		{
			User user = await GetUserAsync(username);

			if (user != null)
			{
				return user.Posts.FindAll(p => p.Status == PostStatus.Shared);
			}

			return null;
		}

		public async Task<Post> CreateNewPostAsync(string username, PostVM post)
		{
			User user = await GetUserAsync(username);

			if (user != null)
			{
				Post newPost = new Post()
				{
					Title = post.Title,
					AuthorUsername = username,
					TimePosted = DateTime.Now,
					Content = post.Content,
					Status = PostStatus.Published
				};

				foreach (var hastag in post.Hashtags)
				{
					newPost.Hashtags.Add(hastag);
				}

				user.Posts.Add(newPost);

				await this._context.Posts.AddAsync(newPost);
				await this._context.SaveChangesAsync();

				return newPost;
			}

			return null;
		}

		public async Task<Post> EditPostAsync(int postID, PostVM postEdit)
		{
			Post post = await GetPostAsync(postID);

			if (post != null)
			{
				post.Title = postEdit.Title;
				post.Content = postEdit.Content;
				post.LastTimeEdited = DateTime.Now;

				await this._context.SaveChangesAsync();

				return post;
			}
			// if there is an issue while editing, it will return the unedited version of the post
			return post; // or in some cases null if the post was not found
		}

		public async Task<bool> DeletePostAsync(int postID)
		{
			//User user = await GetUserAsync(username);
			Post post = await GetPostAsync(postID);

			if (post != null)
			{
				//user.Posts.Remove(post);

				this._context.Posts.Remove(post);
				await this._context.SaveChangesAsync();

				return true;
			}

			return false;
		}

		public async Task<bool> RateUpPostAsync(int postID)
		{
			Post post = await GetPostAsync(postID);

			if (post != null)
			{
				post.PostRating++;
				await this._context.SaveChangesAsync();

				return true;
			}

			return false;
		}

		public async Task<bool> RateDownPostAsync(int postID)
		{
			Post post = await GetPostAsync(postID);

			if (post != null)
			{
				post.PostRating--;
				await this._context.SaveChangesAsync();

				return true;
			}

			return false;
		}

		public async Task<bool> AddCommentToPostAsync(string username, int postID, CommentVM comment)
		{
			User user = await GetUserAsync(username);
			Post post = await GetPostAsync(postID);

			if (user != null && post != null)
			{
				Comment newComment = new Comment()
				{
					AuthorUsername = username,
					TimePosted = DateTime.Now,
					Content = comment.Content
				};

				post.Comments.Add(newComment);

				await this._context.Comments.AddAsync(newComment);
				await this._context.SaveChangesAsync();

				return true;
			}

			return false;
		}

		public async Task<bool> DeleteCommentFromPostAsync(int postID, int commentID)
		{
			Post post = await GetPostAsync(postID);
			Comment comment = await GetCommentAsync(commentID);

			if (post != null && comment != null)
			{
				post.Comments.Remove(comment);

				this._context.Comments.Remove(comment);
				await this._context.SaveChangesAsync();

				return true;
			}

			return false;
		}

		public async Task<Comment> EditCommentInPostAsync(int commentID, CommentVM newComment)
		{
			Comment comment = await GetCommentAsync(commentID);

			if (comment != null)
			{
				comment.Content = newComment.Content;
				await this._context.SaveChangesAsync();

				return comment;
			}

			return null;
		}

		public async Task<bool> RateUpCommentAsync(int commentID)
		{
			Comment comment = await GetCommentAsync(commentID);

			if (comment != null)
			{
				comment.CommentRating++;
				await this._context.SaveChangesAsync();

				return true;
			}

			return false;
		}

		public async Task<bool> RateDownCommentAsync(int commentID)
		{
			Comment comment = await GetCommentAsync(commentID);

			if (comment != null)
			{
				comment.CommentRating--;
				await this._context.SaveChangesAsync();

				return true;
			}

			return false;
		}

		public async Task<bool> SavePostForLaterAsync(string username, int postID)
		{
			//User user = await GetUserAsync(username);
			Post post = await GetPostAsync(postID);

			if (post != null)
			{
				post.Status = PostStatus.SavedForLater;
				await this._context.SaveChangesAsync();

				return true;
			}

			return false;
		}

		public async Task<bool> SharePostToYourPageProfileAsync(string username, int postID)
		{
			//User user = await GetUserAsync(username);
			Post post = await GetPostAsync(postID);

			if (post != null)
			{
				post.Status = PostStatus.Shared;
				await this._context.SaveChangesAsync();

				return true;
			}

			return false;
		}

		public async Task<bool> AddCommentReplyToCommentAsync(string username, int commentID, CommentVM commentReply)
		{
			User user = await GetUserAsync(username);
			Comment comment = await GetCommentAsync(commentID);

			if (user != null && comment != null)
			{
				CommentReply newCommentReply = new CommentReply()
				{
					AuthorUsername = username,
					TimePosted = DateTime.Now,
					Content = comment.Content
				};

				comment.CommentReplies.Add(newCommentReply);
				await this._context.SaveChangesAsync();

				return true;
			}

			return false;
		}

		public async Task<bool> RateUpCommentReplyAsync(int commentReplyID)
		{
			CommentReply commentReply = await GetCommentReplyAsync(commentReplyID);

			if (commentReply != null)
			{
				commentReply.CommentReplyRating++;
				await this._context.SaveChangesAsync();

				return true;
			}

			return false;
		}

		public async Task<bool> RateDownCommentReplyAsync(int commentReplyID)
		{
			CommentReply commentReply = await GetCommentReplyAsync(commentReplyID);

			if (commentReply != null)
			{
				commentReply.CommentReplyRating--;
				await this._context.SaveChangesAsync();

				return true;
			}

			return false;
		}

		private async Task<User> GetUserAsync(string username)
		{
			return await this._context.Users.FindAsync(username);
		}

		private async Task<Post> GetPostAsync(int postID)
		{
			return await this._context.Posts.FindAsync(postID);
		}

		private async Task<Comment> GetCommentAsync(int commentID)
		{
			return await this._context.Comments.FindAsync(commentID);
		}

		private async Task<CommentReply> GetCommentReplyAsync(int commentReplyID)
		{
			return await this._context.CommentReplies.FindAsync(commentReplyID);
		}
	}
}
