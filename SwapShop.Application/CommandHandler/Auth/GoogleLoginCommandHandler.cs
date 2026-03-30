using MediatR;
using Microsoft.Extensions.Configuration;
using SwapShop.Application.Commands.Auth;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;


namespace SwapShop.Application.CommandHandler.Auth
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, ResponseDto<LoginResultDto>>
    {
        private readonly IAccountService _accountService;
        private readonly IEncryptionService _encryptionService;
        private readonly IConfiguration _configuration;

        public GoogleLoginCommandHandler(IAccountService accountService, IEncryptionService encryptionService, IConfiguration configuration)
        {
            _accountService = accountService;
            _encryptionService = encryptionService;
            _configuration = configuration;
        }

        public async Task<ResponseDto<LoginResultDto>> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            var decryptedPassword = _encryptionService.Decrypt(_configuration["EncryptionSettings:GenPass"]);
            return await _accountService.GoogleLoginAsync(request.IdToken, decryptedPassword);
        }
    }

}

