using Mekashron.Domain;
using Mekashron.Domain.Api;
using Mekashron.Domain.Repositories;
using Mekashron.Domain.Services;
using Mekashron.Tools;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

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

        public async Task<Result<MekashronRegisterResponse>> RegisterNewCustomer(CustomerBlank blank)
        {
            blank.Password = GenerateStringNumber(6);

            Result validationResult = ValidateRegisterBlank(blank);
            if (!validationResult.IsSuccess) return Result<MekashronRegisterResponse>.Failure(validationResult.Error!);

            Task<MekashronRegisterResponse> response = 
                _mekashronApiRepository.RegisterNewCustomer(blank);

            if (response.Result.ResultCode is 0 or -5674) 
                response.Result.DownloadUrl = "/api/download";

            //TODO тут вызывать логгинг скачки

            return Result<MekashronRegisterResponse>.Success(await response);

        }

        private String GenerateStringNumber(Int32 length)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            var sb = new StringBuilder(length);

            for (Int32 i = 0; i < length; i++)
            {
                Int32 digit = RandomNumberGenerator.GetInt32(i == 0 ? 1 : 0, 10);
                sb.Append(digit);
            }

            return sb.ToString();
        }

        private Result ValidateRegisterBlank(CustomerBlank blank) 
        {
            if (blank.BusinessId is null) return Result.Failure("BisinessId is invalid", "BisinessIdIsInvalid");
            
            if (blank.OlUsername is null || blank.OlUsername.IsWhiteSpace()) return Result.Failure("Username is incorrect");
            if (Regex.IsMatch(blank.OlUsername, "^[0-9a-zA-Z]$")) return Result.Failure("Username contains forbidden characters (allowed symbols is a-Z, 0-9)", "UsernameCharacterIsInvalid");

            if (blank.OlPassword is null || blank.OlPassword.IsWhiteSpace()) return Result.Failure("");
            const Int32 minPasswordLenght = 6;
            if (blank.OlPassword.Length < minPasswordLenght) return Result.Failure($"Password must be 6 characters long", "PasswordLenghtIsInvalid");
            if (Regex.IsMatch(blank.OlPassword, @"^[a-zA-Z0-9!@#$^&*/]$")) return Result.Failure("Password contains forbidden characters (allowed symbols is a-Z, 0-9 and !@#$^&*/)", "PasswordCharacterIsInvalid");

            if (blank.OlEntityId is null) return Result.Failure("EntityId is Invalid", "EntityIdIsInvalid");
            
            if (blank.CategoryId is null) return Result.Failure("CategoryId is Invalid", "CategoryIdIsInvalid");
            
            if (blank.Email is null || !Regex.IsMatch(blank.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
                return Result.Failure("It's not email", "EmailIsInvalid");

            if (blank.CountryISO.IsWhiteSpace() && blank.CountryISO!.Length != 2) return Result.Failure("Country ISO is invalid", "CountryISOIsInvalid");

            Regex validatePhoneNumberRegex = new Regex("^\\+?[1-9][0-9]{7,14}$");
            if (blank.Phone is null || blank.Phone.IsWhiteSpace()) return Result.Failure("Phone number is empty", "PhoneNumberIsEmpty");
            if (!validatePhoneNumberRegex.IsMatch(blank.Phone)) return Result.Failure("Enter correct phone number", "PhoneNumberIsInvalid");

            return Result.Success();

        }

        public async Task<Result<CustomTableResponse>> SaveLog(CustomFieldsTableBlank blank)
        {
            Task<CustomTableResponse> response = _mekashronApiRepository.SaveLog(blank);
            return Result<CustomTableResponse>.Success(await response);
        }

    }
}
