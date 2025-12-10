using Mekashron.Repository.ApiFriendlyBlanks;

namespace Mekashron.Domain
{
    public class CustomerBlank : IApiRequestBlank
    {
        public Int32? OlEntityId { get; set; }
        public String? OlUsername { get; set; }
        public String? OlPassword { get; set; }
        public Int32? BusinessId { get; set; }
        public Int32? CategoryId { get; set; }
        public String? Email { get; set; }
        public String? Phone { get; set; }
        public String? Name { get; set; }
        public String? Password { get; set; }
        public String? CountryISO { get; set; }


        public string GetXmlFieldTags()
        {
            return
                $@"<ns1:Entity_Add env:encodingStyle=""http://www.w3.org/2003/05/soap-encoding"">
                                <ol_EntityId xsi:type=""xsd:int"">{this.OlEntityId}</ol_EntityId>
                                <ol_UserName xsi:type=""xsd:string"">{this.OlUsername}</ol_UserName>
                                <ol_Password xsi:type=""xsd:string"">{this.OlPassword}</ol_Password>
                                <BusinessId xsi:type=""xsd:int"">{this.BusinessId}</BusinessId>
                                <Employee_EntityId xsi:type=""xsd:int"">0</Employee_EntityId>
                                <CategoryID xsi:type=""xsd:int"">{this.CategoryId}</CategoryID>
                                <Email xsi:type=""xsd:string"">{this.Email}</Email>
                                <Password xsi:type=""xsd:string"">{this.Password}</Password>
                                <FirstName xsi:type=""xsd:string""></FirstName>
                                <LastName xsi:type=""xsd:string""></LastName>
                                <Mobile xsi:type=""xsd:string"">{this.Phone}</Mobile>
                                <CountryISO xsi:type=""xsd:string"">{this.CountryISO}</CountryISO>
                                <affiliate_entityID xsi:type=""xsd:int"">0</affiliate_entityID>
                            </ns1:Entity_Add>";

            
        }
    }
}
