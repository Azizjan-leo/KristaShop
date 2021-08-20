namespace KristaShop.Common.Models
{
    public class EmailMessage
    {
        public string ToEmailAddress { get; }

        public string Subject { get; }

        public string Content { get; }

        public EmailMessage
            (string to, string subject, string content)
        {
            ToEmailAddress = to;
            Subject = subject;
            Content = content;
        }
    }

    public class EmailsSetting
    {
        public string SenderName { get; set; }
        public string FromEmailAddress { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
    }
}