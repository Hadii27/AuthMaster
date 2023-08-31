using AuthMaster.Dtos;
using AuthMaster.Model;
using AuthMaster.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthMaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;

        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddCategory")]
        public async Task<IActionResult> Add(CategoryDto dto)
        {
         
            var cat = new CategoryModel
            {
                CategoryName = dto.name
            };

            var exist = await _categoryService.GetByName(cat.CategoryName);
            if (exist is not null)
            {
                return BadRequest("this already exist");
            }

            if (string.IsNullOrWhiteSpace(dto.name))
            {
                return BadRequest("Category name is required");
            }
            await _categoryService.Add(cat);
            return Ok(cat);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound("This Category dosenot exist");
            }
            _categoryService.Delete(category);
            return Ok(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] CategoryDto dto)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound("This Category dosenot exist");
            }

            if (string.IsNullOrWhiteSpace(dto.name))
            {
                return BadRequest("Category name is required");
            }

            category.CategoryName = dto.name; 
            _categoryService.Update(category);
            return Ok(category);
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAll();
            return Ok(categories);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var cat = await _categoryService.GetCategoryById(id);
            if (cat == null)
            {
                return NotFound("This id is not exist");
            }
            var category = await _categoryService.GetCategoryById(id);
            return Ok(category);
        }
    }
}
