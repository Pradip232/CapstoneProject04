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
    ///     Bad update data scenarios:
    ///     - Name is NULL
    ///     - Name is EMPTY STRING
    ///     - Name contains more characters than what is allowed
    ///     - NULL Location object
    ///     - ID not matching with the ID of the row identified to be updated.
    ///     - ID replaced with a negative value
    ///     - Replacing the object retrieved before the update, with a completely new object
    ///     - Since the HTTP PUT receives a Nullable INT as first parameter, pass a NULL value
    ///
    ///     LIMITATIONS OF WORKING WITH InMemory Database
    ///     - Relationships between Tables are not supported.
    ///     - EF Core DataAnnotation Validations are not supported.
    /// </remarks>
    public partial class LocationsApiTests
    {
        [Fact]
        public async void UpdateLocation_OkResult01()
        {
            // ARRANGE
            var dbName = nameof(LocationsApiTests.UpdateLocation_OkResult01);
            var logger = Mock.Of<ILogger<LocationsController>>();
            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var apiController = new LocationsController(dbContext, logger);
            int editLocationId = 2;
            Location originalLocation, changedLocation;
            changedLocation = new Location
            {
                LocationId = editLocationId,
                LocationName = "--Changed Location Name"
            };

            // ACT #1: Get the Record to Edit.

            // (a) Get the Location to edit (to ensure that the row exists before editing it)
            IActionResult actionResultGet = await apiController.GetLocation(editLocationId);

            // (b) Check if HTTP 200 "Ok" 
            OkObjectResult OkResult = actionResultGet.Should().BeOfType<OkObjectResult>().Subject;

            // (c) Extract the Location Object from the OkObjectResult
            originalLocation = OkResult.Value.Should().BeAssignableTo<Location>().Subject;

            // (d) Check if the data to be edited was received from the API
            Assert.NotNull(originalLocation);

            _testOutputHelper.WriteLine("Retrieved the Data from the API.");

            try
            {
                // ACT #2: Try to update the data, using a completely new object
                //         NOTE: This will throw the InvalidOperationException!
                var actionResultPutAttempt1 = await apiController.PutLocation(editLocationId, changedLocation);

                // ASSERT - if the update was successfull.
                Assert.IsType<OkResult>(actionResultPutAttempt1);

                _testOutputHelper.WriteLine("Updated the changes back to the API - using a new object.");
            }
            catch (System.InvalidOperationException exp)
            {
                // The PUT operation should throw this exception,
                // because it is an object that does not carry change tracking information.

                _testOutputHelper.WriteLine("Failed to update the change back to the API - using a new object");
                _testOutputHelper.WriteLine($"-- Exception Type: {exp.GetType()}");
                _testOutputHelper.WriteLine($"-- Exception Message: {exp.Message}");
                _testOutputHelper.WriteLine($"-- Exception Source: {exp.Source}");
                _testOutputHelper.WriteLine($"-- Exception TargetSite: {exp.TargetSite}");
            }
        }

        [Fact]
        public async void UpdateLocation_OkResult02()
        {
            // ARRANGE
            var dbName = nameof(LocationsApiTests.UpdateLocation_OkResult02);
            var logger = Mock.Of<ILogger<LocationsController>>();
            using var dbContext = DbContextMocker.GetApplicationDbContext(dbName);      // Disposable!
            var apiController = new LocationsController(dbContext, logger);
            int editLocationId = 2;
            Location originalLocation;
            string changedLocationName = "--- CHANGED Location Name";

            // ACT #1: Get the Record to Edit.

            // (a) Get the Location to edit (to ensure that the row exists before editing it)
            IActionResult actionResultGet = await apiController.GetLocation(editLocationId);

            // (b) Check if HTTP 200 "Ok" 
            OkObjectResult OkResult = actionResultGet.Should().BeOfType<OkObjectResult>().Subject;

            // (c) Extract the Location Object from the OkObjectResult
            originalLocation = OkResult.Value.Should().BeAssignableTo<Location>().Subject;

            // (d) Check if the data to be edited was received from the API
            Assert.NotNull(originalLocation);

            _testOutputHelper.WriteLine("Retrieved the Data from the API.");

            // ACT #2: Now, try to update the data
            // SOLUTION: The following lines would work!
            //           Reason, we are modifying the data originally received.
            //           And then, calling the PUT operation.
            //           So, the API is able to find the ChangeTracking data associated to the object.

            // (a) Change the data of the object that was received from the API.
            originalLocation.LocationName = changedLocationName;

            // (b) Call the HTTP PUT API to update the changes (with the updated object)
            var actionResultPutAttempt2 = await apiController.PutLocation(editLocationId, originalLocation);

            // ASSERT - if the update was successfull.
            Assert.IsType<NoContentResult>(actionResultPutAttempt2);

            _testOutputHelper.WriteLine("Updated the changes back to the API - using the object received");
        }
    }
}
