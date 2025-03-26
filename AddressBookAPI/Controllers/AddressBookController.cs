using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entity;
using BusinessLayer.Interface;
using ModelLayer.DTO;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;

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
        private readonly IMapper _mapper;
        private readonly IValidator<AddressBookDTO> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressBookController"/> class.
        /// </summary>
        public AddressBookController(IAddressBookService addressBookService, IMapper mapper, IValidator<AddressBookDTO> validator)
        {
            _addressBookService = addressBookService;
            _mapper = mapper;
            _validator = validator;
        }

        /// <summary>
        /// Retrieves all contacts from the address book.
        /// </summary>
        [HttpGet]
        public IActionResult GetContacts()
        {
            var contacts = _addressBookService.GetAllContacts();
            var contactDTOs = _mapper.Map<List<AddressBookDTO>>(contacts);

            return Ok(new ResponseModel<List<AddressBookDTO>>
            {
                Success = true,
                Message = "Contacts retrieved successfully",
                Data = contactDTOs
            });
        }

        /// <summary>
        /// Retrieves a contact by its ID.
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetContactById(int id)
        {
            var contact = _addressBookService.GetContactById(id);
            if (contact == null)
            {
                return NotFound(new ResponseModel<string> { Success = false, Message = "Contact not found", Data = null });
            }

            var contactDTO = _mapper.Map<AddressBookDTO>(contact);
            return Ok(new ResponseModel<AddressBookDTO>
            {
                Success = true,
                Message = "Contact retrieved successfully",
                Data = contactDTO
            });
        }

        /// <summary>
        /// Adds a new contact to the address book.
        /// </summary>
        [HttpPost]
        public IActionResult AddContact([FromBody] AddressBookDTO contactDTO)
        {
            // Validate DTO
            var validationResult = _validator.Validate(contactDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = "Validation Failed",
                    Data = validationResult.Errors.Select(e => e.ErrorMessage)
                });
            }

            // Convert DTO to Entity
            var contactEntity = _mapper.Map<Contact>(contactDTO);

            var addedContact = _addressBookService.AddContact(contactEntity);
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
        [HttpPut("{id}")]
        public IActionResult UpdateContact(int id, [FromBody] AddressBookDTO contactDTO)
        {
            ValidationResult validationResult = _validator.Validate(contactDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseModel<List<string>>
                {
                    Success = false,
                    Message = "Validation failed",
                    Data = validationResult.Errors.ConvertAll(error => error.ErrorMessage)
                });
            }

            var contact = _mapper.Map<Contact>(contactDTO);
            var result = _addressBookService.UpdateContact(id, contact);
            if (!result)
            {
                return NotFound(new ResponseModel<string> { Success = false, Message = "Contact not found", Data = null });
            }

            var updatedContact = _addressBookService.GetContactById(id);
            var updatedContactDTO = _mapper.Map<AddressBookDTO>(updatedContact);

            return Ok(new ResponseModel<AddressBookDTO>
            {
                Success = true,
                Message = "Contact updated successfully",
                Data = updatedContactDTO
            });
        }


        /// <summary>
        /// Deletes a contact from the address book.
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult DeleteContact(int id)
        {
            var result = _addressBookService.DeleteContact(id);
            if (!result)
            {
                return NotFound(new ResponseModel<string> { Success = false, Message = "Contact not found", Data = null });
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
