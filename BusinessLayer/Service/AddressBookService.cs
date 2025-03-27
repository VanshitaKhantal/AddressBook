using AutoMapper;
using BusinessLayer.Interface;
using ModelLayer.DTO;
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
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor to initialize the AddressBookService with a repository instance.
        /// </summary>
        /// <param name="addressBookRepository">Repository instance for data access.</param>
        public AddressBookService(IAddressBookRL addressBookRepository, IMapper mapper)
        {
            _addressBookRepository = addressBookRepository;
            _mapper = mapper;
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
        public Contact AddContact(Contact contactDTO)
        {
            var contactEntity = _mapper.Map<Contact>(contactDTO);
            _addressBookRepository.AddContact(contactEntity);
            return contactEntity;
        }

        /// <summary>
        /// Updates an existing contact in the address book.
        /// </summary>
        /// <param name="id">The ID of the contact to update.</param>
        /// <param name="contact">The updated contact details.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public bool UpdateContact(int id, Contact contactDTO)
        {
            var existingContact = _addressBookRepository.GetContactById(id);
            if (existingContact == null) return false;

            _mapper.Map(contactDTO, existingContact);
            _addressBookRepository.UpdateContact(id, existingContact);
            return true;
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
