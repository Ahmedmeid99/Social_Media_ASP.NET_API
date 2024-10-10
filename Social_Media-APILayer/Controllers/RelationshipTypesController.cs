using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Social_Media_APILayer.Data;
using Social_Media_APILayer.Models;
using Social_Media_APILayer.Models.Views;

namespace Social_Media_APILayer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RelationshipTypesController : ControllerBase
	{
		private readonly AppDbcontext _context;

		public RelationshipTypesController(AppDbcontext context) // came from program.js whrere the injection 
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<RelationshipType>>> GetUserRelationships()
		{
			try
			{
				var relationshipTyps = await _context.RelationshipTypes.OrderBy((r)=>r.RelationshipTypeId).ToListAsync();


				if (relationshipTyps.Count == 0)
				{
					return NoContent();
				}

				return Ok(relationshipTyps);
			}
			catch (Exception ex)
			{
				// Handle exception or log it
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
