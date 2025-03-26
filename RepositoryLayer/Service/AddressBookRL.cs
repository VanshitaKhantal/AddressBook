using ModelLayer.Entity;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryLayer.Service
{
    /// <summary>
    /// Repository Layer for managing Address Book contacts.
    /// Handles database operations for contacts.
    /// </summary>
    public class AddressBookRL : IAddressBookRL
    {
        private readonly AddressBookContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressBookRL"/> class.
        /// </summary>
        /// <param name="dbContext">Database context for address book.</param>
        public AddressBookRL(AddressBookContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all contacts from the database.
        /// </summary>
        /// <returns>List of all contacts.</returns>
        public List<Contact> GetAllContacts()
        {
            return _dbContext.Contacts.ToList();
        }

        /// <summary>
        /// Retrieves a contact by its ID.
        /// </summary>
        /// <param name="id">The ID of the contact.</param>
        /// <returns>The contact if found; otherwise, null.</returns>
        public Contact GetContactById(int id)
        {
            return _dbContext.Contacts.FirstOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// Adds a new contact to the database.
        /// </summary>
        /// <param name="contact">The contact details to add.</param>
        /// <returns>The newly added contact.</returns>
        public Contact AddContact(Contact contact)
        {
            _dbContext.Contacts.Add(contact);
            _dbContext.SaveChanges();
            return contact;
        }

        /// <summary>
        /// Updates an existing contact in the database.
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
            return true;
        }

        /// <summary>
        /// Deletes a contact from the database.
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
            return true;
        }
    }
}
