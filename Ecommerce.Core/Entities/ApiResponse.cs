using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
    public class ApiResponse
    {
         public HttpStatusCode statusCode {  get; set; }
        public bool IsSuccess { get; set; }
        public string Erorrmessages { get; set; }
        public Object Result { get; set; }

    }
}
