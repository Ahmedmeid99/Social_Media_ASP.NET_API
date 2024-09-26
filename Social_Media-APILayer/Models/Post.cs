using System;
using System.Collections.Generic;

namespace Social_Media_APILayer.Models;

public partial class Post
{
    public int PostId { get; set; }

    public int UserId { get; set; }

    public string? Content { get; set; }

    public string? MediaType { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? LikesCount { get; set; } = 0;

	public int? LoveCount { get; set; } = 0;
    
    public int? DisLikeCount { get; set; } = 0;
    
    public int? HahaCount { get; set; } = 0;
	
    public int? WowCount { get; set; } = 0;
    
    public int? SadCount { get; set; } = 0;
	
    public int? AngryCount { get; set; } = 0;

    public byte[]? MediaData { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
	public virtual ICollection<UserReaction> UserReactions { get; set; } = new List<UserReaction>();

	public virtual User User { get; set; } = null!;
}
