using Mekashron.Domain.Api;
using Mekashron.Tools;

namespace Mekashron.Domain.Services
{
    public interface IMekashronApiService
    {
        Task<Result<MekashronLoginResponse>> Login(LoginBlank blank);
        Task<Result<MekashronRegisterResponse>> RegisterNewCustomer(CustomerBlank blank);
        Task<Result<CustomTableResponse>> SaveLog(CustomFieldsTableBlank blank);
    }
}
