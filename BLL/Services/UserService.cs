using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BAL.Entity;
using BAL.Entity.Auth;
using BAL.Interfaces;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _userManager = userManager;
            _uow = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddRoleToUser(string userEmail, string role)
        {
            if(userEmail == "")
                throw new AccessException("Email can't be null");

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            var result = await _userManager.AddToRoleAsync(user, role);

            if (result.Succeeded)
                return true;
            return false;

        }

        public async Task<IEnumerable<UserModel>> GetAllUsers()
        {
            var mapped = _mapper.Map<IEnumerable<UserModel>>(await _userManager.Users.ToArrayAsync()).ToList();
            var users = await _userManager.Users.ToListAsync();
            for (int i = 0; i < mapped.Count(); i++)
            {
                mapped[i].Roles = await _userManager.GetRolesAsync(users[i]);
            }
            return mapped;
        }

        public async Task DeleteUserById(string id)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == id);
            var files = await GetAllFilesFromUser(id);
            if (user is null)
            {
                throw new AccessException();
            }

            if (files.ToList().Count == 0)
            {
                await _userManager.DeleteAsync(user);
            }
            else
            {
                foreach (var file in files)
                {
                    await _uow.FileRepository.DeleteByIdAsync(((int) file.FileId));
                }
                await _userManager.DeleteAsync(user);
            }
        }
        public async Task<UserModel> GetUserById(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if(user is null)
                throw new ArgumentException(nameof(id));
            var mapped = _mapper.Map<UserModel>(user);
            mapped.Roles = await _userManager.GetRolesAsync(user);
            return mapped;
        }

        public async Task UpdateUser(UserModel user)
        {
            if(user.UserId == "")
                throw new ArgumentException(nameof(user));
            await _userManager.UpdateAsync(_mapper.Map<User>(user));
        }

        public async Task Registration(UserModel user)
        {
            var ur = _mapper.Map<User>(user);
            ur.Id = Guid.NewGuid().ToString();
            var result = await _userManager.CreateAsync(ur, user.Password);
            if (!result.Succeeded)
            {
                throw new AccessException();
            }
            await _userManager.AddToRolesAsync(ur, user.Roles);
        }
        public async Task<IEnumerable<FileModel>> GetAllFilesFromUser(string id)
        {
            var allfiles = await _uow.FileRepository.GetAllWithDetails();
            var mapped = _mapper.Map<IEnumerable<FileModel>>(allfiles.Where(x => x.UserId == id));
            return mapped;
        }

        public async Task PatchValues(string userId, JsonPatchDocument<User> model)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            var baseUser = _mapper.Map<User>(user);
            model.ApplyTo(baseUser);
            await _userManager.UpdateAsync(baseUser);
        }

        public async Task ChangePassword(string username, string pass)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == username);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user,token,pass);
            if (!result.Succeeded)
                throw new ArgumentException("Password is incorrect");
        }
    }
}
