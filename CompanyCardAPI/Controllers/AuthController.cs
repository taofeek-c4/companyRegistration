using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using CompanyCardAPI.Data;
using System.Text;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] string email)
    {
        var existing = await _context.Companies.FirstOrDefaultAsync(c => c.Email == email);
        if (existing != null) return BadRequest("Email already exists.");

        string otp = new Random().Next(100000, 999999).ToString();

        var company = new Company
        {
            Email = email,
            OTP = otp,
            IsVerified = false
        };

        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        // Send email
        using (var client = new SmtpClient("smtp.example.com") // replace with your SMTP config
        {
            Credentials = new NetworkCredential("you@example.com", "yourpassword"),
            EnableSsl = true
        })
        {
            await client.SendMailAsync(new MailMessage("you@example.com", email)
            {
                Subject = "Welcome - Confirm Your Email",
                Body = $"Your OTP is: {otp}"
            });
        }

        return Ok("OTP sent to email.");
    }

    [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] VerifyRequest request)
    {
        var company = await _context.Companies.FirstOrDefaultAsync(c => c.Email == request.Email && c.OTP == request.Otp);
        if (company == null) return BadRequest("Invalid email or OTP.");

        company.IsVerified = true;
        company.Password = HashPassword(request.Password);
        company.OTP = null;

        await _context.SaveChangesAsync();

        return Ok("Account verified and password set.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var company = await _context.Companies.FirstOrDefaultAsync(c => c.Email == request.Email);
        if (company == null || !company.IsVerified) return Unauthorized("Invalid or unverified account.");

        if (!VerifyPassword(request.Password, company.Password))
            return Unauthorized("Incorrect password.");

        return Ok("Login successful.");
    }

    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }

    private bool VerifyPassword(string input, string hashed)
    {
        return HashPassword(input) == hashed;
    }

    public class VerifyRequest
    {
        public string Email { get; set; }
        public string Otp { get; set; }
        public string Password { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
