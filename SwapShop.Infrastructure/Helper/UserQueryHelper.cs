using Microsoft.EntityFrameworkCore;
using Swap_Shop.Domain.Entities;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Admin;
using SwapShop.Domain.Enum;
using SwapShop.Infrastructure.Context;


namespace SwapShop.Infrastructure.Helper
{
    public class UserQueryHelper
    {
        private readonly SwapShopContext _context;
        public UserQueryHelper(SwapShopContext context)
        {
            _context = context;
        }

        public IQueryable<ApplicationUser> ApplySearchAndBase(PaginationFilterDto filter, IQueryable<ApplicationUser> query)
        {
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var term = filter.Search.Trim();
                query = query.Where(u =>
                    (u.FirstName + " " + u.LastName).Contains(term) ||
                    u.Email.Contains(term));
            }

            return query;
        }

        public IQueryable<UserListDto> BuildUserListProjection(IQueryable<ApplicationUser> usersQuery)
        {
            var ratingQ = _context.User_Review_Ratings;
            var listingQ = _context.ListingItems;

            return usersQuery.Select(u => new UserListDto
            {
                Name = (u.FirstName + " " + u.LastName).Trim(),
                RatingScore = (int)Math.Round(
                    ratingQ.Where(r => r.UserId == u.Id)
                           .Select(r => (double?)r.RateScore)
                           .Average() ?? 0.0
                ),
                UserId = u.Id,
                Status = u.IsSuspend ? "Suspended" : (u.EmailConfirmed ? "Active" : "Inactive"),
                SwapCompleted = listingQ.Count(l => l.UserId == u.Id && l.SwapListStatus == SwapListingStatus.Swapped.ToString()),
                DateJoined = u.Created,
                UserRole = listingQ.Any(l => l.UserId == u.Id) ? "Swapper" : "Visitor",
                LastActive = u.LastLoginTime,
                ProfilePicture = u.ProfilePicture
            });
        }

        public IQueryable<UserListDto> ApplySorting(IQueryable<UserListDto> query, PaginationFilterDto filter)
        {
            if (string.IsNullOrWhiteSpace(filter.SortBy))
                return filter.SortDesc ? query.OrderByDescending(u => u.DateJoined) : query.OrderBy(u => u.DateJoined);

            return filter.SortBy.ToLower() switch
            {
                "name" => filter.SortDesc ? query.OrderByDescending(u => u.Name) : query.OrderBy(u => u.Name),
                "rating" => filter.SortDesc ? query.OrderByDescending(u => u.RatingScore) : query.OrderBy(u => u.RatingScore),
                "swaps" => filter.SortDesc ? query.OrderByDescending(u => u.SwapCompleted) : query.OrderBy(u => u.SwapCompleted),
                "date" => filter.SortDesc ? query.OrderByDescending(u => u.DateJoined) : query.OrderBy(u => u.DateJoined),
                _ => filter.SortDesc ? query.OrderByDescending(u => u.DateJoined) : query.OrderBy(u => u.DateJoined),
            };
        }

        public async Task<PaginatedResult<UserListDto>> ExecuteUserListAsync(IQueryable<ApplicationUser> baseQuery, PaginationFilterDto filter)
        {
            baseQuery = ApplySearchAndBase(filter, baseQuery);

            var projected = BuildUserListProjection(baseQuery);

            var totalCount = await projected.CountAsync();

            var sorted = ApplySorting(projected, filter);

            var items = await sorted
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PaginatedResult<UserListDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }
    }
}
