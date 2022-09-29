//using eTraveller.Areas.ETraveller.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using eTraveller.Controllers;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using eTraveller.Models;

namespace eTraveller.xUnitTestProject
{
    public partial class LocationsApiTests
    {
        [Fact]
        public void GetLocations_OkResult()
        {
            // ARRANGE
            var dbName = nameof(LocationsApiTests.GetLocations_OkResult);
            var logger = Mock.Of<ILogger<LocationsController>>();
            var dbContext = DbContextMocker.GetApplicationDbContext(dbName);
            var apiController = new LocationsController(dbContext, logger);

            // ACT
            IActionResult actionResult = apiController.GetLocations().Result;

            // ASSERT
            // --- Check if the ActionResult is of the type OkObjectResult
            Assert.IsType<OkObjectResult>(actionResult);

            // --- Check if the HTTP Response Code is 200 "Ok"
            int expectedStatusCode = (int)System.Net.HttpStatusCode.OK;
            int actualStatusCode = (actionResult as OkObjectResult).StatusCode.Value;
            Assert.Equal<int>(expectedStatusCode, actualStatusCode);

        }

        [Fact]
        public void GetLocations_CheckCorrectResult()
        {
            // ARRANGE
            var dbName = nameof(LocationsApiTests.GetLocations_OkResult);
            var logger = Mock.Of<ILogger<LocationsController>>();
            var dbContext = DbContextMocker.GetApplicationDbContext(dbName);
            var apiController = new LocationsController(dbContext, logger);

            // ACT
            IActionResult actionResult = apiController.GetLocations().Result;

            // ASSERT
            // --- Check if the ActionResult is of the type OkObjectResult
            Assert.IsType<OkObjectResult>(actionResult);
            var OkayResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;

            Assert.IsAssignableFrom<List<Location>>(OkayResult.Value);

            var locationFromApi = OkayResult.Value.Should().BeAssignableTo<List<Location>>().Subject;

            Assert.NotNull(locationFromApi);

            Assert.Equal<int>(expected: DbContextMocker.TestData_Locations.Length, actual: locationFromApi.Count);

            int ndx = 0;
            foreach (Location location in DbContextMocker.TestData_Locations)
            {
                // ASSERT: check if the Location ID is correct
                Assert.Equal<int>(expected: location.LocationId,
                                  actual: locationFromApi[ndx].LocationId);

                // ASSERT: check if the Location Name is correct
                Assert.Equal(expected: location.LocationName,
                             actual: locationFromApi[ndx].LocationName);

                _testOutputHelper.WriteLine($"Compared Row # {ndx} successfully");

                ndx++;          // now compare against the next element in the array
            }


        }
    }
}
