using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;

namespace AddressBook.Controllers
{
    /// <summary>
    /// Controller for user authentication and registration.
    /// Handles user registration and login functionality.
    /// Uses dependency injection for user services and JWT token generation.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userBL;
        private readonly IGenerateToken _tokenGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// Injects user service and token generator for handling authentication.
        /// </summary>
        /// <param name="userBL">Service handling user-related operations.</param>
        /// <param name="tokenGenerator">Service for generating JWT tokens.</param>
        public UserController(IUserService userBL, IGenerateToken tokenGenerator)
        {
            _userBL = userBL;
            _tokenGenerator = tokenGenerator;
        }

        /// <summary>
        /// Registers a new user in the system.
        /// Validates the user request model before processing.
        /// Calls the user service to store user details.
        /// </summary>
        /// <param name="userRequest">User details required for registration.</param>
        /// <returns>Success response with user details if registration is successful, otherwise a bad request response.</returns>
        [HttpPost("register")]
        public IActionResult RegisterUser(UserRequestModel userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("UserRequestModel is not valid");
            }

            var response = _userBL.RegisterUser(userRequest);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// Validates the login request model before processing.
        /// Calls the user service to verify user credentials.
        /// Generates and returns a JWT token upon successful authentication.
        /// </summary>
        /// <param name="loginRequest">User credentials for authentication.</param>
        /// <returns>A JWT token with user details if login is successful, otherwise an unauthorized response.</returns>
        [HttpPost("login")]
        public IActionResult LoginUser(LoginRequestModel loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("LoginRequestModel is not valid");
            }

            var response = _userBL.LoginUser(loginRequest);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            var token = _tokenGenerator.GenerateJwtToken(response.Data);
            return Ok(new { Token = token, User = response.Data });
        }
    }
}
