using DevSkill.Inventory.Domain.Dtos.TransferAccountDtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos.UserDtos;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class UserRepository : Repository<UserDto, Guid>, IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }         
    }
}
