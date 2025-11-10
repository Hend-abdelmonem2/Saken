using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.Response
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public BaseResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }

      
        public static BaseResponse SuccessResponse(string message = "Operation successful")
            => new BaseResponse(true, message);

        public static BaseResponse Failure(string message = "Operation failed")
            => new BaseResponse(false, message);
    }
}
