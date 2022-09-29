using eTraveller.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace eTraveller.xUnitTestProject
{
    public partial class LocationsApiTests
    {
        [Fact]
        public void DeleteLocation_NotFoundResult()
        {
            // ARRANGE
            var dbName = nameof(LocationsApiTests.DeleteLocation_NotFoundResult);
            var logger = Mock.Of<ILogger<LocationsController>>();
            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var apiController = new LocationsController(dbContext, logger);
            int findLocationID = 900;

            // ACT
            IActionResult actionResultDelete = apiController.DeleteLocation(findLocationID).Result;

            // ASSERT - check if the IActionResult is NotFound 
            Assert.IsType<NotFoundResult>(actionResultDelete);

            // ASSERT - check if the Status Code is (HTTP 404) "NotFound"
            int expectedStatusCode = (int)System.Net.HttpStatusCode.NotFound;
            var actualStatusCode = (actionResultDelete as NotFoundResult).StatusCode;
            Assert.Equal<int>(expectedStatusCode, actualStatusCode);
        }

        [Fact]
        public void DeleteLocation_BadRequestResult()
        {
            // ARRANGE
            var dbName = nameof(LocationsApiTests.DeleteLocation_BadRequestResult);
            var logger = Mock.Of<ILogger<LocationsController>>();
            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var apiController = new LocationsController(dbContext, logger);
            int? findLocationID = null;

            // ACT
            IActionResult actionResultDelete = apiController.DeleteLocation(findLocationID).Result;

            // ASSERT - check if the IActionResult is BadRequest 
            Assert.IsType<BadRequestResult>(actionResultDelete);

            // ASSERT - check if the Status Code is (HTTP 400) "BadRequest"
            int expectedStatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            var actualStatusCode = (actionResultDelete as BadRequestResult).StatusCode;
            Assert.Equal<int>(expectedStatusCode, actualStatusCode);
        }

        [Fact]
        public void DeleteLocation_OkResult()
        {
            // ARRANGE
            var dbName = nameof(LocationsApiTests.DeleteLocation_BadRequestResult);
            var logger = Mock.Of<ILogger<LocationsController>>();
            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var apiController = new LocationsController(dbContext, logger);
            int findLocationID = 1;

            // ACT
            IActionResult actionResultDelete = apiController.DeleteLocation(findLocationID).Result;

            // ASSERT - if IActionResult is Ok
            Assert.IsType<OkObjectResult>(actionResultDelete);

            // ASSERT - if Status Code is HTTP 200 (Ok)
            int expectedStatusCode = (int)System.Net.HttpStatusCode.OK;
            var actualStatusCode = (actionResultDelete as OkObjectResult).StatusCode.Value;
            Assert.Equal<int>(expectedStatusCode, actualStatusCode);
        }
    }
}
