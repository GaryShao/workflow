using Microsoft.EntityFrameworkCore;
using SFood.DataAccess.EFCore;

namespace SFood.Migrator.Entry
{
    public class MigrationDbContext : SFoodDbContext
    {
        public MigrationDbContext(DbContextOptions<MigrationDbContext> options) : base(options)
        {
        }
    }
}
