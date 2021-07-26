﻿using LinkDotNet.Infrastructure.Persistence;
using LinkDotNet.Infrastructure.Persistence.RavenDb;
using Microsoft.Extensions.DependencyInjection;

namespace LinkDotNet.Blog.Web.RegistrationExtensions
{
    public static class RavenDbRegistrationExtensions
    {
        public static void UseRavenDbAsStorageProvider(this IServiceCollection services)
        {
            services.AssertNotAlreadyRegistered<IBlogPostRepository>();
            services.AssertNotAlreadyRegistered<IProfileRepository>();

            services.AddSingleton(ctx =>
            {
                var configuration = ctx.GetRequiredService<AppConfiguration>();
                var connectionString = configuration.ConnectionString;
                var databaseName = configuration.DatabaseName;
                return RavenDbConnectionProvider.Create(connectionString, databaseName);
            });
            services.AddScoped<IBlogPostRepository, BlogPostRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
        }
    }
}