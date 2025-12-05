using Mekashron.Domain;
using Mekashron.Domain.Api;
using Mekashron.Domain.Repositories;
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
            String soapXml = MekashronApiXmlGenerator.GenerateXmlRequest(blank);

            StringContent content = new StringContent(soapXml, Encoding.UTF8, "text/xml");

            HttpResponseMessage response = await _httpClient.PostAsync("", content);
            response.EnsureSuccessStatusCode();

            String xmlString = await response.Content.ReadAsStringAsync();

            XDocument xml = XDocument.Parse(xmlString);

            // извлечение JSON из XML
            String json = xml.Descendants("return").First().Value.Trim();

            // десериализация JSON в MekashronRegisterResponse
            MekashronRegisterResponse? result =
                JsonSerializer.Deserialize<MekashronRegisterResponse>
                (
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

            return result ?? throw new Exception("Failed to deserialize MekashronLoginResponse");
        }
    }
}
