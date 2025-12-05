using Mekashron.Domain;
using Mekashron.Domain.Api;
using Mekashron.Domain.Repositories;
using Mekashron.Domain.Services;
using Mekashron.Tools;

namespace Mekashron.Services.Login
{
    public class MekashronApiService : IMekashronApiService
    {
        readonly IMekashronApiRepository _mekashronApiRepository;

        public MekashronApiService(
            IMekashronApiRepository mekashronApiRepository
        )
        {
            _mekashronApiRepository = mekashronApiRepository;
        }

        public async Task<Result<MekashronLoginResponse>> Login(LoginBlank blank)
        {
            if (blank.Username.IsWhiteSpace()) return Result<MekashronLoginResponse>.Failure("Имя пользователя не заполнено");

            if (blank.Password.IsWhiteSpace()) return Result<MekashronLoginResponse>.Failure("Пароль не введен");
            //if (blank.Password.Length < 8) return Result<MekashronLoginResponse>.Failure("Пароль должен содержать хотя бы 8 символов");

            Task<MekashronLoginResponse> response = _mekashronApiRepository.Login(blank);
            return Result<MekashronLoginResponse>.Success(await response);
        }

        public MekashronRegisterResponse RegisterNewCustomer(CustomerBlank blank)
        {
            return _mekashronApiRepository.RegisterNewCustomer(blank);
        }
    }
}
