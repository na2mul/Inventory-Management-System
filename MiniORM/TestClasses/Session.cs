using MiniORM.Assignment2;

namespace MiniORM.TestClasses
{
    public class Session : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public int DurationInHour { get; set; }
        public string LearningObjective { get; set; }

    }
}
