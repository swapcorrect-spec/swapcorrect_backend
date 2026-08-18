using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swap_Shop.Domain.Entities;
using SwapShop.Domain.Dtos.Request.ListingItem;
using SwapShop.Domain.Dtos.Request.Report;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Report;
using SwapShop.Domain.Enitities;
using SwapShop.Domain.Enum;
using SwapShop.Domain.OtherService.Interface;
using SwapShop.Domain.Repository.Interface;

namespace SwapShop.Infrastructure.OtherService.Implementation
{
    public class ReportService : IReportService
    {
        private readonly IActivityLogRepo _activityLogRepo;
        private readonly ISwapShopGenericRepo<UserReport> _userReportRepo;
        private readonly ISwapShopGenericRepo<ReportAdminNote> _reportAdminNoteRepo;
        private readonly ISwapShopGenericRepo<ReportMedia> _reportMediaRepo;
        private readonly ILogger<ReportService> _logger;
        private readonly ISwapShopGenericRepo<User_Review_Rating> _user_Review_RatingRepo;
        private readonly ISwapShopGenericRepo<SwappingProceeding> _swappingProceedingRepo;
        public ReportService(ILogger<ReportService> logger,
            ISwapShopGenericRepo<ReportAdminNote> reportAdminNoteRepo,
            ISwapShopGenericRepo<UserReport> userReportRepo,
            ISwapShopGenericRepo<ReportMedia> reportMediaRepo,
            IActivityLogRepo activityLogRepo,
            ISwapShopGenericRepo<User_Review_Rating> user_Review_RatingRepo,
            ISwapShopGenericRepo<SwappingProceeding> swappingProceedingRepo)
        {
            _logger = logger;
            _reportAdminNoteRepo = reportAdminNoteRepo;
            _userReportRepo = userReportRepo;
            _reportMediaRepo = reportMediaRepo;
            _activityLogRepo = activityLogRepo;
            _user_Review_RatingRepo = user_Review_RatingRepo;
            _swappingProceedingRepo = swappingProceedingRepo;
        }

