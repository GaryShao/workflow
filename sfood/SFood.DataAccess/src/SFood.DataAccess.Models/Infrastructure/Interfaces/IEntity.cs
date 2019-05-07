namespace SFood.DataAccess.Models.Infrastructure.Interfaces
{
    public interface IEntity<T> : IEntity
    {
        new T Id { get ; set; }        
    }

    public interface IEntity
    {
        object Id { get; set; }
    }
}