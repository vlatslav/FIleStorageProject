﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;

namespace PL.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> GetAllCategories()
        {
            
            try
            {
                return (await _categoryService.GetAllAsync()).ToArray();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryModel>> GetCategoryById(int id)
        {
            try
            {
                return await _categoryService.GetByIdAsync(id);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpPost, Authorize(Roles = "Administrator")]

        public async Task<ActionResult> Add([FromBody] CategoryModel value)
        {
            try
            {
                await _categoryService.AddAsync(value);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return CreatedAtAction(nameof(Add), new { id = value.CategoryId }, value);
        }

        [HttpPut, Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Update([FromBody] CategoryModel value)
        {
            try
            {
                await _categoryService.UpdateAsync(value);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Update), new { id = value.CategoryId }, value);
        }
        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return CreatedAtAction(nameof(Delete), new { Id = id });
        }


    }
}
