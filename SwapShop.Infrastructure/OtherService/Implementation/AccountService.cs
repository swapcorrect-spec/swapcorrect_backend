using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Swap_Shop.Domain.Entities;
using SwapShop.Domain.Dtos.Request.Auth;
using SwapShop.Domain.Dtos.Request.Mailing;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Auth;
using SwapShop.Domain.Enitities;
using SwapShop.Domain.Enum;
using SwapShop.Domain.OtherService.Interface;
using SwapShop.Domain.Repository.Interface;

namespace SwapShop.Infrastructure.OtherService.Implementation
{
    public class AccountService : IAccountService
    {

        private readonly IAccountRepo _accountRepo;
        private readonly IEmailServices _emailServices;
        private readonly IActivityLogRepo _activityLogRepo;
        private readonly ISwapShopGenericRepo<ForgetPasswordToken> _forgetPasswordTokenRepo;
        private readonly ILogger<AccountService> _logger;
        private readonly IMapper _mapper;
        private readonly ISwapShopGenericRepo<User_Review_Rating> _user_Review_RatingRepo;
        private readonly ISwapShopGenericRepo<SwappingProceeding> _swappingProceedingRepo;
        private readonly ISwapShopGenericRepo<BankAccount> _bankAccountRepo;
        private readonly IGenerateJwt _generateJwt;
        private readonly IEncryptionService _encryption;
        private readonly IConfiguration _configuration;

        public AccountService(
            IAccountRepo accountRepo,
            ILogger<AccountService> logger,
            IGenerateJwt generateJwt,
            IEmailServices emailServices,
            IActivityLogRepo activityLogRepo,
            IMapper mapper,
            ISwapShopGenericRepo<ForgetPasswordToken> forgetPasswordTokenRepo,
            IEncryptionService encryption,
            IConfiguration configuration,
            ISwapShopGenericRepo<SwappingProceeding> swappingProceedingRepo,
            ISwapShopGenericRepo<User_Review_Rating> user_Review_RatingRepo,
            ISwapShopGenericRepo<BankAccount> bankAccountRepo)
        {
            _accountRepo = accountRepo;
            _logger = logger;
            _generateJwt = generateJwt;
            _emailServices = emailServices;
            _activityLogRepo = activityLogRepo;
            _forgetPasswordTokenRepo = forgetPasswordTokenRepo;
            _mapper = mapper;
            _configuration = configuration;
            _encryption = encryption;
            _swappingProceedingRepo = swappingProceedingRepo;
            _user_Review_RatingRepo = user_Review_RatingRepo;
            _bankAccountRepo = bankAccountRepo;
        }

