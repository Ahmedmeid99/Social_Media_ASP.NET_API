using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Social_Media_APILayer.Data;
using Social_Media_APILayer.Dtos.Message;
using Social_Media_APILayer.Models;

namespace Social_Media_APILayer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MessageController : ControllerBase
	{
		// add update delete get getAll

		private readonly AppDbcontext _context;

		public MessageController(AppDbcontext context) // came from program.js whrere the injection 
		{
			_context = context;
		}

		[HttpPost]
		public async Task<IActionResult> AddMessage([FromForm] MessageAddDto dto)
		{

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			// Map DTO to Entity
			var message = new Message
			{
				SenderId = dto.SenderId,
				ReceiverId = dto.ReceiverId,
				MessageText = dto.MessageText,
				SentAt = DateTime.Now  // Example: Setting created time
			};

			try
			{
				_context.Messages.Add(message);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Log the exception (e.g., using a logging framework)
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}

			return CreatedAtAction(nameof(GetMessageById), new { id = message.MessageId }, message);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Message>> GetMessageById(int id)
		{
			var message = await _context.Messages.FindAsync(id);
			if (message == null)
			{
				return NotFound();
			}

			return message;
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteMessageById(int id)
		{
			// Find the post by ID
			var message = await _context.Messages.FindAsync(id);
			if (message == null)
			{
				return NotFound();
			}

			// Remove the post
			_context.Messages.Remove(message);

			// Save changes asynchronously
			await _context.SaveChangesAsync();

			// Return NoContent (204) as a standard response for deletion
			return NoContent();
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateMessageById(int id, MessageEditDto dto)
		{

			// Retrieve the Message by ID
			var message = await _context.Messages.FindAsync(id);
			if (message == null)
			{
				return NotFound();
			}

			// Update Message properties
			message.MessageText = dto.MessageText;

			// UpdateAt will update date in database (after trigger)
			try
			{
				_context.Messages.Update(message);
				await _context.SaveChangesAsync();

			}
			catch (DbUpdateException ex)
			{
				// Log the exception using a logging framework
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}

			return NoContent(); // 204 status code mean the operation is done
		}

		[HttpGet("users/{userId}/{userId2}")]
		public async Task<ActionResult<IEnumerable<Post>>> GetUserPostsByUserId(int userId,int userId2)
		{
			// Retrieve posts for the specified user
			var messages = await _context.Messages
										  .Where(comment => ( comment.SenderId == userId && comment.ReceiverId == userId2)
										  || (comment.SenderId == userId2 && comment.ReceiverId == userId))
										  .ToListAsync();

			// If no posts found, return a 404 NotFound
			if (messages == null || !messages.Any())
			{
				return NotFound($"No posts between UserId: {userId} and UserId: {userId2}");
			}

			// Map posts to a DTO if needed
			var comments = messages.Select(comment => new MessageAddDto
			{
				SenderId = comment.SenderId,
				ReceiverId = comment.ReceiverId,
				MessageText = comment.MessageText,
				SentAt = comment.SentAt,
				// Include other fields as needed
			}).OrderBy((comment) => comment.SentAt).ToList();

			// Return the list of user posts
			return Ok(comments);
		}

	}
}
