using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces;

public interface IJwtTokenRepository
{
    Task<string> GenerateTokenAsync(User user, IList<string> roles);
}
