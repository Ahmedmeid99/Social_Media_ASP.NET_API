using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Social_Media_APILayer.Data;
using Social_Media_APILayer.Dtos.ProfilePicture;
using Social_Media_APILayer.Models;

namespace Social_Media_APILayer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserProfilePictureController : ControllerBase
	{
		// add update delete get getAll

		private readonly AppDbcontext _context;

		public UserProfilePictureController(AppDbcontext context) // came from program.js whrere the injection 
		{
			_context = context;
		}

		[HttpPost]
		public async Task<IActionResult> AddUserProfilePicture([FromForm] UserProfilePictureAddDto dto)
		{
			if (dto.ImageFile != null)
			{
				try
				{
					using (var memoryStream = new MemoryStream())
					{
						await dto.ImageFile.CopyToAsync(memoryStream);
						dto.PictureData = memoryStream.ToArray();  // Convert to byte array
						
						dto.MediaType = dto.ImageFile.ContentType;  // Set MediaType from the file
					}
				}
				catch (Exception ex)
				{
					// Log the exception (e.g., using a logging framework)
					return BadRequest($"File upload failed: {ex.Message}");
				}
			}
			else
			{
				return BadRequest("Image file is required.");  // Ensure an image file is provided
			}

			if (dto.PictureData == null || dto.PictureData.Length == 0)
			{
				return BadRequest("The PictureData field is required.");  // Additional validation check
			}

			// Map DTO to Entity
			var userProfilePicture = new UserProfilePicture
			{
				UserId = dto.UserId,
				MediaType=dto.MediaType,
				PictureData = dto.PictureData,
				CreatedAt = DateTime.Now,  // Example: Setting created time
				UpdatedAt = DateTime.Now
			};

			try
			{
				_context.UserProfilePictures.Add(userProfilePicture);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Log the exception (e.g., using a logging framework)
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}

			return CreatedAtAction(nameof(GetUserProfilePictureById), new { id = userProfilePicture.UserProfilePictureId }, userProfilePicture);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<UserProfilePicture>> GetUserProfilePictureById(int id)
		{
			var userProfilePicture = await _context.UserProfilePictures.FindAsync(id);
			if (userProfilePicture == null)
			{
				return NotFound();
			}

			return userProfilePicture;
		}

		[HttpGet("users/{userId}")]
		public async Task<ActionResult<UserProfilePicture>> GetUserProfilePictureByUserId(int userId)
		{
			var userProfilePicture = await _context.UserProfilePictures.FirstOrDefaultAsync((p)=> p.UserId ==userId);
			if (userProfilePicture == null)
			{
				return NotFound();
			}

			return userProfilePicture;
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUserProfilePictureById(int id)
		{
			// Find the post by ID
			var userProfilePicture = await _context.UserProfilePictures.FindAsync(id);
			if (userProfilePicture == null)
			{
				return NotFound();
			}

			// Remove the post
			_context.UserProfilePictures.Remove(userProfilePicture);

			// Save changes asynchronously
			await _context.SaveChangesAsync();

			// Return NoContent (204) as a standard response for deletion
			return NoContent();
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdatePostById(int id, UserProfilePictureEditDto dto)
		{
			if (dto.ImageFile != null)
			{
				try
				{
					await using (var memoryStream = new MemoryStream())
					{
						await dto.ImageFile.CopyToAsync(memoryStream);
						dto.PictureData = memoryStream.ToArray();  // Convert to byte array
					}
				}
				catch (Exception ex)
				{
					// Log the exception using a logging framework
					return BadRequest($"File upload failed: {ex.Message}");
				}
			}

			// Retrieve the post by ID
			var userProfilePictures = await _context.UserProfilePictures.FindAsync(id);
			
			if (userProfilePictures == null)
			{
				return NotFound();
			}

			// Update post properties
			userProfilePictures.PictureData = dto.PictureData;

			// UpdateAt will update date in database (after trigger)
			try
			{
				_context.UserProfilePictures.Update(userProfilePictures);
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
