using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace eTraveller.xUnitTestProject
{
    //Added nuget package: Microsoft.EntityFrameworkCore.InMemory
    //Added nuget package: Moq
    //Added nuget package: FluentAssertion
    //Also add project reference to the WebApp
    public partial class LocationsApiTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        public LocationsApiTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }


    }
}
