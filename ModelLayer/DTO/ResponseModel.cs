using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO
{
    public class ResponseModel<T>
    {
        public bool Success { get; set; }  // Indicates if the request was successful (true/false)
        public string Message { get; set; } // A message (e.g., "Contact retrieved successfully")
        public T Data { get; set; } // The actual data being returned (List<Contact>, Contact, etc.)
    }
}
