using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Proact.Services;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace PROACTServer {
    public class Program {
        public static void Main( string[] args ) {
            CreateHostBuilder( args ).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder( string[] args ) =>
            Host.CreateDefaultBuilder( args )
                .ConfigureLogging( logging => logging.AddAzureWebAppDiagnostics() )
                .ConfigureServices( serviceCollection => serviceCollection
                     .Configure<AzureFileLoggerOptions>( options => {
                         options.FileName = "azure-diagnostics-";
                         options.FileSizeLimit = 50 * 1024;
                         options.RetainedFileCountLimit = 5;
                     } )
                     .Configure<AzureBlobLoggerOptions>( options => {
                         options.BlobName = "log.txt";
                     } ) )
                .ConfigureWebHostDefaults( webBuilder => {
                    webBuilder.UseStartup<Startup>();
                } );
    }
}