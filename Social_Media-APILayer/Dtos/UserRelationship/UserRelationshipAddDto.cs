namespace Social_Media_APILayer.Dtos.UserRelationship
{
	public class UserRelationshipAddDto:UserRelationshipEditDto
	{
		public int UserId1 { get; set; }

		public int UserId2 { get; set; }

		public DateTime? CreatedAt { get; set; }
	}
}
