using DAL.Entities;
using DAL.ModelsVM.ProductVM;
using DAL.ModelsVM.SheardModel;

namespace DAL.IRepo
{
    public interface IProductRepo
    {
        Task<Response<Product>> AddProductAsync(Product product);
        Task<Response<Product>> DeleteProductAsync(int product_id);
        Task<Response<Product>> GetAllProductAsync();
        Task<Response<Product>> GetAllAllProductAsync();
        Task<Response<Product>> GetProductbyIdAsync(int Id);
        Task<Response<Product>> GetAllProductProductInCategoryAsync(int Category_id);
        Task<Response<Product>> UpdateProductAsync(string UserId, int product_id, string ImagePath);
        Task<Response<Product>> UpdateProductAsync(string UserId,int product_id, Product productVM);
    }
}
