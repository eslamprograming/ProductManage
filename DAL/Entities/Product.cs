using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Product
    {
        public int? Id { get; set; } // Primary Key
        [Required(ErrorMessage ="Enter your Name")]
        public string Name { get; set; } // اسم المنتج
        public DateTime CreationDate { get; set; } // تاريخ إنشاء المنتج
        public string? CreatedByUserId { get; set; } // معرف المستخدم الذي أنشأ المنتج
        [Required(ErrorMessage ="Enter your start Date")]
        public DateTime StartDate { get; set; } // تاريخ بداية ظهور المنتج
        [Required(ErrorMessage ="Enter Duration")]
        public int Duration { get; set; } // مدة ظهور المنتج (بالأيام)
        [Required(ErrorMessage ="Enter Price")]
        public decimal Price { get; set; } // سعر المنتج
        [Required(ErrorMessage ="Enter Image")]
        public string ImagePath { get; set; } // مسار الصورة
        [Required(ErrorMessage ="Enter Category")]
        public int CategoryId { get; set; } // Foreign Key إلى الفئة
        public Category? Category { get; set; } // العلاقة مع الفئة
    }
}
