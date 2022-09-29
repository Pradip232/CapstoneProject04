using eTraveller.Controllers;
using eTraveller.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace eTraveller.xUnitTestProject
{
    /// <remarks>
    ///     Bad insertion data scenarios:
    ///     - Name is NULL
    ///     - Name is EMPTY STRING
    ///     - Name contains more characters than what is allowed
    ///     - NULL Location object
    ///     
    ///     LIMITATIONS OF WORKING WITH InMemory Database
    ///     - Relationships between Tables are not supported.
    ///     - EF Core DataAnnotation Validations are not supported.
    /// </remarks>
    public partial class LocationsApiTests
    {
        [Fact]
        public void InsertLocation_OkResult()
        {
            // ARRANGE
            var dbName = nameof(LocationsApiTests.InsertLocation_OkResult);
            var logger = Mock.Of<ILogger<LocationsController>>();
            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var apiController = new LocationsController(dbContext, logger);
            Location LocationToAdd = new Location
            {
                LocationId = 5,
                LocationName = "New Location"             // IF = null, then: INVALID!  LocationName is REQUIRED
            };

            // ACT
            IActionResult actionResultPost = apiController.PostLocation(LocationToAdd).Result;

            // ASSERT - check if the IActionResult is Ok
            Assert.IsType<OkObjectResult>(actionResultPost);

            // ASSERT - check if the Status Code is (HTTP 200) "Ok", (HTTP 201 "Created")
            int expectedStatusCode = (int)System.Net.HttpStatusCode.OK;
            var actualStatusCode = (actionResultPost as OkObjectResult).StatusCode.Value;
            Assert.Equal<int>(expectedStatusCode, actualStatusCode);

            // Extract the result from the IActionResult object.
            var postResult = actionResultPost.Should().BeOfType<OkObjectResult>().Subject;

            // ASSERT - if the result is a CreatedAtActionResult
            Assert.IsType<CreatedAtActionResult>(postResult.Value);

            // Extract the inserted Location object
            Location actualLocation = (postResult.Value as CreatedAtActionResult).Value
                                      .Should().BeAssignableTo<Location>().Subject;

            // ASSERT - if the inserted Location object is NOT NULL
            Assert.NotNull(actualLocation);

            Assert.Equal(LocationToAdd.LocationId, actualLocation.LocationId);
            Assert.Equal(LocationToAdd.LocationName, actualLocation.LocationName);
        }
    }
}
