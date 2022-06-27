using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BAL.Entity.Auth;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PL.Extentions;

namespace PL.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthenticationManager _authManager;
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;

        public AuthenticationController(UserManager<User> userManager,
            IMapper mapper, IAuthenticationManager authManager, SignInManager<User> signIn, IUserService service)
        {
            _userManager = userManager;
            _mapper = mapper;
            _authManager = authManager;
            _signInManager = signIn;
            _userService = service;
        }
        [HttpPost("registration")]
        public async Task<ActionResult> RegisterUser([FromBody] UserModel userForRegistration)
        {
            try
            {
                await _userService.Registration(userForRegistration);
                return StatusCode(201);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Authenticate([FromBody] UserLoginModel user)
        {
            if (!await _authManager.ValidateUser(user))
            {
                return Unauthorized();
            }
            return Ok(new { Token = await _authManager.CreateToken() });
        }

        //[HttpGet, Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAllUsers()
        {

            try
            {
                var user = await _userService.GetAllUsers();
                return (user).ToArray();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteUserById(string id)
        {
            try
            {
                await _userService.DeleteUserById(id);
                return new JsonResult("Deleted Successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("{id}"), Authorize(Roles = "Administrator")]
        //[HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUserById(string id)
        {
            try
            {
                var user= await _userService.GetUserById(id);
                return _mapper.Map<UserModel>(user);

            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpGet("files/{id}"), Authorize(Roles = "Administrator")]
        //[HttpGet("files/{id}")]
        public async Task<ActionResult<IEnumerable<FileModel>>> GetAllFiles(string id)
        {
            try
            {
                var files = await _userService.GetAllFilesFromUser(id);
                return files.ToArray();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost("signout"), Authorize]
        //[HttpPost("signout")]
        public async Task<ActionResult> SignOut()
        {
            try
            {
                await _signInManager.SignOutAsync();
                // var chechAuth = _httpContext.User.Identity.IsAuthenticated;
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost("addrole/{role}"), Authorize(Roles = "Administrator")]
        //[HttpPost("addrole/{role}")]
        public async Task<ActionResult> AddRoleToUser([FromBody]string userEmail, string role)
        {

            if (!ModelState.IsValid)
                return BadRequest();
            var result = await _userService.AddRoleToUser(userEmail,role);
            if(result)
                return Ok();
            return BadRequest();
        }

        [HttpPut, Authorize(Roles = "User, Administrator")]
        //[HttpPut]
        public async Task<ActionResult> Update([FromBody] UserModel user)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            await _userManager.UpdateAsync(_mapper.Map<User>(user));
            return Ok();
        }
        [HttpPatch, Authorize(Roles = "User, Administrator")]
        //[HttpPatch("{id}")]
        public async Task<ActionResult> PatchFile([FromRoute] string id, [FromBody] JsonPatchDocument<User> filemodel)
        {
            try
            {
                await _userService.PatchValues(id, filemodel);
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
