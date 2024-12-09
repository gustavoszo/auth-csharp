using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UsersApi.Models;

namespace UsersApi.Data
{
    public class UserDbContext : IdentityDbContext<User>
    {

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    }
}
