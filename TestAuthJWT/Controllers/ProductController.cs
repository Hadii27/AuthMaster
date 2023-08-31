using AuthMaster.Dtos;
using AuthMaster.Model;
using AuthMaster.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using TestAuthJWT.Data;

namespace AuthMaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IproductService _productService;
        private readonly ICategoryService _categoryService;

        private new List<string> _allowedExtention = new List<string> { ".jpg" , ".png" };
        private long _maxSize = 1048576;
        public ProductController(IproductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        [HttpPost("AddProduct")]
        public async Task<IActionResult> Create([FromForm]ProductDto dto)
        {
            var isValid = await _categoryService.IsValidCategory(dto.categoryId);

            if (!_allowedExtention.Contains(Path.GetExtension(dto.image.FileName).ToLower()))
                return BadRequest("Only allowed jpg + png");

            if (dto.image.Length > _maxSize)
                return BadRequest("Only 1mb");

            if (dto.image is null)
                return BadRequest("image is Required ");

            if (!isValid)
            {
                return BadRequest("Invalid category id");
            }

            using var dataStream = new MemoryStream();
            await dto.image.CopyToAsync(dataStream);
            var product = new ProductModel
            {
                Price = dto.Price,
                ProductDescription = dto.ProductDescription,
                ProductName = dto.ProductName,
                image = dataStream.ToArray(),
                categoryId = dto.categoryId,
                Quantity = dto.Quantity,
            };
       
            var exist = await _productService.GetByName(dto.ProductName);
            if (exist is not null)
            {
                return BadRequest("This Product is already exist!");
            }
            _productService.Create(product);
            return Ok(product);
        }


        [HttpGet("Getall")]
        public async Task<IActionResult> GetAll(int CategoryId = 0)
        {
            var product = await _productService.GetAll(CategoryId);
            return Ok(product);
        }

        [HttpGet("Get-by-id")]
        public async Task<IActionResult> GetByid(int id)
        {
            var product = await _productService.GetById(id);
            if (product is null)
            {
                return NotFound("Invalid product ID!");
            }
            return Ok(product);       
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductDto dto)
        {
            var product = await _productService.GetById(id);
            if (product is null)
                return BadRequest("Invalid priduct id");

            var isValid = await _categoryService.IsValidCategory(dto.categoryId);
            if (!isValid)
            {
                return BadRequest("Invalid category id");
            }

            if (dto.image is not null)
            {
                if (!_allowedExtention.Contains(Path.GetExtension(dto.image.FileName)))                
                    return BadRequest("only allowed jpg and png");
                
                if (dto.image.Length>_maxSize)                
                    return BadRequest("Max size is 1mb");                   

                await using var dataStream = new MemoryStream();
                await dto.image.CopyToAsync(dataStream);
                product.image = dataStream.ToArray();
            }
      
            product.ProductDescription = dto.ProductDescription;
            product.ProductName = dto.ProductName;
            product.Price = dto.Price;
            product.categoryId = dto.categoryId;
            product.Quantity = dto.Quantity;

            _productService.Update(product);

            return Ok(product);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetById(id);

            if (product is null)
                return BadRequest("Invalid product id");

            _productService.Delete(product);
            return Ok(product);
        }
        [HttpGet("GetByCategoryId")]
        public async Task<IActionResult> GetByCategoryId(int CategoryId)
        {
            var product = await _productService.GetAll(CategoryId);
            return Ok(product);
        }     
    }
}
