using BLL.Helper;
using BLL.IService;
using DAL.Entities;
using DAL.IRepo;
using DAL.ModelsVM.ProductVM;
using DAL.ModelsVM.SheardModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class ProductService : IProductServic
    {
        private readonly IProductRepo productRepo;

        public ProductService(IProductRepo productRepo)
        {
            this.productRepo = productRepo;
        }

        public async Task<Response<Product>> AddProductAsync(string Id, productVM productVM)
        {
            try
            {
                Product product = new Product();
                product.Name = productVM.Name;
                product.Price=productVM.Price;
                product.ImagePath = await FileHelper.UploadFileAsync(productVM.Image,"wwwroot");
                product.StartDate = productVM.StartDate;
                product.CreationDate = DateTime.Now;
                product.Duration = productVM.Duration;
                product.CategoryId = productVM.CategoryId;
                product.CreatedByUserId = Id;
                var result = await productRepo.AddProductAsync(product);
                return result;
            }
            catch (Exception ex)
            {
                return new Response<Product>()
                {
                    success = false,
                    message = ex.Message
                };
            }
        }

        public async Task<Response<Product>> DeleteProductAsync(int product_id)
        {
            var result = await productRepo.DeleteProductAsync(product_id);
            var deleteImage = await FileHelper.DeleteFile("wwwroot", result.Value.ImagePath);

            return result;
        }

        public async Task<Response<Product>> GetAllProductAsync()
        {
            var result = await productRepo.GetAllProductAsync();
            return result;
        }
        public async Task<Response<Product>> GetAllAllProductAsync()
        {
            var result = await productRepo.GetAllAllProductAsync();
            return result;
        }
       
        public async Task<Response<productVM>> GetProductbyId(int Id)
        {
            var result = await productRepo.GetProductbyIdAsync(Id);
            productVM productVM = new productVM();
            productVM.Name = result.Value.Name;
            productVM.StartDate = result.Value.StartDate;
            productVM.Duration = result.Value.Duration;
            productVM.CategoryId = result.Value.CategoryId;
            productVM.Id = result.Value.Id;
            productVM.Price = result.Value.Price;
            return new Response<productVM>()
            {
                success = true,
                Value=productVM,
                message=result.Value.ImagePath
            };
        }
        public async Task<Response<Product>> GetProductEntitybyId(int Id)
        {
            var result = await productRepo.GetProductbyIdAsync(Id);
            return result;
        }

        public async Task<Response<Product>> GetAllProductProductInCategoryAsync(int Category_id)
        {
            var result = await productRepo.GetAllProductProductInCategoryAsync(Category_id);
            return result;
        }
        public async Task<Response<Product>> UpdateProductAsync(string UserId, int product_id, IFormFile Image)
        {
            var result2 = await productRepo.GetProductbyIdAsync(product_id);
            var deleteImage = await FileHelper.DeleteFile("wwwroot", result2.Value.ImagePath);

            var ImagePath = await FileHelper.UploadFileAsync(Image,"wwwroot");
            var result = await productRepo.UpdateProductAsync(UserId, product_id, ImagePath);
            return result;
        }
        public async Task<Response<Product>> UpdateProductAsync(string UserId,int product_id, Product productVM)
        {
            var result = await productRepo.UpdateProductAsync(UserId,product_id,productVM);
            return result;
        }
    }
}
