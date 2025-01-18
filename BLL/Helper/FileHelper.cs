using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BLL.Helper
{
    public static class FileHelper
    {
        public static async Task<string> UploadFileAsync(IFormFile file, string rootPath, string folderName = "uploads")
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is invalid");

            // التحقق من حجم الصورة (أقل من 1 ميجابايت)
            const long maxFileSize = 1 * 1024 * 1024; // 1 ميجابايت
            if (file.Length > maxFileSize)
                throw new ArgumentException("File size exceeds 1 MB");

            // التحقق من الامتداد (JPG, JPEG, PNG)
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!Array.Exists(allowedExtensions, ext => ext == fileExtension))
                throw new ArgumentException("Unsupported file type. Only JPG, JPEG, and PNG are allowed.");

            // تحديد مسار المجلد
            string uploadFolder = Path.Combine(rootPath, folderName);
            Directory.CreateDirectory(uploadFolder); // إنشاء المجلد إذا لم يكن موجودًا

            // إنشاء اسم فريد للملف
            string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
            string filePath = Path.Combine(uploadFolder, uniqueFileName);

            // نسخ الملف
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // إرجاع المسار النسبي للملف
            return Path.Combine(folderName, uniqueFileName).Replace("\\", "/");
        }

        public static async Task<bool> DeleteFile(string rootPath, string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                throw new ArgumentException("File path is invalid");

            // تحديد المسار الكامل للملف
            string filePath = Path.Combine(rootPath, relativePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return true; // تم الحذف بنجاح
            }

            return false; // الملف غير موجود
        }
    }
}
