using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.Context
{
    public class UserContext: DbContext
    {
        public UserContext() : base()
        {
                
        }
    }
}
