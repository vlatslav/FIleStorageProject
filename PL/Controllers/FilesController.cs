using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Threading.Tasks;
using AutoMapper;
using BAL;
using BAL.Entity;
using BAL.Entity.Auth;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;

namespace PL.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        public static IWebHostEnvironment _hostingEnvironment;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
        private readonly FileDBContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public FileController(IFileService fileService, IWebHostEnvironment env, FileDBContext contxt, UserManager<User> user, IMapper mapper, IUserService userService
            )
        {
            _fileService = fileService;
            _hostingEnvironment = env;
            _context = contxt;
            _userManager = user;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<FileModel>> GetFileById(int id)
        {
            try
            {
                return await _fileService.GetByIdAsync(id);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpPut("{id}"), Authorize(Roles = "User, Administrator")]
        public async Task<ActionResult> Update(int Id, [FromBody] FileModel value)
        {
            value.FileId = Id;
            try
            {
                await _fileService.UpdateAsync(value);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}"), Authorize(Roles = "User, Administrator")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _fileService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
            
        }

        [HttpPost, Authorize(Roles = "User, Administrator")]
        [Route("uploadfile/{categoryId}")]
        public async Task<ActionResult<string>> UploadFile([FromRoute] int categoryId)
        {
            
            try
            {
                string name = HttpContext.User.Identity.Name;
                var user = _userManager.Users.Where(x => x.UserName == name).FirstOrDefault();
                var files = HttpContext.Request.Form.Files;
                await _fileService.UploadFile(categoryId, user,files);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPatch("{id}"),  Authorize(Roles = "User, Administrator")]
        public async Task<ActionResult> PatchFile([FromRoute]int id, [FromBody] JsonPatchDocument<Files> filemodel)
        {
            try
            {
                await _fileService.FilePatch(id, filemodel);
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
        [HttpGet]
        [Route("files/paging")]
        public async Task<ActionResult<IEnumerable<FileModel>>> GetAllFiles([FromQuery] FileParameters fileParameters)
        {


            var result = await _fileService.GetAllFiles(fileParameters);
            if (result is null)
                return NotFound();
            var metadata = new
            {
                result.TotalCount,
                result.PageSize,
                result.CurrentPage,
                result.HasNext,
                result.HasPrevious
            };
            
            Response.Headers.Add("Pagination", JsonConvert.SerializeObject(metadata));
            return result.ToArray();
        }
        [HttpGet]
        [Route("files")]
        public async Task<ActionResult<IEnumerable<FileModel>>> GetFiles()
        {


            var result = await _fileService.GetAllAsync();
            if (result is null)
                return NotFound();
            return result.ToArray();
        }
        [HttpPost]
        [Route("downloadfile/{id}")]
        public async Task<IActionResult> Download(int id) //
        {
            try
            {
                var file = await _fileService.GetByIdAsync(id);
                var provider = new FileExtensionContentTypeProvider();
                string contentType;
                if (!provider.TryGetContentType(file.FilePath, out contentType))
                    contentType = "application/octet-stream";
                _fileService.DownloadFile(file);
                byte[] fileBytes = System.IO.File.ReadAllBytes(file.FilePath);
                return File(fileBytes, contentType, file.FileName);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
