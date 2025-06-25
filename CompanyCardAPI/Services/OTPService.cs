
public class OTPService
{
    public string GenerateOTP()
    {
        return new Random().Next(100000, 999999).ToString();
    }
}
