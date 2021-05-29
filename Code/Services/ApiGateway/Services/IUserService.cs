using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGateway.Models;

namespace ApiGateway.Services
{
    public interface IUserService
    {
        Task<IEnumerable<SMUser>> GetUsers(IEnumerable<string> mailId, string token);
    }
}
