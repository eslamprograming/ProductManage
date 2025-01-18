using BLL.IService;
using DAL.Entities;
using DAL.IRepo;
using DAL.ModelsVM.SheardModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class CategoryService:ICategoryService
    {
        private readonly ICategoryRepo categoryRepo;

        public CategoryService(ICategoryRepo categoryRepo)
        {
            this.categoryRepo = categoryRepo;
        }

        public async Task<Response<Category>> AddCategoryAsync(Category category)
        {
            try
            {
                Category cat = new Category();
                cat.Name = category.Name;
                var result = await categoryRepo.AddCategoryAsync(cat);
                return result;
            }
            catch (Exception ex)
            {
                return new Response<Category>()
                {
                    success=false,
                    message=ex.Message
                };
            }
        }

        public async Task<Response<Category>> DeleteCategoryAsync(int category_Id)
        {
            var result = await categoryRepo.DeleteCategoryAsync(category_Id);
            return result;
        }

        public async Task<Response<Category>> GetAllCategoryAsync()
        {
            var result = await categoryRepo.GetAllCategoryAsync();
            return result;
        }

        public async Task<Response<Category>> UpdateCategoryAsync(int category_Id, string categoryName)
        {
            var result = await categoryRepo.UpdateCategoryAsync(category_Id, categoryName);
            return result;
        }
    }
}
