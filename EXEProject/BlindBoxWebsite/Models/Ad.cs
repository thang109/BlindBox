using System;
using System.Collections.Generic;

namespace BlindBoxWebsite.Models;

public partial class Ad
{
    public int AdId { get; set; }

    public string? ImageUrl { get; set; }

    public string? LinkUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
