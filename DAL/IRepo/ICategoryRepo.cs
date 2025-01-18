using DAL.Entities;
using DAL.ModelsVM.SheardModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepo
{
    public interface ICategoryRepo
    {
        Task<Response<Category>> AddCategoryAsync(Category category);
        Task<Response<Category>> DeleteCategoryAsync(int category_Id);
        Task<Response<Category>> GetAllCategoryAsync();
        Task<Response<Category>> UpdateCategoryAsync(int category_Id,string categoryName);
    }
}
