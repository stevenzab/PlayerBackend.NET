using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PlayerBack.Api.Middleware;
using System;
using System.Threading.Tasks;

namespace PlayerBack.Api.UnitTests.Middleware
{
    [TestClass]
    public class GlobalExceptionHandlerTests
    {
        private MockRepository mockRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
        }

        private GlobalExceptionHandler CreateGlobalExceptionHandler()
        {
            return new GlobalExceptionHandler();
        }

        [TestMethod]
        public async Task TryHandleAsync_SetsResponse_ForGenericException()
        {
            // Arrange
            var handler = CreateGlobalExceptionHandler();
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            var exception = new Exception("Something went wrong");

            // Act
            var result = await handler.TryHandleAsync(context, exception, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        }
    }
}
