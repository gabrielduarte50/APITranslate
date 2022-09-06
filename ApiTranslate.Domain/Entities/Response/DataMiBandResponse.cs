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

    //Summary class 
    public class Slp
    {
        public int st { get; set; }
        public int ed { get; set; }
        public int dp { get; set; }
        public int lt { get; set; }
        public int wk { get; set; }
        public int usrSt { get; set; }
        public int usrEd { get; set; }
        public int wc { get; set; }
        public int @is { get; set; }
        public int lb { get; set; }
        public int to { get; set; }
        public int dt { get; set; }
        public int rhr { get; set; }
        public int ss { get; set; }
    }

    public class Stage
    {
        public int start { get; set; }
        public int stop { get; set; }
        public int mode { get; set; }
        public int dis { get; set; }
        public int cal { get; set; }
        public int step { get; set; }
    }

    public class Stp
    {
        public int ttl { get; set; }
        public int dis { get; set; }
        public int cal { get; set; }
        public int wk { get; set; }
        public int rn { get; set; }
        public int runDist { get; set; }
        public int runCal { get; set; }
        public List<Stage> stage { get; set; }
    }

    public class SummaryMiBand
    {
        public int v { get; set; }
        public Slp slp { get; set; }
        public Stp stp { get; set; }
        public int goal { get; set; }
        public string tz { get; set; }
        public string sn { get; set; }
        public int byteLength { get; set; }
        public int sync { get; set; }
    }
}
