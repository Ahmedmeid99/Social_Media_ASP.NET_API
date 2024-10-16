﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Social_Media_APILayer.Data;
using Social_Media_APILayer.Dtos.Comment;
using Social_Media_APILayer.Dtos.Post;
using Social_Media_APILayer.Models;
using Social_Media_APILayer.Models.Views;

namespace Social_Media_APILayer.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class PostController : ControllerBase
	{
		// add update delete get getAll

		private readonly AppDbcontext _context;

		public PostController(AppDbcontext context) // came from program.js whrere the injection 
		{
			_context = context;
		}

		
		[HttpPost]
		public async Task<IActionResult> AddPost([FromForm] PostAddDto dto)
		{
			if (dto.FormFile != null)
			{
				try
				{
					long fileSizeLimit = 10 * 1024 * 1024; // 10 MB size limit
					if (dto.FormFile.Length > fileSizeLimit)
					{
					    return BadRequest("File size exceeds the allowed limit.");
					}

					using (var memoryStream = new MemoryStream())
					{
					    await dto.FormFile.CopyToAsync(memoryStream);
					    dto.MediaData = memoryStream.ToArray();  // Convert to byte array
					    dto.MediaType = dto.FormFile.ContentType;  // Set MediaType from the file
					}
					}
				catch (Exception ex)
				{
					// Log the exception (e.g., using a logging framework)
					return BadRequest($"File upload failed: {ex.Message}");
				}
			}

			// Check if MediaData is still null and set MediaType to "None"
			if (dto.MediaData == null) dto.MediaType = "None";

			// Validate MediaType if necessary
			var validMediaTypes = new[] { "image/jpeg", "image/png", "video/mp4", "application/pdf", "None" };
			if (!validMediaTypes.Contains(dto.MediaType))
			{
				return BadRequest("Invalid media type. Only specific file types are allowed.");
			}

			// Validate Content and UserId here if necessary
			if (string.IsNullOrEmpty(dto.Content) || dto.UserId <= 0)
			{
				return BadRequest("Invalid content or user ID.");
			}

			// Map DTO to Entity
			var post = new Post
			{
			    UserId = dto.UserId,
			    Content = dto.Content,
			    MediaType = dto.MediaType,
			    MediaData = dto.MediaData,
			    CreatedAt = DateTime.Now  // Example: Setting created time
			};

			try
			{
			    _context.Posts.Add(post);
			    await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
			    // Log the exception (e.g., using a logging framework)
			    return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}

			// Retrieve the created post from the PostsView
			var createdPost = await _context.PostsViews
				.FirstOrDefaultAsync(p => p.PostID == post.PostId);

			if (createdPost == null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Post was created but could not be retrieved.");
			}
			return Ok(createdPost);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdatePostById(int id, PostEditDto dto)
		{
			if (dto.FormFile != null)
			{
				try
				{
					await using (var memoryStream = new MemoryStream())
					{
						await dto.FormFile.CopyToAsync(memoryStream);
						dto.MediaData = memoryStream.ToArray();  // Convert to byte array
						dto.MediaType = dto.FormFile.ContentType;  // Set MediaType from the file
					}
				}
				catch (Exception ex)
				{
					// Log the exception using a logging framework
					return BadRequest($"File upload failed: {ex.Message}");
				}
			}

			// Validate MediaType if necessary
			var validMediaTypes = new[] { "image/jpeg", "image/png", "video/mp4", "application/pdf" };
			if (dto.MediaData != null && !validMediaTypes.Contains(dto.MediaType))
			{
				return BadRequest("Invalid media type. Only specific file types are allowed.");
			}

			// Retrieve the post by ID
			var post = await _context.Posts.FindAsync(id);
			if (post == null)
			{
				return NotFound();
			}

			// Update post properties
			post.Content = dto.Content;
			post.MediaType = dto.MediaType;
			post.MediaData = dto.MediaData;

			// UpdateAt will update date in database (after trigger)
			try
			{
				_context.Posts.Update(post);
				await _context.SaveChangesAsync();

			}
			catch (DbUpdateException ex)
			{
				// Log the exception using a logging framework
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}
			// Retrieve the created post from the PostsView
			var updatedPost = await _context.PostsViews
				.FirstOrDefaultAsync(p => p.PostID == post.PostId);

			if (updatedPost == null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Post was created but could not be retrieved.");
			}

			return Ok(updatedPost);

		}


		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletePostById(int id)
		{
			// Find the post by ID
			var post = await _context.Posts.FindAsync(id);
			if (post == null)
			{
				return NotFound();
			}
			
			// Delete related UserReactions first
			var userReactions = _context.UserReactions.Where(ur => ur.PostId == post.PostId);
			_context.UserReactions.RemoveRange(userReactions);

			// Delete related UserReactions first
			var userComments = _context.Comments.Where(c => c.PostId == post.PostId);
			_context.Comments.RemoveRange(userComments);

			// Remove the post
			_context.Posts.Remove(post);

			// Save changes asynchronously
			await _context.SaveChangesAsync();

			// Return NoContent (204) as a standard response for deletion
			return NoContent();
		}

		
		[HttpGet("{id}")]
		public async Task<ActionResult<Post>> GetPostById(int id)
		{
			var post = await _context.Posts.FindAsync(id);
			if (post == null)
			{
				return NotFound();
			}

			return post;
		}


		[HttpGet]
		public async Task<ActionResult<IEnumerable<PostsView>>> GetPostDetails()
		{
			try
			{
				var posts = await _context.PostsViews.ToListAsync();


				if (posts == null)
				{
					return NoContent();
				}

				return Ok(posts);
			}
			catch (Exception ex)
			{
				// Handle exception or log it
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("user/{userId}/page/{page}/pageSize/{pageSize}")]
		public async Task<ActionResult<IEnumerable<PostsView>>> GetUserPosts(int userId, int page, int pageSize)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

			if (user == null)
			{
				return NotFound();
			}

			var relatedUserPosts = await _context.PostsViews
				.Where(p => _context.UserRelationships
					.Any(r => (r.UserId1 == userId && r.UserId2 == p.UserID) ||
							  (r.UserId2 == userId && r.UserId1 == p.UserID)))
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

		
			return Ok(relatedUserPosts);
		}

		[HttpGet("users/{userId}/posts")]
		public async Task<ActionResult<IEnumerable<Post>>> GetUserPostsByUserId(int userId)
		{
			// Retrieve posts for the specified user
			var userPosts = await _context.PostsViews
										  .Where(post => post.UserID == userId)
										  .ToListAsync();

			// If no posts found, return a 404 NotFound
			if (userPosts == null || !userPosts.Any())
			{
				return NoContent();
			}

			// Return the list of user posts
			return Ok(userPosts);
		}

		[HttpGet("users/{userId}/images")]
		public async Task<ActionResult> GetUserImagesByUserId(int userId)
		{
			// Retrieve posts for the specified user
			var images = await _context.Posts.Select(post => new { post.UserId, post.PostId, post.MediaType , post.MediaData })
										  .Where(post => post.UserId == userId && post.MediaType.StartsWith("image"))
										  .ToListAsync();

			// If no posts found, return a 404 NotFound
			if (images == null || !images.Any())
			{
				return NoContent();
			}

			// Return the list of user posts
			return Ok(images);
		}


		[HttpGet("users/{userId}/videos")]
		public async Task<ActionResult> GetUserVideosByUserId(int userId)
		{
			// Retrieve posts for the specified user
			var videos = await _context.Posts.Select(post => new { post.UserId, post.PostId, post.MediaType, post.MediaData })
										  .Where(post => post.UserId == userId && post.MediaType.StartsWith("video"))
										  .ToListAsync();

			// If no posts found, return a 404 NotFound
			if (videos == null || !videos.Any())
			{
				return NoContent();
			}

			// Return the list of user posts
			return Ok(videos);
		}

		[HttpGet("users/{userId}/files")]
		public async Task<ActionResult> GetUserFilesByUserId(int userId)
		{
			// Retrieve posts for the specified user
			var files = await _context.Posts.Select(post => new { post.UserId, post.PostId, post.MediaType, post.MediaData })
										  .Where(post => post.UserId == userId && post.MediaType.StartsWith("application"))
										  .ToListAsync();

			// If no posts found, return a 404 NotFound
			if (files == null || !files.Any())
			{
				return NoContent();
			}

			// Return the list of user posts
			return Ok(files);
		}


		// Get Random Posts from (frinds, user you follow) -> posts

		[HttpGet("post/{postId}/mediaData")]
		public async Task<ActionResult> GetMediaDataByPostId(int postId)
		{
			var post = await _context.Posts.FirstOrDefaultAsync((p)=> p.PostId == postId);
			if (post == null)
			{
				return NotFound();
			}

			return Ok(new { post.MediaData,post.MediaType});
		}


	}
}
