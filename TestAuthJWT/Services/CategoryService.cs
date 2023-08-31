using AuthMaster.Dtos;
using AuthMaster.Model;
using Microsoft.EntityFrameworkCore;
using TestAuthJWT.Data;

namespace AuthMaster.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;

        public CategoryService(DataContext context)
        {
            _context = context;
        }
   

        public async Task<CategoryModel> Add(CategoryModel category)
        {
            await _context.AddAsync(category);
            _context.SaveChanges();
            return category;
        }

        public CategoryModel Delete(CategoryModel category)
        {       
            _context.Remove(category);
            _context.SaveChanges();
            return category;
        }

        public async Task<IEnumerable<CategoryModel>> GetAll()
        {
           return await _context.categories.OrderBy(c=> c.CategoryName).ToListAsync();
        }

        public async Task<CategoryModel> GetCategoryById(int id)
        {
            return await _context.categories.SingleOrDefaultAsync(c => c.CategoryId == id);
        }

        public CategoryModel Update(CategoryModel category)
        {
            _context.Update(category);
            _context.SaveChanges();
            return (category);
        }

        public async Task<CategoryModel> GetByName(string category)
        {
            return await _context.categories.FirstOrDefaultAsync(c => c.CategoryName == category);
        }

        public async Task<bool> IsValidCategory(int id)
        {
            return await _context.categories.AnyAsync(c => c.CategoryId == id);
        }
    }
}
