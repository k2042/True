using Microsoft.EntityFrameworkCore;
using True.Data.Model;
using True.Data.Model.Dbo;
using True.Users.Infrastructure.Abstractions;

namespace True.Users.Infrastructure.Implementations
{
    public class UserRepository(ITrueDbContext DbContext) : IUserRepository
    {
        public async Task CreateUser(User user, CancellationToken cancellationToken)
        {
            var newUser = new UserDbo()
            {
                Name = user.Name,
                Password = user.Password
            };

            DbContext.Users.Add(newUser);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<User?> FindUser(string name, CancellationToken cancellationToken)
        {
            name = name.ToLower();

            var user = await DbContext.Users.AsNoTracking().SingleOrDefaultAsync(
                u => u.Name.ToLower().Equals(name),
                cancellationToken
            );

            return user != null
                ? new User(user.Name, user.Password)
                : null;
        }
    }
}
