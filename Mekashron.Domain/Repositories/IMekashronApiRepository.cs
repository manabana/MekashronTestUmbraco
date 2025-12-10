using Mekashron.Domain.Api;

namespace Mekashron.Domain.Repositories
{
    public interface IMekashronApiRepository
    {
        Task<MekashronLoginResponse> Login(LoginBlank blank);
        Task<MekashronRegisterResponse> RegisterNewCustomer(CustomerBlank blank);
        Task<CustomTableResponse> SaveLog(CustomFieldsTableBlank blank);
    }
}
