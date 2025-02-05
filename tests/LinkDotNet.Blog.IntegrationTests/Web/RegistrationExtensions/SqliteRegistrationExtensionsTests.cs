﻿using LinkDotNet.Blog.Domain;
using LinkDotNet.Blog.Infrastructure.Persistence;
using LinkDotNet.Blog.Web;
using LinkDotNet.Blog.Web.RegistrationExtensions;
using Microsoft.Extensions.DependencyInjection;

namespace LinkDotNet.Blog.IntegrationTests.Web.RegistrationExtensions;

public class SqliteRegistrationExtensionsTests
{
    [Fact]
    public void ShouldGetValidRepository()
    {
        var serviceCollection = new ServiceCollection();
        var appConfig = new AppConfiguration
        {
            ConnectionString = "Filename=:memory:",
        };
        serviceCollection.AddScoped(_ => appConfig);

        serviceCollection.UseSqliteAsStorageProvider();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        serviceProvider.GetService<IRepository<BlogPost>>().Should().NotBeNull();
        serviceProvider.GetService<IRepository<Skill>>().Should().NotBeNull();
    }
}
