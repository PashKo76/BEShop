using System;
using System.Collections.Generic;

namespace BEShop;

public partial class Comment
{
    public int Id { get; set; }

    public string? Data { get; set; }

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
}
