using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoPayApi.Data.DTO
{
    public class ErrorHandler
    {
        public bool isSuccess { get; set; }
        public dynamic data { get; set; }
        public string error { get; set; }
        public void failFunction(string error)
        {
            this.isSuccess = false;
            this.error = error;
        }
        public void failFunction(string error,dynamic data)
        {
            this.isSuccess = false;
            this.error = error;
            this.data = data;
        }
        public void successFunction()
        {
            this.isSuccess = true;
        }
        public void successFunction(dynamic data)
        {
            this.isSuccess = true;
            data = data;
        }
    }
    
}
