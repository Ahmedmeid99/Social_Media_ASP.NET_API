namespace Social_Media_APILayer.Models.Views
{
    public class PostsView
    {
        public int PostID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte[]? PictureData { get; set; } // Nullable
        public string Content { get; set; }
        public string MediaType { get; set; }
        public DateTime CreatedAt { get; set; }
        public int LikesCount { get; set; }
        public int LoveCount { get; set; }
        public int DisLikeCount { get; set; }
        public int HahaCount { get; set; }
        public int WowCount { get; set; }
        public int SadCount { get; set; }
        public int AngryCount { get; set; }
    }
}
