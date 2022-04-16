using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTranslate.Domain.Entities.Response
{
    public class Datum
    {
        public string uid { get; set; }
        public int data_type { get; set; }
        public string date_time { get; set; }
        public int source { get; set; }
        public string summary { get; set; }
        public string uuid { get; set; }
    }

    public class DataMiBandResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<Datum> data { get; set; }
    }
}
