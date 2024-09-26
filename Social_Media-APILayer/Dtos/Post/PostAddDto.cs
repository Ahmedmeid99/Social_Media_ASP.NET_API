using Social_Media_APILayer.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Social_Media_APILayer.Dtos.Post
{
	public class PostAddDto: PostEditDto
	{
		public int UserId { get; set; }


	}
}
