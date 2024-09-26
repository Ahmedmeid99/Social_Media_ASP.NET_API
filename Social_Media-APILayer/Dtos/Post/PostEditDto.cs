using System.ComponentModel.DataAnnotations.Schema;

namespace Social_Media_APILayer.Dtos.Post
{
	public class PostEditDto
	{
		public string? Content { get; set; }

		public string? MediaType { get; set; }

		public byte[]? MediaData { get; set; }

		[NotMapped]
		public IFormFile? FormFile { get; set; }  // New property for image upload
	}
}
