using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlindBoxWebsite.Models;

public partial class Commission
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CommissionId { get; set; }

    public int OrderId { get; set; }

    public string? ShopeeProductId { get; set; }

    public decimal CommissionAmount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Order Order { get; set; } = null!;
}
