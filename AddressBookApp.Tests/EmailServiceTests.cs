using NUnit.Framework;
using BusinessLayer.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace AddressBookApp.Tests
{
    /// <summary>
    /// Unit tests for the EmailService class.
    /// Ensures that email sending functionality works as expected.
    /// </summary>
    [TestFixture]
    public class EmailServiceTests
    {
        private EmailService _emailService;

        /// <summary>
        /// Sets up the test environment by initializing the EmailService
        /// with a mocked configuration containing SMTP settings.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // Mock configuration with SMTP settings
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Smtp:Host", "smtp.gmail.com" },
                { "Smtp:Port", "587" },
                { "Smtp:Username", "khantalvanshita@gmail.com" },
                { "Smtp:Password", "jwpg zfuy heum gams" },
                { "Smtp:SenderEmail", "khantalvanshita@gmail.com" }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _emailService = new EmailService(configuration);
        }

        /// <summary>
        /// Tests that the SendEmail method executes without throwing an exception when provided with valid inputs.
        /// This ensures that the email sending mechanism is properly configured.
        /// </summary>
        [Test]
        public void SendEmail_ValidInputs_ShouldSendEmailSuccessfully()
        {
            // Arrange
            string to = "khantalvanshita@gmail.com";
            string subject = "Test Email";
            string body = "This is a test email sent from unit test.";

            // Act & Assert
            Assert.DoesNotThrow(() => _emailService.SendEmail(to, subject, body));
        }
    }
}
