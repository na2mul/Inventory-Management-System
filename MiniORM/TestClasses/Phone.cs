using MiniORM.Assignment2;

namespace MiniORM.TestClasses
{
    public class Phone : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public string Extension { get; set; }
        public string CountryCode { get; set; }
        
    }
}
