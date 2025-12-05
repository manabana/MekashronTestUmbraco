using Mekashron.Repository.ApiFriendlyBlanks;
using System.Numerics;

namespace Mekashron.Domain
{
    public class LoginBlank : IApiRequestBlank
    {
        public String Username { get; set; }
        public String Password { get; set; }

        public string GetXmlFieldTags()
        {
            return
                $@"<ns1:Login env:encodingStyle=""http://www.w3.org/2003/05/soap-encoding"">
                            <UserName xsi:type=""xsd:string"">{this.Username}</UserName>
                            <Password xsi:type=""xsd:string"">{this.Password}</Password>
                            <IPs xsi:type=""xsd:string"" />
                        </ns1:Login>";
        }
    }
}
