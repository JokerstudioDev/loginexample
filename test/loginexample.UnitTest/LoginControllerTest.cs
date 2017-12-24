using System;
using loginexample.API.Controllers;
using loginexample.API.DAC;
using loginexample.API.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace loginexample.UnitTest
{
    public class LoginControllerTest
    {
        private readonly LoginController loginController;

        [Fact]
        public void PostLogin_ReturnUserJSONFormat_WhenLoginIsSuccessful()
        {

            var mockDac = new Mock<IAccountDAC>();
            mockDac.Setup(dac => dac.GetUser(It.IsAny<string>(), It.IsAny<string>())).Returns(
                new User()
                {
                    Id = 1,
                    Username = "ploy",
                    Password = "1234"
                }
            );
            var loginController = new LoginController(mockDac.Object);
            var result = loginController.Post(
                new User()
                {
                    Username = "ploy",
                    Password = "1234"
                }
            );

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<User>(viewResult.Value);
            Assert.Equal("ploy", model.Username);
        }

        [Fact]
        public void PostInvalidParameter_ReturnBadRequestResult_WhenLoginIsNotSuccess()
        {

            var mockDac = new Mock<IAccountDAC>();
            mockDac.Setup(dac => dac.GetUser(It.IsAny<string>(), It.IsAny<string>())).Returns(
                new User()
                {
                    Id = 1,
                    Username = "ploy",
                    Password = "1234"
                }
            );
            var loginController = new LoginController(mockDac.Object);
            var result = loginController.Post(
                new User()
                {
                    Username = "ploy"
                }
            );

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void PostUserLogin_ReturnUnauthorizedResult_WhenLoginIsNotSuccess()
        {

            var mockDac = new Mock<IAccountDAC>();
            mockDac.Setup(dac => dac.GetUser(It.IsAny<string>(), It.IsAny<string>())).Returns((User)null);
            var loginController = new LoginController(mockDac.Object);
            var result = loginController.Post(
                new User()
                {
                    Username = "john",
                    Password = "1234"
                }
            );

            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}
