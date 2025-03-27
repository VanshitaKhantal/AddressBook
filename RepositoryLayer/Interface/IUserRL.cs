using ModelLayer.DTO;
using ModelLayer.Entity;
using ModelLayer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    /// <summary>
    /// Interface defining user-related data access operations.
    /// Responsible for handling user registration and login functionality.
    /// </summary>
    public interface IUserRL
    {
        public ResponseModel<UserResponseModel> RegisterUser(User userRequest);
        public ResponseModel<UserResponseModel> LoginUser(LoginRequestModel userRequest);
        User GetUserByEmail(string email);
    }
}