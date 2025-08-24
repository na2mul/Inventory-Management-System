using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos.UserDtos;

namespace DevSkill.Inventory.Application.Features.Users.Queries
{    
    public class UserGetListQueryHandler : IRequestHandler<UserGetListQuery, (IList<UserDto>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public UserGetListQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<(IList<UserDto>, int, int)> Handle(UserGetListQuery request,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
