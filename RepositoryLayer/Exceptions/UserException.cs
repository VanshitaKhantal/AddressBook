using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Exceptions
{
    /// <summary>
    /// Custom exception class for handling user-related errors.
    /// Inherits from the base <see cref="Exception"/> class.
    /// </summary>
    public class UserException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserException"/> class.
        /// </summary>
        /// <param name="message">The error message describing the user-related exception.</param>
        public UserException(string message) : base(message)
        {
        }
    }
}