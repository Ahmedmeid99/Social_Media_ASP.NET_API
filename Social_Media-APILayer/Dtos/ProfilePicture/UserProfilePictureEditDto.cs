using System.ComponentModel.DataAnnotations.Schema;

namespace Social_Media_APILayer.Dtos.ProfilePicture
{
	public class UserProfilePictureEditDto
	{
		public string? MediaType { get; set; }

		public byte[]? PictureData { get; set; }

		[NotMapped]
		public IFormFile? ImageFile { get; set; }  // New property for image upload
	}
}
