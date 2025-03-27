using BusinessLayer.Interface;
using ModelLayer.DTO;
using ModelLayer.Entity;
using ModelLayer.Utility;
using RepositoryLayer.Interface;
using System;
using BCrypt.Net;

namespace BusinessLayer.Service
{
    /// <summary>
    /// Service layer for user authentication and registration.
    /// Handles password hashing and verification.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRL _userRL;

        /// <summary>
        /// Initializes UserService with the user repository layer.
        /// </summary>
        /// <param name="userRL">User repository instance.</param>
        public UserService(IUserRL userRL)
        {
            _userRL = userRL;
        }

        /// <summary>
        /// Registers a new user with a securely hashed password.
        /// </summary>
        /// <param name="userRequest">User request model containing raw password.</param>
        /// <returns>Response model with user details.</returns>
        public ResponseModel<UserResponseModel> RegisterUser(UserRequestModel userRequest)
        {
            // Hash the password before saving it
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);

            User user = new User()
            {
                Name = userRequest.Name,
                Email = userRequest.Email,
                Password = hashedPassword,  // Store the hashed password
                Role = userRequest.Role
            };

            return _userRL.RegisterUser(user);
        }

        /// <summary>
        /// Authenticates a user by verifying the hashed password.
        /// </summary>
        /// <param name="loginRequest">Login request model.</param>
        /// <returns>Response model with authentication result.</returns>
        public ResponseModel<UserResponseModel> LoginUser(LoginRequestModel loginRequest)
        {
            // Fetch user from the repository (returns an entity)
            var userEntity = _userRL.GetUserByEmail(loginRequest.Email);

            // Check if user exists
            if (userEntity == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, userEntity.Password))
            {
                return new ResponseModel<UserResponseModel>(401, false, "Invalid email or password", null);
            }

            // Convert User entity to UserResponseModel DTO
            var userResponse = new UserResponseModel()
            {
                Email = userEntity.Email,
                Name = userEntity.Name,
                Role = userEntity.Role
            };

            return new ResponseModel<UserResponseModel>(200, true, "Login successful", userResponse);
        }

    }
}
