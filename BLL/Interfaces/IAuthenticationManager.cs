using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IAuthenticationManager
    {
        Task<bool> ValidateUser(UserLoginModel userForAuth);
        Task<string> CreateToken();
    }
}
