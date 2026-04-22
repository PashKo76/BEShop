using System;
using System.Collections.Generic;

namespace BEShop;

public partial class Product
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public long? Ammount { get; set; }

    public byte[]? Image { get; set; }

    public string? ShortDesc { get; set; }

    public string? Desc { get; set; }

    public string CommentFromAi { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
