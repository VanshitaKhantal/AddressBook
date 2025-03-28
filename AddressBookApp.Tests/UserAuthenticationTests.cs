using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.Service;
using RepositoryLayer.Context;
using ModelLayer.Entity;
using ModelLayer.DTO;
using RepositoryLayer.Service;
using BusinessLayer.Helper;
using Microsoft.Extensions.Configuration;

namespace AddressBookApp.Tests
{
    /// <summary>
    /// Unit tests for User Authentication functionality in the Address Book application.
    /// Tests include user registration and login scenarios.
    /// </summary>
    [TestFixture]
    public class UserAuthenticationTests
    {
        private UserService _userService;
        private AddressBookContext _dbContext;

        /// <summary>
        /// Sets up the test environment before each test.
        /// Initializes an in-memory database and pre-populates it with test data.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // Use InMemory Database
            var options = new DbContextOptionsBuilder<AddressBookContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new AddressBookContext(options);

            // Seed initial user data
            _dbContext.Users.Add(new User
            {
                Name = "Vanshita",
                Email = "khantalvanshita@gmail.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Vanshita@123"), // Hashed Password
                Role = "User"
            });
            _dbContext.SaveChanges();

            // Initialize dependencies
            var userRepository = new UserRL(_dbContext);
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var emailService = new EmailService(configuration);

            // Initialize User Service
            _userService = new UserService(userRepository, emailService);
        }

        /// <summary>
        /// Tests user registration when the user already exists.
        /// Ensures that duplicate registration is not allowed.
        /// </summary>
        [Test]
        public void RegisterUser_WhenUserAlreadyExists_ShouldReturnFalse()
        {
            // Arrange - Trying to register the same user
            var existingUser = new UserRequestModel
            {
                Name = "Vanshita",
                Email = "khantalvanshita@gmail.com",
                Password = "Vanshita@123",
                Role = "User"
            };

            // Act
            var result = _userService.RegisterUser(existingUser);

            // Assert
            Assert.IsFalse(result.Success, "Expected registration to fail, but it succeeded.");
        }

        /// <summary>
        /// Tests user login with valid credentials.
        /// Ensures that login is successful and the correct user data is returned.
        /// </summary>
        [Test]
        public void LoginUser_WithValidCredentials_ShouldReturnUser()
        {
            // Arrange
            var loginRequest = new LoginRequestModel
            {
                Email = "khantalvanshita@gmail.com",
                Password = "Vanshita@123"
            };

            // Act
            var user = _userService.LoginUser(loginRequest);

            // Assert
            Assert.IsNotNull(user, "Login failed but should have succeeded.");
            Assert.AreEqual(loginRequest.Email, user.Data.Email);
        }

        /// <summary>
        /// Tests user login with invalid credentials.
        /// Ensures that login fails and an appropriate error message is returned.
        /// </summary>
        //[Test]
        //public void LoginUser_WithInvalidCredentials_ShouldReturnErrorMessage()
        //{
        //    // Arrange
        //    var loginRequest = new LoginRequestModel
        //    {
        //        Email = "khantalvanshita@gmail.com",
        //        Password = "WrongPassword"
        //    };

        //    // Act
        //    var user = _userService.LoginUser(loginRequest);

        //    // Assert
        //    Assert.IsNull(user.Data, "Expected login to fail, but it returned a user.");
        //    Assert.AreEqual("Invalid credentials", user.Message);
        //}
    }
}
