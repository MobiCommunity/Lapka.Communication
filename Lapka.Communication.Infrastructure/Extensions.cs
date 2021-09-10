using Convey;
using Convey.CQRS.Queries;
using Convey.HTTP;
using Convey.MessageBrokers.RabbitMQ;
using Convey.WebApi;
using Convey.WebApi.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Convey.Auth;
using Convey.Persistence.MongoDB;
using Lapka.Communication.Application.Events.Abstract;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Infrastructure.Documents;
using Lapka.Communication.Infrastructure.Exceptions;
using Lapka.Communication.Infrastructure.Options;
using Lapka.Communication.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;


namespace Lapka.Communication.Infrastructure
{
    public static class Extensions
    {
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            builder
                .AddQueryHandlers()
                .AddInMemoryQueryDispatcher()
                .AddHttpClient()
                .AddErrorHandler<ExceptionToResponseMapper>()
                .AddExceptionToMessageMapper<ExceptionToMessageMapper>()
                .AddMongo()
                .AddMongoRepository<AdoptPetMessageDocument, Guid>("adoptpetmessage")
                .AddMongoRepository<StrayPetMessageDocument, Guid>("straypetmessage")
                .AddMongoRepository<UserConversationDocument, Guid>("usersconversations")
                .AddJwt()
                // .AddRabbitMq()
                // .AddConsul()
                // .AddFabio()
                // .AddMessageOutbox()
                // .AddMetrics()
                ;
            
            builder.Services.Configure<KestrelServerOptions>
                (o => o.AllowSynchronousIO = true);
            
            builder.Services.Configure<IISServerOptions>(o => o.AllowSynchronousIO = true);
            
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            IServiceCollection services = builder.Services;
            
            ServiceProvider provider = services.BuildServiceProvider();
            IConfiguration configuration = provider.GetService<IConfiguration>();

            services.AddTransient<IGrpcIdentityService, GrpcIdentityService>();
            services.AddTransient<IGrpcPhotoService, GrpcPhotoService>();
            services.AddTransient<IAdoptPetMessageRepository, AdoptPetMessageRepository>();
            services.AddTransient<IStrayPetMessageRepository, StrayPetMessageRepository>();
            services.AddTransient<IUserConversationRepository, UserConversationRepository>();

            services.AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>();
            services.AddSingleton<IDomainToIntegrationEventMapper, DomainToIntegrationEventMapper>();

            services.AddTransient<IEventProcessor, EventProcessor>();
            services.AddTransient<IMessageBroker, DummyMessageBroker>();
            
            FilesMicroserviceOptions filesMicroserviceOptions = new FilesMicroserviceOptions();
            configuration.GetSection("filesMicroservice").Bind(filesMicroserviceOptions);
            services.AddSingleton(filesMicroserviceOptions);
            
            IdentityMicroserviceOptions identityMicroserviceOptions = new IdentityMicroserviceOptions();
            configuration.GetSection("identityMicroservice").Bind(identityMicroserviceOptions);
            services.AddSingleton(identityMicroserviceOptions);
            
            services.AddGrpcClient<PhotoProto.PhotoProtoClient>(o =>
            {
                o.Address = new Uri(filesMicroserviceOptions.UrlHttp2);
            });
            
            services.AddGrpcClient<IdentityProto.IdentityProtoClient>(o =>
            {
                o.Address = new Uri(identityMicroserviceOptions.UrlHttp2);
            });

            builder.Services.Scan(s => s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>)))
                .AsImplementedInterfaces().WithTransientLifetime());

            return builder;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app
                .UseErrorHandler()
                .UseConvey()
                .UseAuthentication()
                //.UseMetrics()
                //.UseRabbitMq()
                ;


            return app;
        }
        
        public static async Task<Guid> AuthenticateUsingJwtGetUserIdAsync(this HttpContext context)
        {
            AuthenticateResult authentication = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

            return authentication.Succeeded ? Guid.Parse(authentication.Principal.Identity.Name) : Guid.Empty;
        }

    }
}