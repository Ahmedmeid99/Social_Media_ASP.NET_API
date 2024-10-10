using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Social_Media_APILayer.Data;
using Social_Media_APILayer.Dtos.Comment;
using Social_Media_APILayer.Models;
using Social_Media_APILayer.Models.NewFolder;

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

		[HttpPost]
		public async Task<IActionResult> AddComment([FromBody] CommentAddDto dto)
		{

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (string.IsNullOrEmpty(dto.CommentText) || dto.UserId <= 0 || dto.PostId <= 0)
			{
				return BadRequest("Invalid content or user ID.");
			}

			// Map DTO to Entity
			var comment = new Comment
			{
				UserId = dto.UserId,
				CommentText = dto.CommentText,
				PostId = dto.PostId,
				CreatedAt = DateTime.Now 
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

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCommentById(int id, CommentEditDto dto)
		{

			var comment = await _context.Comments.FindAsync(id);
			if (comment == null)
			{
				return NotFound();
			}

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


		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCommentById(int id)
		{
			// Find the post by ID
			var comment = await _context.Comments.FindAsync(id);
			if (comment == null)
			{
				return NotFound();
			}

			try
			{

				_context.Comments.Remove(comment);

				await _context.SaveChangesAsync();

				// Return NoContent (204) as a standard response for deletion
				return NoContent();
			}
			catch (Exception ex)
			{
				// Handle exception or log it
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
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

		[HttpGet("{id}")]
		public async Task<ActionResult<Comment>> GetCommentById(int id)
		{
			try
			{

				var comment = await _context.Comments.FindAsync(id);
				if (comment == null)
				{
					return NotFound();
				}

				return comment;
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
				var comment = await _context.CommentsViews.Where((c) => c.PostId == postId).ToListAsync();

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

			try
			{
				// Map posts to a DTO if needed
				var comments = postComments.Select(comment => new Comment
				{
					CommentId = comment.CommentId,
					PostId = comment.PostId,
					UserId = comment.UserId,
					CommentText = comment.CommentText,
					UpdatedAt = comment.UpdatedAt

				}).ToList();

				return Ok(comments);
			}
			catch (Exception ex)
			{
				// Handle exception or log it
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}


		[HttpGet("users/{userId}/comments")]
		public async Task<ActionResult<IEnumerable<Post>>> GetUserPostsByUserId(int userId)
		{
			var userComments = await _context.Comments
										  .Where(comment => comment.UserId == userId)
										  .ToListAsync();

			if (userComments == null || userComments.Count == 0)
			{
				return NotFound($"No posts found for UserId: {userId}");
			}

			try
			{
				var comments = userComments.Select(comment => new Comment
				{
					CommentId = comment.CommentId,
					PostId = comment.PostId,
					UserId = comment.UserId,
					CommentText = comment.CommentText,
					UpdatedAt = comment.UpdatedAt

				}).ToList();

				return Ok(comments);
			}
			catch (Exception ex)
			{
				// Handle exception or log it
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}


	}
}
