using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO
{
    /// <summary>
    /// Represents the request model for initiating a password reset.
    /// </summary>
    public class ForgotPasswordRequestModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
