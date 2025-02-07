using Help.Search.Heroku.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Help.Search.Heroku.Api.Test.Filters
{

    [TestFixture]
    public class GlobalExceptionFilterTests
    {
        private Mock<ILogger<GlobalExceptionFilter>> _mockLogger;
        private GlobalExceptionFilter _exceptionFilter;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<GlobalExceptionFilter>>();
            _exceptionFilter = new GlobalExceptionFilter(_mockLogger.Object);
        }

        public void OnException_ShouldLogError_AndSet500Response()
        {
            // Arrange
            var exception = new Exception("Test exception");
            var context = new ExceptionContext(new ActionContext(), new Mock<IFilterMetadata[]>().Object)
            {
                Exception = exception
            };

            // Act
            _exceptionFilter.OnException(context);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Unhandled exception occurred.")),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

            Assert.That(context.Result, Is.TypeOf<ObjectResult>());
            var objectResult = (ObjectResult)context.Result;
            Assert.That(objectResult.StatusCode, Is.EqualTo(500));
            Assert.That(context.ExceptionHandled, Is.True);
        }
    }

}
