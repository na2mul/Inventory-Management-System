using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Exceptions
{
    public class DuplicateMeasurementUnitNameException : Exception
    {
        public DuplicateMeasurementUnitNameException() : base("MeasurementUnit Name can't be duplicate")
        {

        }
    }
}
