using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Social_Media_APILayer.Data;

namespace Social_Media_APILayer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CountryController : ControllerBase
	{
		private readonly AppDbcontext _context;

		public CountryController(AppDbcontext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult> GetCountries()
		{
			try
			{
				var countries = await _context.Countries
				   .Select(c => new { CountryId = c.CountryId, CountryName = c.CountryName })
				   .ToListAsync();

				if (countries == null || !countries.Any())
				{
					return NotFound($"No countries founded");
				}

				return Ok(countries);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}

		}
	}
}
