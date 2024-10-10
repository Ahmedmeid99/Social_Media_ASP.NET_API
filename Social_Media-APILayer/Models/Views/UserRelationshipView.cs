namespace Social_Media_APILayer.Models.Views
{
	public class UserRelationshipView
	{

		public int RelationshipId { get; set; }

		public int UserId1 { get; set; }

		public int UserId2 { get; set; }

		public DateTime? CreatedAt { get; set; }
		public int RelationshipTypeId { get; set; }

		public string RelationshipTypeName { get; set; } = null!;
	}
}
