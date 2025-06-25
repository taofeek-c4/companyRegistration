public class EmailService
{
    public void SendEmail(string toEmail, string subject, string body)
    {
        Console.WriteLine($"--- MOCK EMAIL TO: {toEmail} ---");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Body: {body}");
        Console.WriteLine("----------------------------------\n");
    }
}
