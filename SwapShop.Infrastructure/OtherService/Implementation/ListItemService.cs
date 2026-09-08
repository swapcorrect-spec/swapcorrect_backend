using Austistic.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swap_Shop.Domain.Entities;
using Microsoft.Extensions.Configuration;
using SwapShop.Domain.Dtos.Request.ListingItem;
using SwapShop.Domain.Dtos.Request.Mailing;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.FavListItems;
using SwapShop.Domain.Dtos.Response.ListingItem;
using SwapShop.Domain.Enitities;
using SwapShop.Domain.Enum;
using SwapShop.Domain.OtherService.Interface;
using SwapShop.Domain.Repository.Interface;
using SwapShop.Infrastructure.Helper;

namespace SwapShop.Infrastructure.OtherService.Implementation
{
    public class ListItemService : IListItemService
    {
        private readonly ISwapShopGenericRepo<FavListItem> _faveListItemRepo;
        private readonly IActivityLogRepo _activityLogRepo;
        private readonly IHelper _helper;
        private readonly ISwapShopGenericRepo<ListingItem> _listingItemRepo;
        private readonly ISwapShopGenericRepo<Item_Media> _itemMediaRepo;
        private readonly ISwapShopGenericRepo<ItemCategory> _itemCategoryRepo;
        private readonly ISwapShopGenericRepo<SwapListRequest> _swapListRequestRepo;
        private readonly ISwapShopGenericRepo<User_Review_Rating> _user_Review_RatingRepo;
        private readonly ISwapShopGenericRepo<SwappingProceeding> _swappingProceedingRepo;
        private readonly ISwapShopGenericRepo<UserRoom> _userRoomRepo;
        private readonly ISwapShopGenericRepo<Room> _roomRepo;
        private readonly ILogger<AccountService> _logger;
        private readonly IEmailServices _emailServices;
        private readonly IConfiguration _configuration;
        private readonly IAccountRepo _accountRepo;

