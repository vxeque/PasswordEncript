using System.Security.Cryptography.X509Certificates;

namespace passwordManagent2.Models
{
    public class PasswordEntry
    {
        public long Id { get; set; }
        public string SiteName { get; set; }
        public string UserName { get; set; }
        public string Encryptedpassword { get; set; }
        public string? Note { get; set; }
        public string? Url { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string SecurityStatus { get; set; }

    }
}
