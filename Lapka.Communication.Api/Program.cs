using Convey;
using Convey.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Open.Serialization.Json.Newtonsoft;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Lapka.Communication.Api.Attributes;
using Lapka.Communication.Api.Grpc.Controllers;
using Lapka.Communication.Application;
using Lapka.Communication.Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Lapka.Communication.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateWebHostBuilder(args).Build().RunAsync();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).ConfigureKestrel(options =>
                {
                    options.ListenAnyIP(5004, o => o.Protocols = HttpProtocols.Http1);
                    options.ListenAnyIP(5014, o => o.Protocols = HttpProtocols.Http2);
                }).ConfigureServices(services =>
                {
                    services.AddControllers();

                    services.TryAddSingleton(new JsonSerializerFactory().GetSerializer());

                    services
                        .AddConvey()
                        .AddInfrastructure()
                        .AddApplication();
                    
                    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                    services.AddGrpc();

                    services.AddSwaggerGen(c =>
                    {

                        c.SwaggerDoc("v1", new OpenApiInfo
                        {
                            Version = "v1",
                            Title = "Communication Microservice",
                            Description = ""
                        });
                        string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                        string xmlFile2 = "Lapka.Communication.Application.xml";
                        string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                        string xmlPath2 = Path.Combine(AppContext.BaseDirectory, xmlFile2);
                        c.OperationFilter<BasicAuthOperationsFilter>();
                        c.IncludeXmlComments(xmlPath);
                        c.IncludeXmlComments(xmlPath2);
                    });

                    services.BuildServiceProvider();
                }).Configure(app =>
                {
                    app
                        .UseConvey()
                        .UseInfrastructure()
                        .UseRouting()
                        .UseSwagger(c => { c.RouteTemplate = "api/communication/swagger/{documentname}/swagger.json"; })
                        .UseSwaggerUI(c =>
                        {
                            c.SwaggerEndpoint("/api/communication/swagger/v1/swagger.json", "My API V1");
                            c.RoutePrefix = "api/communication/swagger";
                        })
                        .UseEndpoints(e =>
                        {
                            e.MapControllers();
                            e.MapGrpcService<GrpcMessageController>();
                            e.Map("ping", async ctx => { await ctx.Response.WriteAsync("Alive"); });
                        });
                })
                .UseLogging();
    }
}