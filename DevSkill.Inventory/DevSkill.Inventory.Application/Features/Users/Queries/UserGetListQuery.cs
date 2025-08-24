using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos.UserDtos;
using DevSkill.Inventory.Domain.Features.Users.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Users.Queries
{
    public class UserGetListQuery : DataTables, IRequest<(IList<UserDto>,int, int)>, IUserGetListQuery
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
    }
}
