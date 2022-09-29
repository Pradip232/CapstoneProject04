using eTraveller.Controllers;
using eTraveller.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace eTraveller.xUnitTestProject
{
    public partial class LocationsApiTests
    {
        [Fact]
        public void GetLocationById_NotFoundResult()
        {
            // ARRANGE
            var dbName = nameof(LocationsApiTests.GetLocationById_NotFoundResult);
            var logger = Mock.Of<ILogger<LocationsController>>();

            // using (var db = DbContextMocker.GetApplicationDbContext(dbName))
            // {
            // }           // db.Dispose(); implicitly called when you exit the USING Block

            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var apiController = new LocationsController(dbContext, logger);
            int findLocationID = 900;

            // ACT
            IActionResult actionResultGet = apiController.GetLocation(findLocationID).Result;

            // ASSERT - check if the IActionResult is NotFound 
            Assert.IsType<NotFoundResult>(actionResultGet);

            // ASSERT - check if the Status Code is (HTTP 404) "NotFound"
            int expectedStatusCode = (int)System.Net.HttpStatusCode.NotFound;
            var actualStatusCode = (actionResultGet as NotFoundResult).StatusCode;
            Assert.Equal<int>(expectedStatusCode, actualStatusCode);
        }

        [Fact]
        public void GetLocationById_BadRequestResult()
        {
            // ARRANGE
            var dbName = nameof(LocationsApiTests.GetLocationById_BadRequestResult);
            var logger = Mock.Of<ILogger<LocationsController>>();
            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var controller = new LocationsController(dbContext, logger);
            int? findLocationID = null;

            // ACT
            IActionResult actionResultGet = controller.GetLocation(findLocationID).Result;

            // ASSERT - check if the IActionResult is BadRequest
            Assert.IsType<BadRequestResult>(actionResultGet);

            // ASSERT - check if the Status Code is (HTTP 400) "BadRequest"
            int expectedStatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            var actualStatusCode = (actionResultGet as BadRequestResult).StatusCode;
            Assert.Equal<int>(expectedStatusCode, actualStatusCode);
        }

        [Fact]
        public void GetLocationById_OkResult()
        {
            // ARRANGE
            var dbName = nameof(LocationsApiTests.GetLocationById_OkResult);
            var logger = Mock.Of<ILogger<LocationsController>>();
            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var controller = new LocationsController(dbContext, logger);
            int findLocationID = 2;

            // ACT
            IActionResult actionResultGet = controller.GetLocation(findLocationID).Result;

            // ASSERT - if IActionResult is Ok
            Assert.IsType<OkObjectResult>(actionResultGet);

            // ASSERT - if Status Code is HTTP 200 (Ok)
            int expectedStatusCode = (int)System.Net.HttpStatusCode.OK;
            var actualStatusCode = (actionResultGet as OkObjectResult).StatusCode.Value;
            Assert.Equal<int>(expectedStatusCode, actualStatusCode);
        }

        [Fact]
        public void GetLocationById_CorrectResult()
        {
            // ARRANGE
            var dbName = nameof(LocationsApiTests.GetLocationById_OkResult);
            var logger = Mock.Of<ILogger<LocationsController>>();
            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var controller = new LocationsController(dbContext, logger);
            int findLocationID = 2;
            Location expectedLocation = DbContextMocker.TestData_Locations
                                        .SingleOrDefault(c => c.LocationId == findLocationID);

            // ACT
            IActionResult actionResultGet = controller.GetLocation(findLocationID).Result;

            // ASSERT - if IActionResult is Ok
            Assert.IsType<OkObjectResult>(actionResultGet);

            // ASSERT - if IActionResult (i.e., OkObjectResult) contains an object of the type Location
            OkObjectResult okResult = actionResultGet.Should().BeOfType<OkObjectResult>().Subject;
            Assert.IsType<Location>(okResult.Value);

            // Extract the Location object from the result.
            Location actualLocation = okResult.Value.Should().BeAssignableTo<Location>().Subject;
            _testOutputHelper.WriteLine($"Found: LocationID == {actualLocation.LocationId}");

            // ASSERT - if Location is NOT NULL
            Assert.NotNull(actualLocation);

            // ASSERT - if the LocationId is containing the expected data.
            Assert.Equal<int>(expected: expectedLocation.LocationId,
                              actual: actualLocation.LocationId);

            // ASSERT - if the CateogoryName is correct 
            Assert.Equal(expected: expectedLocation.LocationName,
                         actual: actualLocation.LocationName);
        }
    }
}
