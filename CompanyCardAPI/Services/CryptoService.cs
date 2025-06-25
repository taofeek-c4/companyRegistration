
using BCrypt.Net;

public class CryptoService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    public string EncryptPin(string pin)
    {
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pin));
    }

    public string DecryptPin(string encryptedPin)
    {
        return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encryptedPin));
    }
}
