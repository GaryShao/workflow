using System.ComponentModel.DataAnnotations;
using SFood.DataAccess.Models.Infrastructure.Interfaces;

namespace SFood.DataAccess.Models.Infrastructure
{
    public abstract class Entity<T>: IEntity<T>
    {
        [Key]
        public T Id { get; set; }

        object IEntity.Id
        {
            get => Id;
            set => Id = (T)value;
        }
    }
}