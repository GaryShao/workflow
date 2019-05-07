using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Newtonsoft.Json;
using SFood.Migrator.Entry.Dtos;
using System.IO;

namespace SFood.Migrator.Entry
{
    public class MigrationDbContextFactory : IDesignTimeDbContextFactory<MigrationDbContext>
    {
        public MigrationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MigrationDbContext>();
            builder.UseSqlServer(RetrieveConnectionString());

            return new MigrationDbContext(builder.Options);
        }

        private string RetrieveConnectionString()
        {
            Setting setting = null;
            using (var r = new StreamReader("appsetting.json"))
            {
                var json = r.ReadToEnd();
                setting = JsonConvert.DeserializeObject<Setting>(json);
            }
            System.Console.WriteLine(setting.SFoodConnectionString);
            return setting.SFoodConnectionString;
        }
    }
}
