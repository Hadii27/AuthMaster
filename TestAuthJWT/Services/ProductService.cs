using AuthMaster.Dtos;
using AuthMaster.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAuthJWT.Data;
using TestAuthJWT.Model;

namespace AuthMaster.Services
{
    public class ProductService : IproductService
    {
        private readonly DataContext _dataContext;
        public ProductService (DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ProductModel> Create(ProductModel product)
        {
            await _dataContext.AddAsync(product);
            _dataContext.SaveChanges();
            return product;       
        }

        public ProductModel Delete(ProductModel product)
        {
            _dataContext.Remove(product);
            _dataContext.SaveChanges();
            return product;
        }
   
        public async Task<IEnumerable<ProductModel>> GetAll(int CategoryId = 0)
        {
            return await _dataContext.products
                .Where(c=>c.categoryId == CategoryId || CategoryId == 0)
                .Include(c => c.category)
                .ToListAsync();
        }


        public async Task<ProductModel> GetById(int id)
        {
            return await _dataContext.products.SingleOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<ProductModel> GetByName(string name)
        {
            return await _dataContext.products.Include(c => c.category).FirstOrDefaultAsync(p => p.ProductName == name);
        }

        public ProductModel Update(ProductModel product)
        {
            _dataContext.Update(product);
            _dataContext.SaveChanges();
            return product;
        }
    }
}
