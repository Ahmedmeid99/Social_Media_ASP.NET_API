using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Social_Media_APILayer.Data;
using Social_Media_APILayer.Dtos.UserBackgroundPicture;
using Social_Media_APILayer.Models;

namespace Social_Media_APILayer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserBackgroundPictureController : ControllerBase
	{
		// add update delete get getAll

		private readonly AppDbcontext _context;

		public UserBackgroundPictureController(AppDbcontext context) // came from program.js whrere the injection 
		{
			_context = context;
		}

		[HttpPost]
		public async Task<IActionResult> AdduserBackgroundPicture([FromForm] UserBackgroundPictureAddDto dto)
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

			if (dto.PictureData == null) 
			{
				return BadRequest("PictureData is null here !!!");
			}

			if (dto.UserId <= 0) 
			{
				return BadRequest("UserId is unvalid");
			}

			// Map DTO to Entity
			var userBackgroundPicture = new UserBackgroundPicture
			{
				PictureData = dto.PictureData,
				MediaType = dto.MediaType,
				UserId = dto.UserId,
				CreatedAt = DateTime.Now  // Example: Setting created time
			};

			try
			{
				_context.UserBackgroundPictures.Add(userBackgroundPicture);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Log the exception (e.g., using a logging framework)
				return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
			}

			return CreatedAtAction(nameof(GetuserBackgroundPictureById), new { id = userBackgroundPicture.UserBackgroundPictureId }, userBackgroundPicture);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdatePostById(int id, UserBackgroundPictureEditDto dto)
		{
			if (dto.ImageFile != null)
			{
				try
				{
					await using (var memoryStream = new MemoryStream())
					{
						await dto.ImageFile.CopyToAsync(memoryStream);
						dto.PictureData = memoryStream.ToArray();  // Convert to byte array

						if (dto.PictureData == null || dto.PictureData.Length == 0)
						{
							return BadRequest("Failed to read file data.");
						}

						dto.MediaType = dto.ImageFile.ContentType;
					}
				}
				catch (Exception ex)
				{
					// Log the exception using a logging framework
					return BadRequest($"File upload failed: {ex.Message}");
				}
			}

			// Retrieve the post by ID
			var userBackgroundPictures = await _context.UserBackgroundPictures.FindAsync(id);

			if (userBackgroundPictures == null)
			{
				return NotFound();
			}

			// Update post properties
			userBackgroundPictures.PictureData = dto.PictureData;

			// UpdateAt will update date in database (after trigger)
			try
			{
				_context.UserBackgroundPictures.Update(userBackgroundPictures);
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
		public async Task<IActionResult> DeleteuserBackgroundPictureById(int id)
		{
			// Find the post by ID
			var userBackgroundPicture = await _context.UserBackgroundPictures.FindAsync(id);
			if (userBackgroundPicture == null)
			{
				return NotFound();
			}

			try
			{
				// Remove the post
				_context.UserBackgroundPictures.Remove(userBackgroundPicture);

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
		public async Task<ActionResult<UserBackgroundPicture>> GetuserBackgroundPictureById(int id)
		{
			try
			{


				var userBackgroundPicture = await _context.UserBackgroundPictures.FindAsync(id);
				if (userBackgroundPicture == null)
				{
					return NotFound();
				}

				return userBackgroundPicture;
			}
			catch (Exception ex) 
			{
				return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
			}
		}

		[HttpGet("users/{userId}")]
		public async Task<ActionResult<UserBackgroundPicture>> GetUserBackgroundPictureByUserId(int userId)
		{
			try
			{


				var userBackgroundPicture = await _context.UserBackgroundPictures.FirstOrDefaultAsync((p) => p.UserId == userId);
				if (userBackgroundPicture == null)
				{
					return NotFound();
				}

				return userBackgroundPicture;
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

	}
}
