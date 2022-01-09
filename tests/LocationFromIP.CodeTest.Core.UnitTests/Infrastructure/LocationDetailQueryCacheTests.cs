using LocationFromIP.CodeTest.Core.Interfaces;
using LocationFromIP.CodeTest.Core.Models;
using LocationFromIP.CodeTest.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LocationFromIP.CodeTest.Core.UnitTests.Infrastructure
{
    public class LocationDetailQueryCacheTests
    {
        [Fact]
        public async Task WhenCacheMissThenPopulateCache()
        {
            // Arrange
            const string IpAddress = "127.0.0.1";
            var expectedResult = new LocationDetailResult();

            var cacheMock = new Mock<IDistributedCache>();
            Expression<Func<IDistributedCache, Task<byte[]?>>> cacheGetExpression = x => x.GetAsync(IpAddress, CancellationToken.None);
            cacheMock.Setup(cacheGetExpression).ReturnsAsync((byte[]?)null);

            var queryMock = new Mock<ILocationDetailQuery>();
            queryMock.Setup(x => x.GetLocationDetail(IpAddress)).ReturnsAsync(expectedResult);

            var cachedQuery = new LocationDetailQueryCache(cacheMock.Object, queryMock.Object);

            // Act
            var locationDetail = await cachedQuery.GetLocationDetail(IpAddress);

            // Assert
            Assert.NotNull(locationDetail);
            Assert.Same(expectedResult, locationDetail);

            cacheMock.Verify(cacheGetExpression, Times.Exactly(1));
            cacheMock.Verify(x=> x.SetAsync(IpAddress, It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), CancellationToken.None), Times.Exactly(1));
        }

        [Fact]
        public async Task WhenCacheHitThenDoNotProxyRequest()
        {
            // Arrange
            const string IpAddress = "127.0.0.1";
            var cachedObject = Encoding.UTF8.GetBytes("{}");

            var cacheMock = new Mock<IDistributedCache>();
            Expression<Func<IDistributedCache, Task<byte[]>>> cacheGetExpression = x => x.GetAsync(IpAddress, CancellationToken.None);
            cacheMock.Setup(cacheGetExpression).ReturnsAsync(cachedObject);

            var queryMock = new Mock<ILocationDetailQuery>();

            var cachedQuery = new LocationDetailQueryCache(cacheMock.Object, queryMock.Object);

            // Act
            var locationDetail = await cachedQuery.GetLocationDetail(IpAddress);

            // Assert
            Assert.NotNull(locationDetail);

            cacheMock.Verify(cacheGetExpression, Times.Exactly(1));

            queryMock.Verify(x => x.GetLocationDetail(It.IsAny<string>()), Times.Never());
            cacheMock.Verify(x => x.SetAsync(IpAddress, It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), CancellationToken.None), Times.Never());
        }
    }
}
