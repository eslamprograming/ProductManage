using DAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ModelsVM.ProductVM
{
    public class productVM
    {
        public int? Id { get; set; }
        [Required(ErrorMessage ="Enter product Name")]
        [MinLength(3,ErrorMessage ="Name more than 3 char")]
        public string Name { get; set; } // اسم المنتج
        [Required(ErrorMessage ="Enter the start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } // تاريخ بداية ظهور المنتج
        [Required(ErrorMessage ="Enter Duration")]
        public int Duration { get; set; } // مدة ظهور المنتج (بالأيام)
        [Required(ErrorMessage ="Enter Price")]
        public decimal Price { get; set; }
        [Required(ErrorMessage ="Enter Your Image")]
        public IFormFile Image { get; set; }
        [Required(ErrorMessage ="Enter your Category")]
        public int CategoryId { get; set; }
    }
}
