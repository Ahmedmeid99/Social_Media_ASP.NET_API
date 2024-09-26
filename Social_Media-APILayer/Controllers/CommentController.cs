using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Social_Media_APILayer.Data;
using Social_Media_APILayer.Dtos.Comment;
using Social_Media_APILayer.Models;

namespace Social_Media_APILayer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CommentController : ControllerBase
	{
		// add update delete get getAll

		private readonly AppDbcontext _context;

		public CommentController(AppDbcontext context) // came from program.js whrere the injection 
		{
			_context = context;
		}


		[HttpGet]
		public async Task<ActionResult<IEnumerable<CommentsView>>> GetPostComments()
		{
			try
			{
				var comment = await _context.CommentsViews.ToListAsync();

				if (comment == null)
				{
					return NoContent();
				}

				return Ok(comment);
			}
			catch (Exception ex)
			{
				// Handle exception or log it
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("post/{postId}")]
		public async Task<ActionResult<IEnumerable<CommentsView>>> GetPostCommentsByPostId(int postId)
		{
			try
			{
				var comment = await _context.CommentsViews.Where((c)=> c.PostId == postId).ToListAsync();

				if (comment == null)
				{
					return NoContent();
				}

				return Ok(comment);
			}
			catch (Exception ex)
			{
				// Handle exception or log it
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}


		[HttpPost]
		public async Task<IActionResult> AddComment([FromBody] CommentAddDto dto)
		{

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			// Map DTO to Entity
			var comment = new Comment
			{
				UserId = dto.UserId,
				CommentText = dto.CommentText,
				PostId = dto.PostId,
				CreatedAt = DateTime.Now  // Example: Setting created time
			};

			try
			{
				_context.Comments.Add(comment);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Log the exception (e.g., using a logging framework)
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}

			return CreatedAtAction(nameof(GetCommentById), new { id = comment.CommentId }, comment);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Comment>> GetCommentById(int id)
		{
			var comment = await _context.Comments.FindAsync(id);
			if (comment == null)
			{
				return NotFound();
			}

			return comment;
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCommentById(int id)
		{
			// Find the post by ID
			var comment = await _context.Comments.FindAsync(id);
			if (comment == null)
			{
				return NotFound();
			}

			// Remove the post
			_context.Comments.Remove(comment);

			// Save changes asynchronously
			await _context.SaveChangesAsync();

			// Return NoContent (204) as a standard response for deletion
			return NoContent();
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCommentById(int id, CommentEditDto dto)
		{

			// Retrieve the comment by ID
			var comment = await _context.Comments.FindAsync(id);
			if (comment == null)
			{
				return NotFound();
			}

			// Update comment properties
			comment.CommentText = dto.CommentText;

			// UpdateAt will update date in database (after trigger)
			try
			{
				_context.Comments.Update(comment);
				await _context.SaveChangesAsync();

			}
			catch (DbUpdateException ex)
			{
				// Log the exception using a logging framework
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}

			return NoContent(); // 204 status code mean the operation is done
		}

		[HttpGet("posts/{postId}/comments")]
		public async Task<ActionResult<IEnumerable<Comment>>> GetPostCommentsByUserId(int postId)
		{
			// Retrieve posts for the specified user
			var postComments = await _context.Comments
										  .Where(post => post.PostId == postId)
										  .ToListAsync();

			// If no posts found, return a 404 NotFound
			if (postComments == null || !postComments.Any())
			{
				return NotFound($"No posts found for UserId: {postId}");
			}

			// Map posts to a DTO if needed
			var comments = postComments.Select(comment => new Comment
			{
				CommentId = comment.CommentId,
				PostId = comment.PostId,
				UserId = comment.UserId,
				CommentText = comment.CommentText,
				UpdatedAt = comment.UpdatedAt
				// Include other fields as needed
			}).ToList();

			// Return the list of user posts
			return Ok(comments);
		}


		[HttpGet("users/{userId}/comments")]
		public async Task<ActionResult<IEnumerable<Post>>> GetUserPostsByUserId(int userId)
		{
			// Retrieve posts for the specified user
			var userComments = await _context.Comments
										  .Where(comment => comment.UserId == userId)
										  .ToListAsync();

			// If no posts found, return a 404 NotFound
			if (userComments == null || !userComments.Any())
			{
				return NotFound($"No posts found for UserId: {userId}");
			}

			// Map posts to a DTO if needed
			var comments = userComments.Select(comment => new Comment
			{
				CommentId= comment.CommentId,
				PostId = comment.PostId,
				UserId = comment.UserId,
				CommentText = comment.CommentText,
				UpdatedAt = comment.UpdatedAt
				// Include other fields as needed
			}).ToList();

			// Return the list of user posts
			return Ok(comments);
		}


	}
}
