using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entity;
using BusinessLayer.Interface;
using ModelLayer.DTO;
using ModelLayer.Utility;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing contacts in the address book.
    /// Provides endpoints for creating, retrieving, updating, and deleting contacts.
    /// Requires authentication via JWT token.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/addressbook")]
    public class AddressBookController : ControllerBase
    {
        private readonly IAddressBookService _addressBookService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddressBookDTO> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressBookController"/> class.
        /// </summary>
        /// <param name="addressBookService">Service handling address book operations.</param>
        /// <param name="mapper">AutoMapper instance for mapping entities.</param>
        /// <param name="validator">FluentValidation validator for AddressBookDTO.</param>
        public AddressBookController(IAddressBookService addressBookService, IMapper mapper, IValidator<AddressBookDTO> validator)
        {
            _addressBookService = addressBookService;
            _mapper = mapper;
            _validator = validator;
        }

        /// <summary>
        /// Retrieves all contacts from the address book.
        /// </summary>
        /// <returns>List of contacts with a success response.</returns>
        [HttpGet]
        public IActionResult GetContacts()
        {
            var contacts = _addressBookService.GetAllContacts();
            var contactDTOs = _mapper.Map<List<AddressBookDTO>>(contacts);

            return Ok(new ResponseModel<List<AddressBookDTO>>(200, true, "Contacts retrieved successfully", contactDTOs));
        }

        /// <summary>
        /// Retrieves a specific contact by ID.
        /// </summary>
        /// <param name="id">The ID of the contact.</param>
        /// <returns>The requested contact if found, otherwise a 404 response.</returns>
        [HttpGet("{id}")]
        public IActionResult GetContactById(int id)
        {
            var contact = _addressBookService.GetContactById(id);
            if (contact == null)
            {
                return NotFound(new ResponseModel<string>(404, false, "Contact not found", null));
            }

            var contactDTO = _mapper.Map<AddressBookDTO>(contact);
            return Ok(new ResponseModel<AddressBookDTO>(200, true, "Contact retrieved successfully", contactDTO));
        }

        /// <summary>
        /// Adds a new contact to the address book.
        /// </summary>
        /// <param name="contactDTO">The contact details to be added.</param>
        /// <returns>Created contact details with a 201 response.</returns>
        [HttpPost]
        public IActionResult AddContact([FromBody] AddressBookDTO contactDTO)
        {
            var validationResult = _validator.Validate(contactDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseModel<object>(400, false, "Validation Failed", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            var contactEntity = _mapper.Map<Contact>(contactDTO);
            var addedContact = _addressBookService.AddContact(contactEntity);
            if (addedContact == null)
            {
                return BadRequest(new ResponseModel<string>(400, false, "Failed to add contact", null));
            }

            return CreatedAtAction(nameof(GetContactById), new { id = addedContact.Id },
                new ResponseModel<Contact>(201, true, "Contact added successfully", addedContact));
        }

        /// <summary>
        /// Updates an existing contact in the address book.
        /// </summary>
        /// <param name="id">The ID of the contact to update.</param>
        /// <param name="contactDTO">Updated contact details.</param>
        /// <returns>Updated contact details or a 404 response if the contact is not found.</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateContact(int id, [FromBody] AddressBookDTO contactDTO)
        {
            ValidationResult validationResult = _validator.Validate(contactDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseModel<List<string>>(400, false, "Validation failed", validationResult.Errors.ConvertAll(error => error.ErrorMessage)));
            }

            var contact = _mapper.Map<Contact>(contactDTO);
            var result = _addressBookService.UpdateContact(id, contact);
            if (!result)
            {
                return NotFound(new ResponseModel<string>(404, false, "Contact not found", null));
            }

            var updatedContact = _addressBookService.GetContactById(id);
            var updatedContactDTO = _mapper.Map<AddressBookDTO>(updatedContact);

            return Ok(new ResponseModel<AddressBookDTO>(200, true, "Contact updated successfully", updatedContactDTO));
        }

        /// <summary>
        /// Deletes a contact from the address book.
        /// </summary>
        /// <param name="id">The ID of the contact to delete.</param>
        /// <returns>Success response if deleted, or a 404 response if the contact is not found.</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteContact(int id)
        {
            var result = _addressBookService.DeleteContact(id);
            if (!result)
            {
                return NotFound(new ResponseModel<string>(404, false, "Contact not found", null));
            }

            return Ok(new ResponseModel<string>(200, true, "Contact deleted successfully", null));
        }
    }
}
