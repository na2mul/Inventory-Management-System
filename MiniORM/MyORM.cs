using MiniORM.Assignment2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    public class MyORM<G,T> where T : class, IEntity<Guid>, new()
    {

    }
}
