using MiniORM.Assignment2;

namespace MiniORM.TestClasses
{
    public class Address : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        
    }
}
