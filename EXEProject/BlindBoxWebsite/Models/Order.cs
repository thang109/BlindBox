﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlindBoxWebsite.Models;

public partial class Order
{
    [Key]
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public decimal TotalPrice { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Commission> Commissions { get; set; } = new List<Commission>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<OrderInfo> OrderInfos { get; set; } = new List<OrderInfo>();

    public virtual User User { get; set; } = null!;

}
