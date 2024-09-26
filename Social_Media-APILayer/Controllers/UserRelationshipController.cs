using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Social_Media_APILayer.Data;
using Social_Media_APILayer.Dtos.UserRelationship;
using Social_Media_APILayer.Models;

namespace Social_Media_APILayer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserRelationshipController : ControllerBase
	{
		// add update delete get getAll

		private readonly AppDbcontext _context;

		public UserRelationshipController(AppDbcontext context) // came from program.js whrere the injection 
		{
			_context = context;
		}

		[HttpPost]
		public async Task<IActionResult> AddUserUserRelationship([FromForm] UserRelationshipAddDto dto)
		{
			

			// Map DTO to Entity
			var userRelationship = new UserRelationship
			{
				UserId1 = dto.UserId1,
				UserId2 = dto.UserId2,
				RelationshipTypeId = dto.RelationshipTypeId,
				CreatedAt = DateTime.Now  // Example: Setting created time
			};

			try
			{
				_context.UserRelationships.Add(userRelationship);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Log the exception (e.g., using a logging framework)
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}

			return CreatedAtAction(nameof(GetUserRelationshipById), new { id = userRelationship.RelationshipId }, userRelationship);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<UserRelationship>> GetUserRelationshipById(int id)
		{
			var userRelationship = await _context.UserRelationships.FindAsync(id);
			if (userRelationship == null)
			{
				return NotFound();
			}

			return userRelationship;
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUserRelationshipById(int id)
		{
			// Find the post by ID
			var userRelationship = await _context.UserRelationships.FindAsync(id);
			if (userRelationship == null)
			{
				return NotFound();
			}

			// Remove the post
			_context.UserRelationships.Remove(userRelationship);

			// Save changes asynchronously
			await _context.SaveChangesAsync();

			// Return NoContent (204) as a standard response for deletion
			return NoContent();
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateUserRelationshipById(int id, UserRelationshipEditDto dto)
		{
			
			// Retrieve the post by ID
			var userRelationship = await _context.UserRelationships.FindAsync(id);

			if (userRelationship == null)
			{
				return NotFound();
			}

			// Update post properties
			userRelationship.RelationshipTypeId = dto.RelationshipTypeId;

			// UpdateAt will update date in database (after trigger)
			try
			{
				_context.UserRelationships.Update(userRelationship);
				await _context.SaveChangesAsync();

			}
			catch (DbUpdateException ex)
			{
				// Log the exception using a logging framework
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}

			return NoContent(); // 204 status code mean the operation is done
		}


	}
}
