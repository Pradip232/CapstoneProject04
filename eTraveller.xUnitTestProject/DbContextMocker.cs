using eTraveller.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Location = eTraveller.Models.Location;

namespace eTraveller.xUnitTestProject
{
    public static class DbContextMocker
    {
        public static ApplicationDbContext GetApplicationDbContext(string dbName)
        {
            // Create a fresh service provider for the InMemory Database instance
            var serviceProvider = new ServiceCollection()
                                  .AddEntityFrameworkInMemoryDatabase()
                                  .BuildServiceProvider();

            // Create a new options instance telling the context
            // to use InMemory database with the new service provider created above
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                           .UseInMemoryDatabase(dbName)
                           .UseInternalServiceProvider(serviceProvider)
                           .Options;

            // Create the instance of the DbContext
            var dbContext = new ApplicationDbContext(options);

            //Add entities to the InMemory Database to seed the test data
            dbContext.SeedData();

            return dbContext;
        }



        //NOTE: InMemory Databases do not support Relationships, and Database Constraints properly.
        //So, seed the Identity Column Values also.
        internal static readonly Location[] TestData_Locations = {
            new Location
            {
                LocationId = 1,
                LocationName = "First Location"
            },
            new Location
            {
                LocationId = 2,
                LocationName = "Second Location"
            },
            new Location
            {
                LocationId = 3,
                LocationName = "Third Location"
            },
        };


        /// <summary>
        ///     An extension Method for the DbContext object to Seed the Test Data.
        /// </summary>
        /// <param name="context">Application Db Context object.</param>
        private static void SeedData(this ApplicationDbContext context)
        {
            context.Locations.AddRange(TestData_Locations);

            // Commit the Changes to the database
            context.SaveChanges();
        }
    }
}
