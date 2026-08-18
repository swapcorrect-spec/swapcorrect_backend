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
        private readonly ISwapShopGenericRepo<WithdrawalRequest> _withdrawalRequestRepo;
        public PaymentService(ILogger<PaymentService> logger,
            IPaystackService paystackService, IConfiguration configuration,
            ISwapShopGenericRepo<SwappingProceeding> swappingProceedingRepo,
            IAccountRepo accountRepo, ISwapShopGenericRepo<Payments> paymentsRepo,
            ISwapShopGenericRepo<WithdrawalRequest> withdrawalRequestRepo)
        {
            _logger = logger;
            _paystackService = paystackService;
            _configuration = configuration;
            _swappingProceedingRepo = swappingProceedingRepo;
            _accountRepo = accountRepo;
            _paymentsRepo = paymentsRepo;
            _withdrawalRequestRepo = withdrawalRequestRepo;
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


        public async Task<ResponseDto<PaginatedResult<TransactionDto>>> GetAllTransactions(string? userId, string? searchParam, TransactionDateFilter dateFilter, int pageNumber, int pageSize)
        {
            var response = new ResponseDto<PaginatedResult<TransactionDto>>();
            try
            {
                var page = pageNumber > 0 ? pageNumber : 1;
                var size = pageSize > 0 ? pageSize : 10;

                var query = _paymentsRepo.GetQueryable().AsNoTracking();

                if (!string.IsNullOrWhiteSpace(userId))
                    query = query.Where(p => p.UserId == userId);

                if (!string.IsNullOrWhiteSpace(searchParam))
                    query = query.Where(p =>
                        p.User.FirstName.Contains(searchParam) ||
                        p.User.LastName.Contains(searchParam) ||
                        p.User.Email.Contains(searchParam) ||
                        p.OrderReferenceId.Contains(searchParam) ||
                        p.FeeType.Contains(searchParam));

                if (dateFilter != TransactionDateFilter.All)
                {
                    var cutoff = dateFilter == TransactionDateFilter.LastWeek
                        ? DateTime.UtcNow.AddDays(-7)
                        : DateTime.UtcNow.AddMonths(-1);
                    query = query.Where(p => p.CreatedPaymentTime >= cutoff);
                }

                var totalCount = await query.CountAsync();
                var items = await query
                    .OrderByDescending(p => p.CreatedPaymentTime)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .Select(p => new TransactionDto
                    {
                        TransactionId = p.Id,
                        UserId = p.UserId,
                        UserFullName = p.User.FirstName + " " + p.User.LastName,
                        UserEmail = p.User.Email,
                        Amount = p.Amount,
                        FeeType = p.FeeType,
                        PaymentType = p.PaymentType,
                        PaymentChannel = p.PaymentChannel,
                        PaymentStatus = p.PaymentStatus,
                        Description = p.Description,
                        SwapId = p.SwapId,
                        RoomName = p.RoomName,
                        CreatedPaymentTime = p.CreatedPaymentTime,
                        CompletePaymentTime = p.CompletePaymentTime == default ? null : p.CompletePaymentTime
                    })
                    .ToListAsync();

                response.Result = new PaginatedResult<TransactionDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = page,
                    PageSize = size,
                    TotalPages = (int)Math.Ceiling((double)totalCount / size)
                };
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error fetching transactions" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
            }
            return response;
        }

        public async Task<ResponseDto<TransactionStatsDto>> GetTransactionStats()
        {
            var response = new ResponseDto<TransactionStatsDto>();
            try
            {
                var now = DateTime.UtcNow;
                var todayStart = now.Date;
                var weekStart = now.AddDays(-7).Date;
                var monthStart = now.AddMonths(-1).Date;

                // Pull only the fields needed for stats
                var all = await _paymentsRepo.GetQueryable()
                    .AsNoTracking()
                    .Select(p => new
                    {
                        p.Amount,
                        p.PaymentStatus,
                        p.FeeType,
                        p.PaymentType,
                        p.PaymentChannel,
                        p.CreatedPaymentTime
                    })
                    .ToListAsync();

                static decimal ParseAmount(string raw)
                    => decimal.TryParse(raw, out var v) ? v : 0m;

                var stats = new TransactionStatsDto
                {
                    TotalTransactions = all.Count,
                    TotalRevenue = all.Sum(p => ParseAmount(p.Amount)),

                    SuccessCount = all.Count(p => p.PaymentStatus == "SUCCESS"),
                    SuccessRevenue = all.Where(p => p.PaymentStatus == "SUCCESS").Sum(p => ParseAmount(p.Amount)),

                    PendingCount = all.Count(p => p.PaymentStatus == "CREATED" || p.PaymentStatus == "PENDING"),
                    PendingRevenue = all.Where(p => p.PaymentStatus == "CREATED" || p.PaymentStatus == "PENDING").Sum(p => ParseAmount(p.Amount)),

                    FailedCount = all.Count(p => p.PaymentStatus == "FAILED"),
                    FailedRevenue = all.Where(p => p.PaymentStatus == "FAILED").Sum(p => ParseAmount(p.Amount)),

                    TodayCount = all.Count(p => p.CreatedPaymentTime >= todayStart),
                    TodayRevenue = all.Where(p => p.CreatedPaymentTime >= todayStart).Sum(p => ParseAmount(p.Amount)),

                    ThisWeekCount = all.Count(p => p.CreatedPaymentTime >= weekStart),
                    ThisWeekRevenue = all.Where(p => p.CreatedPaymentTime >= weekStart).Sum(p => ParseAmount(p.Amount)),

                    ThisMonthCount = all.Count(p => p.CreatedPaymentTime >= monthStart),
                    ThisMonthRevenue = all.Where(p => p.CreatedPaymentTime >= monthStart).Sum(p => ParseAmount(p.Amount)),

                    ByFeeType = all
                        .GroupBy(p => p.FeeType ?? "Unknown")
                        .Select(g => new TransactionGroupStatDto
                        {
                            Label = g.Key,
                            Count = g.Count(),
                            TotalRevenue = g.Sum(p => ParseAmount(p.Amount))
                        })
                        .OrderByDescending(g => g.Count)
                        .ToList(),

                    ByPaymentType = all
                        .GroupBy(p => p.PaymentType ?? "Unknown")
                        .Select(g => new TransactionGroupStatDto
                        {
                            Label = g.Key,
                            Count = g.Count(),
                            TotalRevenue = g.Sum(p => ParseAmount(p.Amount))
                        })
                        .OrderByDescending(g => g.Count)
                        .ToList(),

                    ByPaymentChannel = all
                        .GroupBy(p => p.PaymentChannel ?? "Unknown")
                        .Select(g => new TransactionGroupStatDto
                        {
                            Label = g.Key,
                            Count = g.Count(),
                            TotalRevenue = g.Sum(p => ParseAmount(p.Amount))
                        })
                        .OrderByDescending(g => g.Count)
                        .ToList()
                };

                response.Result = stats;
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error fetching transaction stats" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
            }
            return response;
        }

        public async Task<ResponseDto<string>> SubmitWithdrawal(string userId, string swapId)
        {
            var response = new ResponseDto<string>();
            try
            {
                var swap = await _swappingProceedingRepo.GetByIdAsync(swapId);
                if (swap == null)
                {
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string>() { "Swap not found" };
                    return response;
                }
                if (swap.Userid != userId)
                {
                    response.StatusCode = 403;
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string>() { "Unauthorized" };
                    return response;
                }
                if (swap.Status != SwapProceedingStatus.AdvNegotiationSwapped.ToString() &&
                    swap.Status != SwapProceedingStatus.Swapped.ToString())
                {
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string>() { "Swap must be completed before requesting withdrawal" };
                    return response;
                }

                var existingRequest = await _withdrawalRequestRepo.GetQueryable()
                    .FirstOrDefaultAsync(w => w.SwapId == swapId && w.UserId == userId && w.Status == "Pending");
                if (existingRequest != null)
                {
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string>() { "A pending withdrawal request already exists for this swap" };
                    return response;
                }

                var payment = await _paymentsRepo.GetQueryable()
                    .Where(p => p.SwapId == swapId && p.UserId == userId && p.PaymentStatus == "Paid")
                    .OrderByDescending(p => p.CreatedPaymentTime)
                    .FirstOrDefaultAsync();

                var amount = payment?.Amount ?? "0";

                await _withdrawalRequestRepo.Add(new WithdrawalRequest
                {
                    UserId = userId,
                    SwapId = swapId,
                    Amount = amount,
                    Status = "Pending"
                });
                await _withdrawalRequestRepo.SaveChanges();

                swap.Status = SwapProceedingStatus.SettlementRequest.ToString();
                _swappingProceedingRepo.Update(swap);
                await _swappingProceedingRepo.SaveChanges();

                response.Result = "Withdrawal request submitted successfully";
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error submitting withdrawal request" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
            }
            return response;
        }

        public async Task<ResponseDto<string>> TreatWithdrawal(string withdrawalId, string? adminNote)
        {
            var response = new ResponseDto<string>();
            try
            {
                var withdrawal = await _withdrawalRequestRepo.GetByIdAsync(withdrawalId);
                if (withdrawal == null)
                {
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string>() { "Withdrawal request not found" };
                    return response;
                }
                if (withdrawal.Status == "Treated")
                {
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string>() { "Withdrawal request is already treated" };
                    return response;
                }

                withdrawal.Status = "Treated";
                withdrawal.AdminNote = adminNote;
                _withdrawalRequestRepo.Update(withdrawal);
                await _withdrawalRequestRepo.SaveChanges();

                response.Result = "Withdrawal request marked as treated";
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error treating withdrawal request" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
            }
            return response;
        }

        public async Task<ResponseDto<string>> CompleteAdvanceSwap(string userId, string swapId)
        {
            var response = new ResponseDto<string>();
            try
            {
                var swap = await _swappingProceedingRepo.GetByIdAsync(swapId);
                if (swap == null)
                {
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string>() { "Swap not found" };
                    return response;
                }
                if (swap.Status != SwapProceedingStatus.AdvNegotiation.ToString())
                {
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string>() { "Swap must be in AdvNegotiation status to be completed" };
                    return response;
                }
                if (swap.Userid != userId)
                {
                    response.StatusCode = 403;
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string>() { "Unauthorized" };
                    return response;
                }

                swap.Status = SwapProceedingStatus.AdvNegotiationSwapped.ToString();
                _swappingProceedingRepo.Update(swap);
                await _swappingProceedingRepo.SaveChanges();

                response.Result = "Advance swap completed successfully";
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error completing advance swap" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
            }
            return response;
        }

        public async Task<ResponseDto<PaginatedResult<WithdrawalResponseDto>>> GetWithdrawals(string? userId, WithdrawalStatus status, int pageNumber, int pageSize)
        {
            var response = new ResponseDto<PaginatedResult<WithdrawalResponseDto>>();
            try
            {
                var page = pageNumber > 0 ? pageNumber : 1;
                var size = pageSize > 0 ? pageSize : 10;

                var query = _withdrawalRequestRepo.GetQueryable().AsNoTracking();

                if (!string.IsNullOrWhiteSpace(userId))
                    query = query.Where(w => w.UserId == userId);

                if (status != WithdrawalStatus.All)
                    query = query.Where(w => w.Status == status.ToString());

                var totalCount = await query.CountAsync();
                var items = await query
                    .OrderByDescending(w => w.Created)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .Select(w => new WithdrawalResponseDto
                    {
                        WithdrawalId = w.Id,
                        UserId = w.UserId,
                        UserFullName = w.User.FirstName + " " + w.User.LastName,
                        SwapId = w.SwapId,
                        Amount = w.Amount,
                        Status = w.Status,
                        AdminNote = w.AdminNote,
                        CreatedOn = w.Created
                    })
                    .ToListAsync();

                response.Result = new PaginatedResult<WithdrawalResponseDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = page,
                    PageSize = size,
                    TotalPages = (int)Math.Ceiling((double)totalCount / size)
                };
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error fetching withdrawal requests" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
            }
            return response;
        }


    }
}