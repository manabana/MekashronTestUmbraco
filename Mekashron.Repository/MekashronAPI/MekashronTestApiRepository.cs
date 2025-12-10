using Mekashron.Domain;
using Mekashron.Domain.Api;
using Mekashron.Domain.Repositories;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;


namespace Mekashron.Repository.MekashronAPI
{
    public class MekashronTestApiRepository : IMekashronApiRepository
    {
        private readonly HttpClient _httpClient;

        public MekashronTestApiRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MekashronApi");
        }

        public async Task<MekashronLoginResponse> Login(LoginBlank blank)
        {
            String soapXml = GenerateXmlRequest(blank);

            StringContent content = new StringContent(soapXml, Encoding.UTF8, "text/xml");

            HttpResponseMessage response = await _httpClient.PostAsync("", content);
            response.EnsureSuccessStatusCode();

            String xmlString = await response.Content.ReadAsStringAsync();

            XDocument xml = XDocument.Parse(xmlString);

            // извлечение JSON из XML
            String json = xml.Descendants("return").First().Value.Trim();

            // десериализация JSON в MekashronLoginResponse
            MekashronLoginResponse? result =
                JsonSerializer.Deserialize<MekashronLoginResponse>
                (
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

            return result ?? throw new Exception("Failed to deserialize MekashronLoginResponse");
        }

        private String GenerateXmlRequest(LoginBlank blank)
        {
            return
                $@"<?xml version=""1.0"" encoding=""UTF-8""?>
                    <env:Envelope 
                        xmlns:env=""http://www.w3.org/2003/05/soap-envelope"" 
                        xmlns:ns1=""urn:ICUTech.Intf-IICUTech"" 
                        xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
                        xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                        xmlns:enc=""http://www.w3.org/2003/05/soap-encoding"">
                        <env:Body>
                        <ns1:Login env:encodingStyle=""http://www.w3.org/2003/05/soap-encoding"">
                            <UserName xsi:type=""xsd:string"">{blank.Username}</UserName>
                            <Password xsi:type=""xsd:string"">{blank.Password}</Password>
                            <IPs xsi:type=""xsd:string"" />
                        </ns1:Login>
                        </env:Body>
                    </env:Envelope>";
        }

        public Task<MekashronRegisterResponse> RegisterNewCustomer(CustomerBlank blank)
        {
            throw new NotImplementedException();
        }

        public Task<CustomTableResponse> SaveLog(CustomFieldsTableBlank blank)
        {
            throw new NotImplementedException();
        }
    }
}