        public async Task<ResponseDto<string>> RegisterUser(SignUp signUp, string Role)
        {
            var response = new ResponseDto<string>();
            try
            {
                var checkUserExist = await _accountRepo.FindUserByEmailAsync(signUp.Email);
                if (checkUserExist != null)
                {
                    response.ErrorMessages = new List<string>() { "User with the email already exist" };
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    return response;
                }
                var checkUserNameExist = await _accountRepo.FindUserByuSERNAMEAsync(signUp.UserName);
                if (checkUserNameExist != null)
                {
                    response.ErrorMessages = new List<string>() { "User with the username already exist" };
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    return response;
                }
                var checkRole = await _accountRepo.RoleExist(Role);
                if (checkRole == false)
                {
                    response.ErrorMessages = new List<string>() { "Role is not available" };
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.DisplayMessage = "Error";
                    return response;
                }
                var mapAccount = new ApplicationUser();

                mapAccount.FirstName = signUp.FirstName;
                mapAccount.LastName = signUp.LastName;
                mapAccount.Country = signUp.Country;
                mapAccount.Email = signUp.Email;
                mapAccount.PhoneNumber = signUp.PhoneNumber;
                mapAccount.UserName = signUp.UserName;
                mapAccount.DeliveryAddress = signUp.DeliveryAddress;
                mapAccount.City = signUp.City;
                mapAccount.Gender = signUp.Gender;
                mapAccount.State = signUp.State;
                mapAccount.Country = signUp.Country;


                var createUser = await _accountRepo.SignUpAsync(mapAccount, signUp.Password);
                if (createUser == null)
                {
                    response.ErrorMessages = new List<string>() { "User not created successfully" };
                    response.StatusCode = StatusCodes.Status501NotImplemented;
                    response.DisplayMessage = "Error";
                    return response;
                }
                var addRole = await _accountRepo.AddRoleAsync(createUser, Role);
                if (addRole == false)
                {
                    response.ErrorMessages = new List<string>() { "Fail to add role to user" };
                    response.StatusCode = StatusCodes.Status501NotImplemented;
                    response.DisplayMessage = "Error";
                    return response;
                }
                var GenerateConfirmEmailToken = new ConfirmEmailToken()
                {
                    Token = _accountRepo.GenerateConfirmEmailToken(),
                    UserId = createUser.Id
                };
                var Generatetoken = await _accountRepo.SaveGenerateConfirmEmailToken(GenerateConfirmEmailToken);
                if (Generatetoken == null)
                {
                    response.ErrorMessages = new List<string>() { "Fail to generate confirm email token for company" };
                    response.StatusCode = StatusCodes.Status501NotImplemented;
                    response.DisplayMessage = "Error";
                    return response;
                }
              

                var baseUrl = $"{_configuration["FrontendBaseUrl"]}verify?token={GenerateConfirmEmailToken.Token}&email={createUser.Email}";

                var body = $@"
                                <!DOCTYPE html>
                                       <html>
                                        <head>
                                        <meta charset=""UTF-8"" />
                                        <title>Email Confirmation</title>
                                        <style>
                                            body {{
                                              font-family: Arial, sans-serif;
                                              background-color: #f9f9f9;
                                              color: #333;
                                              padding: 20px;
                                            }}
                                            .container {{
                                              background-color: #fff;
                                              border-radius: 8px;
                                              padding: 20px;
                                              max-width: 600px;
                                              margin: 0 auto;
                                              box-shadow: 0 2px 8px rgba(0,0,0,0.1);
                                            }}
                                            .btn {{
                                              display: inline-block;
                                              background-color: #007BFF;
                                              color: #fff !important;
                                              padding: 10px 20px;
                                              margin-top: 20px;
                                              border-radius: 5px;
                                              text-decoration: none;
                                              font-weight: bold;
                                            }}
                                            .btn:hover {{
                                              background-color: #0056b3;
                                            }}
                                            p {{
                                              line-height: 1.5;
                                            }}
                                          </style>
                                        </head>
                                        <body>
                                          <div class=""container"">
                                            <h2>Email Confirmation</h2>
                                            <p>Hello {createUser.FirstName},</p>
                                            <p>Thank you for registering. Please confirm your email address by clicking the button below:</p>
                                            <p>
                                              <a href=""{baseUrl}"" class=""btn"">Confirm Email</a>
                                            </p>
                                            <p>If the button above doesn’t work, copy and paste the following link into your browser:</p>
                                            <p><a href=""{baseUrl}"">{baseUrl}</a></p>
                                            <p>Best regards,<br/>The Team</p>
                                          </div>
                                        </body>
                                        </html>";

                var message = new Message(
                    new[] { createUser.Email },
                    "Confirm Your Email",
                    body
                );

                _emailServices.SendEmail(message);

                await _activityLogRepo.AddActivitylog(createUser.Id, "Sign Up", "Register as a new user");
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Successful";
                response.Result = "User successfully created";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in resgistering the user" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }

        public async Task<ResponseDto<string>> UpdateUserRole(string id, string role)
        {
            var response = new ResponseDto<string>();
            try
            {
                var findUser = await _accountRepo.FindUserByIdAsync(id);
                if (findUser == null)
                {
                    response.ErrorMessages = new List<string>() { "There is no user with the email provided" };
                    response.StatusCode = 404;
                    response.DisplayMessage = "Error";
                    return response;
                }
                var checkRole = await _accountRepo.RoleExist(role);
                if (checkRole == false)
                {
                    response.ErrorMessages = new List<string>() { "Role is not available" };
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.DisplayMessage = "Error";
                    return response;
                }
                var getExistingRoles = await _accountRepo.GetUserRoles(findUser);
                if (getExistingRoles.Count > 0)
                {
                    var removeExistingRoles = await _accountRepo.RemoveRoleAsync(findUser, getExistingRoles);
                    if (removeExistingRoles == false)
                    {
                        response.ErrorMessages = new List<string>() { "Error in removing role for user" };
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        response.DisplayMessage = "Error";
                        return response;
                    }
                }

                var addRole = await _accountRepo.AddRoleAsync(findUser, role);
                if (addRole == false)
                {
                    response.ErrorMessages = new List<string>() { "Fail to add role to user" };
                    response.StatusCode = StatusCodes.Status501NotImplemented;
                    response.DisplayMessage = "Error";
                    return response;
                }
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Successful";
                response.Result = "User role updated successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in updating user role" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }
        public async Task<ResponseDto<LoginResultDto>> LoginUser(SignInModel signIn)
        {
            var response = new ResponseDto<LoginResultDto>();
            try
            {
                var checkUserExist = await _accountRepo.FindUserByEmailAsync(signIn.Email);
                if (checkUserExist == null)
                {
                    response.ErrorMessages = new List<string>() { "There is no user with the email provided" };
                    response.StatusCode = 404;
                    response.DisplayMessage = "Error";
                    return response;
                }
                if (checkUserExist.IsSuspend == true)
                {
                    response.ErrorMessages = new List<string>() { "User is suspended, contact admin" };
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    return response;
                }

                var checkPassword = await _accountRepo.CheckAccountPassword(checkUserExist, signIn.Password);
                if (checkPassword == false)
                {
                    response.ErrorMessages = new List<string>() { "Invalid Email or Password" };
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    return response;
                }
               /* if (!checkUserExist.EmailConfirmed)
                {
                    response.ErrorMessages = new List<string>() { "Please confirm your email address" };
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    return response;

                }*/
                checkUserExist.LastLoginTime = DateTime.UtcNow;
                await _accountRepo.UpdateUserInfo(checkUserExist);
                await _activityLogRepo.AddActivitylog(checkUserExist.Id, "Sign in", "Sign in to the platform");
                var generateToken = await _generateJwt.GenerateToken(checkUserExist);
                if (generateToken == null)
                {
                    response.ErrorMessages = new List<string>() { "Error in generating jwt for user" };
                    response.StatusCode = 501;
                    response.DisplayMessage = "Error";
                    return response;
                }

                var getUserRole = await _accountRepo.GetUserRoles(checkUserExist);
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Successfully login";
                response.Result = new LoginResultDto() { Jwt = generateToken, UserRole = getUserRole };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in login the user" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }

        public async Task<ResponseDto<string>> ChangeOnlineStatus(string userId, bool IsOnline)
        {

            var response = new ResponseDto<string>();
            try
            {
                var User = await _accountRepo.FindUserByIdAsync(userId);

                if (IsOnline)
                {
                    User.IsOnline = true;
                    User.LastSeen = null;
                }
                else
                {
                    User.IsOnline = false;
                    User.LastSeen = DateTime.UtcNow.ToString("d/M/yyyy h:mm tt");
                }
                await _accountRepo.UpdateUserInfo(User);


                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Success";
                response.Result = $"User status set to {IsOnline}";
                return response;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in changing user status" };
                response.StatusCode = 501;
                response.DisplayMessage = "Error";
                return response;
            }
        }
        public async Task<ResponseDto<UserInfo>> UserInfoAsync(string userId)
        {
            var response = new ResponseDto<UserInfo>();
            try
            {
                var fetchUser = await _accountRepo.FindUserByIdFullinfoAsync(userId);
                if (fetchUser == null)
                {
                    response.ErrorMessages = new List<string>() { "Invalid user" };
                    response.DisplayMessage = "Error";
                    response.StatusCode = 400;
                    return response;
                }

                var getUserRole = await _accountRepo.GetUserRoles(fetchUser);

              

                var getSwapping = await _swappingProceedingRepo.GetQueryable().Include(u => u.List).Where(u => u.Userid == userId || u.List.UserId == userId).ToListAsync();
                int listingCount=0;
                int swappCount =0;
                if (getSwapping.Any())
                {
                    swappCount = getSwapping.Where(u => u.Status == SwapProceedingStatus.Swapped.ToString()).Count();
                    listingCount = getSwapping.Where(u => u.List.UserId == userId).Count();
                }
                var rateUser = await _user_Review_RatingRepo.GetQueryable()
                 .AsNoTracking().Where(u => u.UserId == userId).ToListAsync();
                double avgRate = 0;
                if (rateUser.Any())
                {
                    avgRate = rateUser.Average(r => (double)r.RateScore);
                }
                
                var result = new UserInfo()
                {
                    Id = fetchUser.Id,
                    Email = fetchUser.Email,
                    UserName = fetchUser.UserName,
                    FirstName = fetchUser.FirstName,
                    LastName = fetchUser.LastName,
                    Country = fetchUser.Country,
                    PhoneNumber = fetchUser.PhoneNumber,
                    ProfilePicture = fetchUser.ProfilePicture,
                    City = fetchUser.City,
                    DeliveryAddress = fetchUser.DeliveryAddress,
                    Gender = fetchUser.Gender,
                    IsSuspendUser = fetchUser.IsSuspend,
                    State = fetchUser.State,
                    IsEmailConfirmed = fetchUser.EmailConfirmed,
                    IsTwoFactorEnable = fetchUser.IsTwoFactorEnable,
                    IsFlag = fetchUser.IsFlag,
                    LastLoginTime = fetchUser.LastLoginTime,
                    Created = fetchUser.Created,
                    UserRole = getUserRole,
                    Rating = avgRate,
                    SwapCount = swappCount,
                    ListingCount = listingCount

                };


                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Success";
                response.Result = result;
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in getting user info" };
                response.StatusCode = 501;
                response.DisplayMessage = "Error";
                return response;
            }
        }

        public async Task<ResponseDto<string>> ResetPassword(ResetPassword resetPassword)
        {
            var response = new ResponseDto<string>();
            try
            {
                var findUser = await _accountRepo.FindUserByEmailAsync(resetPassword.Email);
                if (findUser == null)
                {
                    response.ErrorMessages = new List<string>() { "There is no user with the email provided" };
                    response.StatusCode = 404;
                    response.DisplayMessage = "Error";
                    return response;
                }
                var retrieveToken = await _forgetPasswordTokenRepo.GetQueryable().FirstOrDefaultAsync(u => u.userid == findUser.Id);
                if (retrieveToken == null)
                {
                    response.ErrorMessages = new List<string>() { "invalid user token" };
                    response.StatusCode = 404;
                    response.DisplayMessage = "Error";
                    return response;
                }
                resetPassword.Token = retrieveToken.gentoken;
                var resetPasswordAsync = await _accountRepo.ResetPasswordAsync(findUser, resetPassword);
                if (resetPasswordAsync == null)
                {
                    response.ErrorMessages = new List<string>() { "Invalid token" };
                    response.DisplayMessage = "Error";
                    response.StatusCode = 400;
                    return response;
                }

                _forgetPasswordTokenRepo.Delete(retrieveToken);
                await _forgetPasswordTokenRepo.SaveChanges();
                await _activityLogRepo.AddActivitylog(findUser.Id, "Reset Password", "Reset your new password");
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Success";
                response.Result = "Successfully reset user password";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in reset user password" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }
        public async Task<ResponseDto<string>> ResetPasswordSignedInUser(string userid, string newPassword)
        {
            var response = new ResponseDto<string>();
            try
            {
                var findUser = await _accountRepo.FindUserByIdAsync(userid);
                if (findUser == null)
                {
                    response.ErrorMessages = new List<string>() { "There is no user with the userid provided" };
                    response.StatusCode = 404;
                    response.DisplayMessage = "Error";
                    return response;
                }
                var result = await _accountRepo.ForgotPassword(findUser);
                if (result == null)
                {
                    response.ErrorMessages = new List<string>() { "Error in generating reset token for user" };
                    response.StatusCode = 501;
                    response.DisplayMessage = "Error";
                    return response;
                }
                var resetPassword = new ResetPassword()
                {
                    Email = findUser.Email,
                    Password = newPassword,
                    Token =result
                };
             
                var resetPasswordAsync = await _accountRepo.ResetPasswordAsync(findUser, resetPassword);
                if (resetPasswordAsync == null)
                {
                    response.ErrorMessages = new List<string>() { "Invalid token" };
                    response.DisplayMessage = "Error";
                    response.StatusCode = 400;
                    return response;
                }

                await _activityLogRepo.AddActivitylog(findUser.Id, "Reset Password", "Reset your new password");
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Success";
                response.Result = "Successfully reset user password";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in reset user password" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }

        public async Task<ResponseDto<string>> ConfirmEmailAsync(int token, string email)
        {
            var response = new ResponseDto<string>();
            try
            {
                var findUser = await _accountRepo.FindUserByEmailAsync(email);
                if (findUser == null)
                {
                    response.ErrorMessages = new List<string>() { "There is no user with the email provided" };
                    response.StatusCode = 404;
                    response.DisplayMessage = "Error";
                    return response;
                }
                var retrieveToken = await _accountRepo.retrieveUserToken(findUser.Id);
                if (retrieveToken == null)
                {
                    response.ErrorMessages = new List<string>() { "Error user token " };
                    response.DisplayMessage = "Error";
                    response.StatusCode = 400;
                    return response;
                }
                if (retrieveToken.Token != token)
                {
                    response.ErrorMessages = new List<string>() { "Invalid user token" };
                    response.DisplayMessage = "Error";
                    response.StatusCode = 400;
                    return response;
                }
                var deleteToken = await _accountRepo.DeleteUserToken(retrieveToken);
                if (deleteToken == false)
                {
                    response.ErrorMessages = new List<string>() { "Error removing user token" };
                    response.DisplayMessage = "Error";
                    response.StatusCode = 400;
                    return response;
                }
                findUser.EmailConfirmed = true;
                var updateUserConfirmState = await _accountRepo.UpdateUserInfo(findUser);
                if (updateUserConfirmState == false)
                {
                    response.ErrorMessages = new List<string>() { "Error in confirming user token" };
                    response.DisplayMessage = "Error";
                    response.StatusCode = 400;
                    return response;
                }
                await _activityLogRepo.AddActivitylog(findUser.Id, "Confirm Email", "Confirm your email address");
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Success";
                response.Result = "Successfully comfirm user token";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in confirming user token" };
                response.StatusCode = 501;
                response.DisplayMessage = "Error";
                return response;
            }
        }
        public async Task<ResponseDto<string>> ForgotPassword(string Email)
        {
            var response = new ResponseDto<string>();
            try
            {
                var checkUser = await _accountRepo.FindUserByEmailAsync(Email);
                if (checkUser == null)
                {
                    response.ErrorMessages = new List<string>() { "Email is not available" };
                    response.StatusCode = 404;
                    response.DisplayMessage = "Error";
                    return response;
                }
                var result = await _accountRepo.ForgotPassword(checkUser);
                if (result == null)
                {
                    response.ErrorMessages = new List<string>() { "Error in generating reset token for user" };
                    response.StatusCode = 501;
                    response.DisplayMessage = "Error";
                    return response;
                }
                var retrieveToken = await _forgetPasswordTokenRepo.GetQueryable().FirstOrDefaultAsync(u => u.userid == checkUser.Id);
                if (retrieveToken != null)
                {
                    _forgetPasswordTokenRepo.Delete(retrieveToken);
                    await _forgetPasswordTokenRepo.SaveChanges();
                }
                var generateToken = _accountRepo.GenerateToken();
                var savetoken = await _forgetPasswordTokenRepo.Add(new ForgetPasswordToken()
                {
                    token = generateToken.ToString(),
                    gentoken = result,
                    userid = checkUser.Id
                });
                await _forgetPasswordTokenRepo.SaveChanges();
                var message = new Message(new string[] { checkUser.Email }, "Reset Password Code", $"<p>Your reset password code is below<p><br/><h6>{generateToken}</h6><br/> <p>Please use it in your reset password page</p>");
                _emailServices.SendEmail(message);
                await _activityLogRepo.AddActivitylog(checkUser.Id, "Forget Password", "Request for forget password");
                response.DisplayMessage = "Success";
                response.Result = "Reset password token sent to registered email";
                response.StatusCode = 200;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in generating reset token for user" };
                response.StatusCode = 501;
                response.DisplayMessage = "Error";
                return response;
            }
        }
        public async Task<ResponseDto<string>> SuspendUserAsync(string useremail)
        {
            var response = new ResponseDto<string>();
            try
            {
                var findUser = await _accountRepo.FindUserByEmailAsync(useremail);
                if (findUser == null)
                {
                    response.ErrorMessages = new List<string>() { "There is no user with the email provided" };
                    response.StatusCode = 404;
                    response.DisplayMessage = "Error";
                    return response;
                }
                findUser.IsSuspend = true;
                var updateUser = await _accountRepo.UpdateUserInfo(findUser);
                if (updateUser == false)
                {
                    response.ErrorMessages = new List<string>() { "Error in suspending user" };
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.DisplayMessage = "Error";
                    return response;
                }

                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Success";
                response.Result = "Successfully suspend user";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in suspending user" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }
        public async Task<ResponseDto<string>> UnSuspendUserAsync(string useremail)
        {
            var response = new ResponseDto<string>();
            try
            {
                var findUser = await _accountRepo.FindUserByEmailAsync(useremail);
                if (findUser == null)
                {
                    response.ErrorMessages = new List<string>() { "There is no user with the email provided" };
                    response.StatusCode = 404;
                    response.DisplayMessage = "Error";
                    return response;
                }
                findUser.IsSuspend = false;
                var updateUser = await _accountRepo.UpdateUserInfo(findUser);
                if (updateUser == false)
                {
                    response.ErrorMessages = new List<string>() { "Error in unsuspending user" };
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.DisplayMessage = "Error";
                    return response;
                }
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Success";
                response.Result = "Successfully unsuspend user";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in unsuspending user" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }

        public async Task<ResponseDto<string>> DeleteUser(string email)
        {
            var response = new ResponseDto<string>();
            try
            {
                var findUser = await _accountRepo.FindUserByEmailAsync(email);
                if (findUser == null)
                {
                    response.ErrorMessages = new List<string>() { "There is no user with the email provided" };
                    response.StatusCode = 404;
                    response.DisplayMessage = "Error";
                    return response;
                }
                var deleteUser = await _accountRepo.DeleteUserByEmail(findUser);
                if (deleteUser == false)
                {
                    response.ErrorMessages = new List<string>() { "Error in deleting user" };
                    response.StatusCode = 501;
                    response.DisplayMessage = "Error";
                    return response;
                }
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Success";
                response.Result = "Successfully delete user";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in deleting user" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }
        public async Task<ResponseDto<ApplicationUser>> GetUserbyId(string userId)
        {
            var response = new ResponseDto<ApplicationUser>();
            try
            {
                var findUser = await _accountRepo.FindUserByIdAsync(userId);
                if (findUser == null)
                {
                    response.ErrorMessages = new List<string>() { "There is no user with the id provided" };
                    response.StatusCode = 404;
                    response.DisplayMessage = "Error";
                    return response;
                }

                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Success";
                response.Result = findUser;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in getting user details" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }

        public async Task<ResponseDto<string>> UpdateUser(string id, UpdateUserDto updateUser)
        {
            var response = new ResponseDto<string>();
            try
            {
                var findUser = await _accountRepo.FindUserByIdAsync(id);
                if (findUser == null)
                {
                    response.ErrorMessages = new List<string>() { "There is no user with the id provided" };
                    response.StatusCode = 404;
                    response.DisplayMessage = "Error";
                    return response;
                }
                var mapUpdateDetails = _mapper.Map(updateUser, findUser);
                var updateUserDetails = await _accountRepo.UpdateUserInfo(mapUpdateDetails);
                if (updateUserDetails == false)
                {
                    response.ErrorMessages = new List<string>() { "Error in updating user info" };
                    response.StatusCode = StatusCodes.Status501NotImplemented;
                    response.DisplayMessage = "Error";
                    return response;
                }
                await _activityLogRepo.AddActivitylog(findUser.Id, "Update Details", "Some of your details have been updated");
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Success";
                response.Result = "Successfully update user information";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in updating user info" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }
        public async Task<ResponseDto<string>> CreateAccountOrUpdate(string userId, BankAccountSavedRequestDto requestDto)
        {
            var response = new ResponseDto<string>();
            try
            {
                var findUser = await _accountRepo.FindUserByIdAsync(userId);
                if (findUser == null)
                {
                    response.ErrorMessages = new List<string>() { "There is no user with the id provided" };
                    response.StatusCode = 404;
                    response.DisplayMessage = "Error";
                    return response;
                }
                var getAccountInfo = await _bankAccountRepo.GetQueryable().FirstOrDefaultAsync(u=>u.UserId == userId);
                if (getAccountInfo == null) {
                    await _bankAccountRepo.Add(new BankAccount()
                    {
                        UserId = userId,
                        AccountName = requestDto.AccountName,
                        AccountNumber = requestDto.AccountNumber,
                        BankCode = requestDto.BankCode,
                    });
                    await _bankAccountRepo.SaveChanges();

                    response.StatusCode = StatusCodes.Status200OK;
                    response.DisplayMessage = "Success";
                    response.Result = "Successfully update withdrawal account information";
                    return response;
                }
                getAccountInfo.AccountNumber= requestDto.AccountNumber;
                getAccountInfo.AccountName= requestDto.AccountName;
                getAccountInfo.BankCode= requestDto.BankCode;
                _bankAccountRepo.Update(getAccountInfo);
                await _bankAccountRepo.SaveChanges();

                await _activityLogRepo.AddActivitylog(findUser.Id, "Bank Account Details Update", "Some of your details have been updated");
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Success";
                response.Result = "Successfully update user information";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in updating user bank account info" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }
        public async Task<ResponseDto<BankAccount>> GetUserAccountDetails(string userId)
        {
            var response = new ResponseDto<BankAccount>();
            try
            {
                var findUser = await _accountRepo.FindUserByIdAsync(userId);
                if (findUser == null)
                {
                    response.ErrorMessages = new List<string>() { "There is no user with the id provided" };
                    response.StatusCode = 404;
                    response.DisplayMessage = "Error";
                    return response;
                }
                var getAccountInfo = await _bankAccountRepo.GetQueryable().FirstOrDefaultAsync(u=>u.UserId == userId);
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Success";
                response.Result = getAccountInfo;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in getting user bank account info" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }

        public async Task<ResponseDto<LoginResultDto>> GoogleLoginAsync(string idToken, string genericPassword)
        {
            var response = new ResponseDto<LoginResultDto>();

            try
            {
                var checkPassword = _encryption.Decrypt(_configuration["EncryptionSettings:GenPass"]).Equals(genericPassword);
                if (checkPassword == false)
                {
                    response.ErrorMessages = new List<string>() { "Invalid Credential" };
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    return response;
                }

                // Validate the Google token
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
                var email = payload.Email;
                var fullName = payload.Name.Split(" ");
                //var picture = payload.Picture;
                string LastName = fullName[1];
                string FirstName = fullName[0];


                var checkUserExist = await _accountRepo.FindUserByEmailAsync(email);
                if (checkUserExist == null)
                {
                    // Register new user
                    var newUser = new ApplicationUser
                    {
                        Email = email,
                        UserName = email,
                        FirstName = payload.GivenName,
                        LastName = payload.FamilyName,
                        Country = "US",
                        Gender = "Male",
                        DeliveryAddress = "",
                        ProfilePicture = payload.Picture
                    };

                    var createdUser = await _accountRepo.SignUpAsync(newUser, genericPassword);
                    if (createdUser == null)
                    {
                        response.StatusCode = 500;
                        response.DisplayMessage = "User creation failed";
                        response.ErrorMessages = new List<string> { "Could not register new user" };
                        return response;
                    }

                    var roleAssigned = await _accountRepo.AddRoleAsync(createdUser, "Visitor");
                    if (!roleAssigned)
                    {
                        response.StatusCode = 500;
                        response.DisplayMessage = "Failed to assign role";
                        response.ErrorMessages = new List<string> { "Role assignment failed" };
                        return response;
                    }

                    checkUserExist = createdUser;
                }

                var token = await _generateJwt.GenerateToken(checkUserExist);
                var roles = await _accountRepo.GetUserRoles(checkUserExist);

                response.Result = new LoginResultDto
                {
                    Jwt = token,
                    UserRole = roles
                };
                response.StatusCode = 200;
                response.DisplayMessage = "Login successful";

                await _activityLogRepo.AddActivitylog(checkUserExist.Id, "Sign Up", "Register as a new user");

                // var message = new Message(new string[] { checkUserExist.Email }, "Confirm Email Token", $"<p>Your confirm email code is below<p><h6>{GenerateConfirmEmailToken.Token}</h6>");
                // _emailServices.SendEmail(message);

            }
            catch (InvalidJwtException)
            {
                response.StatusCode = 401;
                response.DisplayMessage = "Invalid Google token";
                response.ErrorMessages = new List<string> { "Token verification failed" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Google login failed");
                response.StatusCode = 500;
                response.DisplayMessage = "Internal server error";
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }
    }
}
