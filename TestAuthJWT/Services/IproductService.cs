using AuthMaster.Model;

namespace AuthMaster.Services
{
    public interface IproductService
    {
        Task<IEnumerable<ProductModel>> GetAll(int CategoryId = 0);

        Task<ProductModel> GetById(int id);

        Task<ProductModel> GetByName(string name);

        Task<ProductModel> Create(ProductModel product);

        ProductModel Update(ProductModel product);

        ProductModel Delete(ProductModel product);



    }
}
