using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Social_Media_APILayer.Data;
using Social_Media_APILayer.Dtos.UserReaction;
using Social_Media_APILayer.Models;

namespace Social_Media_APILayer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserReactionController : ControllerBase
	{
		// add update delete get getAll

		private readonly AppDbcontext _context;

		public UserReactionController(AppDbcontext context) // came from program.js whrere the injection 
		{
			_context = context;
		}

		[HttpPost]
		public async Task<IActionResult> AddUserReaction([FromBody] UserReactionAddDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			// Step 1: Check if the user has already reacted to this post
			var existingReaction = await _context.UserReactions
												 .FirstOrDefaultAsync(r => r.UserId == dto.UserId && r.PostId == dto.PostId);

			// Step 2: If a previous reaction exists, remove it
			if (existingReaction != null)
			{
				_context.UserReactions.Remove(existingReaction);
				// You could also reduce the previous reaction count here, if necessary
			}

			// Step 3: Create a new reaction
			var userReaction = new UserReaction
			{
				PostId = dto.PostId,
				UserId = dto.UserId,
				ReactionTypeId = dto.ReactionTypeId,
				CreatedAt = DateTime.Now
			};

			// Step 4: Add the new reaction
			_context.UserReactions.Add(userReaction);

			try
			{
				// Step 5: Save changes
				await _context.SaveChangesAsync();

				// Return the newly created reaction details
				return CreatedAtAction(nameof(GetUserReactionById), new { id = userReaction.ReactionId }, userReaction);
			}
			catch (Exception ex)
			{
				// Log the exception if needed
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<UserReaction>> GetUserReactionById(int id)
		{
			var UserReaction = await _context.UserReactions.FindAsync(id);
			if (UserReaction == null)
			{
				return NotFound();
			}

			return UserReaction;
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUserReactionById(int id)
		{
			// Find the post by ID
			var UserReaction = await _context.UserReactions.FindAsync(id);
			if (UserReaction == null)
			{
				return NotFound();
			}

			// Remove the post
			_context.UserReactions.Remove(UserReaction);

			// Save changes asynchronously
			await _context.SaveChangesAsync();

			// Return NoContent (204) as a standard response for deletion
			return NoContent();
		}

		[HttpDelete("user/{userId}/post/{postId}")]
		public async Task<IActionResult> DeleteUserReactionByUserId_PostId(int userId,int postId)
		{
			// Find the post by ID
			var UserReaction = await _context.UserReactions.FirstOrDefaultAsync((u)=> u.UserId == userId && u.PostId == postId);
			if (UserReaction == null)
			{
				return NotFound();
			}

			// Remove the post
			_context.UserReactions.Remove(UserReaction);

			// Save changes asynchronously
			await _context.SaveChangesAsync();

			// Return NoContent (204) as a standard response for deletion
			return NoContent();
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateUserReactionById(int id,[FromBody] UserReactionEditDto dto)
		{

			// Retrieve the UserReaction by ID
			var UserReaction = await _context.UserReactions.FindAsync(id);
			if (UserReaction == null)
			{
				return NotFound();
			}

			// Update UserReaction properties
			UserReaction.ReactionTypeId = dto.ReactionTypeId;

			// UpdateAt will update date in database (after trigger)
			try
			{
				_context.UserReactions.Update(UserReaction);
				await _context.SaveChangesAsync();

			}
			catch (DbUpdateException ex)
			{
				// Log the exception using a logging framework
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}

			return NoContent(); // 204 status code mean the operation is done
		}

		[HttpGet("posts/{postId}")]
		public async Task<ActionResult<IEnumerable<Post>>> GetUserReactionByPostId(int postId)
		{
			// Retrieve posts for the specified user
			var UserReactions = await _context.UserReactions
										  .Where(reaction => reaction.PostId == postId)
										  .ToListAsync();

			// If no posts found, return a 404 NotFound
			if (UserReactions == null || !UserReactions.Any())
			{
				return NotFound($"No posts with postId : {postId}");
			}

			// Map posts to a DTO if needed
			var reactions = UserReactions.Select(reaction => new UserReactionAddDto
			{
				UserId=reaction.UserId,
				PostId=reaction.PostId,
				ReactionTypeId=reaction.ReactionTypeId,
				// Include other fields as needed
			}).ToList();

			// Return the list of user posts
			return Ok(reactions);
		}

		[HttpGet("users/{userd}")]
		public async Task<ActionResult<IEnumerable<Post>>> GetUserReactionByUserId(int userd)
		{
			// Retrieve posts for the specified user
			var UserReactions = await _context.UserReactions
										  .Where(reaction => reaction.UserId == userd)
										  .ToListAsync();

			// If no posts found, return a 404 NotFound
			if (UserReactions == null || !UserReactions.Any())
			{
				return NotFound($"No posts with postId : {userd}");
			}

			// Map posts to a DTO if needed
			var reactions = UserReactions.Select(reaction => new UserReactionAddDto
			{
				UserId = reaction.UserId,
				PostId = reaction.PostId,
				ReactionTypeId = reaction.ReactionTypeId,
				// Include other fields as needed
			}).ToList();

			// Return the list of user posts
			return Ok(reactions);
		}
		

	}
}
