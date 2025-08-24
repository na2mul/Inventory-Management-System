using DevSkill.Inventory.Domain.Features.Categories.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Categories.Commands
{
    public class CategoryAddCommand : IRequest, ICategoryAddCommand
    {
        public Guid Id { get; set; }
        public string? CategoryName { get; set; }        
    }
}
