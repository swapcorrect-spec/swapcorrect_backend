using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwapShop.Api.ResponsHandler;
using SwapShop.Application.Commands.Auth;
using SwapShop.Application.Commands.Payment;
using SwapShop.Application.Commands.Report;
using SwapShop.Application.Queries.Auth;
using SwapShop.Application.Queries.Payment;
using SwapShop.Domain.Dtos.Request.Payment;
using SwapShop.Domain.Dtos.Response.Auth;
using SwapShop.Domain.Enum;
using System.IdentityModel.Tokens.Jwt;

namespace SwapShop.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("paystack/initialize")]
        public Task<IActionResult> InitializePaystackPayment(InitializePayment req)
        {

            var userid = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var mapData = new IntializePaymentForPaystackCommand()
            {
                UserId = userid,
               IsChargeFee= req.IsChargeFee,
               RoomName = req.RoomName,
               ProceedingId = req.ProceedingId,
            };

            return MediatorResponseHelper.Handle(_mediator, mapData, this);
        }
        [HttpPost("paystack/confirm")]
        public Task<IActionResult> ConfirmPaystackPayment(VerifyPaystackPayment req)
        {

            var userid = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var mapData = new ConfirmPaystackPaymentCommand()
            {
                UserId = userid,
               Reference=req.Reference,
            };

            return MediatorResponseHelper.Handle(_mediator, mapData, this);
        }
        [HttpPost("paystack/banks")]
        public Task<IActionResult> GetAllBanks()
        {

            var userid = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var mapData = new BankDetailsQuery()
            {
            
            };

            return MediatorResponseHelper.Handle(_mediator, mapData, this);
        }
        [HttpPost("add_update/bank_account")]
        public Task<IActionResult> BankAccount(BankAccountSavedRequestDto req)
        {

            var userid = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var mapData = new CreateAccountOrUpdateCommand()
            {
                UserId = userid,
                requestDto = req,


            };

            return MediatorResponseHelper.Handle(_mediator, mapData, this);
        }
        [HttpGet("bank_account/info")]
        public Task<IActionResult> GetBankAccount()
        {

            var userid = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var mapData = new GetUserAccountDetailsQuery()
            {
                UserId = userid,



            };

            return MediatorResponseHelper.Handle(_mediator, mapData, this);
        }

        [HttpPost("withdrawal/submit")]
        public Task<IActionResult> SubmitWithdrawal(SubmitWithdrawalRequest req)
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var mapData = new SubmitWithdrawalCommand
            {
                UserId = userid,
                SwapId = req.SwapId
            };
            return MediatorResponseHelper.Handle(_mediator, mapData, this);
        }

        [HttpPost("advance-swap/complete")]
        public Task<IActionResult> CompleteAdvanceSwap(CompleteAdvanceSwapRequest req)
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var mapData = new CompleteAdvanceSwapCommand
            {
                UserId = userid,
                SwapId = req.SwapId
            };
            return MediatorResponseHelper.Handle(_mediator, mapData, this);
        }


    }
}