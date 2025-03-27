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
using System.Security.Cryptography;

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

        public ResponseModel<string> GeneratePasswordResetToken(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return new ResponseModel<string>((int)HttpStatusCode.NotFound, false, "User not found", null);

            string token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            user.ResetToken = token;
            user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);

            _context.SaveChanges();
            return new ResponseModel<string>((int)HttpStatusCode.OK, true, "Reset token generated", token);
        }

        public ResponseModel<bool> ResetPassword(string token, string newPassword)
        {
            var user = _context.Users.FirstOrDefault(u => u.ResetToken == token && u.ResetTokenExpiry > DateTime.UtcNow);
            if (user == null)
                return new ResponseModel<bool>((int)HttpStatusCode.BadRequest, false, "Invalid or expired token", false);

            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            _context.SaveChanges();
            return new ResponseModel<bool>((int)HttpStatusCode.OK, true, "Password reset successful", true);
        }
    }
}