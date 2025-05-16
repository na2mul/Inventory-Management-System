using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Exceptions
{
    public class DuplicateAuthorNameException : Exception
    {
        public DuplicateAuthorNameException() : base("Author Name can't be duplicate")
        {

        }
    }
}
