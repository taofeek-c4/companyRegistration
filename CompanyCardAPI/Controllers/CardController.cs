using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using CompanyCardAPI.Data;

[ApiController]
[Route("api/cards")]
public class CardsController : ControllerBase
{
    private readonly AppDbContext _context;

    public CardsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateCard([FromBody] int companyId)
    {
        var company = await _context.Companies.FindAsync(companyId);
        if (company == null || !company.IsVerified)
            return BadRequest("Company not found or not verified.");

        string cardNumber = GenerateNumber();
        string pin = GeneratePin();

        var card = new Card
        {
            CardNumber = cardNumber,
            Pin = Hash(pin),
            CompanyId = companyId
        };

        _context.Cards.Add(card);
        await _context.SaveChangesAsync();

        return Ok(new { cardNumber, pin });
    }

    [HttpGet("{companyId}")]
    public async Task<IActionResult> GetCards(int companyId)
    {
        var cards = await _context.Cards
            .Where(c => c.CompanyId == companyId)
            .Select(c => new
            {
                c.Id,
                c.CardNumber,
                c.IssuedAt
            }).ToListAsync();

        return Ok(cards);
    }

    private string GenerateNumber()
    {
        var rand = new Random();
        return $"{rand.Next(1000,9999)}-{rand.Next(1000,9999)}-{rand.Next(1000,9999)}-{rand.Next(1000,9999)}";
    }

    private string GeneratePin()
    {
        return new Random().Next(1000, 9999).ToString();
    }

    private string Hash(string value)
    {
        using var sha = SHA256.Create();
        return Convert.ToBase64String(sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(value)));
    }
}
