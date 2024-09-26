using System;
using System.Collections.Generic;

namespace Social_Media_APILayer.Models;

public partial class RelationshipType
{
    public int RelationshipTypeId { get; set; }

    public string RelationshipTypeName { get; set; } = null!;

    public virtual ICollection<UserRelationship> UserRelationships { get; set; } = new List<UserRelationship>();
}
