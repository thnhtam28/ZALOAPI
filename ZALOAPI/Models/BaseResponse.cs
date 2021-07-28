using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZALOAPI.Models
{
    public class BaseResponse<T> where T: new()
    {
        protected T GetObject()
        {
            return new T();
        }
        public T data { get; set; }
        public int error { get; set; }
        public string message { get; set; }
    }
}