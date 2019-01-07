using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using IgiLab.Constants;
using Microsoft.AspNetCore.Builder;

namespace IgiLab
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Other preparations before start
            System.IO.Directory.CreateDirectory(Pathes.RESOURCES_DIR);

            BuildWebHost(args).Run();
            
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
