using DAL.DBContext;
using DAL.Entities;
using DAL.IRepo;
using DAL.ModelsVM.ProductVM;
using DAL.ModelsVM.SheardModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo
{
    public class ProductRepo : IProductRepo
    {
        private readonly ApplicationDBContext db;

        public ProductRepo(ApplicationDBContext db)
        {
            this.db = db;
        }

        public async Task<Response<Product>> AddProductAsync(Product product)
        {
            try
            {
                await db.Products.AddAsync(product);
                await db.SaveChangesAsync();
                return new Response<Product>()
                {
                    success = true,
                };
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
            try
            {
                var product = await db.Products.FindAsync(product_id);
                if (product == null) {
                    return new Response<Product>()
                    {
                        success = false,
                        message = "this category is not found"
                    };
                }
                db.Products.Remove(product);
                await db.SaveChangesAsync();
                return new Response<Product>()
                {
                    success = true,
                    Value=product
                };
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

        public async Task<Response<Product>> GetAllProductAsync()
        {
            try
            {
                var products = await db.Products.Where(n => n.StartDate.AddDays(n.Duration) >= DateTime.Now&& n.StartDate<=DateTime.Now).ToListAsync();
                
                return new Response<Product>()
                {
                    success = true
                    ,values=products
                };
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
        public async Task<Response<Product>> GetAllAllProductAsync()
        {
            try
            {
                var products = await db.Products.Include(n => n.Category).ToListAsync();

                return new Response<Product>()
                {
                    success = true
                    ,
                    values = products
                };
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
        public async Task<Response<Product>> GetProductbyIdAsync(int Id)
        {
            try
            {
                var products = await db.Products.FindAsync(Id);

                return new Response<Product>()
                {
                    success = true
                    ,
                    Value = products
                };
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

        public async Task<Response<Product>> GetAllProductProductInCategoryAsync(int Category_id)
        {
            try
            {
                var products = await db.Products.Where(n => n.CategoryId == Category_id).ToListAsync();
                return new Response<Product>()
                {
                    success = true,
                    values=products
                };
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
        public async Task<Response<Product>> UpdateProductAsync(string UserId, int product_id, string ImagePath)
        {
            try
            {
                var product = await db.Products.FindAsync(product_id);
                if (product != null)
                {
                    product.ImagePath = ImagePath;

                    ProductLog productLog = new ProductLog();
                    productLog.ProductId = product_id;
                    productLog.UpdateDateTime = DateTime.Now;
                    productLog.UpdatedByUserId = UserId;
                    productLog.UpdateDetails = "تعديل صوره المنتج ";
                    await db.ProductLogs.AddAsync(productLog);
                    await db.SaveChangesAsync();
                    return new Response<Product>()
                    {
                        success = true,
                        Value = product
                    };
                }
                return new Response<Product>()
                {
                    success = false,
                    message = "this product is not found"
                };
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
        public async Task<Response<Product>> UpdateProductAsync(string UserId,int product_id, Product productVM)
        {
            try
            {
                var product = await db.Products.FindAsync(product_id);
                if (product != null)
                {
                    product.Name = productVM.Name;
                    //product.CreationDate = productVM.CreationDate;
                    product.StartDate = productVM.StartDate;
                    product.Duration = productVM.Duration;
                    product.Price = productVM.Price;

                    ProductLog productLog = new ProductLog();
                    productLog.ProductId = product_id;
                    productLog.UpdateDateTime = DateTime.Now;
                    productLog.UpdatedByUserId = UserId;
                    productLog.UpdateDetails = "تعديل تفاصيل المنتج ";
                    await db.ProductLogs.AddAsync(productLog);
                    await db.SaveChangesAsync();
                    return new Response<Product>()
                    {
                        success = true,
                        Value=product
                    };
                }
                return new Response<Product>()
                {
                    success = false,
                    message="this product is not found"
                };
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
    }
}
