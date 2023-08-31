using AuthMaster.Dtos;
using AuthMaster.Model;
using System.ComponentModel.DataAnnotations;

namespace AuthMaster.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryModel>> GetAll();
        Task<CategoryModel> GetCategoryById(int id);
        Task<CategoryModel> Add(CategoryModel category);
        CategoryModel Update(CategoryModel category);
        CategoryModel Delete(CategoryModel category);
        Task<CategoryModel> GetByName(string category);
        Task<bool> IsValidCategory(int id);
    }
}
