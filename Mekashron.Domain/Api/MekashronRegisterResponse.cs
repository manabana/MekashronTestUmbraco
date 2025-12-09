namespace Mekashron.Domain.Api
{
    public class MekashronRegisterResponse
    {
        public Int32 ResultCode { get; set; }
        public String? ResultMessage { get; set; }
        public Int32? EntityId { get; set; }
        public String? DownloadUrl { get; set; }
    }
}
