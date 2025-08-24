using Amazon.S3;
using Amazon.SQS;
using Autofac;

namespace DevSkill.Inventory.Worker
{
    public class WorkerModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;

        public WorkerModule(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AmazonSQSClient>().As<IAmazonSQS>().InstancePerLifetimeScope();
            builder.RegisterType<AmazonS3Client>().As<IAmazonS3>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
