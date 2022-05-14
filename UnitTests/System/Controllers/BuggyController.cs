using API.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace UnitTests.System.Controllers
{
    public class BuggyControllerTests
    {
        [Fact]
        public void GetNotFound_Should_Return_404()
        {
            var controller = new BuggyController();

            var result = (NotFoundResult)controller.GetNotFound();

            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public void GetBadRequest_Should_Return_400()
        {
            var controller = new BuggyController();

            var result = (BadRequestObjectResult)controller.GetBadRequest();

            result.StatusCode.Should().Be(400);
        }

        [Fact]
        public void GetServerError_Should_Return_Exception()
        {
            var controller = new BuggyController();

            var ex = Assert.Throws<Exception>(() => controller.GetServerError());

            Assert.Equal("This is a server error", ex.Message);
        }

        [Fact]
        public void GetUnauthorized_Should_Return_401()
        {
            var controller = new BuggyController();

            var result = (UnauthorizedResult)controller.GetUnauthorised();

            result.StatusCode.Should().Be(401);
        }
    }
}
