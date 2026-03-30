using Swap_Shop.Domain.Entities;
using SwapShop.Domain.Dtos.Request.Auth;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Auth;
using SwapShop.Domain.Enitities;

namespace SwapShop.Domain.OtherService.Interface
{
    public interface IAccountService
    {

        Task<ResponseDto<BankAccount>> GetUserAccountDetails(string userId);
        Task<ResponseDto<string>> UpdateUser(string id, UpdateUserDto updateUser);
        Task<ResponseDto<string>> DeleteUser(string email);
        Task<ResponseDto<string>> RegisterUser(SignUp signUp, string Role);
        Task<ResponseDto<LoginResultDto>> LoginUser(SignInModel signIn);
        Task<ResponseDto<string>> ChangeOnlineStatus(string userId, bool IsOnline);
        Task<ResponseDto<UserInfo>> UserInfoAsync(string userId);
        Task<ResponseDto<string>> ForgotPassword(string CompanyEmail);
        Task<ResponseDto<string>> ConfirmEmailAsync(int token, string email);
        Task<ResponseDto<string>> ResetPassword(ResetPassword resetPassword);
        Task<ResponseDto<string>> SuspendUserAsync(string useremail);
        Task<ResponseDto<string>> UnSuspendUserAsync(string useremail);
        Task<ResponseDto<ApplicationUser>> GetUserbyId(string userId);
        Task<ResponseDto<string>> UpdateUserRole(string id, string role);
        Task<ResponseDto<string>> ResetPasswordSignedInUser(string userid, string newPassword);
        Task<ResponseDto<LoginResultDto>> GoogleLoginAsync(string idToken, string genericPassword);
        Task<ResponseDto<string>> CreateAccountOrUpdate(string userId, BankAccountSavedRequestDto requestDto);

    }
}
