using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Payment;
using SwapShop.Domain.Enitities;
using SwapShop.Domain.Enum;
using SwapShop.Domain.OtherService.Interface;
using SwapShop.Domain.Repository.Interface;

namespace SwapShop.Infrastructure.OtherService.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> _logger;
        private readonly IPaystackService _paystackService;
        private readonly IConfiguration _configuration;
        private readonly IAccountRepo _accountRepo;
        private readonly ISwapShopGenericRepo<SwappingProceeding> _swappingProceedingRepo;
        private readonly ISwapShopGenericRepo<Payments> _paymentsRepo;
        public PaymentService(ILogger<PaymentService> logger,
            IPaystackService paystackService, IConfiguration configuration,
            ISwapShopGenericRepo<SwappingProceeding> swappingProceedingRepo,
            IAccountRepo accountRepo, ISwapShopGenericRepo<Payments> paymentsRepo)
        {
            _logger = logger;
            _paystackService = paystackService;
            _configuration = configuration;
            _swappingProceedingRepo = swappingProceedingRepo;
            _accountRepo = accountRepo;
            _paymentsRepo = paymentsRepo;
        }


        public async Task<ResponseDto<PaystackInitializeResponse>> IntializePaymentForPaystack(string proceedingId, string userid, bool isChargeFee, string roomName)
        {

            var chargeFee = int.Parse(_configuration["AdvanceChargeFee"]);



            var response = new ResponseDto<PaystackInitializeResponse>();
            try
            {
                var retrieveProceed = await _swappingProceedingRepo.GetQueryable().Include(u=>u.List).FirstOrDefaultAsync(u => u.Id == proceedingId);
                if (retrieveProceed == null)
                {
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string>() { "Swap Proceed is invalid" };
                    return response;
                }
                var getUserInfo = await _accountRepo.FindUserByIdAsync(userid);
                if (getUserInfo == null)
                {
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string>() { "Invalid user" };
                    return response;
                }
                Random random = new Random();
                var randomNumber = random.Next(1000000, 1000000000);
                var reference = randomNumber.ToString();
                var description = String.Empty;
               
                if (isChargeFee)
                {
                    var callBackCharge = _configuration["CallbackUrl:AdvanceChargeFeeCallbackUrl"] + $"?roomName={roomName}";
                    var checkProceedForCurrentNegotiation = await _swappingProceedingRepo
     .GetQueryable()
     .FirstOrDefaultAsync(u =>
         u.ListId == retrieveProceed.ListId
         && u.Status != SwapProceedingStatus.Negotiation.ToString()
         && u.Status != SwapProceedingStatus.Closed.ToString()
     );

                    if (checkProceedForCurrentNegotiation != null)
                    {
                        response.StatusCode = 400;
                        response.DisplayMessage = "Error";
                        response.ErrorMessages = new List<string>
    {
        $"Swap proceeding for this item is currently in '{checkProceedForCurrentNegotiation.Status}' state and is not available for charging a fee."
    };
                        return response;
                    }

                    description = $"Charges payment for advance mode";
                    
                    var makeChargePaymentInitiation = await _paystackService.InitializeTransactionAsync(chargeFee, getUserInfo.Email, reference, callBackCharge);

                    if (makeChargePaymentInitiation.Status != true)
                    {
                        response.StatusCode = 400;
                        response.DisplayMessage = "Error";
                        response.ErrorMessages = new List<string> {makeChargePaymentInitiation.Message};
                        return response;
                    }
                    await _paymentsRepo.Add(new Payments()
                    {
                        Amount = chargeFee.ToString(),
                        Description = description,
                        OrderReferenceId = reference,
                        PaymentChannel = "Paystack",
                        PaymentType = "Debit",
                        UserId = userid,
                        RoomName = roomName,
                        FeeType="Advance Holding Charge",
                        SwapId= proceedingId
                    });
                    await _paymentsRepo.SaveChanges();
                    response.StatusCode = StatusCodes.Status200OK;
                    response.DisplayMessage = "Successful";
                    response.Result = makeChargePaymentInitiation;
                    return response;

                }
                if(retrieveProceed.Status != SwapProceedingStatus
                    .AwaitingVendorHoldingFee.ToString())
                {
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string>() { "Swap proceed not available in mode to initiate advance payment for holding fee" };
                    return response;
                }

                description = $"Holding Charges payment for advance mode";
                var callBackHolding = _configuration["CallbackUrl:HoldingFeeCallbackUrl"] + $"?roomName={roomName}";
                var makeHoldingPaymentInitiation = await _paystackService.InitializeTransactionAsync(int.Parse(retrieveProceed.List.EstimatedAmount.ToString()), getUserInfo.Email, reference, callBackHolding);
                await _paymentsRepo.Add(new Payments()
                {
                    Amount = retrieveProceed.List.EstimatedAmount.ToString(),
                    Description = description,
                    OrderReferenceId = reference,
                    PaymentChannel = "Paystack",
                    PaymentType = "Debit",
                    UserId = userid,
                    FeeType = "Advance Holding Fee",
                    SwapId = proceedingId,RoomName=roomName
                });
                await _paymentsRepo.SaveChanges();
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Successful";
                response.Result = makeHoldingPaymentInitiation;
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Failed to initiate payment on paystack" };
                response.DisplayMessage = $"Error";
                response.StatusCode = StatusCodes.Status500InternalServerError;
                return response;
            }
        }
        public async Task<ResponseDto<string>> ConfirmPaystackpayment(string reference,string userid)
        {
            var response = new ResponseDto<string>();
          
            try
            {
                var retrieveOrder = await _paymentsRepo.GetQueryable().Include(u=>u.Swap).
                    FirstOrDefaultAsync(u => u.OrderReferenceId == reference && u.UserId == userid);
                if (retrieveOrder == null)
                {
                    response.ErrorMessages = new List<string>() {
                        "Invalid reference" };
                    response.DisplayMessage = $"Error";
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }
               
                if (retrieveOrder.IsActive == false)
                {
                    response.ErrorMessages = new List<string> { "Invalid Transaction" };
                    response.DisplayMessage = "Error";
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                var checkPaystackPayment = await _paystackService.VerifyTransactionAsync(reference);
                if (!checkPaystackPayment)
                {
                    response.ErrorMessages = new List<string>() {
                        "Invalid transaction reference from paystack" };
                    response.DisplayMessage = $"Error";
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }
                if (true)
                {
                    retrieveOrder.IsActive = false;
                    retrieveOrder.PaymentStatus = "Paid";
                    retrieveOrder.CompletePaymentTime = DateTime.UtcNow;
                    if(retrieveOrder.FeeType== "Advance Holding Charge")
                    {
                        retrieveOrder.Swap.Status= SwapProceedingStatus.AwaitingVendorHoldingFee.ToString();
                    }
                    else
                    {
                        retrieveOrder.Swap.Status= SwapProceedingStatus.AdvNegotiation.ToString();
                    }
                    _swappingProceedingRepo.Update(retrieveOrder.Swap);
                    _paymentsRepo.Update(retrieveOrder);

                    

                    await _paymentsRepo.SaveChanges();
                    response.StatusCode = StatusCodes.Status200OK;
                    response.DisplayMessage = "Successful";
                    response.Result = "Payment Successfully completed";
                    return response;

                }
                retrieveOrder.CompletePaymentTime = DateTime.UtcNow;
                retrieveOrder.IsActive = false;
                retrieveOrder.PaymentStatus = "Faied";
                _paymentsRepo.Update(retrieveOrder);
                await _paymentsRepo.SaveChanges();
                response.ErrorMessages = new List<string> { "Invalid stripe Transaction" };
                response.DisplayMessage = "Error";
                response.StatusCode = StatusCodes.Status400BadRequest;
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in validating transaction" };
                response.DisplayMessage = $"Error";
                response.StatusCode = StatusCodes.Status500InternalServerError;
                return response;
            }




        }
        public async Task<ResponseDto<List<PaystackBank>>> BankDetails ()
        {
            var response = new ResponseDto<List<PaystackBank>>();
          
            try
            {
                var result = await _paystackService.GetAllBank();
                response.StatusCode = 200;
                response.Result = result.data;
                response.DisplayMessage = "Success";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in getting all banks" };
                response.DisplayMessage = $"Error";
                response.StatusCode = StatusCodes.Status500InternalServerError;
                return response;
            }




        }


    }
}
