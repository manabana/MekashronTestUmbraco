namespace Mekashron.Tools
{
    public class Error
    {
        public String Message { get; }
        public String? Key { get; }

        public Error(String message, String? key = null)
        {
            Message = message;
            Key = key;
        }
    }
}
