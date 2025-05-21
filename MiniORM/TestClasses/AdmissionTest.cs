using MiniORM.Assignment2;

namespace MiniORM.TestClasses
{
    public class AdmissionTest : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public double TestFees { get; set; }
       
    }
}
