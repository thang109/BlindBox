using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlindBoxWebsite.Models;

public partial class BlindBox
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BlindBoxId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Product { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public List<string> GetProductList()
    {
        if (string.IsNullOrEmpty(Product))
        {
            return new List<string>();
        }

        return Product.Split(';')
                      .Select(x => x.Trim())
                      .ToList();
    }
}
