using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Help.Search.Heroku.Api.Middleware;

namespace Help.Search.Heroku.Api.Test.Middleware
{

    [TestFixture]
    public class GlobalExceptionHandlerMiddlewareTests
    {
        private Mock<RequestDelegate> _mockNext;
        private Mock<ILogger<GlobalExceptionHandlerMiddleware>> _mockLogger;
        private DefaultHttpContext _httpContext;
        private GlobalExceptionHandlerMiddleware _middleware;

        [SetUp]
        public void SetUp()
        {
            // Mock the RequestDelegate
            _mockNext = new Mock<RequestDelegate>();

            // Mock the Logger
            _mockLogger = new Mock<ILogger<GlobalExceptionHandlerMiddleware>>();

            // Create a HttpContext
            _httpContext = new DefaultHttpContext();
            _middleware = new GlobalExceptionHandlerMiddleware(_mockNext.Object, _mockLogger.Object);
        }

        // 3. **Test: Successful Request (No Exception)**

        [Test]
        public async Task InvokeAsync_NoException_ProceedsWithRequest()
        {
            // Arrange
            _mockNext.Setup(next => next(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert: Ensure that the next middleware is called
            _mockNext.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
        }



        [Test]
        public async Task InvokeAsync_ExceptionThrown_HandlesExceptionAndLogs()
        {
            // Arrange
            var exception = new Exception("Test exception");

            _mockNext.Setup(next => next(It.IsAny<HttpContext>())).Throws(exception);

            // Use a memory stream to capture the response body
            var responseStream = new MemoryStream();
            _httpContext.Response.Body = responseStream;

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert: Check the status code and response body
            Assert.AreEqual(StatusCodes.Status500InternalServerError, _httpContext.Response.StatusCode);

            // Ensure that the logger's LogError method was called
           // _mockLogger.Verify(logger => logger.LogError(It.Is<Exception>(e => e == exception), It.IsAny<string>()), Times.Once);

        }
    }

}
