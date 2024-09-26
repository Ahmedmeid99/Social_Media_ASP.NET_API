using System;
using System.Collections.Generic;

namespace Social_Media_APILayer.Models;

public partial class UserRelationship
{
    public int RelationshipId { get; set; }

    public int UserId1 { get; set; }

    public int UserId2 { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? RelationshipTypeId { get; set; }

    public virtual RelationshipType? RelationshipType { get; set; }

    public virtual User UserId1Navigation { get; set; } = null!;

    public virtual User UserId2Navigation { get; set; } = null!;
}
