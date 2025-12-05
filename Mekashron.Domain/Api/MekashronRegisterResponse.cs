namespace Mekashron.Domain.Api
{
    public class MekashronRegisterResponse : MekashronResponse
    {
        public Int32 EntityId { get; }
        public String AffiliateResultCode { get; }
        public String AffiliateResultMessage { get; }

        public MekashronRegisterResponse(
            String code,
            String message,
            Int32 entityId,
            String affiliateResultCode,
            String affilateResultMessage
        ) : base(code, message)
        {
            EntityId = entityId;
            AffiliateResultCode = affiliateResultCode;
            AffiliateResultMessage = affilateResultMessage;
        }
    }
}
