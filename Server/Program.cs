using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Server
{
    public class Program
    {

        public static string Ip { get; set; }

        public static int  Port { get; set; }

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder().AddCommandLine(args).Build();

            Ip = config["ip"];
            Port = int.Parse(config["port"]);

            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().UseUrls($"http://{Ip}:{Port}");

        }

    }
}
