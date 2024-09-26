namespace Social_Media_APILayer.Models
{
	public class ReactionType
	{
		public int ReactionTypeId { get; set; }

		public string ReactionName { get; set; } = null!;

		public virtual ICollection<UserReaction> UserReactions { get; set; } = new List<UserReaction>();

	}
}
