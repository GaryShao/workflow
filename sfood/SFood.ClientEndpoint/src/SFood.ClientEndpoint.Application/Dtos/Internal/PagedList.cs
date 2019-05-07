using System.Collections.Generic;

namespace SFood.ClientEndpoint.Application.Dtos.Internal
{
    public class PagedList<TEntity> where TEntity : class
    {
        public PagedList()
        {
            Entities = new List<TEntity>();
        }

        public int Count { get; set; }

        public List<TEntity> Entities { get; set; }
    }
}
