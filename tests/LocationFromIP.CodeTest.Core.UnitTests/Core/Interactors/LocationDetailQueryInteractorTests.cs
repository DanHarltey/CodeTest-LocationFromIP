using LocationFromIP.CodeTest.Core.Interactors;
using LocationFromIP.CodeTest.Core.Interfaces;
using LocationFromIP.CodeTest.Core.Models;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LocationFromIP.CodeTest.Core.UnitTests.Core.Interactors
{
    public class LocationDetailQueryInteractorTests
    {
        [Fact]
        public async Task WhenIpAddressFoundThenLocationDetail()
        {
            const string IpAddress = "127.0.0.1";

            // Arrange
            var queryResult = new LocationDetailResult()
            {
                HasFoundIpAddress = true,
                Detail = new()
                {
                }
            };

            var mock = new Mock<ILocationDetailQuery>();
            mock.Setup(x => x.GetLocationDetail(IpAddress))
                .ReturnsAsync(queryResult);

            var interactor = new LocationDetailQueryInteractor(mock.Object);

            // Act
            var actual = await interactor.GetLocationDetail(IpAddress);

            // Assert
            Assert.NotNull(actual);
            Assert.Same(queryResult, actual);
        }

        [Fact]
        public async Task WhenIpAddressNotFoundThenReturnNotFound()
        {
            const string IpAddress = "127.0.0.1";

            // Arrange
            var queryResult = new LocationDetailResult()
            {
                HasFoundIpAddress = false,
            };

            var mock = new Mock<ILocationDetailQuery>();
            mock.Setup(x => x.GetLocationDetail(IpAddress))
                .ReturnsAsync(queryResult);

            var interactor = new LocationDetailQueryInteractor(mock.Object);

            // Act
            var actual = await interactor.GetLocationDetail(IpAddress);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.HasFoundIpAddress);
            Assert.Null(actual.Detail);
        }

        [Fact]
        public async Task WhenIpAddressNullThenThrowArgumentNull()
        {
            // Arrange
            var expected = new LocationDetailResult();
            var mock = new Mock<ILocationDetailQuery>();

            var interactor = new LocationDetailQueryInteractor(mock.Object);

            // Act
            var method = () => interactor.GetLocationDetail(null);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(method);
        }
    }
}
