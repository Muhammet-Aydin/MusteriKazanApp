using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusteriKazan.Web.Models
{
    public class BaseResult<T>
    {
        public int StatusCode { get; set; }
        public bool HasError { get; set; }
        public T Result { get; set; }
        public string Message { get; set; }
    }
}