        public async Task<ResponseDto<string>> ReportUser(ReportUserDto req, string userid)
        {
            var response = new ResponseDto<string>();
            try
            {

                var addReport = await _userReportRepo.Add(new UserReport()
                {

                    UserId = userid,
                    ReportedUserId = req.ReportedUserId,
                    Description = req.Description,
                    ReportType = req.ReportType,

                });
                if (req.EvidenceMediaFiles.Any())
                {


                    var evidenceMedia = new List<ReportMedia>();

                    foreach (var itemMedia in req.EvidenceMediaFiles)
                    {
                        evidenceMedia.Add(new ReportMedia()
                        {
                            ReportId = addReport.Id,
                            MediaType = itemMedia.MediaType,
                            Url = itemMedia.Url,
                        });
                    }
                    await _reportMediaRepo.AddRanges(evidenceMedia);

                }

                await _activityLogRepo.AddActivitylog(userid, "Report User", $"This user is reported");
                await _userReportRepo.SaveChanges();
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Successful";
                response.Result = "User reported successfully awaiting admin review";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, JsonConvert.SerializeObject(ex));
                response.ErrorMessages = new List<string>() { "Error in reporting user on the platform, please try again later" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }

        }
        public async Task<ResponseDto<string>> AddReportReportAdminNoted(string Note, string ReportId)
        {
            var response = new ResponseDto<string>();
            try
            {

                var addReportNote = await _reportAdminNoteRepo.Add(new ReportAdminNote()
                {
                    ReportId = ReportId,
                    Note = Note,

                });

                await _reportAdminNoteRepo.SaveChanges();
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Successful";
                response.Result = "Report note added";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, JsonConvert.SerializeObject(ex));
                response.ErrorMessages = new List<string>() { "Error in adding reporting note on the platform, please try again later" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }

        }
        public async Task<ResponseDto<string>> ChangeReportStatus(ReportUserStatus status, string ReportId)
        {
            var response = new ResponseDto<string>();
            try
            {

                var GetReport = await _userReportRepo.GetByIdAsync(ReportId);
                GetReport.Status = status.ToString();
                _userReportRepo.Update(GetReport);

                await _reportAdminNoteRepo.SaveChanges();
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Successful";
                response.Result = "Report status change successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, JsonConvert.SerializeObject(ex));
                response.ErrorMessages = new List<string>() { "Error in changing report status on the platform, please try again later" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }

        }
        public async Task<ResponseDto<SingleReportDetails>> GetSingleReportDetails(string ReportId)
        {
            var response = new ResponseDto<SingleReportDetails>();
            try
            {

                var getReport = await _userReportRepo
    .GetQueryable()
    .Include(u => u.User)
    .Include(u => u.ReportedUser)
    .Include(u => u.ReportMedias)
    .Include(u => u.ReportAdminNotes)
    .Where(u => u.Id == ReportId) // filter first
    .Select(u => new SingleReportDetails
    {
        Created = u.Created,
        Reason = u.Description,
        ReportedPersonId = u.ReportedUserId, // use the FK directly (safer)
        ReportedPersonImg = u.ReportedUser != null ? u.ReportedUser.ProfilePicture : null,
        ReportedPersonName = u.ReportedUser != null
            ? (u.ReportedUser.FirstName + " " + u.ReportedUser.LastName).Trim()
            : null,
        ReporterId = u.UserId,
        ReporterImg = u.User != null ? u.User.ProfilePicture : null,
        ReporterName = u.User != null
            ? (u.User.FirstName + " " + u.User.LastName).Trim()
            : null,
        ReportId = u.Id,
        ReportType = u.ReportType,
        Status = u.Status,
        EvidenceImg = u.ReportMedias
            .Select(b => new ListItemMedia
            {
                MediaType = b.MediaType,
                Url = b.Url
            })
            .ToList(),
        Notes = u.ReportAdminNotes
            .Select(c => c.Note)
            .ToList()
    })
    .FirstOrDefaultAsync();


                var getSwapping = await _swappingProceedingRepo.GetQueryable().Include(u => u.List).Where(u => u.Userid == getReport.ReportedPersonId || u.List.UserId == getReport.ReportedPersonId)
                    .Where(u => u.Status == SwapProceedingStatus.Swapped.ToString()).ToListAsync();
                int listingCount = 0;
                int swappCount = 0;
                if (getSwapping.Any())
                {
                    swappCount = getSwapping.Count();

                }
                var rateUser = await _user_Review_RatingRepo.GetQueryable()
                 .AsNoTracking().Where(u => u.UserId == getReport.ReportedPersonId).ToListAsync();
                double avgRate = 0;
                if (rateUser.Any())
                {
                    avgRate = rateUser.Average(r => (double)r.RateScore);
                }

                getReport.ReportedPersonTotalSwap = swappCount.ToString();
                getReport.ReportedPersonRating = avgRate.ToString();
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Successful";
                response.Result = getReport;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, JsonConvert.SerializeObject(ex));
                response.ErrorMessages = new List<string>() { "Error in getting single report details on the platform, please try again later" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }

        }

        public async Task<ResponseDto<PaginatedResult<UserReportPaginatedDto>>> SearchUserReportPaginated(string? searhParam, ReportUserStatus status,
           ReportDateFilter reportFilerDate, string? userId, int pageNumber, int perpageSize)
        {
            var response = new ResponseDto<PaginatedResult<UserReportPaginatedDto>>();

            try
            {
                // safe defaults for pagination
                var page = pageNumber > 0 ? pageNumber : 1;
                var pageSize = perpageSize > 0 ? perpageSize : 10;

                // base query
                var query = _userReportRepo
                    .GetQueryable()
                    .AsNoTracking();
                // eager load navigations used in the projection (optional but often prevents client eval)



                // filters

                if (!string.IsNullOrWhiteSpace(userId))
                    query = query.Where(x => x.UserId == userId || x.ReportedUserId == userId);

                if (!string.IsNullOrWhiteSpace(searhParam))
                {

                    searhParam = searhParam.Trim().ToLower();
                    query = query.Where(x =>
                        x.User.FirstName.ToLower().Contains(searhParam) ||
                        x.User.LastName.ToLower().Contains(searhParam) ||
                        x.User.UserName.ToLower().Contains(searhParam) ||
                        x.ReportedUser.FirstName.ToLower().Contains(searhParam) ||
                        x.ReportedUser.LastName.ToLower().Contains(searhParam) ||
                        x.ReportedUser.UserName.ToLower().Contains(searhParam))
                     ;

                }




                // date filter
                if (reportFilerDate != ReportDateFilter.All)
                {
                    DateTime filterDate;
                    switch (reportFilerDate)
                    {
                        case ReportDateFilter.LastWeek:
                            filterDate = DateTime.UtcNow.AddDays(-7);
                            break;
                        case ReportDateFilter.LastMonth:
                            filterDate = DateTime.UtcNow.AddMonths(-1);
                            break;
                        default:
                            filterDate = DateTime.MinValue;
                            break;
                    }

                    query = query.Where(x => x.Created >= filterDate);
                }
                if (status != ReportUserStatus.All)
                {

                    query = query.Where(x => x.Status == status.ToString());
                }

                // total count before paging
                var totalCount = await query.CountAsync();

                // fetch page
                var candidateItems = await query
                    .OrderByDescending(u => u.Created)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(u => new UserReportPaginatedDto
                    {
                        Created = u.Created,
                        Reason = u.Description,
                        ReportedPersonId = u.ReportedUser.Id,
                        ReportedPersonImg = u.ReportedUser.ProfilePicture,
                        ReportedPersonName = u.ReportedUser.FirstName + " " + u.ReportedUser.LastName,
                        ReporterId = u.User.Id,
                        ReporterImg = u.User.ProfilePicture,
                        ReporterName = u.User.FirstName + " " + u.User.LastName,
                        ReportId = u.Id,
                        ReportType = u.ReportType,
                        Status = u.Status,
                    })
                    .ToListAsync();

                var data = new PaginatedResult<UserReportPaginatedDto>
                {
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                    Items = candidateItems,
                    TotalCount = totalCount,       // total items matching filters
                    PageNumber = page,
                    PageSize = pageSize
                };



                response.Result = data;
                response.StatusCode = 200;
                response.DisplayMessage = "Success";

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in User Report Paginated");
                response.ErrorMessages = new List<string> { "Error fetching listings" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }
    }
}
