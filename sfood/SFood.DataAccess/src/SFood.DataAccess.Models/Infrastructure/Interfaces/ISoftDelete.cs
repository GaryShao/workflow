namespace SFood.DataAccess.Models.Infrastructure.Interfaces
{
    public interface ISoftDelete
    {
         bool IsDeleted { get; set; }
    }
}