namespace Mekashron.Domain.Api
{
    public abstract class MekashronResponse
    {
        public String ResultCode { get; }
        public String ResultMessage { get; }

        protected MekashronResponse(String code, String message)
        {
            ResultCode = code;
            ResultMessage = message;
        }
    }
}
