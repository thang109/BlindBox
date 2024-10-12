using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlindBoxWebsite.Models;

public partial class Quiz
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int QuizId { get; set; }

    public int UserId { get; set; }

    public string? QuizResult { get; set; }

    public string? SuggestedProducts { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
