namespace Social_Media_APILayer.Dtos.Comment
{
	public class CommentAddDto:CommentEditDto
	{
		public int PostId { get; set; }
		public int UserId { get; set; }
	}
}
