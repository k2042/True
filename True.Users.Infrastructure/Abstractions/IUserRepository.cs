using System;
using System.Collections.Generic;
using System.Text;

namespace True.Users.Infrastructure.Abstractions
{
    public interface IUserRepository
    {
        Task CreateUser(User user, CancellationToken cancellationToken);
        Task<User?> FindUser(string name, CancellationToken cancellationToken);
    }
}
