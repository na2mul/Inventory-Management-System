using DevSkill.Inventory.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain
{
    public interface IUnitOfWork
    {
        ISqlUtility SqlUtility { get; }
        void Save();
        Task SaveAsync();
    }
}
