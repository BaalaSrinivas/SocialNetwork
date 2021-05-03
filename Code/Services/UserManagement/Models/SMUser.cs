using System;

namespace UserManagement.Models
{
    public class SMUser
    {
        public string Name { get; set; }
        public string ProfileName { get; set; }
        public string MailId { get; set; }
        public Gender Gender { get; set; }
        public Guid ProfileImageId { get; set; }
        public string About { get; set; }
        public string Location { get; set; }
        public string Headline { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
