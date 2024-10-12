using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlindBoxWebsite.Models;

public partial class Ad
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AdId { get; set; }

    public string? ImageUrl { get; set; }

    public string? LinkUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
