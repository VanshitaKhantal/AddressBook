using BusinessLayer.Helper;
using BusinessLayer.Interface;
using Microsoft.Extensions.Configuration;
using ModelLayer.DTO;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace AddressBookApp.Tests
{
    /// <summary>
    /// Unit tests for JWT token generation service.
    /// Ensures token creation and claim validation functionality.
    /// </summary>
    [TestFixture]
    public class JwtServiceTests
    {
        private IGenerateToken _jwtService;
        private IConfiguration _configuration;

        /// <summary>
        /// Sets up the test environment by initializing configuration and JWT service.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Jwt:Key", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.KMUFsIDTnFmyG3nMiGM6H9FNFUROf3wh7SmqJp-QV30" },
                { "Jwt:Issuer", "your_issuer" },
                { "Jwt:Audience", "your_audience" }
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _jwtService = new GenerateToken(_configuration);
        }

        /// <summary>
        /// Verifies that GenerateJwtToken returns a non-null and non-empty token for a valid user.
        /// </summary>
        [Test]
        public void GenerateJwtToken_ValidUser_ReturnsValidToken()
        {
            // Arrange
            var user = new UserResponseModel
            {
                Name = "Vanshita",
                Email = "khantalvanshita@gmail.com",
                Role = "User"
            };

            // Act
            string token = _jwtService.GenerateJwtToken(user);

            // Assert
            Assert.IsNotNull(token);
            Assert.IsNotEmpty(token);
        }

        /// <summary>
        /// Verifies that the generated JWT token contains the correct claims.
        /// </summary>
        [Test]
        public void GenerateJwtToken_TokenContainsCorrectClaims()
        {
            // Arrange
            var user = new UserResponseModel
            {
                Name = "Vanshita",
                Email = "khantalvanshita@gmail.com",
                Role = "User"
            };

            // Act
            string token = _jwtService.GenerateJwtToken(user);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Assert
            Assert.IsNotNull(jwtToken);
            Assert.AreEqual("your_issuer", jwtToken.Issuer);
            Assert.AreEqual("your_audience", jwtToken.Audiences.FirstOrDefault());
            Assert.AreEqual("Vanshita", jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value);
            Assert.AreEqual("khantalvanshita@gmail.com", jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value);
            Assert.AreEqual("User", jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value);
        }
    }
}
