using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.FavListItems;
using SwapShop.Domain.Enitities;
using SwapShop.Domain.OtherService.Interface;
using SwapShop.Domain.Repository.Interface;

namespace SwapShop.Infrastructure.OtherService.Implementation
{
    public class FavListItemService : IFavListItemService
    {

        private readonly ISwapShopGenericRepo<FavListItem> _faveListItemRepo;
        private readonly ILogger<FavListItemService> _logger;
        public FavListItemService(ISwapShopGenericRepo<FavListItem> faveListItemRepo, 
            ILogger<FavListItemService> logger)
        {
            _faveListItemRepo = faveListItemRepo;
            _logger = logger;
        }
        public async Task<ResponseDto<string>> AddToFavoritesAsync(string userId, string listingId)
        {
           
            
            var response = new ResponseDto<string>();
            try
            {
                var checkFavAdded = await _faveListItemRepo.GetQueryable()
                    .FirstOrDefaultAsync(f => f.UserId == userId && f.ListId == listingId);
                if(checkFavAdded != null)
                {
                    response.DisplayMessage = "Success";
                    response.Result = "Item already added";
                    response.StatusCode = 200;
                    return response;
                }
                var fav = new FavListItem { UserId = userId, ListId = listingId };
                await _faveListItemRepo.Add(fav);
                await _faveListItemRepo.SaveChanges();
                response.DisplayMessage = "Success";
                response.Result = "Item added successfully";
                response.StatusCode = 200;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in adding user favorites" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
           
        }

        public async Task<ResponseDto<List<FavListItemResponseDto>>> GetUserFavouritesAsync(string userId)
        {

            var response = new ResponseDto<List<FavListItemResponseDto>>();
            try
            {
                var retrieveFavList = await _faveListItemRepo.GetQueryable().Where(u => u.UserId == userId).Select(u => new FavListItemResponseDto
                {
                    ListingId = u.ListId,
                    UserId= u.List.UserId,
                    Username = u.List.User.UserName,
                    Email = u.List.User.Email,
                    EstimatedAmount = u.List.EstimatedAmount,
                    EstimatedCurrency = u.List.EstimatedCurrency,
                    CategoryName = u.List.Category.CategoryName,
                    ItemDescription = u.List.ItemDescription,
                    FullName = u.List.User.FirstName + " " + u.List.User.LastName,
                    ItemCondition = u.List.ItemCondition,
                    ItemName = u.List.ItemName,
                    ListType = u.List.ListType,
                    PhoneNumber = u.List.User.PhoneNumber,
                    ProfilePicture = u.List.User.ProfilePicture,
                    ReviewStage = u.List.ReviewStage,
                    Media = u.List.Media.Select(d => new ImageItemDto
                    {
                        Url = d.Url,
                        MediaType = d.MediaType,
                    }).ToList(),
                    SwapListRequest = u.List.SwapListRequest.Select(f => f.ItemNeededName).ToList()
                }).ToListAsync();
                response.Result = retrieveFavList;
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in fetching Get user favorites" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }

        public async Task<ResponseDto<string>> RemoveFromFavouritesAsync(string userId, string listingId)
        {

            var response = new ResponseDto<string>();
            try
            {
                var checkFavAdded = await _faveListItemRepo.GetQueryable()
                 .FirstOrDefaultAsync(f => f.UserId == userId && f.ListId == listingId);
                if (checkFavAdded == null)
                {
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string>() { "Item not mapped to user already " };
                    response.StatusCode = 400;
                    return response;
                }
                _faveListItemRepo.Delete(checkFavAdded);
                await _faveListItemRepo.SaveChanges();
                response.DisplayMessage = "Success";
                response.Result = "Item remove successfully";
                response.StatusCode = 200;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in delete user favorites" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
           
        }
    }
}
