using System.ComponentModel.DataAnnotations;

public class Company
{
    public int Id { get; set; }

    [Required]
    public string Email { get; set; }

    public string Password { get; set; }  // Set after OTP verification

    public string OTP { get; set; }       // Temporary field used during verification

    public bool IsVerified { get; set; }

    public ICollection<Card> Cards { get; set; } = new List<Card>();
}
