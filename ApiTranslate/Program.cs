using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ApiTranslate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

// usar de referencia - https://restsharp.dev/usage.html#authenticator
// https://stackoverflow.com/questions/24057939/login-using-google-oauth-2-0-with-c-sharp
//COMUNICAR COM OUTRA API PARA O ENVIO, VOU RPECISAR DE UM SISTEMA DE HOST - CLIENT SERVER - OU OUTRA PARADA?
