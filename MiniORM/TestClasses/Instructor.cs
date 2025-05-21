using MiniORM.Assignment2;

namespace MiniORM.TestClasses
{
    public class Instructor : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public PresentAddress PresentAddress { get; set; }
        public PermanentAddress PermanentAddress { get; set; }
        public List<Phone> PhoneNumber { get; set; }
        
    }
}
