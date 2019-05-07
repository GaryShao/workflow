using Microsoft.EntityFrameworkCore;
using SFood.DataAccess.EFCore;

namespace SFood.Auth.Host
{
    public class ApplicationDbContext : SFoodDbContext
    {
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
