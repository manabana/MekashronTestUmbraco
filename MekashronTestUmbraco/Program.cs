using Mekashron.Domain.Repositories;
using Mekashron.Domain.Services;
using Mekashron.Repository.MekashronAPI;
using Mekashron.Services.Login;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IMekashronApiService, MekashronApiService>();
builder.Services.AddScoped<IMekashronApiRepository, MekashronApiRepository>();

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddComposers()
    .Build();

WebApplication app = builder.Build();

await app.BootUmbracoAsync();


app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();
