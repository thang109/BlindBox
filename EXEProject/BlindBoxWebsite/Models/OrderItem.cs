using System;
using System.Collections.Generic;

namespace BlindBoxWebsite.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int BlindBoxId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual BlindBox BlindBox { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
