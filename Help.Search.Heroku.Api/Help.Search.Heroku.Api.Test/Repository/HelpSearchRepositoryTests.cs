using Help.Search.Heroku.Api.Controllers;
using Help.Search.Heroku.Api.Data;
using Help.Search.Heroku.Api.DTOs;
using Help.Search.Heroku.Api.Models;
using Help.Search.Heroku.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Help.Search.Heroku.Api.Test.Repository
{
    [TestFixture]
    public class HelpSearchRepositoryTests
    {
        private Mock<ILogger<HelpSearchController>> _mockLogger;
        private HelpSearchRepository _repository;
        private Mock<IConfiguration> _mockIConfiguration;

        [SetUp]
        public void Setup()
        {
            _mockIConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<HelpSearchController>>();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

           var _context = new ApplicationDbContext(options);
            _context.Data.Add(new SearchdData() { BroadbandId= new Random().Next(1,100), Name ="xyz"});
            _context.SaveChanges();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"message\": \"success\"}"),
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            _repository = new HelpSearchRepository(_context, _mockLogger.Object, _mockIConfiguration.Object, httpClient);
        }

        [Test]
        public async Task SearchAsync_ReturnsOkResult_WithSearchItems()
        {
            // Arrange


            // Act
            var result = await _repository.SearchAsync("xyz");

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<string>(result);

        }

        [Test]
        public async Task Search_ReturnsOkResult_WithSearchItems()
        {
            // Arrange
           

            // Act
            var result = await _repository.SearchAsyncTest("xyz");

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsInstanceOf<SearchResponse>(result);

        }
    }
}
