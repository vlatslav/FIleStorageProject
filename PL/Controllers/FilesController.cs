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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;

namespace PL.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        public static IWebHostEnvironment _hostingEnvironment;
        private readonly IFileService _fileService;
        private readonly ICategoryService _categoryService;
        private readonly FileDBContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public FileController(IFileService fileService, IWebHostEnvironment env, FileDBContext contxt, UserManager<User> user, IMapper mapper, ICategoryService categoryService
            )
        {
            _fileService = fileService;
            _hostingEnvironment = env;
            _context = contxt;
            _userManager = user;
            _mapper = mapper;
            _categoryService = categoryService;
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
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(Update), new { id = value.FileId }, value);
        }

        [HttpDelete("{id}"), Authorize(Roles = "User, Administrator")]
        //[HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _fileService.DeleteAsync(id);
            return CreatedAtAction(nameof(Delete), new {Id = id});
        }

        [HttpPost, Authorize(Roles = "User, Administrator")]
        //[HttpPost]
        [Route("uploadfile/{categoryId}")]
        public async Task<ActionResult<string>> UploadFile([FromRoute] int categoryId)
        {
            
            try
            {
                string name = HttpContext.User.Identity.Name;
                var user = _userManager.Users.Where(x => x.UserName == name).FirstOrDefault();
                var files = HttpContext.Request.Form.Files;
                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        FileInfo fl = new FileInfo(file.FileName);
                        var newfilename = "File_" + DateTime.Now.TimeOfDay.Milliseconds + fl.Extension;
                        var path = Path.Combine("", _hostingEnvironment.ContentRootPath + "\\Files\\" + newfilename);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        var f = new FileModel()
                        {
                            Date = DateTime.Now,
                            FilePath = path,
                            ContentType = file.ContentType,
                            FileName = newfilename,
                            UserId = user.Id,
                            CategoryId = categoryId
                        };
                        await _fileService.AddAsync(f);
                    }
                    return Ok();
                }

                return NotFound();
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
        [Route("files")]
        public async Task<ActionResult<IEnumerable<FileModel>>> GetAllFiles()
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
            var file = await _fileService.GetByIdAsync(id);
            var provider = new FileExtensionContentTypeProvider();
            if (file is null)
                return NotFound();
            string contentType;
            if (!provider.TryGetContentType(file.FilePath, out contentType))
                contentType = "application/octet-stream";
            byte[] fileBytes;
            if (System.IO.File.Exists(file.FilePath))
                fileBytes = System.IO.File.ReadAllBytes(file.FilePath);
            else
                return NotFound();
            return File(fileBytes, contentType,file.FileName);
        }
    }
}
