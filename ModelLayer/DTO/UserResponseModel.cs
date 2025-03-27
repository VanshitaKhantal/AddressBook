using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO
{
    /// <summary>
    /// Data Transfer Object (DTO) for user response data.
    /// Represents the essential user details returned after registration or login.
    /// </summary>
    public class UserResponseModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }
    }
}