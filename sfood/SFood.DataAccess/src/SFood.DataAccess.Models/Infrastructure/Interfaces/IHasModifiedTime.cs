using System;

namespace SFood.DataAccess.Models.Infrastructure.Interfaces
{
    public interface IHasModifiedTime
    {
         DateTime? LastModifiedTime { get; set; }
    }
}