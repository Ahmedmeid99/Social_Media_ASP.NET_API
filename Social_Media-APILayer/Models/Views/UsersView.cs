namespace Social_Media_APILayer.Models.Views
{
    public class UsersView
    {
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public byte[]? PictureData { get; set; } // Nullable
		public string MediaType { get; set; }
		public int CountryId { get; set; }

	}
}
