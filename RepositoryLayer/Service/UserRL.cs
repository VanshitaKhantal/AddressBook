using Microsoft.EntityFrameworkCore;
using ModelLayer.DTO;
using ModelLayer.Entity;
using ModelLayer.Utility;
using RepositoryLayer.Context;
using RepositoryLayer.Exceptions;
using RepositoryLayer.Interface;
using System;
using System.Linq;
using System.Net;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly AddressBookContext _context;

        public UserRL(AddressBookContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Registers a new user by saving hashed password in the database.
        /// </summary>
        public ResponseModel<UserResponseModel> RegisterUser(User userRequest)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == userRequest.Email);
                if (user != null)
                {
                    throw new UserException("User already registered. Try with a different email.");
                }

                _context.Users.Add(userRequest);
                _context.SaveChanges();

                var newUserResponse = new UserResponseModel()
                {
                    Email = userRequest.Email,
                    Name = userRequest.Name,
                    Role = userRequest.Role
                };

                return new ResponseModel<UserResponseModel>((int)HttpStatusCode.Created, true, "User registered successfully", newUserResponse);
            }
            catch (UserException ex)
            {
                return new ResponseModel<UserResponseModel>((int)HttpStatusCode.BadRequest, false, ex.Message, null);
            }
            catch (Exception ex)
            {
                return new ResponseModel<UserResponseModel>((int)HttpStatusCode.InternalServerError, false, "An error occurred while registering the user.", null);
            }
        }

        /// <summary>
        /// Retrieves a user by email for login verification.
        /// </summary>
        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }


        /// <summary>
        /// Validates user login by checking email and password hash.
        /// </summary>
        public ResponseModel<UserResponseModel> LoginUser(LoginRequestModel loginRequest)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == loginRequest.Email);

                if (user == null)
                {
                    return new ResponseModel<UserResponseModel>((int)HttpStatusCode.Unauthorized, false, "Invalid email or password", null);
                }

                var userResponse = new UserResponseModel()
                {
                    Email = user.Email,
                    Name = user.Name,
                    Role = user.Role
                };

                return new ResponseModel<UserResponseModel>((int)HttpStatusCode.OK, true, "Login successful", userResponse);
            }
            catch (Exception ex)
            {
                return new ResponseModel<UserResponseModel>((int)HttpStatusCode.InternalServerError, false, "An error occurred during login", null);
            }
        }
    }
}
