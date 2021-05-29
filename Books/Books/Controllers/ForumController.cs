using Books.Models;
using Books.Repositories;
using Books.ViewModels.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Books.Controllers
{
	[ApiController]
	[AllowAnonymous]
	[Route("[controller]")]
	public class ForumController : Controller
	{
		private readonly ForumRepository _forumRepository;

		public ForumController(ForumRepository repository)
		{
			this._forumRepository = repository;
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("SeePost/{postID}")]
		public async Task<IActionResult> SeePostAsync(int postID)
		{
			Post post = await this._forumRepository.SeePostAsync(postID);

			if (post != null)
			{
				return Ok(post);
			}

			return NotFound("Post not found!");
		}

		[HttpGet]
		[Authorize(Roles = "User")]
		[Authorize(Roles = "Author")]
		[Route("PublishedPosts/{username}")]
		public async Task<IActionResult> GetPublishedPostsAsync(string username)
		{
			List<Post> publishedPosts = await this._forumRepository.GetPublishedPostsAsync(username);

			if (publishedPosts != null)
			{
				return Ok(publishedPosts);
			}

			return BadRequest("There are currently no posts to show!");
		}

		[HttpGet]
		[Authorize(Roles = "User")]
		[Authorize(Roles = "Author")]
		[Route("SavedPosts/{username}")]
		public async Task<IActionResult> GetSavedForLaterPostsAsync(string username)
		{
			List<Post> savedForLaterPosts = await this._forumRepository.GetSavedForLaterPostsAsync(username);

			if (savedForLaterPosts != null)
			{
				return Ok(savedForLaterPosts);
			}

			return BadRequest("There are currently no posts to show!");
		}

		[HttpGet]
		[Authorize(Roles = "User")]
		[Authorize(Roles = "Author")]
		[Route("SharedPosts/{username}")]
		public async Task<IActionResult> GetSharedPostsAsync(string username)
		{
			List<Post> sharedPosts = await this._forumRepository.GetSharedPostsAsync(username);

			if (sharedPosts != null)
			{
				return Ok(sharedPosts);
			}

			return BadRequest("There are currently no posts to show!");
		}

		[HttpPost]
		[Authorize(Roles = "User")]
		[Authorize(Roles = "Author")]
		[Route("CreateNewPost")]
		public async Task<IActionResult> CreateNewPostAsync(string username, [FromBody] PostVM newPost)
		{
			Post newCreatedPost = await this._forumRepository.CreateNewPostAsync(username, newPost);

			if (newCreatedPost != null)
			{
				return Ok(newCreatedPost);
			}

			return BadRequest("There was an issue while creating your post!");
		}

		[HttpPost]
		[Authorize(Roles = "User")]
		[Authorize(Roles = "Author")]
		[Route("AddCommentToPost/{username}/{postID}")]
		public async Task<IActionResult> AddCommentToPostAync(string username, int postID, [FromBody] CommentVM newComment)
		{ //return the whole post (for an update version)
			if (await this._forumRepository.AddCommentToPostAsync(username, postID, newComment))
			{
				return Ok("Comment successfully added to post!");
			}

			return BadRequest("Something went wrong when adding your comment!");
		}

		[HttpPut]
		[Authorize(Roles = "User")]
		[Authorize(Roles = "Author")]
		[Route("EditPost/{postID}")]
		public async Task<IActionResult> EditPostAsync(int postID, PostVM postEdit)
		{
			Post post = await this._forumRepository.EditPostAsync(postID, postEdit);

			if (post != null)
			{
				return Ok(post);
			}

			if (post != null && post.LastTimeEdited == DateTime.MinValue)
			{
				return BadRequest("There was an issue while editing your post!");
			}

			return NotFound("Post not found!");
		}

		[HttpPut]
		[Authorize(Roles = "User")]
		[Authorize(Roles = "Author")]
		[Route("RateUpPost/{postID}")]
		public async Task<IActionResult> RateUpPostAsync(int postID)
		{
			if (await this._forumRepository.RateUpPostAsync(postID))
			{
				return Ok("Post rated up!");
			}

			return NotFound("Post not found!");
		}

		[HttpPut]
		[Authorize(Roles = "User")]
		[Authorize(Roles = "Author")]
		[Route("RateDownPost/{postID}")]
		public async Task<IActionResult> RateDownPostAsync(int postID)
		{
			if (await this._forumRepository.RateDownPostAsync(postID))
			{
				return Ok("Post rated down!");
			}

			return NotFound("Post not found!");
		}

		[HttpDelete]
		[Authorize(Roles = "User")]
		[Authorize(Roles = "Author")]
		[Route("DeletePost/{postID}")]
		public async Task<IActionResult> DeletePostAsync(int postID)
		{
			if (await this._forumRepository.DeletePostAsync(postID))
			{
				return Ok("Post deleted successfully");
			}

			return NotFound("Post not found!");
		}

		[HttpDelete]
		[Authorize(Roles = "User")]
		[Authorize(Roles = "Author")]
		[Route("DeleteComment/{postID}/{commentID}")]
		public async Task<IActionResult> DeleteCommentFromPostAsync(int postID, int commentID)
		{
			if (await this._forumRepository.DeleteCommentFromPostAsync(postID, commentID))
			{
				return Ok("Post successfully deleted!");
			}

			return NotFound("Could not find the request post to delete!");
		}
	}
}
