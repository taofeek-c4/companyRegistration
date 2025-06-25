using System;
using System.ComponentModel.DataAnnotations;

public class Card
{
    public int Id { get; set; }

    [Required]
    public string CardNumber { get; set; }

    [Required]
    public string Pin { get; set; } // Hashed PIN

    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

    // Foreign key
    public int CompanyId { get; set; }
    public Company Company { get; set; }
}
