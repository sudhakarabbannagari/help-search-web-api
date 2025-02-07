using Help.Search.Heroku.Api.Controllers;
using Help.Search.Heroku.Api.Filters;
using Help.Search.Heroku.Api.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Help.Search.Heroku.Api.Test.Controllers
{
    [TestFixture]
    public class HelpSearchControllerTest
    {
        private Mock<IHelpSearchRepository> _mockSearchRepository;
        private Mock<ITokenRepository> _mockTokenRepository;
        private HelpSearchController _controller;
        private Mock<ILogger<HelpSearchController>> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockSearchRepository = new Mock<IHelpSearchRepository>();
            _mockLogger = new Mock<ILogger<HelpSearchController>>();
            _mockTokenRepository = new Mock<ITokenRepository>();
            _controller = new HelpSearchController(_mockLogger.Object,_mockTokenRepository.Object,_mockSearchRepository.Object);
        }

        [Test]
        public async Task Search_ReturnsOkResult_WithSearchItems()
        {
            // Arrange
            _mockSearchRepository.Setup(service => service.SearchAsync(It.IsAny<string>())).ReturnsAsync("{\r\n  \r\n    \"message\": \"Test\",\r\n   \r\n}");

            // Act
            var result = await _controller.Search("test");

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var res = (OkObjectResult)result;
            Assert.AreEqual(200, res.StatusCode);
            _mockSearchRepository.Verify(service => service.SearchAsync(It.IsAny<string>()),Times.Once);
        }
        [Test]
        public async Task Search_SearchNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            _mockSearchRepository.Setup(service => service.SearchAsync(It.IsAny<string>())).ReturnsAsync("{\r\n  \r\n    \"message\": \"Test\",\r\n   \r\n}");

            // Act
            var result = await _controller.Search("");

            // Assert
            Assert.NotNull(result);
            _mockSearchRepository.Verify(service => service.SearchAsync(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task GenerateToken_ReturnsUnauthorizedResult()
        {
            // Arrange
            var data = new DTOs.SearchResponse() { Success = true, Data = new Models.SearchdData() };
            _mockTokenRepository.Setup(s => s.GenerateTokenAsync(It.IsAny<string>())).ReturnsAsync("TestToken");

            // Act
            var result = await _controller.GenerateToken("test");

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<UnauthorizedResult>(result);
        }
        [Test]
        public async Task GenerateToken_ReturnsOKResultWithToken()
        {
            // Arrange
            var data = new DTOs.SearchResponse() { Success = true, Data = new Models.SearchdData() };
            _mockTokenRepository.Setup(s => s.GenerateTokenAsync(It.IsAny<string>())).ReturnsAsync("TestToken");

            // Act
            var result = await _controller.GenerateToken("admin");

            // Assert
            Assert.NotNull(result);
            _mockTokenRepository.Verify(s => s.GenerateTokenAsync(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
    }
}
