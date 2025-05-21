using MiniORM.Assignment2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniORM.TestClasses;

public class Topic : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<Session> Sessions { get; set; }

}
