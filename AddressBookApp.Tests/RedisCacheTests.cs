using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RepositoryLayer.Helper;
using System;
using System.Text;

namespace AddressBookApp.Tests
{
    /// <summary>
    /// Unit tests for RedisCacheService.
    /// Ensures caching operations (Set, Get, Remove) function correctly.
    /// </summary>
    [TestFixture]
    public class RedisCacheTests
    {
        private Mock<IDistributedCache> _mockCache;
        private RedisCacheService _redisCacheService;

        /// <summary>
        /// Initializes mock dependencies before each test.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _mockCache = new Mock<IDistributedCache>();
            _redisCacheService = new RedisCacheService(_mockCache.Object);
        }

        /// <summary>
        /// Verifies that RemoveCache calls the Remove method on IDistributedCache.
        /// </summary>
        [Test]
        public void RemoveCache_ValidKey_ShouldCallRemoveMethod()
        {
            // Arrange
            var cacheKey = "test_key";

            // Act
            _redisCacheService.RemoveCache(cacheKey);

            // Assert
            _mockCache.Verify(x => x.Remove(cacheKey), Times.Once);
        }

        /// <summary>
        /// Verifies that SetCache correctly stores serialized data in the cache.
        /// </summary>
        [Test]
        public void SetCache_ValidData_ShouldStoreDataInCache()
        {
            // Arrange
            var cacheKey = "test_key";
            var testData = new { Name = "John Doe", Age = 30 };
            var serializedData = JsonConvert.SerializeObject(testData);

            // Act
            _redisCacheService.SetCache(cacheKey, testData, 10);

            // Assert
            _mockCache.Verify(x => x.Set(
                cacheKey,
                It.Is<byte[]>(data => Encoding.UTF8.GetString(data) == serializedData),
                It.IsAny<DistributedCacheEntryOptions>()), Times.Once);
        }

        /// <summary>
        /// Verifies that GetCache retrieves data correctly when the key exists.
        /// </summary>
        [Test]
        public void GetCache_KeyExists_ShouldReturnCachedData()
        {
            // Arrange
            var cacheKey = "test_key";
            var testData = new { Name = "John Doe", Age = 30 };
            var serializedData = JsonConvert.SerializeObject(testData);
            var cachedBytes = Encoding.UTF8.GetBytes(serializedData);

            _mockCache.Setup(x => x.Get(cacheKey)).Returns(cachedBytes);

            // Act
            var result = _redisCacheService.GetCache<object>(cacheKey);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(JsonConvert.SerializeObject(result), serializedData);
        }

        /// <summary>
        /// Verifies that GetCache returns default when the key does not exist in cache.
        /// </summary>
        [Test]
        public void GetCache_KeyDoesNotExist_ShouldReturnDefault()
        {
            // Arrange
            var cacheKey = "non_existent_key";
            _mockCache.Setup(x => x.Get(cacheKey)).Returns((byte[])null);

            // Act
            var result = _redisCacheService.GetCache<object>(cacheKey);

            // Assert
            Assert.IsNull(result);
        }
    }
}
