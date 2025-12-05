using Mekashron.Domain.Api;
using Mekashron.Tools;

namespace Mekashron.Domain.Services
{
    public interface IMekashronApiService
    {
        Task<Result<MekashronLoginResponse>> Login(LoginBlank blank);
        MekashronRegisterResponse RegisterNewCustomer(CustomerBlank blank);
    }
}
