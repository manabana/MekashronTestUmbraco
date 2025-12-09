using Mekashron.Domain.Repositories;
using Mekashron.Domain.Repositories.GeoIp;
using Mekashron.Domain.Services;
using Mekashron.Repository.MekashronAPI;
using Mekashron.Repository.OuterServices;
using Mekashron.Services.Login;
using Mekashron.Services.OuterServices;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// внедрение HttpClient через Dependency Injection, чтобы использовать в репозитории
String? mekashronBaseUrl = builder.Configuration["MekashronApi:BaseUrl"];
if (mekashronBaseUrl is null)
    throw new Exception("MekashronAPI:BaseURL в appsettings.json не указан или указан некорректно");
builder.Services.AddHttpClient("MekashronApi", client =>
    {
        client.BaseAddress = new Uri(mekashronBaseUrl);
    }
);

builder.Services.AddScoped<IGeoIpService,GeoIpService>();
builder.Services.AddHttpClient<IGeoIpRepository, GeoIpRepository>();

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
