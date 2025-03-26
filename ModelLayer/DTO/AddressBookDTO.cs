using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO
{
    /// <summary>
    /// Data Transfer Object (DTO) for Address Book entries.
    /// </summary>
    public class AddressBookDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string PhoneNumber { get; set; }
    }
}
