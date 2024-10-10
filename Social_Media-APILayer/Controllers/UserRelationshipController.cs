using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Social_Media_APILayer.Data;
using Social_Media_APILayer.Dtos.UserRelationship;
using Social_Media_APILayer.Models;
using Social_Media_APILayer.Models.Views;

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
		public async Task<IActionResult> AddUserUserRelationship([FromBody] UserRelationshipAddDto dto)
		{
			if (dto.UserId1 <= 0 || dto.UserId2 <= 0 || dto.RelationshipTypeId <=0)
			{
				return BadRequest("UserId1 or UserId2 is unvalid");
			}

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

				var userRelationshipView = await _context.UserRelationshipViews.FirstOrDefaultAsync((r) => r.RelationshipId == userRelationship.RelationshipId);
				if (userRelationshipView == null)
				{
					return NotFound();
				}
				return Ok(userRelationshipView);
			}
			catch (Exception ex)
			{
				// Log the exception (e.g., using a logging framework)
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}

		}

		[HttpPut("user1/{userId1}/user2/{userId2}")]
		public async Task<IActionResult> UpdateUserRelationshipById(int userId1, int userId2, [FromBody] UserRelationshipEditDto dto)
		{

			// Retrieve the post by ID
			var userRelationship = await _context.UserRelationships.FirstOrDefaultAsync((r) => (r.UserId1 == userId1 && r.UserId2 == userId2)
				|| (r.UserId1 == userId2 && r.UserId2 == userId1));

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

				var userRelationshipView = await _context.UserRelationshipViews.FirstOrDefaultAsync((r) => r.RelationshipId == userRelationship.RelationshipId);
				if (userRelationshipView == null)
				{
					return NotFound();
				}
				return Ok(userRelationshipView); // 204 status code mean the operation is done
			}
			catch (Exception ex)
			{
				// Log the exception using a logging framework
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}


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

			try
			{
				// Remove the post
				_context.UserRelationships.Remove(userRelationship);

				// Save changes asynchronously
				await _context.SaveChangesAsync();

				// Return NoContent (204) as a standard response for deletion
				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}


		[HttpGet("{id}")]
		public async Task<ActionResult<UserRelationship>> GetUserRelationshipById(int id)
		{
			if (id <= 0)
			{
				return BadRequest("is is unvalid");
			}
			try
			{
				var userRelationship = await _context.UserRelationships.FindAsync(id);
				if (userRelationship == null)
				{
					return NotFound();
				}

				return userRelationship;
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("user/{userId}")]
		public async Task<ActionResult<IEnumerable<UserRelationship>>> GetUserRelationshipsByUserId(int userId)
		{
			if (userId <= 0 )
			{
				return BadRequest("UserId is unvalid");
			}

			try
			{
				var userRelationship = await _context.UserRelationships.Where((r)=>r.UserId1 == userId).ToListAsync();
				if (userRelationship.Count == 0)
				{
					return NotFound();
				}

				return userRelationship;
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
		[HttpGet("user1/{userId1}/user2/{userId2}")]
		public async Task<ActionResult<UserRelationshipView>> GetUserRelationship(int userId1, int userId2)
		{
			if (userId1 <= 0 || userId2 <= 0 )
			{
				return BadRequest("UserId1 or UserId2 is unvalid");
			}

			try
			{
				var userRelationship = await _context.UserRelationshipViews.FirstOrDefaultAsync((r) =>( r.UserId1 == userId1 && r.UserId2 == userId2) ||
																									(r.UserId1 == userId2 && r.UserId2 == userId1));
				if (userRelationship == null)
				{
					return NotFound();
				}

				return userRelationship;
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
		
		
	}
}
