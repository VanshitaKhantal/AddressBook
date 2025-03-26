using AutoMapper;
using ModelLayer.DTO;
using ModelLayer.Entity;

namespace BusinessLayer.Mapping
{
    /// <summary>
    /// Mapping profile for AutoMapper to map entities to DTOs and vice versa.
    /// </summary>
    public class AddressBookMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressBookMappingProfile"/> class.
        /// Defines the mappings between <see cref="Contact"/> entity and <see cref="AddressBookDTO"/>.
        /// </summary>
        public AddressBookMappingProfile()
        {
            CreateMap<Contact, AddressBookDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FullName}"));
            // DTO to Entity Mapping
            CreateMap<AddressBookDTO, Contact>();
        }
    }
}
