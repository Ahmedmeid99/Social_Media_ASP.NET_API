using System.ComponentModel.Design;

namespace Social_Media_APILayer.Models.NewFolder
{
    public class CommentsView
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte[]? PictureData { get; set; } // Nullable
        public string CommentText { get; set; }


    }
}
