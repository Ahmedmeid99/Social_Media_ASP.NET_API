using System;
using System.Collections.Generic;

namespace Social_Media_APILayer.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public char? Gender { get; set; }

    public string Email { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string Phone { get; set; } = null!;

    public string? Address { get; set; }

    public int CountryId { get; set; }


    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Message> MessageReceivers { get; set; } = new List<Message>();

    public virtual ICollection<Message> MessageSenders { get; set; } = new List<Message>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();


    public virtual ICollection<UserBackgroundPicture> UserBackgroundPictures { get; set; } = new List<UserBackgroundPicture>();


	public virtual ICollection<UserReaction> UserReactions { get; set; } = new List<UserReaction>();

	public virtual ICollection<UserProfilePicture> UserProfilePictures { get; set; } = new List<UserProfilePicture>();

    public virtual ICollection<UserRelationship> UserRelationshipUserId1Navigations { get; set; } = new List<UserRelationship>();

    public virtual ICollection<UserRelationship> UserRelationshipUserId2Navigations { get; set; } = new List<UserRelationship>();
}
