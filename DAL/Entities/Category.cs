using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Category
    {
        public int Id { get; set; } // Primary Key
        public string Name { get; set; } // اسم الفئة
        public ICollection<Product> Products { get; set; } // العلاقة مع المنتجات
    }
}
