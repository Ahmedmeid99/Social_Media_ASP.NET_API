using System;
using System.Collections.Generic;

namespace Social_Media_APILayer.Models;

public partial class UserProfilePicture
{
    public int UserProfilePictureId { get; set; }

	public string? MediaType { get; set; }

	public byte[] PictureData { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
