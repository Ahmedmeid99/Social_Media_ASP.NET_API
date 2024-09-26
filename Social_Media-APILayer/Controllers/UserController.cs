
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Social_Media_APILayer.Data;
using Social_Media_APILayer.Dtos.Usre;
using Social_Media_APILayer.Models;
using Social_Media_APILayer.Globals;
using Microsoft.AspNetCore.Identity;

namespace Social_Media_APILayer.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{

		private readonly AppDbcontext _context;

		public UserController(AppDbcontext context) // came from program.js whrere the injection 
		{
			_context = context;
		}

		// Sigup Login update getAll delete

		[HttpPost("signup")]
		public async Task<ActionResult> SignUp([FromBody] UserAddEditDto dto)
		{
			// Validate the incoming request data (model) against UserAddEditDto
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			// Check if the username already exists
			if (await _context.Users.AnyAsync(u => u.UserName == dto.UserName))
			{
				return Conflict(new { message = "Username already exists." });
			}

			// Check if the email already exists
			if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
			{
				return Conflict(new { message = "Email already exists." });
			}

			// Create a new User entity
			var user = new User
			{
				UserName = dto.UserName,
				PasswordHash = PasswordHasher.HashingPassword(dto.Password), // Use the PasswordHasher
				Gender = dto.Gender,
				Email = dto.Email,
				DateOfBirth = dto.DateOfBirth,
				Phone = dto.Phone,
				Address = dto.Address,
				CountryId = dto.CountryId,
				CreatedAt = DateTime.Now
			};

			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			return Ok(new { UserId = user.UserId });
		}


		[HttpPost("login")]
		public async Task<ActionResult> Login([FromBody] UserLogInDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { message = "Invalid input data", errors = ModelState });
			}

			var passwordHash = PasswordHasher.HashingPassword(dto.Password);
			var user = await _context.Users
				.FirstOrDefaultAsync(u => u.UserName == dto.UserName && u.PasswordHash == passwordHash);

			if (user != null)
			{
				
				return Ok( user );
			}
			else
			{
				return Unauthorized(new { message = "Invalid username or password" });
			}
		}

		// delete and related posts of this user
		// Get ByUserId

		[HttpPut("update/{id}")]
		public async Task<ActionResult> Update(int id, [FromBody] UserAddEditDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var user = await _context.Users.FindAsync(id);

			if (user == null)
			{
				return NotFound();
			}

			// Only update fields that are provided in the DTO
			user.UserName = dto.UserName ?? user.UserName;
			user.Address = dto.Address ?? user.Address;
			user.Email = dto.Email ?? user.Email;

			if (!string.IsNullOrEmpty(dto.Password))
			{
				user.PasswordHash = PasswordHasher.HashingPassword(dto.Password);
			}

			user.Phone = dto.Phone ?? user.Phone;
			user.DateOfBirth = dto.DateOfBirth != default ? dto.DateOfBirth : user.DateOfBirth;
			user.CountryId = dto.CountryId != 0 ? dto.CountryId : user.CountryId;
			

			// Save changes to the database
			await _context.SaveChangesAsync();

			return Ok(user);
		}


	}
}
