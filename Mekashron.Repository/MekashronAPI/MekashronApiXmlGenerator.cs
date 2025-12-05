using Mekashron.Repository.ApiFriendlyBlanks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mekashron.Repository.MekashronAPI
{
    public static class MekashronApiXmlGenerator
    {
        public static String GenerateXmlRequest(IApiRequestBlank apiBlank)
        {
            return $@"
                <?xml version=""1.0"" encoding=""UTF-8""?>
                    <env:Envelope 
                        xmlns:env=""http://www.w3.org/2003/05/soap-envelope"" 
                        xmlns:ns1=""urn:BusinessApiIntf-IBusinessAPI"" 
                        xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
                        xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                        xmlns:enc=""http://www.w3.org/2003/05/soap-encoding"">
                        <env:Body>
                            {apiBlank.GetXmlFieldTags()}
                        </env:Body>
                    </env:Envelope>
            ";
        }
    }
}
