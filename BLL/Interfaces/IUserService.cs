using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BAL.Entity.Auth;
using BusinessLogicLayer.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BusinessLogicLayer.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<UserModel>> GetAllUsers();

        public Task<UserModel> GetUserById(string id);

        public Task<IEnumerable<FileModel>> GetAllFilesFromUser(string id);
        public Task ChangePassword(string username, string pass);

        public Task UpdateUser(UserModel user);
        public Task DeleteUserById(string id);
        public Task Registration(UserModel user);
        public Task<bool> AddRoleToUser(string userEmail, string role);

        public Task PatchValues(string userid, JsonPatchDocument<User> model);
    }
}