        public ListItemService(ILogger<AccountService> logger,
            ISwapShopGenericRepo<SwapListRequest> swapListRequestRepo,
            ISwapShopGenericRepo<ItemCategory> itemCategoryRepo,
            ISwapShopGenericRepo<Item_Media> itemMediaRepo,
            ISwapShopGenericRepo<ListingItem> listingItemRepo,
            IActivityLogRepo activityLogRepo, IHelper helper,
            ISwapShopGenericRepo<User_Review_Rating> user_Review_RatingRepo,
            ISwapShopGenericRepo<SwappingProceeding> swappingProceedingRepo,
            ISwapShopGenericRepo<UserRoom> userRoomRepo, ISwapShopGenericRepo<Room> roomRepo,
            ISwapShopGenericRepo<FavListItem> faveListItemRepo,
            IEmailServices emailServices, IConfiguration configuration,
            IAccountRepo accountRepo)
        {
            _logger = logger;
            _swapListRequestRepo = swapListRequestRepo;
            _itemCategoryRepo = itemCategoryRepo;
            _itemMediaRepo = itemMediaRepo;
            _listingItemRepo = listingItemRepo;
            _activityLogRepo = activityLogRepo;
            _helper = helper;
            _user_Review_RatingRepo = user_Review_RatingRepo;
            _emailServices = emailServices;
            _configuration = configuration;
            _accountRepo = accountRepo;
            _swappingProceedingRepo = swappingProceedingRepo;
            _userRoomRepo = userRoomRepo;
            _roomRepo = roomRepo;
            _faveListItemRepo = faveListItemRepo;
        }
        public async Task<ResponseDto<string>> CreateItemRequest(ListItemReq req, string userid)
        {
            var response = new ResponseDto<string>();
            try
            {
                var checkCategory = await _itemCategoryRepo.GetByIdAsync(req.CategoryId);
                if (checkCategory == null)
                {
                    response.ErrorMessages = new List<string>() { "Invalid Category" };
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    return response;
                }
                var addListItem = await _listingItemRepo.Add(new ListingItem()
                {
                    EstimatedAmount = req.EstimatedAmount,
                    EstimatedCurrency = req.EstimatedCurrency,
                    CategoryId = req.CategoryId,
                    ItemCondition = req.ItemCondition,
                    ItemDescription = req.ItemDescription,
                    ItemName = req.ItemName,
                    UserId = userid,
                    ListType = req.ListType,
                    Location = req.Location,

                });
                if (req.ListMediaFiles.Any())
                {


                    var listmedia = new List<Item_Media>();

                    foreach (var itemMedia in req.ListMediaFiles)
                    {
                        listmedia.Add(new Item_Media()
                        {
                            ItemId = addListItem.Id,
                            MediaType = itemMedia.MediaType,
                            Url = itemMedia.Url,
                        });
                    }
                    await _itemMediaRepo.AddRanges(listmedia);

                }
                if (req.ListingSwapReq.Any())
                {


                    var listSwap = new List<SwapListRequest>();

                    foreach (var reqItem in req.ListingSwapReq)
                    {
                        listSwap.Add(new SwapListRequest()
                        {

                            ListedItemId = addListItem.Id,
                            ItemNeededName = reqItem.ItemNeededName,
                        });
                    }
                    await _swapListRequestRepo.AddRanges(listSwap);
                }
                await _activityLogRepo.AddActivitylog(userid, "List item", $"List a new item in {req.ListType} Mode");
                await _listingItemRepo.SaveChanges();

                var adminEmail = _configuration["EmailConfiguration:AdminEmail"];
                if (!string.IsNullOrWhiteSpace(adminEmail))
                {
                    var emailBody = EmailTemplate.Build(
                        title: "New Listing Awaiting Approval",
                        greetingName: null,
                        bodyHtml: "<p style=\"margin:0 0 4px 0;\">A new item has been listed on SwapCorrect and is waiting for review.</p>"
                                  + EmailTemplate.DetailTable(
                                      ("Item Name", req.ItemName),
                                      ("Description", req.ItemDescription),
                                      ("Estimated Value", $"{req.EstimatedCurrency} {req.EstimatedAmount}"),
                                      ("Listing Type", req.ListType.ToString()),
                                      ("Location", req.Location ?? "-"),
                                      ("Listed By (User ID)", userid))
                                  + "<p style=\"margin:0;\">Sign in to the admin panel to approve or reject this listing.</p>"
                    );
                    var adminMessage = new Message(new[] { adminEmail }, "New Item Listing – Awaiting Approval", emailBody);
                    await _emailServices.SendEmailAsync(adminMessage);
                }

                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Successful";
                response.Result = "Item listed successfully awaiting admin review";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, JsonConvert.SerializeObject(ex));
                response.ErrorMessages = new List<string>() { "Error in listing item on the platform, please try again later" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }

        }
        public async Task<ResponseDto<string>> UpdateListingItemRequest(UpdateListingReq req, string userid)
        {
            var response = new ResponseDto<string>();
            try
            {
                var retrieveListing = await _listingItemRepo.GetByIdAsync(req.ListId);
                if (retrieveListing == null)
                {
                    response.ErrorMessages = new List<string>() { "Invalid ListItem" };
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    return response;
                }
                var checkCategory = await _itemCategoryRepo.GetByIdAsync(req.CategoryId);
                if (checkCategory == null)
                {
                    response.ErrorMessages = new List<string>() { "Invalid Category" };
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    return response;
                }

                retrieveListing.EstimatedAmount = req.EstimatedAmount;
                retrieveListing.EstimatedCurrency = req.EstimatedCurrency;
                retrieveListing.CategoryId = req.CategoryId;
                retrieveListing.ItemCondition = req.ItemCondition;
                retrieveListing.ItemDescription = req.ItemDescription;
                retrieveListing.ItemName = req.ItemName;

                retrieveListing.ListType = req.ListType;
                retrieveListing.Location = req.Location;

                _listingItemRepo.Update(retrieveListing);
                var retrieveOldMedia = await _itemMediaRepo.GetQueryable().Where(u => u.ItemId == req.ListId).ToListAsync();
                if (retrieveOldMedia.Any())
                {
                    _itemMediaRepo.Delete(retrieveOldMedia);
                }
                if (req.ListMediaFiles.Any())
                {


                    var listmedia = new List<Item_Media>();

                    foreach (var itemMedia in req.ListMediaFiles)
                    {
                        listmedia.Add(new Item_Media()
                        {
                            ItemId = req.ListId,
                            MediaType = itemMedia.MediaType,
                            Url = itemMedia.Url,
                        });
                    }
                    await _itemMediaRepo.AddRanges(listmedia);

                }

                var retrieveOldListingSwapReq = await _swapListRequestRepo.GetQueryable().Where(u => u.ListedItemId == req.ListId).ToListAsync();
                if (retrieveOldListingSwapReq.Any())
                {
                    _swapListRequestRepo.Delete(retrieveOldListingSwapReq);
                }

                if (req.ListingSwapReq.Any())
                {


                    var listSwap = new List<SwapListRequest>();

                    foreach (var reqItem in req.ListingSwapReq)
                    {
                        listSwap.Add(new SwapListRequest()
                        {

                            ListedItemId = req.ListId,
                            ItemNeededName = reqItem.ItemNeededName,
                        });
                    }
                    await _swapListRequestRepo.AddRanges(listSwap);
                }
                await _activityLogRepo.AddActivitylog(userid, "List item updated", $"Update List item in {req.ListType} Mode");
                await _listingItemRepo.SaveChanges();
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Successful";
                response.Result = "Item listed successfully updated";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, JsonConvert.SerializeObject(ex));
                response.ErrorMessages = new List<string>() { "Error in updating listing item on the platform, please try again later" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }

        }
        public async Task<ResponseDto<List<ItemCategory>>> GetAllCategory()
        {
            var response = new ResponseDto<List<ItemCategory>>();
            try
            {
                var checkCategory = await _itemCategoryRepo.GetQueryable().ToListAsync();

                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Successful";
                response.Result = checkCategory;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, JsonConvert.SerializeObject(ex));
                response.ErrorMessages = new List<string>() { "Error in getting all category, please try again later" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }

        }
        public async Task<ResponseDto<List<ListedItemResp>>> GetItemByRaterHotPick(string? userId, int limit)
        {
            var response = new ResponseDto<List<ListedItemResp>>();

            try
            {
                List<ListedItemResp> candidateItems = null;

                // 1) Top 5 users by average rating (tie-breaker: rating count)
                var topUsers = await _user_Review_RatingRepo.GetQueryable()
                    .AsNoTracking()
                    .GroupBy(r => r.UserId)
                    .Select(g => new
                    {
                        UserId = g.Key,
                        AvgRating = g.Average(r => (double)r.RateScore),
                        Count = g.Count()
                    })
                    .OrderByDescending(x => x.AvgRating)
                    .ThenByDescending(x => x.Count)
                    .Take(5)
                    .ToListAsync();

                var validTopUserIds = topUsers?
                    .Where(u => !string.IsNullOrWhiteSpace(u.UserId))
                    .Select(u => u.UserId)
                    .ToList();

                if (validTopUserIds != null && validTopUserIds.Count > 0)
                {
                    // Listings from top-rated users
                    candidateItems = await _listingItemRepo.GetQueryable()
                        .AsNoTracking()
                        .Where(li => validTopUserIds.Contains(li.UserId))
                        .Where(li => li.ReviewStage == ListingReiviewStage.Approved.ToString())
                        .Where(li => string.IsNullOrEmpty(userId) || li.UserId != userId)
                        .OrderByDescending(li => li.Created)
                        .Select(u => new ListedItemResp
                        {
                            ListingId = u.Id,
                            Username = u.User.UserName,
                            Email = u.User.Email,
                            EstimatedAmount = u.EstimatedAmount,
                            EstimatedCurrency = u.EstimatedCurrency,
                            CategoryName = u.Category.CategoryName,
                            ItemDescription = u.ItemDescription,
                            FullName = u.User.FirstName + " " + u.User.LastName,
                            ItemCondition = u.ItemCondition,
                            ItemName = u.ItemName,
                            UserId = u.User.Id,
                            ListType = u.ListType,
                            PhoneNumber = u.User.PhoneNumber,
                            ProfilePicture = u.User.ProfilePicture,
                            ReviewStage = u.ReviewStage,
                            Media = u.Media.Select(d => new ImageItemDto
                            {
                                Url = d.Url,
                                MediaType = d.MediaType,
                            }).ToList(),
                            IsFavItem = !string.IsNullOrEmpty(userId) && u.favListItems.Any(d => d.UserId == userId),
                            IsFlagged = u.IsFlagged,
                            SwapListRequest = u.SwapListRequest.Select(f => f.ItemNeededName).ToList()
                        })
                        .Take(limit)
                        .ToListAsync();

                    // Fallback: no listings found for top users
                    if (candidateItems == null || candidateItems.Count == 0)
                    {
                        candidateItems = await _listingItemRepo.GetQueryable()
                            .AsNoTracking()
                            .Where(u => u.ReviewStage == ListingReiviewStage.Approved.ToString())
                            .Where(u => string.IsNullOrEmpty(userId) || u.UserId != userId)
                            .OrderByDescending(u => u.Created)
                            .Select(u => new ListedItemResp
                            {
                                Username = u.User.UserName,
                                Email = u.User.Email,
                                EstimatedAmount = u.EstimatedAmount,
                                EstimatedCurrency = u.EstimatedCurrency,
                                CategoryName = u.Category.CategoryName,
                                ItemDescription = u.ItemDescription,
                                FullName = u.User.FirstName + " " + u.User.LastName,
                                ItemCondition = u.ItemCondition,
                                ItemName = u.ItemName,
                                ListType = u.ListType,
                                ListingId = u.Id,
                                PhoneNumber = u.User.PhoneNumber,
                                ProfilePicture = u.User.ProfilePicture,
                                ReviewStage = u.ReviewStage,
                                UserId = u.User.Id,
                                Media = u.Media.Select(d => new ImageItemDto
                                {
                                    Url = d.Url,
                                    MediaType = d.MediaType,
                                }).ToList(),
                                IsFavItem = !string.IsNullOrEmpty(userId) && u.favListItems.Any(d => d.UserId == userId),
                                IsFlagged = u.IsFlagged,
                                SwapListRequest = u.SwapListRequest.Select(f => f.ItemNeededName).ToList()
                            })
                            .Take(limit)
                            .ToListAsync();
                    }
                }

                response.Result = candidateItems ?? new List<ListedItemResp>();
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching rated hot picks");
                response.ErrorMessages = new List<string> { "Error in fetching GetItemByRaterHotPick" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
            }

            return response;
        }

        public async Task<ResponseDto<List<ListedItemResp>>> GetItemByUserPreviousWantItem(string? userId, int limit)
        {
            var response = new ResponseDto<List<ListedItemResp>>();

            try
            {
                List<ListedItemResp> candidateItems = null;

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    // Retrieve distinct item names the user has requested before
                    var wantedItemNames = await _swapListRequestRepo
                        .GetQueryable()
                        .AsNoTracking()
                        .Include(s => s.ListedItem)
                        .Where(s => s.ListedItem != null && s.ListedItem.UserId == userId)
                        .Select(s => s.ItemNeededName)
                        .Where(n => !string.IsNullOrWhiteSpace(n))
                        .Distinct()
                        .ToListAsync();

                    if (wantedItemNames != null && wantedItemNames.Count > 0)
                    {
                        var loweredWanted = wantedItemNames
                            .Select(n => n.Trim().ToLower())
                            .ToList();

                        // Fetch approved items not owned by the user server-side, then match in-memory
                        // (EF Core cannot translate local-list Any() with string pattern matching to SQL)
                        var approvedItems = await _listingItemRepo
                            .GetQueryable()
                            .AsNoTracking()
                            .Where(li => li.ReviewStage == ListingReiviewStage.Approved.ToString() && li.UserId != userId)
                            .OrderByDescending(u => u.Created)
                            .Select(u => new ListedItemResp
                            {
                                ListingId = u.Id,
                                Username = u.User.UserName,
                                UserId = u.User.Id,
                                Email = u.User.Email,
                                EstimatedAmount = u.EstimatedAmount,
                                EstimatedCurrency = u.EstimatedCurrency,
                                CategoryName = u.Category.CategoryName,
                                ItemDescription = u.ItemDescription,
                                FullName = u.User.FirstName + " " + u.User.LastName,
                                ItemCondition = u.ItemCondition,
                                ItemName = u.ItemName,
                                ListType = u.ListType,
                                PhoneNumber = u.User.PhoneNumber,
                                ProfilePicture = u.User.ProfilePicture,
                                ReviewStage = u.ReviewStage,
                                Media = u.Media.Select(d => new ImageItemDto
                                {
                                    Url = d.Url,
                                    MediaType = d.MediaType,
                                }).ToList(),
                                IsFavItem = u.favListItems.Any(d => d.UserId == userId),
                                IsFlagged = u.IsFlagged,
                                SwapListRequest = u.SwapListRequest.Select(f => f.ItemNeededName).ToList()
                            })
                            .ToListAsync();

                        candidateItems = approvedItems
                            .Where(li => loweredWanted.Any(w =>
                                (li.ItemName ?? "").ToLower().Contains(w) ||
                                (li.ItemDescription ?? "").ToLower().Contains(w) ||
                                (li.CategoryName ?? "").ToLower().Contains(w)))
                            .Take(limit)
                            .ToList();

                        // if nothing matched by preference, fall back to latest items from the fetched set
                        if (candidateItems == null || candidateItems.Count == 0)
                            candidateItems = approvedItems.Take(limit).ToList();
                    }
                }

                // Fallback: get latest approved listings if no userId or no wanted-item preferences found
                if (candidateItems == null || candidateItems.Count == 0)
                {
                    candidateItems = await _listingItemRepo
                        .GetQueryable()
                        .AsNoTracking()
                        .Where(u => u.ReviewStage == ListingReiviewStage.Approved.ToString() &&
                                    (string.IsNullOrEmpty(userId) || u.UserId != userId))
                        .OrderByDescending(u => u.Created)
                        .Select(u => new ListedItemResp
                        {
                            Username = u.User.UserName,
                            Email = u.User.Email,
                            UserId = u.User.Id,
                            ListingId = u.Id,
                            EstimatedAmount = u.EstimatedAmount,
                            EstimatedCurrency = u.EstimatedCurrency,
                            CategoryName = u.Category.CategoryName,
                            ItemDescription = u.ItemDescription,
                            FullName = u.User.FirstName + " " + u.User.LastName,
                            ItemCondition = u.ItemCondition,
                            ItemName = u.ItemName,
                            ListType = u.ListType,
                            PhoneNumber = u.User.PhoneNumber,
                            ProfilePicture = u.User.ProfilePicture,
                            ReviewStage = u.ReviewStage,
                            Media = u.Media.Select(d => new ImageItemDto
                            {
                                Url = d.Url,
                                MediaType = d.MediaType,
                            }).ToList(),
                            IsFavItem = !string.IsNullOrWhiteSpace(userId) && u.favListItems.Any(d => d.UserId == userId),
                            IsFlagged = u.IsFlagged,
                            SwapListRequest = u.SwapListRequest.Select(f => f.ItemNeededName).ToList()
                        })
                        .Take(limit)
                        .ToListAsync();
                }

                response.Result = candidateItems ?? new List<ListedItemResp>();
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetItemByUserPreviousWantItem");
                response.ErrorMessages = new List<string> { "Error in fetching user preference-based listings" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
            }

            return response;
        }

        public async Task<ResponseDto<List<ListedItemResp>>> GetItemElectronics(string? userId, int limit)
        {
            var response = new ResponseDto<List<ListedItemResp>>();

            try
            {
                // Fetch approved "Electronics" listings with optional limit
                var candidateItems = await _listingItemRepo
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(u => u.ReviewStage == ListingReiviewStage.Approved.ToString() &&
                                u.Category.CategoryName.ToLower() == "electronics" &&
                                (string.IsNullOrEmpty(userId) || u.UserId != userId))
                    .OrderByDescending(u => u.Created)
                    .Select(u => new ListedItemResp
                    {

                        ListingId = u.Id,
                        Username = u.User.UserName,
                        Email = u.User.Email,
                        EstimatedAmount = u.EstimatedAmount,
                        EstimatedCurrency = u.EstimatedCurrency,
                        CategoryName = u.Category.CategoryName,
                        ItemDescription = u.ItemDescription,
                        FullName = u.User.FirstName + " " + u.User.LastName,
                        ItemCondition = u.ItemCondition,
                        ItemName = u.ItemName,
                        ListType = u.ListType,
                        UserId = u.User.Id,
                        PhoneNumber = u.User.PhoneNumber,
                        ProfilePicture = u.User.ProfilePicture,
                        ReviewStage = u.ReviewStage,
                        Media = u.Media.Select(d => new ImageItemDto
                        {
                            Url = d.Url,
                            MediaType = d.MediaType,
                        }).ToList(),
                        IsFavItem = !string.IsNullOrWhiteSpace(userId) && u.favListItems.Any(d => d.UserId == userId),
                        IsFlagged = u.IsFlagged,
                        SwapListRequest = u.SwapListRequest.Select(f => f.ItemNeededName).ToList()
                    })
                    .Take(limit)
                    .ToListAsync();

                response.Result = candidateItems ?? new List<ListedItemResp>();
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetItemElectronics");
                response.ErrorMessages = new List<string> { "Error in fetching electronics listings" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
            }

            return response;
        }
        public async Task<ResponseDto<ListedItemResp>> GetSingleListing(string? userId, string listingId)
        {
            var response = new ResponseDto<ListedItemResp>();

            try
            {

                var candidateItems = await _listingItemRepo
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(u => u.Id == listingId)

                    .Select(u => new ListedItemResp
                    {
                        ListingId = u.Id,
                        Username = u.User.UserName,
                        Email = u.User.Email,
                        EstimatedAmount = u.EstimatedAmount,
                        EstimatedCurrency = u.EstimatedCurrency,
                        CategoryName = u.Category.CategoryName,
                        ItemDescription = u.ItemDescription,
                        FullName = u.User.FirstName + " " + u.User.LastName,
                        ItemCondition = u.ItemCondition,
                        ItemName = u.ItemName,
                        ListType = u.ListType,
                        PhoneNumber = u.User.PhoneNumber,
                        ProfilePicture = u.User.ProfilePicture,
                        ReviewStage = u.ReviewStage,
                        UserId = u.User.Id,

                        Media = u.Media.Select(d => new ImageItemDto
                        {
                            Url = d.Url,
                            MediaType = d.MediaType,
                        }).ToList(),
                        IsFavItem = !string.IsNullOrWhiteSpace(userId) && u.favListItems.Any(d => d.UserId == userId),
                        IsFlagged = u.IsFlagged,
                        SwapListRequest = u.SwapListRequest.Select(f => f.ItemNeededName).ToList()
                    }).FirstOrDefaultAsync();


                var getSwapping = await _swappingProceedingRepo.GetQueryable().Include(u => u.List).Where(u => u.Userid == userId || u.List.UserId == userId).ToListAsync();
                int swappCount = 0;
                int listingCount = 0;
                if (getSwapping.Any())
                {
                    swappCount = getSwapping.Where(u => u.Status == SwapProceedingStatus.Swapped.ToString()).Count();

                }
                var rateUser = await _user_Review_RatingRepo.GetQueryable()
                .AsNoTracking().Where(u => u.UserId == userId).ToListAsync();
                double avgRate = 0;
                if (rateUser.Any())
                {
                    avgRate = rateUser.Average(r => (double)r.RateScore);
                }
                candidateItems.SwapCount = swappCount;
                candidateItems.Rating = avgRate;
                response.Result = candidateItems;
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getsingleListing");
                response.ErrorMessages = new List<string> { "Error in fetching single listings" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
            }

            return response;
        }
        public async Task<ResponseDto<string>> RemoveSingleListing(string? userId, string listingId)
        {
            var response = new ResponseDto<string>();

            try
            {

                var retrieveListing = await _listingItemRepo.GetByIdAsync(listingId);
                if (retrieveListing == null)
                {
                    response.ErrorMessages = new List<string>() { "Invalid ListItem" };
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    return response;
                }
                _listingItemRepo.Delete(retrieveListing);
                await _listingItemRepo.SaveChanges();





                response.Result = "Listing Item deleted successfully";
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting single listing");
                response.ErrorMessages = new List<string> { "Error in deleting single listing" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
            }

            return response;
        }

        public async Task<ResponseDto<PaginatedResult<ListedItemResp>>> SearchpaginatedListing(string? userId, string? listinguserId, string? searhParam, 
            string? categoryId, string? location, decimal lowestRange, decimal highestRange, ListingDateFilter listingDate, int pageNumber, int perpageSize)
        {
            var response = new ResponseDto<PaginatedResult<ListedItemResp>>();

            try
            {
                // safe defaults for pagination
                var page = pageNumber > 0 ? pageNumber : 1;
                var pageSize = perpageSize > 0 ? perpageSize : 10;

                // base query
                var query = _listingItemRepo
                    .GetQueryable()
                    .AsNoTracking()
                    // eager load navigations used in the projection (optional but often prevents client eval)
                    .Include(x => x.User)
                    .Include(x => x.Category)
                    .Include(x => x.Media)
                    .Include(x => x.SwapListRequest)
                    .Include(x => x.favListItems)
                    .Where(x => x.SwapListStatus != SwapListingStatus.Swapped.ToString());

                // filters
                if (!string.IsNullOrWhiteSpace(categoryId))
                    query = query.Where(x => x.CategoryId == categoryId);

                if (!string.IsNullOrWhiteSpace(listinguserId))
                    query = query.Where(x => x.UserId == listinguserId);
                else if (!string.IsNullOrWhiteSpace(userId))
                    query = query.Where(x => x.UserId != userId);

                if (!string.IsNullOrWhiteSpace(searhParam))
                    query = query.Where(x =>
                        x.ItemName.Contains(searhParam) ||
                        x.ItemDescription.Contains(searhParam) ||
                        (x.Category != null && x.Category.CategoryName.Contains(searhParam)));

                if (!string.IsNullOrWhiteSpace(location))
                    query = query.Where(x => x.Location != null && x.Location.Contains(location));

                if (lowestRange > 0)
                    query = query.Where(x => (decimal)x.EstimatedAmount >= lowestRange);

                if (highestRange > 0)
                    query = query.Where(x => (decimal)x.EstimatedAmount <= highestRange);

                // date filter
                if (listingDate != ListingDateFilter.All)
                {
                    DateTime filterDate;
                    switch (listingDate)
                    {
                        case ListingDateFilter.LastWeek:
                            filterDate = DateTime.UtcNow.AddDays(-7);
                            break;
                        case ListingDateFilter.LastMonth:
                            filterDate = DateTime.UtcNow.AddMonths(-1);
                            break;
                        default:
                            filterDate = DateTime.MinValue;
                            break;
                    }

                    query = query.Where(x => x.Created >= filterDate);
                }

                // total count before paging
                var totalCount = await query.CountAsync();

                // fetch page
                var candidateItems = await query
                    .OrderByDescending(u => u.Created)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(u => new ListedItemResp
                    {
                        Username = u.User != null ? u.User.UserName : null,
                        UserId = u.User.Id,
                        Email = u.User != null ? u.User.Email : null,
                        EstimatedAmount = u.EstimatedAmount,
                        EstimatedCurrency = u.EstimatedCurrency,
                        CategoryName = u.Category != null ? u.Category.CategoryName : null,
                        ItemDescription = u.ItemDescription,
                        FullName = u.User != null ? (u.User.FirstName + " " + u.User.LastName).Trim() : null,
                        ItemCondition = u.ItemCondition,
                        ItemName = u.ItemName,
                        ListType = u.ListType,
                        PhoneNumber = u.User != null ? u.User.PhoneNumber : null,
                        ProfilePicture = u.User != null ? u.User.ProfilePicture : null,
                        ReviewStage = u.ReviewStage,
                        Media = u.Media.Select(d => new ImageItemDto
                        {
                            Url = d.Url,
                            MediaType = d.MediaType,
                        }).ToList(),
                        ListingId = u.Id,
                        IsFavItem = !string.IsNullOrEmpty(userId) && u.favListItems.Any(d => d.UserId == userId),
                        IsFlagged = u.IsFlagged,
                        SwapListRequest = u.SwapListRequest.Select(f => f.ItemNeededName).ToList()
                    })
                    .ToListAsync();

                var data = new PaginatedResult<ListedItemResp>
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
                _logger.LogError(ex, "Error in SearchpaginatedListing");
                response.ErrorMessages = new List<string> { "Error fetching listings" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }
        public async Task<ResponseDto<PaginatedResult<ListedItemResp>>> AdminSearchpaginatedListing(string? userId, string? listinguserId, string? searhParam, 
            string? categoryId, string? location, SwapListingStatus listingStatus, ListingReiviewStage reviewStage, decimal lowestRange, decimal highestRange, ListingDateFilter listingDate, int pageNumber, int perpageSize)
        {
            var response = new ResponseDto<PaginatedResult<ListedItemResp>>();

            try
            {
                // safe defaults for pagination
                var page = pageNumber > 0 ? pageNumber : 1;
                var pageSize = perpageSize > 0 ? perpageSize : 10;

                // base query
                var query = _listingItemRepo
                    .GetQueryable()
                    .AsNoTracking();
                    
                // filters
                if (SwapListingStatus.All != listingStatus)
                    query = query.Where(x => x.SwapListStatus == listingStatus.ToString());

                if (ListingReiviewStage.All != reviewStage)
                    query = query.Where(x => x.ReviewStage == reviewStage.ToString());
                if (!string.IsNullOrWhiteSpace(categoryId))
                    query = query.Where(x => x.CategoryId == categoryId);

                if (!string.IsNullOrWhiteSpace(listinguserId))
                    query = query.Where(x => x.UserId == listinguserId);

                if (!string.IsNullOrWhiteSpace(searhParam))
                    query = query.Where(x =>
                        x.ItemName.Contains(searhParam) ||
                        x.ItemDescription.Contains(searhParam) ||
                        (x.Category != null && x.Category.CategoryName.Contains(searhParam)));

                if (!string.IsNullOrWhiteSpace(location))
                    query = query.Where(x => x.Location != null && x.Location.Contains(location));

                if (lowestRange > 0)
                    query = query.Where(x => (decimal)x.EstimatedAmount >= lowestRange);

                if (highestRange > 0)
                    query = query.Where(x => (decimal)x.EstimatedAmount <= highestRange);

                // date filter
                if (listingDate != ListingDateFilter.All)
                {
                    DateTime filterDate;
                    switch (listingDate)
                    {
                        case ListingDateFilter.LastWeek:
                            filterDate = DateTime.UtcNow.AddDays(-7);
                            break;
                        case ListingDateFilter.LastMonth:
                            filterDate = DateTime.UtcNow.AddMonths(-1);
                            break;
                        default:
                            filterDate = DateTime.MinValue;
                            break;
                    }

                    query = query.Where(x => x.Created >= filterDate);
                }

                // total count before paging
                var totalCount = await query.CountAsync();

                // fetch page
                var candidateItems = await query
                    .OrderByDescending(u => u.Created)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(u => new ListedItemResp
                    {
                        Username = u.User != null ? u.User.UserName : null,
                        UserId = u.User.Id,
                        Email = u.User != null ? u.User.Email : null,
                        EstimatedAmount = u.EstimatedAmount,
                        EstimatedCurrency = u.EstimatedCurrency,
                        CategoryName = u.Category != null ? u.Category.CategoryName : null,
                        ItemDescription = u.ItemDescription,
                        FullName = u.User != null ? (u.User.FirstName + " " + u.User.LastName).Trim() : null,
                        ItemCondition = u.ItemCondition,
                        ItemName = u.ItemName,
                        ListType = u.ListType,
                        PhoneNumber = u.User != null ? u.User.PhoneNumber : null,
                        ProfilePicture = u.User != null ? u.User.ProfilePicture : null,
                        ReviewStage = u.ReviewStage,
                        Media = u.Media.Select(d => new ImageItemDto
                        {
                            Url = d.Url,
                            MediaType = d.MediaType,
                        }).ToList(),
                        ListingId = u.Id,
                        IsFavItem = !string.IsNullOrEmpty(userId) && u.favListItems.Any(d => d.UserId == userId),
                        IsFlagged = u.IsFlagged,
                        SwapListRequest = u.SwapListRequest.Select(f => f.ItemNeededName).ToList()
                    })
                    .ToListAsync();

                var data = new PaginatedResult<ListedItemResp>
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
                _logger.LogError(ex, "Error in SearchpaginatedListing");
                response.ErrorMessages = new List<string> { "Error fetching listings" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }
        public async Task<ResponseDto<PaginatedResult<SwapProceedingResp>>> SearchpaginatedListingSwap( string? listinguserId, string? searhParam,
            SwapListingEnumStatus swapListingStatus, ListingDateFilter listingDate, int pageNumber, int perpageSize)
        {
            var response = new ResponseDto<PaginatedResult<SwapProceedingResp>>();

            try
            {
                // safe defaults for pagination
                var page = pageNumber > 0 ? pageNumber : 1;
                var pageSize = perpageSize > 0 ? perpageSize : 10;

                // base query
                var query = _swappingProceedingRepo
                    .GetQueryable()
                    .AsNoTracking();





                // filters
                if (swapListingStatus != SwapListingEnumStatus.All)
                {
                    var stat = swapListingStatus.ToString();
                    query = query.Where(x => x.Status == stat);
                }



                if (!string.IsNullOrWhiteSpace(listinguserId))
                    query = query.Where(x => x.List.UserId == listinguserId || x.Userid == listinguserId);

                if (!string.IsNullOrWhiteSpace(searhParam))
                    query = query.Where(x =>
                        x.List.ItemName.Contains(searhParam) ||
                        x.List.ItemDescription.Contains(searhParam) ||
                        (x.List.Category != null && x.List.Category.CategoryName.Contains(searhParam)));




                // date filter
                if (listingDate != ListingDateFilter.All)
                {
                    DateTime filterDate;
                    switch (listingDate)
                    {
                        case ListingDateFilter.LastWeek:
                            filterDate = DateTime.UtcNow.AddDays(-7);
                            break;
                        case ListingDateFilter.LastMonth:
                            filterDate = DateTime.UtcNow.AddMonths(-1);
                            break;
                        default:
                            filterDate = DateTime.MinValue;
                            break;
                    }

                    query = query.Where(x => x.Created >= filterDate);
                }

                // total count before paging
                var totalCount = await query.CountAsync();

                // fetch page
                var candidateItems = await query
                    .OrderByDescending(u => u.Created)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(u => new SwapProceedingResp
                    {

                        visitorUserId = u.User.Id,
                        CreatedOn = u.List.Created,
                        lastActivity = u.Created,
                        ListedItem = u.List.ItemName,
                        swapperUserId = u.List.User.Id,
                        SwapperRequestItem = string.Join(",",
    u.List.SwapListRequest.Select(s => s.ItemNeededName)
),
                        Status = u.Status,
                        swapperImage = u.List.User.ProfilePicture,
                        swapperName = u.List.User.FirstName + " " + u.List.User.LastName,
                        swapProceedId = u.Id,
                        visitorImage = u.User.ProfilePicture,
                        visitorName = u.User.FirstName + " " + u.User.LastName,
                    }).OrderByDescending(u => u.CreatedOn)
                    .ToListAsync();


                var data = new PaginatedResult<SwapProceedingResp>
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
                _logger.LogError(ex, "Error in SearchpaginatedListingSwap");
                response.ErrorMessages = new List<string> { "Error fetching listings Swap" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }
        public async Task<ResponseDto<SwapProceedingResp>> SingleListingSwapProceed(string swapProceedId)
        {
            var response = new ResponseDto<SwapProceedingResp>();

            try
            {
                var getSingleProceed = await _swappingProceedingRepo.GetQueryable().Where(u => u.Id == swapProceedId)

                    .Select(u => new SwapProceedingResp
                    {

                        visitorUserId = u.User.Id,
                        CreatedOn = u.List.Created,
                        lastActivity = u.Created,
                        ListedItem = u.List.ItemName,
                        swapperUserId = u.List.User.Id,
                        SwapperRequestItem = string.Join(",",
    u.List.SwapListRequest.Select(s => s.ItemNeededName)
),
                        Status = u.Status,
                        swapperImage = u.List.User.ProfilePicture,
                        swapperName = u.List.User.FirstName + " " + u.List.User.LastName,
                        swapProceedId = u.Id,
                        visitorImage = u.User.ProfilePicture,
                        visitorName = u.User.FirstName + " " + u.User.LastName,
                    })
                    .FirstOrDefaultAsync();

                if (getSingleProceed == null)
                {
                    response.ErrorMessages = new List<string> { "Swap proceeding not found" };
                    response.StatusCode = 404;
                    response.DisplayMessage = "Error";
                    return response;
                }

                var checkChatRoomBetweenUser = await _userRoomRepo.GetQueryable().Include(u => u.Room)
                   .FirstOrDefaultAsync(ur =>
                  (ur.UserId == getSingleProceed.visitorUserId && ur.SwapperId == getSingleProceed.swapperUserId) ||
                  (ur.UserId == getSingleProceed.swapperUserId && ur.SwapperId == getSingleProceed.visitorUserId));

                getSingleProceed.RoomName = checkChatRoomBetweenUser?.Room?.RoomName;


                response.Result = getSingleProceed;
                response.StatusCode = 200;
                response.DisplayMessage = "Success";

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SingleListingSwapProceed");
                response.ErrorMessages = new List<string> { "Error fetching Single Listing SwapProceed" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }
        public async Task<ResponseDto<string>> StartSwap(string userId, string listId)
        {

            var response = new ResponseDto<string>();
            try
            {
                var Items = await _listingItemRepo.GetByIdAsync(listId);
                if (Items == null)
                {
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string>() { "Invalid listed item" };
                    response.StatusCode = 400;
                    return response;
                }
                var checkExistingSwapping = await _swappingProceedingRepo
      .GetQueryable()
      .Include(u => u.List)
      .FirstOrDefaultAsync(u =>
          (
              // userId ↔ Items.UserId (either can be swapper or visitor)
              (u.Userid == userId && u.List.UserId == Items.UserId) ||
              (u.Userid == Items.UserId && u.List.UserId == userId)
          )
          // only active proceedings
          && u.Status != SwapProceedingStatus.Swapped.ToString()
          && u.Status != SwapProceedingStatus.Closed.ToString()
      );

                if (checkExistingSwapping != null)
                {
                    var existingRoom = await _userRoomRepo.GetQueryable()
                        .Include(ur => ur.Room)
                        .FirstOrDefaultAsync(ur =>
                            (ur.UserId == userId && ur.SwapperId == Items.UserId) ||
                            (ur.UserId == Items.UserId && ur.SwapperId == userId));

                    var errorMessages = new List<string>
                    {
                        "You already have an ongoing swap with this user. Please complete it before starting a new one.",
                    };

                    if (!string.IsNullOrEmpty(existingRoom?.Room?.RoomName))
                    {
                        errorMessages.Add(existingRoom.Room.RoomName);
                    }

                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = errorMessages;
                    return response;
                }


                var checkChatRoomBetweenUser = await _userRoomRepo.GetQueryable()
                     .FirstOrDefaultAsync(ur =>
                    (ur.UserId == userId && ur.SwapperId == Items.UserId) ||
                    (ur.UserId == Items.UserId && ur.SwapperId == userId));
                if (checkChatRoomBetweenUser == null)
                {
                    var roomUser = await _userRoomRepo.Add(new UserRoom()
                    {
                        UserId = userId,
                        SwapperId = Items.UserId,

                    });
                    var number = _helper.GenerateSecureRandomAlphanumeric(10);
                    await _roomRepo.Add(new Room()
                    {
                        RoomName = number,
                        UserRoomId = roomUser.Id,

                    });


                }
                await _swappingProceedingRepo.Add(new SwappingProceeding()
                {
                    Userid = userId,
                    ListId = listId,

                });
                Items.SwapListStatus = SwapListingStatus.Negotiation.ToString();
                _listingItemRepo.Update(Items);
                await _listingItemRepo.SaveChanges();
                response.Result = "Item swapping started";
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in starting swapping" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }
        public async Task<ResponseDto<string>> CloseSwap(string userId, string swapId)
        {

            var response = new ResponseDto<string>();
            try
            {
                var checkExistingSwapping = await _swappingProceedingRepo.GetByIdAsync(swapId);
                if (checkExistingSwapping == null)
                {
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string>() { "User currently do not have a swapping proceeding ongoing, pls create before closing" };
                    return response;
                }
                var Items = await _listingItemRepo.GetByIdAsync(checkExistingSwapping.ListId);
                if (Items == null)
                {
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string>() { "Invalid listed item" };
                    response.StatusCode = 400;
                    return response;
                }
               

                checkExistingSwapping.Status = SwapProceedingStatus.Closed.ToString();
                Items.SwapListStatus = SwapProceedingStatus.Closed.ToString();
                _swappingProceedingRepo.Update(checkExistingSwapping);
                _listingItemRepo.Update(Items);
                await _listingItemRepo.SaveChanges();
                response.Result = "Item swapping closed";
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in closing swapping" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }
        
        public async Task<ResponseDto<string>> SwitchSwapStatus(string userId, string SwapId, SwapProceedingStatus swapProceedingStatus)
        {

            var response = new ResponseDto<string>();
            try
            {
                var checkExistingSwapping = await _swappingProceedingRepo.GetByIdAsync(SwapId);
                if (checkExistingSwapping == null)
                {
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string>() { "User currently do not have a swapping proceeding ongoing, pls create before closing" };
                    return response;
                }
                var Items = await _listingItemRepo.GetByIdAsync(checkExistingSwapping.ListId);
                if (Items == null)
                {
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string>() { "Invalid listed item" };
                    response.StatusCode = 400;
                    return response;
                }
              

                checkExistingSwapping.Status = swapProceedingStatus.ToString();
                Items.SwapListStatus = swapProceedingStatus.ToString();
                _swappingProceedingRepo.Update(checkExistingSwapping);
                _listingItemRepo.Update(Items);
                await _listingItemRepo.SaveChanges();
                response.Result = $"Item swapping {swapProceedingStatus}";
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in switching swapping proceeding status" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }
        public async Task<ResponseDto<UserDashboardCard>> GetUserDashboardCard(string userId)
        {

            var response = new ResponseDto<UserDashboardCard>();
            try
            {
                var result = new UserDashboardCard();
                var Items = await _listingItemRepo.GetQueryable().
                    Where(u=>u.UserId == userId).CountAsync();
                result.ListedCount = Items;

               
                var checkExistingSwapping = await _swappingProceedingRepo.GetQueryable()
                    .Include(u => u.List)
                    .Where(u => u.Userid == userId || u.List.UserId == userId).ToListAsync();
                if (!checkExistingSwapping.Any())
                {
                    result.CompletedCount = 0;
                    result.PendingConfirmationCount = 0;
                    result.OngoingCount = 0;
                    response.StatusCode = 200;
                    response.DisplayMessage = "Success";
                    response.Result = result;
                    return response;
                }
                result.OngoingCount = checkExistingSwapping
                    .Count(u => u.Status == SwapProceedingStatus.Negotiation.ToString());
                result.PendingConfirmationCount = checkExistingSwapping
                    .Count(u => u.Status == SwapProceedingStatus.AwaitingConfirmation.ToString());
                result.CompletedCount = checkExistingSwapping
                    .Count(u => u.Status == SwapProceedingStatus.Swapped.ToString());
               
              
                response.Result = result;
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in geting user dashboard card stats" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }
        public async Task<ResponseDto<string>> AdminReview(string userId, string listId, ListingReiviewStage review, string? rejectionNote = null)
        {

            var response = new ResponseDto<string>();
            try
            {
                var Items = await _listingItemRepo.GetByIdAsync(listId);
                if (Items == null)
                {
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string>() { "Invalid listed item" };
                    response.StatusCode = 400;
                    return response;
                }

                if (review == ListingReiviewStage.Rejected)
                {
                    if (string.IsNullOrWhiteSpace(rejectionNote))
                    {
                        response.DisplayMessage = "Error";
                        response.ErrorMessages = new List<string>() { "A rejection note is required when rejecting an item" };
                        response.StatusCode = 400;
                        return response;
                    }
                    Items.RejectionNote = rejectionNote;
                }
                else
                {
                    Items.RejectionNote = null;
                }

                Items.ReviewStage = review.ToString();
                _listingItemRepo.Update(Items);
                await _listingItemRepo.SaveChanges();

                await NotifyOwnerOfReviewOutcome(Items, review, rejectionNote);

                response.Result = $"Item reveiw to {review} successfully";
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in reveiwing swapping item" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }

        private async Task NotifyOwnerOfReviewOutcome(ListingItem item, ListingReiviewStage review, string? rejectionNote)
        {
            // email failures must never roll back a completed review
            try
            {
                var owner = await _accountRepo.FindUserByIdAsync(item.UserId);
                if (owner == null || string.IsNullOrWhiteSpace(owner.Email))
                    return;

                var isApproved = review == ListingReiviewStage.Approved;
                var listingUrl = $"{_configuration["FrontendBaseUrl"]}listing/{item.Id}";

                var details = EmailTemplate.DetailTable(
                    ("Item Name", item.ItemName),
                    ("Estimated Value", $"{item.EstimatedCurrency} {item.EstimatedAmount}"),
                    ("Status", review.ToString()));

                string bodyHtml;
                string subject;

                if (isApproved)
                {
                    subject = "Your Listing Has Been Approved";
                    bodyHtml = "<p style=\"margin:0 0 4px 0;\">Good news &mdash; your listing has been reviewed and approved. It is now live and visible to other users.</p>"
                               + details
                               + EmailTemplate.Callout("Approved", "Your item is now available for swap requests.")
                               + "<p style=\"margin:0;\">You can view or manage your listing from your dashboard at any time.</p>";
                }
                else
                {
                    subject = "Your Listing Was Not Approved";
                    bodyHtml = "<p style=\"margin:0 0 4px 0;\">Your listing has been reviewed and unfortunately it was not approved.</p>"
                               + details
                               + EmailTemplate.Callout("Reason for rejection", rejectionNote ?? "No reason was provided.", isNegative: true)
                               + "<p style=\"margin:0;\">Please update your listing to address the points above and submit it again for review.</p>";
                }

                var message = new Message(new[] { owner.Email }, subject, EmailTemplate.Build(
                    title: subject,
                    greetingName: owner.FirstName,
                    bodyHtml: bodyHtml,
                    ctaText: "View Listing",
                    ctaUrl: listingUrl));

                await _emailServices.SendEmailAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to notify owner of review outcome for listing {ListingId}", item.Id);
            }
        }

        public async Task<ResponseDto<string>> FlagContent(string contentId, FlagContentType contentType, bool isFlagged)
        {
            var response = new ResponseDto<string>();
            try
            {
                if (contentType == FlagContentType.Listing)
                {
                    var listing = await _listingItemRepo.GetByIdAsync(contentId);
                    if (listing == null)
                    {
                        response.DisplayMessage = "Error";
                        response.ErrorMessages = new List<string>() { "Listing not found" };
                        response.StatusCode = 400;
                        return response;
                    }
                    listing.IsFlagged = isFlagged;
                    _listingItemRepo.Update(listing);
                    await _listingItemRepo.SaveChanges();
                    if (isFlagged)
                    {
                        await NotifyUserOfFlagAsync(listing.UserId, "Listing", listing.ItemName, contentId);
                    }
                    response.Result = isFlagged ? "Listing flagged successfully" : "Listing unflagged successfully";
                }
                else
                {
                    var swap = await _swappingProceedingRepo.GetByIdAsync(contentId);
                    if (swap == null)
                    {
                        response.DisplayMessage = "Error";
                        response.ErrorMessages = new List<string>() { "Swap not found" };
                        response.StatusCode = 400;
                        return response;
                    }
                    swap.IsFlagged = isFlagged;
                    _swappingProceedingRepo.Update(swap);
                    await _swappingProceedingRepo.SaveChanges();
                    if (isFlagged)
                    {
                        await NotifyUserOfFlagAsync(swap.Userid, "Swap", swap.ListId, contentId);
                    }
                    response.Result = isFlagged ? "Swap flagged successfully" : "Swap unflagged successfully";
                }

                response.StatusCode = 200;
                response.DisplayMessage = "Success";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error flagging content" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }

        private async Task NotifyUserOfFlagAsync(string userId, string contentType, string contentName, string contentId)
        {
            try
            {
                var user = await _accountRepo.FindUserByIdAsync(userId);
                if (user == null || string.IsNullOrWhiteSpace(user.Email))
                    return;

                var body = EmailTemplate.Build(
                    title: $"Your {contentType} Has Been Flagged",
                    greetingName: user.FirstName,
                    bodyHtml: $"<p style=\"margin:0 0 4px 0;\">Your {contentType.ToLowerInvariant()} has been flagged by our moderation team for review.</p>"
                              + EmailTemplate.DetailTable(
                                  ($"{contentType} ID", contentId),
                                  ("Name or Reference", contentName))
                              + EmailTemplate.Callout(
                                  "What happens next",
                                  "Our team will review the content. If action is required, we will contact you with more details.",
                                  isNegative: true)
                              + "<p style=\"margin:0;\">Please ensure that your content follows the SwapCorrect community guidelines.</p>");

                await _emailServices.SendEmailAsync(new Message(
                    new[] { user.Email },
                    $"Your {contentType} Has Been Flagged",
                    body));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to notify user {UserId} that {ContentType} {ContentId} was flagged", userId, contentType, contentId);
            }
        }


    }
}
