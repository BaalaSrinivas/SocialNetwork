using NewsfeedService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsfeedService.Services
{
    public interface IContentService
    {
        Task<IEnumerable<Guid>> GetUsersPosts(UserPostDTO userPostDTO, string token);
    }
}
