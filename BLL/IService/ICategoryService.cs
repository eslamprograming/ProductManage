using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.ModelsVM.SheardModel;

namespace BLL.IService
{
    public interface ICategoryService
    {
        Task<Response<Category>> AddCategoryAsync(Category category);
        Task<Response<Category>> DeleteCategoryAsync(int category_Id);
        Task<Response<Category>> GetAllCategoryAsync();
        Task<Response<Category>> UpdateCategoryAsync(int category_Id, string categoryName);
    }
}
