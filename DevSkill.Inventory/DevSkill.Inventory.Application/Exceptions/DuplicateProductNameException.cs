using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Exceptions
{
    public class DuplicateProductNameException : Exception
    {
        public DuplicateProductNameException() : base("Product Name can't be duplicate")
        {

        }
    }
}
