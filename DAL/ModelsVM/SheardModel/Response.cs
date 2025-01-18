using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ModelsVM.SheardModel
{
    public class Response<T>
    {
        public bool success { get; set; }
        public string? message { get; set; }
        public List<T>? values { get; set; }
        public T? Value { get; set; }
        public int? groups { get; set; }
        public int? group { get; set; }
        public int? pagging { get; set; } = 10;
    }
}
