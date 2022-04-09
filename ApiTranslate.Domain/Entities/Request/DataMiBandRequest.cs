using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTranslate.Domain.Entities
{
    public class DataMiBandRequest
    {
        public string DeviceId { get; set; }
        public DateTime startDate { get; set; } //format of YYYY-MM-DD
        public DateTime endDate { get; set; }
    }
}
