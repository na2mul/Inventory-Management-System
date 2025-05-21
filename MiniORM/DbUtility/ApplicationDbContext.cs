using Microsoft.EntityFrameworkCore;
using MiniORM.TestClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MiniORM.DbUtility
{
    public class ApplicationDbContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DbConnectionString.connection);

        }

        public DbSet<Instructor> Instructor { get; set; }
        public DbSet<PresentAddress> PresentAddress { get; set; }
        public DbSet<PermanentAddress> PermanentAddress { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Phone> Phone { get; set; }
        public DbSet<Session> Session { get; set; }
        public DbSet<Topic> Topic { get; set; }
        public DbSet<AdmissionTest> AdmissionTest { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
