using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Model
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileName { get; set; }
        public string MailId { get; set; }
        public Gender Gender { get; set; }
        public Guid ProfileImageId { get; set; }
        public string About { get; set; }
        public string Location { get; set; }
        public string Intro { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
