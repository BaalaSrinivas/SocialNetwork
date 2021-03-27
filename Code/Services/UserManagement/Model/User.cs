using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Model
{
    public class User
    {
        public string Name { get; set; }

        public string MailId { get; set; }

        public string ProfileName { get; set; }

        public Guid UserId { get; set; }
    }
}
