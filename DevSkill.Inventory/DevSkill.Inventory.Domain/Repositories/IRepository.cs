using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface IRepository<T,G> where T : IEntity<G>
    {
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);

    }
}
