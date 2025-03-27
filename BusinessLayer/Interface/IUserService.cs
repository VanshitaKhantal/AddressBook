using ModelLayer.DTO;
using ModelLayer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Interface for user authentication and registration services.
    /// Defines methods for user registration , login , ForgotPassword and ResetPassword functionality.
    /// </summary>
    public interface IUserService
    {
        ResponseModel<UserResponseModel> RegisterUser(UserRequestModel userRequest);
        ResponseModel<UserResponseModel> LoginUser(LoginRequestModel loginRequest);
        ResponseModel<string> ForgotPassword(string email);
        ResponseModel<bool> ResetPassword(string token, string newPassword);

    }
}
