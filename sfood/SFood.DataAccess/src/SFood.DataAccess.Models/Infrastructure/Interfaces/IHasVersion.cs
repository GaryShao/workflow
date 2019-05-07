namespace SFood.DataAccess.Models.Infrastructure.Interfaces
{
    public interface IHasVersion
    {
        byte[] Version { get; set; }
    }
}
