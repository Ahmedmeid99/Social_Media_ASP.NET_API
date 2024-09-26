namespace Social_Media_APILayer.Dtos.UserReaction
{
	public class UserReactionAddDto:UserReactionEditDto
	{
		public int UserId { get; set; }

		public int PostId { get; set; }

	}
}
