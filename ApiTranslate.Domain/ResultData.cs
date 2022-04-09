using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTranslate.Domain
{
    public sealed class ResultData
    {
        public ResultData(bool success = true, object data = null)
        {
            Success = success;
            Data = data;
        }

        public bool Success { get; set; }
        public object Data { get; set; }
    }
}

