using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestructuringTool.DAL
{
    public class RestructuringContext : DbContext
    {
        static RestructuringContext()
        {
            //Database.SetInitializer<TestContext>(new MyDatabaseInitializer());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<RestructuringContext, Configuration>());
        }

        public RestructuringContext()
            : base("Name=RestructuringContext")
        {
            //Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<RestracturingProgram> RestracturingPrograms { get; set; }

        public DbSet<ListItem> ListItems { get; set; }

        public DbSet<Expense> Expenses { get; set; }

        public DbSet<Program> Programs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            /*modelBuilder.Configurations.Add(new DependentMap());
            modelBuilder.Configurations.Add(new EmployeeMap());
            modelBuilder.Configurations.Add(new EmployeeStatuMap());
            modelBuilder.Configurations.Add(new JobTitleMap());
            modelBuilder.Configurations.Add(new PtoRequestMap());
            modelBuilder.Configurations.Add(new PtoRequestStatuMap());*/
        }
    }

    internal sealed class Configuration : DbMigrationsConfiguration<RestructuringContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            //ContextKey = "ConsoleApplication1.TestContext";
        }

        protected override void Seed(RestructuringContext context)
        {
            context.Programs.AddOrUpdate(p => p.Name,
                new Program() {Name = "Program1"},
                new Program() {Name = "Program2"},
                new Program() {Name = "Program3"},
                new Program() {Name = "Program4"},
                new Program() {Name = "Program5"});

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }

    /*public class MyDatabaseInitializer : CreateDatabaseIfNotExists<RestructuringContext>
    {
        public override void InitializeDatabase(RestructuringContext context)
        {
            base.InitializeDatabase(context);
        }

        protected override void Seed(RestructuringContext context)
        {
            base.Seed(context);
        }
    }*/
}
