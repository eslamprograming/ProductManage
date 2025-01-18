using DAL.Entities;
using DAL.ModelsVM.ProductVM;
using DAL.ModelsVM.SheardModel;
using Microsoft.AspNetCore.Http;

namespace BLL.IService
{
    public interface IProductServic
    {
        Task<Response<Product>> AddProductAsync(string Id,productVM productVM);
        Task<Response<Product>> DeleteProductAsync(int product_id);
        Task<Response<Product>> GetAllProductAsync();
        Task<Response<Product>> GetAllAllProductAsync();
        Task<Response<productVM>> GetProductbyId(int Id);
        Task<Response<Product>> GetProductEntitybyId(int Id);
        Task<Response<Product>> GetAllProductProductInCategoryAsync(int Category_id);
        Task<Response<Product>> UpdateProductAsync(string UserId, int product_id, IFormFile Image);
        Task<Response<Product>> UpdateProductAsync(string UserId, int product_id, Product productVM);

    }
}
