using ModelLayer.Entity;
using RepositoryLayer.Context;
using RepositoryLayer.Helper;
using RepositoryLayer.Interface;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryLayer.Service
{
    /// <summary>
    /// Repository Layer for managing Address Book contacts with Redis caching.
    /// Handles database operations for contacts and caches results to improve performance.
    /// </summary>
    public class AddressBookRL : IAddressBookRL
    {        /// Initializes a new instance of the <see cref="AddressBookRL"/> class.

        private readonly AddressBookContext _dbContext;
        private readonly RedisCacheService _redisCache;

        /// <summary>
        /// </summary>
        /// <param name="dbContext">Database context for address book.</param>
        /// <param name="redisCache">Redis cache service for caching contact data.</param>
        public AddressBookRL(AddressBookContext dbContext, RedisCacheService redisCache)
        {
            _dbContext = dbContext;
            _redisCache = redisCache;
        }

        /// <summary>
        /// Retrieves all contacts from the database with Redis caching.
        /// </summary>
        /// <returns>List of all contacts.</returns>
        public List<Contact> GetAllContacts()
        {
            string cacheKey = "AddressBook_AllContacts";
            var cachedContacts = _redisCache.GetCache<List<Contact>>(cacheKey);

            if (cachedContacts != null)
            {
                return cachedContacts;
            }

            var contacts = _dbContext.Contacts.ToList();

            // Store data in Redis for 10 minutes
            _redisCache.SetCache(cacheKey, contacts, 10);

            return contacts;
        }

        /// <summary>
        /// Retrieves a contact by its ID with Redis caching.
        /// </summary>
        /// <param name="id">The ID of the contact.</param>
        /// <returns>The contact if found; otherwise, null.</returns>
        public Contact GetContactById(int id)
        {
            string cacheKey = $"AddressBook_Contact_{id}";
            var cachedContact = _redisCache.GetCache<Contact>(cacheKey);

            if (cachedContact != null)
            {
                return cachedContact;
            }

            var contact = _dbContext.Contacts.FirstOrDefault(c => c.Id == id);

            if (contact != null)
            {
                _redisCache.SetCache(cacheKey, contact, 10);
            }

            return contact;
        }

        /// <summary>
        /// Adds a new contact to the database and clears the cache.
        /// </summary>
        /// <param name="contact">The contact details to add.</param>
        /// <returns>The newly added contact.</returns>
        public Contact AddContact(Contact contact)
        {
            _dbContext.Contacts.Add(contact);
            _dbContext.SaveChanges();

            // Clear cache after adding a new contact
            _redisCache.RemoveCache("AddressBook_AllContacts");

            return contact;
        }

        /// <summary>
        /// Updates an existing contact in the database and clears related cache.
        /// </summary>
        /// <param name="id">The ID of the contact to update.</param>
        /// <param name="contact">The updated contact details.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public bool UpdateContact(int id, Contact contact)
        {
            var existingContact = _dbContext.Contacts.FirstOrDefault(c => c.Id == id);
            if (existingContact == null)
                return false;

            existingContact.FullName = contact.FullName;
            existingContact.Address = contact.Address;
            existingContact.City = contact.City;
            existingContact.State = contact.State;
            existingContact.ZipCode = contact.ZipCode;
            existingContact.PhoneNumber = contact.PhoneNumber;

            _dbContext.SaveChanges();

            // Clear cache after update
            _redisCache.RemoveCache("AddressBook_AllContacts");
            _redisCache.RemoveCache($"AddressBook_Contact_{id}");

            return true;
        }

        /// <summary>
        /// Deletes a contact from the database and clears related cache.
        /// </summary>
        /// <param name="id">The ID of the contact to delete.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        public bool DeleteContact(int id)
        {
            var contact = _dbContext.Contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null)
                return false;

            _dbContext.Contacts.Remove(contact);
            _dbContext.SaveChanges();

            // Clear cache after deletion
            _redisCache.RemoveCache("AddressBook_AllContacts");
            _redisCache.RemoveCache($"AddressBook_Contact_{id}");

            return true;
        }
    }
}
