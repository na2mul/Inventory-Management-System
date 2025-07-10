using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Exceptions
{
    public class DuplicateNameException : Exception
    {
        public DuplicateNameException(string name) : base($"{name} Name can't be duplicate")
        {

        }
    }
}
