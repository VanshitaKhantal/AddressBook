using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Utility
{
    /// <summary>
    /// Generic response model to standardize API responses.
    /// Contains metadata such as status code, success flag, message, and data payload.
    /// </summary>
    /// <typeparam name="T">The type of data being returned in the response.</typeparam>
    public class ResponseModel<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseModel{T}"/> class.
        /// </summary>
        /// <param name="statusCode">HTTP status code of the response.</param>
        /// <param name="success">Indicates whether the operation was successful.</param>
        /// <param name="message">Descriptive message about the operation result.</param>
        /// <param name="data">The actual data being returned in the response.</param>
        public ResponseModel(int statusCode, bool success, string message, T data)
        {
            StatusCode = statusCode;
            Success = success;
            Message = message;
            Data = data;
        }

        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }


    }
}
