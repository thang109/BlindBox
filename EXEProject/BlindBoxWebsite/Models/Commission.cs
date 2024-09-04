using System;
using System.Collections.Generic;

namespace BlindBoxWebsite.Models;

public partial class Commission
{
    public int CommissionId { get; set; }

    public int OrderId { get; set; }

    public string? ShopeeProductId { get; set; }

    public decimal CommissionAmount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Order Order { get; set; } = null!;
}
