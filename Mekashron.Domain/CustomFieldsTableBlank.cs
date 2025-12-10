using Mekashron.Repository.ApiFriendlyBlanks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mekashron.Domain
{
    public class CustomFieldsTableBlank: IApiRequestBlank
    {
        public Int32? OlEntityId { get; set; }
        public String? OlUsername { get; set; }
        public String? OlPassword { get; set; }
        public Int32 TableId { get; set; }
        public Int32 RecordId { get; set; }
        public Int32 EntityId { get; set; }
        public String? QueryString { get; set; }
        public DateTime CurrentDateTimeUTC { get; set; }
        public String? VisitorIp { get; set; }

        public string GetXmlFieldTags()
        {
            return $@"<ns1:CustomFields_Tables_Update env:encodingStyle=""http://www.w3.org/2003/05/soap-encoding"">
                            <ol_EntityID xsi:type=""xsd:int"">{this.OlEntityId}</ol_EntityID>
                            <ol_Username xsi:type=""xsd:string"">{this.OlUsername}</ol_Username>
                            <ol_Password xsi:type=""xsd:string"">{this.OlPassword}</ol_Password>
                            <TableID xsi:type=""xsd:int"">{this.TableId}</TableID>
                            <RecordID xsi:type=""xsd:int"">{this.RecordId}</RecordID>
                            <NamesArray 
                                enc:itemType=""xsd:string"" 
                                enc:arraySize=""3"" 
                                xsi:type=""ns2:ArrayOfString""
                            >
                                <item xsi:type=""xsd:string"">CustomField100</item>
                                <item xsi:type=""xsd:string"">CustomField101</item>
                                <item xsi:type=""xsd:string"">CustomField102</item>
                            </NamesArray>
                            <ValuesArray 
                                enc:itemType=""xsd:string"" 
                                enc:arraySize=""3"" 
                                xsi:type=""ns2:ArrayOfString""
                            >
                                <item xsi:type=""xsd:string"">{this.QueryString}</item>
                                <item xsi:type=""xsd:string"">{this.CurrentDateTimeUTC}</item>
                                <item xsi:type=""xsd:string"">{this.VisitorIp}</item>
                            </ValuesArray>
                
                        </ns1:CustomFields_Tables_Update>";
        }
    }
}
