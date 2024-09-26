using Social_Media_APILayer.Models;

namespace Social_Media_APILayer.Dtos.Post
{
	public class PostDetailsDto
	{
		public int PostId { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; } = string.Empty;
		public string UserEmail { get; set; } = string.Empty;
		public string? Content { get; set; }
		public string? MediaType { get; set; }
		public byte[]? MediaData { get; set; }
		public DateTime? CreatedAt { get; set; }
		public int LikesCount { get; set; }
		public int LoveCount { get; set; }
		public int DislikeCount { get; set; }
		public int HahaCount { get; set; }
		public int WowCount { get; set; }
		public int SadCount { get; set; }
		public int AngryCount { get; set; }
		public UserProfilePicture userProfilePicture { get; set; } = null!;
		public ICollection<CommentDto> Comments { get; set; } = new List<CommentDto>();
	}

	public class CommentDto
	{
		public int CommentId { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; } = string.Empty;
		public string Content { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; }
		public UserProfilePicture ProfilePicture { get; set; } = null!; // Changed to object

	}

}
