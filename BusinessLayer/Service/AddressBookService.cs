using BusinessLayer.Interface;
using ModelLayer.Entity;
using RepositoryLayer.Interface;
using System.Collections.Generic;

namespace BusinessLayer.Service
{
    /// <summary>
    /// Service layer for managing address book contacts.
    /// Handles business logic and interacts with the repository layer.
    /// </summary>
    public class AddressBookService : IAddressBookService
    {
        /// <summary>
        /// Repository instance for accessing contact data.
        /// </summary>
        private readonly IAddressBookRL _addressBookRepository;

        /// <summary>
        /// Constructor to initialize the AddressBookService with a repository instance.
        /// </summary>
        /// <param name="addressBookRepository">Repository instance for data access.</param>
        public AddressBookService(IAddressBookRL addressBookRepository)
        {
            _addressBookRepository = addressBookRepository;
        }

        /// <summary>
        /// Retrieves all contacts from the address book.
        /// </summary>
        /// <returns>List of all contacts.</returns>
        public List<Contact> GetAllContacts()
        {
            return _addressBookRepository.GetAllContacts();
        }

        /// <summary>
        /// Retrieves a specific contact by its ID.
        /// </summary>
        /// <param name="id">The ID of the contact.</param>
        /// <returns>The contact if found; otherwise, null.</returns>
        public Contact GetContactById(int id)
        {
            return _addressBookRepository.GetContactById(id);
        }

        /// <summary>
        /// Adds a new contact to the address book.
        /// </summary>
        /// <param name="contact">The contact details to add.</param>
        /// <returns>The newly added contact.</returns>
        public Contact AddContact(Contact contact)
        {
            return _addressBookRepository.AddContact(contact);
        }

        /// <summary>
        /// Updates an existing contact in the address book.
        /// </summary>
        /// <param name="id">The ID of the contact to update.</param>
        /// <param name="contact">The updated contact details.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public bool UpdateContact(int id, Contact contact)
        {
            return _addressBookRepository.UpdateContact(id, contact);
        }

        /// <summary>
        /// Deletes a contact from the address book.
        /// </summary>
        /// <param name="id">The ID of the contact to delete.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        public bool DeleteContact(int id)
        {
            return _addressBookRepository.DeleteContact(id);
        }
    }
}
