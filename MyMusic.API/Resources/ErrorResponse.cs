using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.API.Resources
{
    public class ErrorResponse<T>
    {
        public int status { get; set; }
        public string message { get; set; }
        public T errors { get; set; }
    }
}
