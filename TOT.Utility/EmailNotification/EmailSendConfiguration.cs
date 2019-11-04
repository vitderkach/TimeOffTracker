using TOT.Interfaces;

namespace TOT.Utility.EmailNotification
{
    public class EmailSendConfiguration : IEmailSendConfiguration
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
    }
}
