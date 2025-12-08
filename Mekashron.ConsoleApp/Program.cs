using Mekashron.Domain;
using Mekashron.Domain.Repositories;
using Mekashron.Domain.Services;
using Mekashron.Repository.MekashronAPI;
using Mekashron.Services.Login;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text.Json;

namespace Mekashron.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // DI контейнер
            var services = new ServiceCollection();

            // внедрение HttpClient через Dependency Injection, чтобы использовать в репозитории
            String? mekashronBaseUrl = "http://bzq.mekashron.co.il:33326/soap/IICUTech";
            if (mekashronBaseUrl is null)
                throw new Exception("MekashronAPI:BaseURL в appsettings.json не указан или указан некорректно");
            services.AddHttpClient("MekashronApi", client =>
            {
                client.BaseAddress = new Uri(mekashronBaseUrl);
            }
            );

            // регистрируем репозиторий и сервис
            services.AddTransient<IMekashronApiRepository, MekashronApiRepository>();
            services.AddTransient<IMekashronApiService, MekashronApiService>();

            var provider = services.BuildServiceProvider();

            // получаем сервис
            var service = provider.GetRequiredService<IMekashronApiService>();

            Console.Write("Email: ");
            var email = Console.ReadLine();
            Console.WriteLine();
            Console.Write("phone: ");
            var phone = Console.ReadLine();
            Console.WriteLine();
            Console.Write("name: ");
            var name = Console.ReadLine();


            CustomerBlank blank = new CustomerBlank
            {
                OlEntityId = 31159,
                OlUsername = "server@mekashron.com",
                OlPassword = "123456",
                BusinessId = 1,
                CategoryId = 150,
                Email = email,
                Phone = phone,
                Name = name,
            };
                


            var res = await service.RegisterNewCustomer(blank);
            Console.WriteLine(JsonSerializer.Serialize(res, new JsonSerializerOptions { WriteIndented = true }));

            }
    }
}
