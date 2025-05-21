using MiniORM.Assignment2;

namespace MiniORM.TestClasses
{
    public class Course : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Instructor Teacher { get; set; }
        public List<Topic> Topics { get; set; }
        public double Fees { get; set; }
        public List<AdmissionTest> Tests { get; set; }

    }
}


