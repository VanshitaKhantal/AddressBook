using FluentValidation;
using ModelLayer.DTO;

namespace BusinessLayer.Validators
{
    /// <summary>
    /// Validator for <see cref="AddressBookDTO"/> using FluentValidation.
    /// Ensures that required fields are provided and follow validation rules.
    /// </summary>
    public class AddressBookEntryValidator : AbstractValidator<AddressBookDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressBookEntryValidator"/> class.
        /// Defines validation rules for the <see cref="AddressBookDTO"/> properties.
        /// </summary>
        public AddressBookEntryValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full Name is required")
                .MaximumLength(100).WithMessage("Full Name cannot exceed 100 characters");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number is required")
                .Matches(@"^\d{10}$").WithMessage("Phone Number must be exactly 10 digits");
        }
    }
}
