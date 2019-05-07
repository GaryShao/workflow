using System;

namespace SFood.DataAccess.Models.Infrastructure.Interfaces
{
    public interface IHasCreatedTime
    {
         DateTime CreatedTime { get; set; }
    }
}