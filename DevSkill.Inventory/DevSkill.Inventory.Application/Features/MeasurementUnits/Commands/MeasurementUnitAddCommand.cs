using DevSkill.Inventory.Domain.Features.MeasurementUnits.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.MeasurementUnits.Commands
{
    public class MeasurementUnitAddCommand : IRequest, IMeasurementUnitAddCommand
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}
