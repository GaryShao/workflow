using Microsoft.EntityFrameworkCore;
using SFood.DataAccess.EFCore;

namespace SFood.DataAccess.Migrator
{
    public class MigrationDbContext : SFoodDbContext
    {
        public MigrationDbContext(DbContextOptions<MigrationDbContext> options) : base(options)
        {
        }
    }
}
