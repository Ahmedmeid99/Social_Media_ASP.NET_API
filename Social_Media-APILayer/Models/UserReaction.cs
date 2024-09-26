namespace Social_Media_APILayer.Models
{
	public class UserReaction
	{
		public int ReactionId { get; set; }

		public int UserId { get; set; }

		public int PostId { get; set; }

		public int ReactionTypeId { get; set; }

		public DateTime CreatedAt { get; set; }

		public virtual User User { get; set; } = null!;

		public virtual Post Post { get; set; } = null!;

		public virtual ReactionType ReactionType { get; set; } = null!;
	}
}
