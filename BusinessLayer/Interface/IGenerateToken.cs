using ModelLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Interface for generating JWT tokens.
    /// Provides a method to create a signed JWT token for authenticated users.
    /// </summary>
    public interface IGenerateToken
    {
        string GenerateJwtToken(UserResponseModel user);
    }
}