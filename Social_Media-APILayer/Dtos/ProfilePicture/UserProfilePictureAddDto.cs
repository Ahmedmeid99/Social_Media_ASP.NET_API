using System.ComponentModel.DataAnnotations.Schema;

namespace Social_Media_APILayer.Dtos.ProfilePicture
{
	public class UserProfilePictureAddDto:UserProfilePictureEditDto
	{
		public int UserId { get; set; }

	}
}
