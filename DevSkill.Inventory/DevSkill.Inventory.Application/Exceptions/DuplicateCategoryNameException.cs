using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Exceptions
{
    public class DuplicateCategoryNameException : Exception
    {
        public DuplicateCategoryNameException() : base("Category Name can't be duplicate")
        {

        }
    }
}
