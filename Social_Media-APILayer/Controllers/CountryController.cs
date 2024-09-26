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

		//get getAll

		[HttpGet]
		public async Task<ActionResult> GetCountries()
		{
			var countries = await _context.Countries
			   .Select(c => new { CountryId = c.CountryId, CountryName = c.CountryName })
			   .ToListAsync();

			if (countries.Count == 0)
			{
				return NotFound("There are no countries found.");
			}

			return Ok(countries);

		}
	}
}
