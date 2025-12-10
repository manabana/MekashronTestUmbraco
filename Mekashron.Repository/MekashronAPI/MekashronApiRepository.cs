using Mekashron.Domain;
using Mekashron.Domain.Api;
using Mekashron.Domain.Repositories;
using Mekashron.Repository.ApiFriendlyBlanks;
using Mekashron.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace Mekashron.Repository.MekashronAPI
{
    public class MekashronApiRepository : IMekashronApiRepository
    {
        private readonly HttpClient _httpClient;

        public MekashronApiRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MekashronApi");
        }

        public Task<MekashronLoginResponse> Login(LoginBlank blank)
        {
            throw new NotImplementedException();
        }

        public async Task<MekashronRegisterResponse> RegisterNewCustomer(CustomerBlank blank)
        {
            return await FetchApi<MekashronRegisterResponse, CustomerBlank>(blank);
        }

        public async Task<CustomTableResponse> SaveLog(CustomFieldsTableBlank blank)
        {
            return await FetchApi<CustomTableResponse, CustomFieldsTableBlank>(blank);
        }

        private async Task<R> FetchApi<R, T>(T blank) where T : IApiRequestBlank
        {
            String soapXml = MekashronApiXmlGenerator.GenerateXmlRequest(blank);

            using StringContent content =
                new(soapXml, Encoding.UTF8, "text/xml");

            HttpResponseMessage response = await _httpClient.PostAsync("", content);
            response.EnsureSuccessStatusCode();

            String xmlString = await response.Content.ReadAsStringAsync();

            XDocument xml = XDocument.Parse(xmlString);

            var returnElement =
                xml.Descendants()
                .FirstOrDefault(e => e.Name.LocalName == "return");

            // извлечение JSON из XML
            String json = returnElement!.Value.Trim();

            // десериализация JSON в MekashronRegisterResponse
            R? result =
                JsonSerializer.Deserialize<R>
                (
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

            return result ?? throw new Exception($"Failed to deserialize {nameof(R)}");
        }
    }
}
