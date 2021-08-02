using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAndAccessManagement.Services
{
    public interface IEmailService
    {
        public bool SendEmail(string mailId, string subject, string emailBody);
    }
}
