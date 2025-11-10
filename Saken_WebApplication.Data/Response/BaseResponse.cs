using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.Response
{
    public class BaseResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T? Data { get; set; }

        // Constructors
        public BaseResponse(bool success, string message, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        
        public static BaseResponse<T> SuccessResponse(T? data, string message = "Operation successful")
        {
            return new BaseResponse<T>(true, message, data);
        }

        public static BaseResponse<T> SuccessResponse(BaseResponse<DTO.UserPreferences.UserPreferencesDto> result, string message = "Operation successful")
        {
            return new BaseResponse<T>(true, message);
        }

        public static BaseResponse<T> Failure(string message = "Operation failed")
        {
            return new BaseResponse<T>(false, message);
        }
    }
}
