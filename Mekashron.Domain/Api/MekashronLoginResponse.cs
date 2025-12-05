namespace Mekashron.Domain.Api
{
    public class MekashronLoginResponse
    {
        public Int32 ResultCode { get; set; }
        public String ResultMessage { get; set; }

        public Int32 EntityId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Company { get; set; }
        public String Address { get; set; }
        public String City { get; set; }
        public String Country { get; set; }
        public String Zip { get; set; }
        public String Phone { get; set; }
        public String Mobile { get; set; }
        public String Email { get; set; }
        public Int32 EmailConfirm { get; set; }
        public Int32 MobileConfirm { get; set; }
        public Int32 CountryID { get; set; }
        public Int32 Status { get; set; }
        public String lid { get; set; }
        public String FTPHost { get; set; }
        public Int32 FTPPort { get; set; }
    }
}
