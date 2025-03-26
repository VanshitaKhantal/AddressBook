using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entity;
using BusinessLayer.Interface;
using System.Collections.Generic;
using ModelLayer.DTO;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing contacts in the address book.
    /// </summary>
    [ApiController]
    [Route("api/addressbook")]
    public class AddressBookController : ControllerBase
    {
        private readonly IAddressBookService _addressBookService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressBookController"/> class.
        /// </summary>
        /// <param name="addressBookService">Service for address book operations.</param>
        public AddressBookController(IAddressBookService addressBookService)
        {
            _addressBookService = addressBookService;
        }

        /// <summary>
        /// Retrieves all contacts from the address book.
        /// </summary>
        /// <returns>List of contacts.</returns>
        [HttpGet]
        public IActionResult GetContacts()
        {
            List<Contact> contacts = _addressBookService.GetAllContacts();
            return Ok(new ResponseModel<List<Contact>>
            {
                Success = true,
                Message = "Contacts retrieved successfully",
                Data = contacts
            });
        }

        /// <summary>
        /// Retrieves a contact by its ID.
        /// </summary>
        /// <param name="id">The ID of the contact.</param>
        /// <returns>Contact details if found, otherwise NotFound response.</returns>
        [HttpGet("{id}")]
        public IActionResult GetContactById(int id)
        {
            var contact = _addressBookService.GetContactById(id);
            if (contact == null)
            {
                return NotFound(new ResponseModel<string> { Success = false, Message = "Contact not found", Data = null });
            }
            return Ok(new ResponseModel<Contact>
            {
                Success = true,
                Message = "Contact retrieved successfully",
                Data = contact
            });
        }

        /// <summary>
        /// Adds a new contact to the address book.
        /// </summary>
        /// <param name="contact">Contact details to be added.</param>
        /// <returns>The newly added contact details.</returns>
        [HttpPost]
        public IActionResult AddContact([FromBody] Contact contact)
        {
            if (contact == null)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid contact data",
                    Data = null
                });
            }

            var addedContact = _addressBookService.AddContact(contact);
            if (addedContact == null)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Failed to add contact",
                    Data = null
                });
            }

            return Ok(new ResponseModel<Contact>
            {
                Success = true,
                Message = "Contact added successfully",
                Data = addedContact
            });
        }

        /// <summary>
        /// Updates an existing contact in the address book.
        /// </summary>
        /// <param name="id">The ID of the contact to update.</param>
        /// <param name="contact">Updated contact details.</param>
        /// <returns>The updated contact details if successful.</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateContact(int id, [FromBody] Contact contact)
        {
            var result = _addressBookService.UpdateContact(id, contact);
            if (!result)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Contact not found",
                    Data = null
                });
            }

            var updatedContact = _addressBookService.GetContactById(id);
            return Ok(new ResponseModel<Contact>
            {
                Success = true,
                Message = "Contact updated successfully",
                Data = updatedContact
            });
        }

        /// <summary>
        /// Deletes a contact from the address book.
        /// </summary>
        /// <param name="id">The ID of the contact to delete.</param>
        /// <returns>A success message if deletion is successful.</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteContact(int id)
        {
            var result = _addressBookService.DeleteContact(id);
            if (!result)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Contact not found",
                    Data = null
                });
            }
            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Contact deleted successfully",
                Data = null
            });
        }
    }
}
