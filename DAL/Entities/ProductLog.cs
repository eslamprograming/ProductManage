using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class ProductLog
    {
        public int Id { get; set; } // Primary Key
        public int ProductId { get; set; } // معرف المنتج المرتبط بالتحديث
        public Product Product { get; set; } // العلاقة مع المنتج
        public string UpdatedByUserId { get; set; } // معرف المستخدم الذي قام بالتحديث
        public DateTime UpdateDateTime { get; set; } // وقت التحديث
        public string UpdateDetails { get; set; } // تفاصيل التحديث
    }
}
