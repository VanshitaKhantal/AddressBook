using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ModelLayer.Entity;
using NUnit.Framework;
using RepositoryLayer.Context;
using RepositoryLayer.Helper;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AddressBookApp.Tests
{
    /// <summary>
    /// Unit tests for CRUD operations in the AddressBook Repository Layer.
    /// Ensures correct functionality of adding, retrieving, updating, and deleting contacts.
    /// </summary>
    [TestFixture]
    public class AddressBookCrudTests
    {
        private AddressBookContext _dbContext;
        private AddressBookRL _addressBookRepo;

        /// <summary>
        /// Sets up the test environment before each test.
        /// Initializes the in-memory database and populates it with test data.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AddressBookContext>()
                .UseInMemoryDatabase(databaseName: "AddressBookTestDB")
                .Options;

            _dbContext = new AddressBookContext(options);
            _dbContext.Database.EnsureDeleted(); // Reset DB for fresh tests
            _dbContext.Database.EnsureCreated();

            // Create a Fake DistributedCache instance
            var distributedCache = new MemoryDistributedCache(new OptionsWrapper<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions()));

            // Pass it to RedisCacheService
            var redisCacheService = new RedisCacheService(distributedCache);

            // Pass RedisCacheService to AddressBookRL
            _addressBookRepo = new AddressBookRL(_dbContext, redisCacheService);

            // Seed initial data
            _dbContext.Contacts.Add(new Contact
            {
                Id = 1,
                FullName = "Rekha Khantal",
                Address = "Chirgaon",
                City = "Jhansi",
                State = "Uttar Pradesh",
                ZipCode = "284301",
                PhoneNumber = "284318467270"
            });
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tests retrieving all contacts from the database.
        /// Ensures that the method returns the correct number of contacts.
        /// </summary>
        [Test]
        public void GetAllContacts_ShouldReturnContacts()
        {
            // Act
            var result = _addressBookRepo.GetAllContacts();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        /// <summary>
        /// Tests retrieving a contact by a valid ID.
        /// Ensures that the correct contact details are returned.
        /// </summary>
        [Test]
        public void GetContactById_ExistingId_ShouldReturnContact()
        {
            // Act
            var result = _addressBookRepo.GetContactById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Rekha Khantal", result.FullName);
        }

        /// <summary>
        /// Tests retrieving a contact by a non-existing ID.
        /// Ensures that the method returns null.
        /// </summary>
        [Test]
        public void GetContactById_NonExistingId_ShouldReturnNull()
        {
            // Act
            var result = _addressBookRepo.GetContactById(99);

            // Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// Tests adding a new contact to the database.
        /// Ensures that the contact count increases by one.
        /// </summary>
        [Test]
        public void AddContact_ShouldIncreaseCount()
        {
            // Arrange
            var newContact = new Contact
            {
                Id = 2,
                FullName = "Misti Khantal",
                Address = "Pahala Pura",
                City = "Jhansi",
                State = "Uttar Pradesh",
                ZipCode = "284301",
                PhoneNumber = "123506"
            };

            // Act
            _addressBookRepo.AddContact(newContact);
            var result = _addressBookRepo.GetAllContacts();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        /// <summary>
        /// Tests updating an existing contact.
        /// Ensures that the update operation modifies the correct details.
        /// </summary>
        [Test]
        public void UpdateContact_ExistingId_ShouldUpdateContact()
        {
            // Arrange
            var updatedContact = new Contact
            {
                FullName = "Rekha Khantal",
                Address = "Chirgaon",
                City = "Jhansi",
                State = "Uttar Pradesh",
                ZipCode = "284301",
                PhoneNumber = "284318467270"
            };

            // Act
            var result = _addressBookRepo.UpdateContact(1, updatedContact);
            var updated = _addressBookRepo.GetContactById(1);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("284318467270", updated.PhoneNumber);
        }

        /// <summary>
        /// Tests updating a non-existing contact.
        /// Ensures that the method returns false when trying to update a contact that doesn't exist.
        /// </summary>
        [Test]
        public void UpdateContact_NonExistingId_ShouldReturnFalse()
        {
            // Arrange
            var updatedContact = new Contact
            {
                FullName = "Unknown Person",
                Address = "No Address",
                City = "Nowhere",
                State = "XX",
                ZipCode = "00000",
                PhoneNumber = "0000000000"
            };

            // Act
            var result = _addressBookRepo.UpdateContact(99, updatedContact);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Tests deleting an existing contact.
        /// Ensures that the contact is removed successfully.
        /// </summary>
        [Test]
        public void DeleteContact_ExistingId_ShouldRemoveContact()
        {
            // Act
            var result = _addressBookRepo.DeleteContact(1);
            var deletedContact = _addressBookRepo.GetContactById(1);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(deletedContact);
        }

        /// <summary>
        /// Tests deleting a non-existing contact.
        /// Ensures that the method returns false when trying to delete a contact that doesn't exist.
        /// </summary>
        [Test]
        public void DeleteContact_NonExistingId_ShouldReturnFalse()
        {
            // Act
            var result = _addressBookRepo.DeleteContact(99);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Cleans up the test environment after each test.
        /// Disposes the in-memory database instance to ensure fresh data for the next test.
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
            _dbContext.Dispose();
        }
    }
}
