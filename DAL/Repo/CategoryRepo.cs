using DAL.DBContext;
using DAL.Entities;
using DAL.IRepo;
using DAL.ModelsVM.SheardModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly ApplicationDBContext db;

        public CategoryRepo(ApplicationDBContext db)
        {
            this.db = db;
        }

        public async Task<Response<Category>> AddCategoryAsync(Category category)
        {
            try
            {
                await db.Categories.AddAsync(category);
                await db.SaveChangesAsync();
                return new Response<Category>()
                {
                    success = true,
                };
            }
            catch (Exception ex)
            {
                return new Response<Category>()
                {
                    success = false,
                    message = ex.Message
                };
            }
        }

        public async Task<Response<Category>> DeleteCategoryAsync(int category_Id)
        {
            try
            {
                
                var category = await db.Categories.Where(n=>n.Id==category_Id).Include(n=>n.Products).SingleOrDefaultAsync();
                if (category == null)
                {
                    return new Response<Category>()
                    {
                        success = false,
                        message = "this Category is not found"
                    };
                }
                else if (category.Products != null)
                {
                    return new Response<Category>()
                    {
                        success = false,
                        message = "this category is content a product so you can not delete this category"
                    };
                }
                db.Categories.Remove(category);
                await db.SaveChangesAsync();
                return new Response<Category>()
                {
                    success=true
                };
            }
            catch (Exception ex)
            {
                return new Response<Category>()
                {
                    success = false,
                    message = ex.Message
                };
            }
        }

        public async Task<Response<Category>> GetAllCategoryAsync()
        {
            try
            {
                var Categories = await db.Categories.ToListAsync();
                return new Response<Category>()
                {
                    success=true,
                    values=Categories
                };
            }
            catch (Exception ex)
            {
                return new Response<Category>()
                {
                    success = false,
                    message = ex.Message
                };
            }
        }

        public async Task<Response<Category>> UpdateCategoryAsync(int category_Id, string categoryName)
        {
            try
            {
                var category = await db.Categories.FindAsync(category_Id);
                if (category == null)
                {
                    return new Response<Category>()
                    {
                        success = false,
                        message = "this category is not exist"
                    };
                }
                category.Name = categoryName;
                await db.SaveChangesAsync();
                return new Response<Category>()
                {
                    success = true,
                    Value = category
                };
            }
            catch (Exception ex)
            {
                return new Response<Category>()
                {
                    success = false,
                    message = ex.Message
                };
            }
        }
    }
}
