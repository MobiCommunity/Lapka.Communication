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
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.Outbox;
using Convey.Persistence.MongoDB;
using Lapka.Communication.Application.Events.Abstract;
using Lapka.Communication.Application.Events.External;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Application.Services.Elastic;
using Lapka.Communication.Application.Services.Grpc;
using Lapka.Communication.Application.Services.Repositories;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Infrastructure.Elastic.Options;
using Lapka.Communication.Infrastructure.Elastic.Services;
using Lapka.Communication.Infrastructure.Exceptions;
using Lapka.Communication.Infrastructure.Grpc.Options;
using Lapka.Communication.Infrastructure.Grpc.Services;
using Lapka.Communication.Infrastructure.Mongo.Documents;
using Lapka.Communication.Infrastructure.Mongo.Repositories;
using Lapka.Communication.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Nest;


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
                .AddMongoRepository<ShelterDocument, Guid>("shelter")
                .AddMongoRepository<ShelterMessageDocument, Guid>("sheltermessage")
                .AddMongoRepository<UserConversationDocument, Guid>("usersconversations")
                .AddMongoRepository<ShelterPetDocument, Guid>("shelterpets")
                .AddJwt()
                .AddRabbitMq()
                .AddMessageOutbox()
                // .AddConsul()
                // .AddFabio()
                // .AddMetrics()
                ;
            
            builder.Services.Configure<KestrelServerOptions>
                (o => o.AllowSynchronousIO = true);
            
            builder.Services.Configure<IISServerOptions>(o => o.AllowSynchronousIO = true);
            
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            IServiceCollection services = builder.Services;
            
            ServiceProvider provider = services.BuildServiceProvider();
            IConfiguration configuration = provider.GetService<IConfiguration>();

            FilesMicroserviceOptions filesMicroserviceOptions = new FilesMicroserviceOptions();
            configuration.GetSection("filesMicroservice").Bind(filesMicroserviceOptions);
            services.AddSingleton(filesMicroserviceOptions);
            
            IdentityMicroserviceOptions identityMicroserviceOptions = new IdentityMicroserviceOptions();
            configuration.GetSection("identityMicroservice").Bind(identityMicroserviceOptions);
            services.AddSingleton(identityMicroserviceOptions);
            
            PetsMicroserviceOptions petsMicroserviceOptions = new PetsMicroserviceOptions();
            configuration.GetSection("petsMicroservice").Bind(petsMicroserviceOptions);
            services.AddSingleton(petsMicroserviceOptions);
            
            ElasticSearchOptions elasticSearchOptions = new ElasticSearchOptions();
            configuration.GetSection("elasticSearch").Bind(elasticSearchOptions);
            services.AddSingleton(elasticSearchOptions);
            ConnectionSettings elasticConnectionSettings = new ConnectionSettings(new Uri(elasticSearchOptions.Url));
            
            services.AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>();
            services.AddSingleton<IDomainToIntegrationEventMapper, DomainToIntegrationEventMapper>();
            services.AddSingleton<IElasticClient>(new ElasticClient(elasticConnectionSettings));

            services.AddTransient<IShelterRepository, ShelterRepository>();
            services.AddTransient<IShelterPetRepository, ShelterPetRepository>();
            services.AddTransient<IGrpcShelterService, GrpcShelterService>();
            services.AddTransient<IGrpcPhotoService, GrpcPhotoService>();
            services.AddTransient<IGrpcPetService, GrpcPetService>();
            services.AddTransient<IShelterMessageRepository, ShelterMessageRepository>();
            services.AddTransient<IUserConversationRepository, UserConversationRepository>();
            services.AddTransient<IShelterMessageFactory, ShelterMessageFactory>();
            services.AddTransient<IShelterMessageElasticsearchUpdater, ShelterMessageElasticsearchUpdater>();
            services.AddTransient<IUserConversationElasticsearchUpdater, UserConversationElasticsearchUpdater>();
            services.AddTransient<IEventProcessor, EventProcessor>();
            services.AddTransient<IMessageBroker, MessageBroker>();
            
            services.AddGrpcClient<PhotoProto.PhotoProtoClient>(o =>
            {
                o.Address = new Uri(filesMicroserviceOptions.UrlHttp2);
            });
            
            services.AddGrpcClient<ShelterProto.ShelterProtoClient>(o =>
            {
                o.Address = new Uri(identityMicroserviceOptions.UrlHttp2);
            });
            
            services.AddGrpcClient<PetProto.PetProtoClient>(o =>
            {
                o.Address = new Uri(petsMicroserviceOptions.UrlHttp2);
            });
            
            services.AddHostedService<ElasticSearchSeeder>();
            
            builder.Services.Scan(s => s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>)))
                .AsImplementedInterfaces().WithTransientLifetime());

            builder.Build();
            
            return builder;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app
                .UseErrorHandler()
                .UseConvey()
                .UseAuthentication()
                .UseRabbitMq()
                .SubscribeEvent<ShelterPetMade>()
                .SubscribeEvent<ShelterPetRemoved>()
                .SubscribeEvent<ShelterAdded>()
                .SubscribeEvent<ShelterRemoved>()
                .SubscribeEvent<ShelterOwnerAssigned>()
                .SubscribeEvent<ShelterOwnerUnassigned>()
                //.UseMetrics()
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