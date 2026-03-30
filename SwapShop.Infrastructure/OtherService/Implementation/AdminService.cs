using Microsoft.EntityFrameworkCore;
using Swap_Shop.Domain.Entities;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Admin;
using SwapShop.Domain.Enitities;
using SwapShop.Domain.Enum;
using SwapShop.Domain.HelperExtension;
using SwapShop.Domain.OtherService.Interface;
using SwapShop.Infrastructure.Context;
using SwapShop.Infrastructure.Helper;


namespace SwapShop.Infrastructure.OtherService.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly SwapShopContext _context;
        private readonly UserQueryHelper _userQueryHelper;
        public AdminService(SwapShopContext context)
        {
            _context = context;
            _userQueryHelper = new UserQueryHelper(context);

        }
        public async Task<PaginatedResult<RecentActivityDto>> GetRecentActivitiesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = _context.UserActivitylogs
                .OrderByDescending(a => a.Created);

            var totalCount = await query.CountAsync(cancellationToken);
            var activities = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new RecentActivityDto
                {
                    ActivityType = a.ActivitiesType,
                    Description = a.ActivitiesDescription,
                    UserName = a.User.UserName,
                    TimeAgo = a.Created.ToTimeAgo()
                })
                .ToListAsync(cancellationToken);

            return new PaginatedResult<RecentActivityDto>
            {
                Items = activities,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(PeriodicFilter filter)
        {
            var (currentStart, prevStart, prevEnd) = DateRangeHelper.GetDateRanges(filter);
            var now = DateTime.UtcNow;
            int totalUsers = await _context.Users.CountAsync();

            // ----------------------------- ACTIVE USERS -----------------------------
            var activeUsersThis = await _context.Users
                .CountAsync(u => u.EmailConfirmed && u.Created >= currentStart);

            var activeUsersPrev = (filter != PeriodicFilter.AllTime)
                ? await _context.Users.CountAsync(u => u.EmailConfirmed && u.Created >= prevStart && u.Created < prevEnd)
                : 0;

            double activeUsersPercent = activeUsersPrev == 0
                ? 100
                : ((double)(activeUsersThis - activeUsersPrev) / activeUsersPrev) * 100;

            var activeUsers = new DashboardCardDto
            {
                Count = activeUsersThis,
                PercentageChange = Math.Round(activeUsersPercent, 2),
                IsIncrease = activeUsersPercent >= 0
            };

            // ----------------------------- REGISTERED USERS -----------------------------
            var registeredUsersThis = await _context.Users
                .CountAsync(u => !u.EmailConfirmed && u.Created >= currentStart);

            var registeredUsersPrev = (filter != PeriodicFilter.AllTime)
                ? await _context.Users.CountAsync(u => !u.EmailConfirmed && u.Created >= prevStart && u.Created < prevEnd)
                : 0;

            double registeredUsersPercent = registeredUsersPrev == 0
                ? 100
                : ((double)(registeredUsersThis - registeredUsersPrev) / registeredUsersPrev) * 100;

            var registeredUsers = new DashboardCardDto
            {
                Count = registeredUsersThis,
                PercentageChange = Math.Round(registeredUsersPercent, 2),
                IsIncrease = registeredUsersPercent >= 0
            };

            // ----------------------------- COMPLETED SWAPS -----------------------------
            var completedSwapsThis = await _context.ListingItems
                .CountAsync(l => l.SwapListStatus == SwapListingStatus.Swapped.ToString()
                                 && l.Created >= currentStart);

            var completedSwapsPrev = (filter != PeriodicFilter.AllTime)
                ? await _context.ListingItems.CountAsync(l => l.SwapListStatus == SwapListingStatus.Swapped.ToString()
                                                              && l.Created >= prevStart && l.Created < prevEnd)
                : 0;

            double completedSwapsPercent = completedSwapsPrev == 0
                ? 100
                : ((double)(completedSwapsThis - completedSwapsPrev) / completedSwapsPrev) * 100;

            var completedSwaps = new DashboardCardDto
            {
                Count = completedSwapsThis,
                PercentageChange = Math.Round(completedSwapsPercent, 2),
                IsIncrease = completedSwapsPercent >= 0
            };

            // ----------------------------- ACTIVE SWAPS -----------------------------
            var activeSwapsThis = await _context.ListingItems
                .CountAsync(l => (l.SwapListStatus == SwapListingStatus.Published.ToString() ||
                                  l.SwapListStatus == SwapListingStatus.Negotiation.ToString())
                                 && l.Created >= currentStart);

            var activeSwapsPrev = (filter != PeriodicFilter.AllTime)
                ? await _context.ListingItems.CountAsync(l =>
                    (l.SwapListStatus == SwapListingStatus.Published.ToString() ||
                     l.SwapListStatus == SwapListingStatus.Negotiation.ToString()) &&
                    l.Created >= prevStart && l.Created < prevEnd)
                : 0;

            double activeSwapsPercent = activeSwapsPrev == 0
                ? 100
                : ((double)(activeSwapsThis - activeSwapsPrev) / activeSwapsPrev) * 100;

            var activeSwaps = new DashboardCardDto
            {
                Count = activeSwapsThis,
                PercentageChange = Math.Round(activeSwapsPercent, 2),
                IsIncrease = activeSwapsPercent >= 0
            };

            // ----------------------------- SWAPPER & VISITOR -----------------------------
            var currentSwapperIds = await _context.ListingItems
                .Where(l => l.Created >= currentStart)
                .Select(l => l.UserId)
                .Distinct()
                .ToListAsync();

            int currentSwappers = currentSwapperIds.Count;
            int currentVisitors = totalUsers - currentSwappers;

            int prevSwappers = 0;
            int prevVisitors = 0;

            if (filter != PeriodicFilter.AllTime)
            {
                var prevSwapperIds = await _context.ListingItems
                    .Where(l => l.Created >= prevStart && l.Created < prevEnd)
                    .Select(l => l.UserId)
                    .Distinct()
                    .ToListAsync();

                prevSwappers = prevSwapperIds.Count;
                prevVisitors = totalUsers - prevSwappers;
            }

            double swapperChange = prevSwappers == 0 ? 100 : ((double)(currentSwappers - prevSwappers) / prevSwappers) * 100;
            double visitorChange = prevVisitors == 0 ? 100 : ((double)(currentVisitors - prevVisitors) / prevVisitors) * 100;

            var swapper = new DashboardCardDto
            {
                Count = currentSwappers,
                PercentageChange = Math.Round(swapperChange, 2),
                IsIncrease = swapperChange >= 0
            };

            var visitor = new DashboardCardDto
            {
                Count = currentVisitors,
                PercentageChange = Math.Round(visitorChange, 2),
                IsIncrease = visitorChange >= 0
            };

            // ----------------------------- FINAL RETURN -----------------------------
            return new DashboardSummaryDto
            {
                ActiveUsers = activeUsers,
                RegisteredUsers = registeredUsers,
                CompletedSwaps = completedSwaps,
                ActiveSwaps = activeSwaps,
                Swapper = swapper,
                Visitor = visitor
            };
        }
        public async Task<AdvancedAnalyticsDto> GetAdvancedAnalyticsAsync()
        {
            var totalUsers = await _context.Users.CountAsync();
            var activeUsers = await _context.Users.CountAsync(u => u.EmailConfirmed);

            var totalListings = await _context.ListingItems.CountAsync();
            var approvedListings = await _context.ListingItems.CountAsync(l => l.ReviewStage == ListingReiviewStage.Approved.ToString());

            var activeSwaps = await _context.ListingItems
                .CountAsync(l => l.SwapListStatus == SwapListingStatus.Negotiation.ToString() || l.SwapListStatus == SwapListingStatus.Published.ToString());

            var completedSwaps = await _context.ListingItems
                .CountAsync(l => l.SwapListStatus == SwapListingStatus.Swapped.ToString());

            // Calculate percentages relative to totals
            var metrics = new List<AnalyticsMetricDto>
            {
                new() { Name = "Active Users", Percentage = totalUsers == 0 ? 0 : (double)activeUsers / totalUsers * 100 },
                new() { Name = "Approved Listings", Percentage = totalListings == 0 ? 0 : (double)approvedListings / totalListings * 100 },
                new() { Name = "Active Swaps", Percentage = totalListings == 0 ? 0 : (double)activeSwaps / totalListings * 100 },
                new() { Name = "Completed Swaps", Percentage = totalListings == 0 ? 0 : (double)completedSwaps / totalListings * 100 },
                new() { Name = "Total Listings", Percentage = 100 } // Always 100% baseline
            };

            // Monthly breakdown for bar chart (Jan–Dec)
            var monthlySwaps = await _context.ListingItems
                .GroupBy(l => l.Created.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToListAsync();

            var monthDict = Enumerable.Range(1, 12).ToDictionary(
                m => new DateTime(2000, m, 1).ToString("MMM"),
                m => monthlySwaps.FirstOrDefault(x => x.Month == m)?.Count ?? 0
            );

            return new AdvancedAnalyticsDto
            {
                Metrics = metrics,
                MonthlySwaps = monthDict
            };
        }

        public async Task<PaginatedResult<UserListDto>> GetUsersAsync(PaginationFilterDto filter, AdminFilterType filterType, int days = 30)
        {
            var q = _context.Users.AsQueryable();

            switch (filterType)
            {
                case AdminFilterType.Active:
                    q = q.Where(u => u.EmailConfirmed && !u.IsSuspend);
                    break;

                case AdminFilterType.Suspended:
                    q = q.Where(u => u.IsSuspend);
                    break;

                case AdminFilterType.Flagged:
                    q = q.Where(u => u.IsFlag);
                    break;

                case AdminFilterType.NewSignups:
                    var cutoff = DateTime.UtcNow.AddDays(-days);
                    q = q.Where(u => u.Created >= cutoff);
                    break;

                case AdminFilterType.All:
                default:
                    // no filtering
                    break;
            }

            return await _userQueryHelper.ExecuteUserListAsync(q, filter);
        }


        public async Task<PaginatedResult<SwapActivityDto>> GetSwapsActivityAsync(int pageNumber, int pageSize, PeriodicFilter filter)
        {
            var query = _context.UserRooms
                .Include(ur => ur.User)
                .Include(ur => ur.Swapper)
                .Include(ur => ur.Room).AsQueryable();

            DateTime startDate = DateTime.MinValue;
            DateTime now = DateTime.UtcNow;

            // 🔹 Apply Period Filter
            switch (filter)
            {
                case PeriodicFilter.Today:
                    startDate = now.Date;
                    query = query.Where(x => x.Created >= startDate);
                    break;

                case PeriodicFilter.ThisWeek:
                    int diff = (7 + (int)now.DayOfWeek - (int)DayOfWeek.Monday) % 7;
                    startDate = now.AddDays(-diff).Date;
                    query = query.Where(x => x.Created >= startDate);
                    break;

                case PeriodicFilter.ThisMonth:
                    startDate = new DateTime(now.Year, now.Month, 1);
                    query = query.Where(x => x.Created >= startDate);
                    break;

                case PeriodicFilter.AllTime:
                default:
                    break;
            }

            var totalCount = await query.CountAsync();

            // 🔹 Get paged UserRoom list
            var userRoomList = await query
                .OrderByDescending(x => x.Created)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // 🔹 Collect all user IDs
            var userIds = userRoomList.Select(x => x.UserId).Distinct().ToList();
            var swapperIds = userRoomList.Select(x => x.SwapperId).Distinct().ToList();
            var allUserIds = userIds.Concat(swapperIds).Distinct().ToList();

            // 🔹 Get latest item per user
            var userItems = await _context.ListingItems
                .Where(l => allUserIds.Contains(l.UserId))
                .GroupBy(l => l.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    ItemName = g.OrderByDescending(i => i.Created)
                                .Select(i => i.ItemName)
                                .FirstOrDefault()
                })
                .ToListAsync();

            var userItemsDict = userItems.ToDictionary(x => x.UserId, x => x.ItemName);

            // 🔹 Get latest status per user
            var swapperActivities = await _context.SwappingProceedings
                .Where(s => allUserIds.Contains(s.Userid))
                .GroupBy(s => s.Userid)
                .Select(g => new
                {
                    UserId = g.Key,
                    LatestStatus = g.OrderByDescending(s => s.Created)
                                    .Select(s => s.Status)
                                    .FirstOrDefault()
                })
                .ToListAsync();

            var statusDict = swapperActivities.ToDictionary(x => x.UserId, x => x.LatestStatus);

            // 🔹 Build result list
            var swaps = userRoomList.Select(x => new SwapActivityDto
            {
                OwnerName = $"{x.User.FirstName} {x.User.LastName}",
                SwapperName = $"{x.Swapper.FirstName} {x.Swapper.LastName}",
                OwnerItem = userItemsDict.ContainsKey(x.UserId) ? userItemsDict[x.UserId] : "N/A",
                SwapperItem = userItemsDict.ContainsKey(x.SwapperId) ? userItemsDict[x.SwapperId] : "N/A",
                Status = statusDict.ContainsKey(x.UserId) ? statusDict[x.UserId] :
                         statusDict.ContainsKey(x.SwapperId) ? statusDict[x.SwapperId] : "Negotiating",
                InitiatedOn = x.Created,

                // 🔹 Since messages are removed, we can default LastActivity to swap creation date
                LastActivity = x.DateUpdated ?? x.Created
            }).ToList();

            // 🔹 Return paginated result
            return new PaginatedResult<SwapActivityDto>
            {
                Items = swaps,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public async Task<SwapDetailsDto> GetSwapDetailsAsync(string userRoomId)
        {
            var userRoom = await _context.UserRooms
                .Include(ur => ur.User)
                .Include(ur => ur.Swapper)
                .Include(ur => ur.Room)
                    .ThenInclude(r => r.Messages)
                        .ThenInclude(m => m.SentBy)
                .FirstOrDefaultAsync(ur => ur.Id == userRoomId);

            if (userRoom == null)
                throw new Exception("Swap not found.");

            // Owner & swapper items
            var ownerItem = await _context.ListingItems
                .Where(i => i.UserId == userRoom.UserId)
                .OrderByDescending(i => i.Created)
                .FirstOrDefaultAsync();

            var swapperItem = await _context.ListingItems
                .Where(i => i.UserId == userRoom.SwapperId)
                .OrderByDescending(i => i.Created)
                .FirstOrDefaultAsync();

            var dto = new SwapDetailsDto
            {
                SwapId = userRoom.Id,
                Status = "Negotiating",
                InitiatedOn = userRoom.Created,
                LastActivity = userRoom.Room?.Messages?.OrderByDescending(m => m.Created).FirstOrDefault()?.Created,

                // Owner Info
                OwnerId = userRoom.UserId,
                OwnerFullName = $"{userRoom.User.FirstName} {userRoom.User.LastName}",
                OwnerProfilePicture = userRoom.User.ProfilePicture,
                OwnerItemId = ownerItem?.Id,
                OwnerItemName = ownerItem?.ItemName,
                OwnerItemImageUrl = ownerItem?.Media?.FirstOrDefault()?.Url,

                // Swapper Info
                SwapperId = userRoom.SwapperId,
                SwapperFullName = $"{userRoom.Swapper.FirstName} {userRoom.Swapper.LastName}",
                SwapperProfilePicture = userRoom.Swapper.ProfilePicture,
                SwapperItemId = swapperItem?.Id,
                SwapperItemName = swapperItem?.ItemName,
                SwapperItemImageUrl = swapperItem?.Media?.FirstOrDefault()?.Url,

                // Chat
                ChatHistory = userRoom.Room?.Messages?
                    .OrderBy(m => m.Created)
                    .Select(m => new SwapChatDto
                    {
                        SenderId = m.SentById,
                        SenderName = $"{m.SentBy.FirstName} {m.SentBy.LastName}",
                        Message = m.Message,
                        SentAt = m.Created,
                        Seen = m.ReadCount.Any()
                    })
                    .ToList() ?? new List<SwapChatDto>()
            };

            return dto;
        }



        /* public async Task<PaginatedResult<SwapActivityDto>> GetSwapsActivityAsync(int pageNumber, int pageSize, PeriodicFilter filter)
         {
             var query = _context.UserRooms
                 .Include(u => u.User)
                 .Include(u => u.Swapper)
                 .Include(u => u.Room)
                     .ThenInclude(r => r.Messages)
                 .OrderByDescending(u => u.Created)
                 .AsQueryable();

             // Optional date filter
             if (!string.IsNullOrEmpty(filter))
             {
                 DateTime startDate = DateTime.UtcNow;

                 switch (filter.ToLower())
                 {
                     case "today":
                         startDate = DateTime.UtcNow.Date;
                         query = query.Where(x => x.Created >= startDate);
                         break;
                     case "thisweek":
                         startDate = DateTime.UtcNow.AddDays(-7);
                         query = query.Where(x => x.Created >= startDate);
                         break;
                     case "thismonth":
                         startDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
                         query = query.Where(x => x.Created >= startDate);
                         break;
                 }
             }

             var totalCount = await query.CountAsync();

             var swaps = await query
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .Select(x => new SwapActivityDto
                 {
                     OwnerName = $"{x.User.FirstName} {x.User.LastName}",
                     SwapperName = $"{x.Swapper.FirstName} {x.Swapper.LastName}",
                     OwnerItem = _context.ListingItems.FirstOrDefault(l => l.UserId == x.UserId)!.ItemName,
                     SwapperItem = _context.ListingItems.FirstOrDefault(l => l.UserId == x.SwapperId)!.ItemName,
                     Status = _context.SwappingProceedings
                         .Where(s => s.Userid == x.UserId || s.Userid == x.SwapperId)
                         .OrderByDescending(s => s.Created)
                         .Select(s => s.Status)
                         .FirstOrDefault() ?? "Negotiating",
                     InitiatedOn = x.Created,
                     LastActivity = x.Room.Messages.OrderByDescending(m => m.Created).Select(m => m.Created).FirstOrDefault()
                 })
                 .ToListAsync();

             return new PaginatedResult<SwapActivityDto>
             {
                 Items = swaps,
                 TotalCount = totalCount,
                 PageNumber = pageNumber,
                 PageSize = pageSize,
                 TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
             };
         }*/


        /* public async Task<DashboardSummaryDto> SwapperAndVisitor(PeriodicFilter filter)IeeTek
         * 
         {
             var (currentStart, prevStart, prevEnd) = DateRangeHelper.GetDateRanges(filter);

             // --- Total users (needed for Visitor calculation)
             int totalUsers = await _context.Users.CountAsync();

             // --- Current Swappers
             var currentSwapperIds = await _context.ListingItems
                 .Where(l => l.Created >= currentStart)
                 .Select(l => l.UserId)
                 .Distinct()
                 .ToListAsync();

             int currentSwappers = currentSwapperIds.Count;
             int currentVisitors = totalUsers - currentSwappers;

             // --- Previous Period
             int prevSwappers = 0;
             int prevVisitors = 0;

             if (filter != PeriodicFilter.AllTime)
             {
                 var prevSwapperIds = await _context.ListingItems
                     .Where(l => l.Created >= prevStart && l.Created < prevEnd)
                     .Select(l => l.UserId)
                     .Distinct()
                     .ToListAsync();

                 prevSwappers = prevSwapperIds.Count;
                 prevVisitors = totalUsers - prevSwappers;
             }

             // --- Percentage changes
             double swapperChange = 0;
             double visitorChange = 0;
             bool swapperIncrease = false;
             bool visitorIncrease = false;

             if (prevSwappers > 0)
             {
                 swapperChange = ((double)(currentSwappers - prevSwappers) / prevSwappers) * 100;
                 swapperIncrease = currentSwappers >= prevSwappers;
             }

             if (prevVisitors > 0)
             {
                 visitorChange = ((double)(currentVisitors - prevVisitors) / prevVisitors) * 100;
                 visitorIncrease = currentVisitors >= prevVisitors;
             }

             return new DashboardSummaryDto
             {
                 Swapper = new DashboardCardDto
                 {
                     Count = currentSwappers,
                     PercentageChange = Math.Round(swapperChange, 2),
                     IsIncrease = swapperIncrease
                 },
                 Visitor = new DashboardCardDto
                 {
                     Count = currentVisitors,
                     PercentageChange = Math.Round(visitorChange, 2),
                     IsIncrease = visitorIncrease
                 }
             };
         }*/



        /* public async Task<DashboardCardDto> TotalSwapperAsync(PeriodicFilter filter)
         {
             var (currentStart, prevStart, prevEnd) = DateRangeHelper.GetDateRanges(filter);

             var currentSwapperIds = await _context.ListingItems
                 .Where(l => l.Created >= currentStart)
                 .Select(l => l.UserId)
                 .Distinct()
                 .ToListAsync();

             int currentSwappers = currentSwapperIds.Count;

             int prevSwappers = 0;
             if (filter != PeriodicFilter.AllTime)
             {
                 var prevSwapperIds = await _context.ListingItems
                     .Where(l => l.Created >= prevStart && l.Created < prevEnd)
                     .Select(l => l.UserId)
                     .Distinct()
                     .ToListAsync();
                 prevSwappers = prevSwapperIds.Count;
             }

             double percentageChange = 0;
             bool isIncrease = false;

             if (prevSwappers > 0)
             {
                 percentageChange = ((double)(currentSwappers - prevSwappers) / prevSwappers) * 100;
                 isIncrease = currentSwappers >= prevSwappers;
             }

             return new DashboardCardDto
             {
                 Count = currentSwappers,
                 PercentageChange = Math.Round(percentageChange, 2),
                 IsIncrease = isIncrease
             };
         }

         public async Task<DashboardCardDto> TotalVisitorAsync(PeriodicFilter filter)
         {
             var (currentStart, prevStart, prevEnd) = DateRangeHelper.GetDateRanges(filter);

             int totalUsers = await _context.Users.CountAsync();

             // --- Current Visitors
             var currentSwapperIds = await _context.ListingItems
                 .Where(l => l.Created >= currentStart)
                 .Select(l => l.UserId)
                 .Distinct()
                 .ToListAsync();

             int currentVisitors = totalUsers - currentSwapperIds.Count;

             // --- Previous Visitors
             int prevVisitors = 0;
             if (filter != PeriodicFilter.AllTime)
             {
                 var prevSwapperIds = await _context.ListingItems
                     .Where(l => l.Created >= prevStart && l.Created < prevEnd)
                     .Select(l => l.UserId)
                     .Distinct()
                     .ToListAsync();

                 prevVisitors = totalUsers - prevSwapperIds.Count;
             }

             double percentageChange = 0;
             bool isIncrease = false;

             if (prevVisitors > 0)
             {
                 percentageChange = ((double)(currentVisitors - prevVisitors) / prevVisitors) * 100;
                 isIncrease = currentVisitors >= prevVisitors;
             }

             return new DashboardCardDto
             {
                 Count = currentVisitors,
                 PercentageChange = Math.Round(percentageChange, 2),
                 IsIncrease = isIncrease
             };
         }
    */


        /*  public Task<PaginatedResult<UserListDto>> GetAllUsersAsync(PaginationFilterDto filter)
          {
              var q = _context.Users.AsQueryable();
              return _userQueryHelper.ExecuteUserListAsync(q, filter);
          }

          public Task<PaginatedResult<UserListDto>> GetActiveUsersAsync(PaginationFilterDto filter)
          {
              var q = _context.Users.Where(u => u.EmailConfirmed && !u.IsSuspend);
              return _userQueryHelper.ExecuteUserListAsync(q, filter);
          }

          public Task<PaginatedResult<UserListDto>> GetSuspendedUsersAsync(PaginationFilterDto filter)
          {
              var q = _context.Users.Where(u => u.IsSuspend);
              return _userQueryHelper.ExecuteUserListAsync(q, filter);
          }

          public Task<PaginatedResult<UserListDto>> GetFlaggedUsersAsync(PaginationFilterDto filter)
          {
              var q = _context.Users.Where(u => u.IsFlag);
              return _userQueryHelper.ExecuteUserListAsync(q, filter);
          }

          public Task<PaginatedResult<UserListDto>> GetNewSignupsAsync(PaginationFilterDto filter, int days = 30)
          {
              var cutoff = DateTime.UtcNow.AddDays(-days);
              var q = _context.Users.Where(u => u.Created >= cutoff);
              return _userQueryHelper.ExecuteUserListAsync(q, filter);
          }
    */

        /* public async Task<DashboardCardDto> SwapperAndVisitor(DashboardMetricType metricType, PeriodicFilter filter)
        {
            var (currentStart, prevStart, prevEnd) = DateRangeHelper.GetDateRanges(filter);

            // --- Total users (needed for Visitor calculation)
            int totalUsers = await _context.Users.CountAsync();

            // --- Current Swappers
            var currentSwapperIds = await _context.ListingItems
                .Where(l => l.Created >= currentStart)
                .Select(l => l.UserId)
                .Distinct()
                .ToListAsync();

            int currentSwappers = currentSwapperIds.Count;

            // --- Current Metric
            int currentValue = metricType switch
            {
                DashboardMetricType.Swapper => currentSwappers,
                DashboardMetricType.Visitor => totalUsers - currentSwappers,
                _ => 0
            };

            // --- Previous Period
            int prevValue = 0;
            if (filter != PeriodicFilter.AllTime)
            {
                var prevSwapperIds = await _context.ListingItems
                    .Where(l => l.Created >= prevStart && l.Created < prevEnd)
                    .Select(l => l.UserId)
                    .Distinct()
                    .ToListAsync();

                int prevSwappers = prevSwapperIds.Count;

                prevValue = metricType switch
                {
                    DashboardMetricType.Swapper => prevSwappers,
                    DashboardMetricType.Visitor => totalUsers - prevSwappers,
                    _ => 0
                };
            }

            // --- Percentage Change
            double percentageChange = 0;
            bool isIncrease = false;

            if (prevValue > 0)
            {
                percentageChange = ((double)(currentValue - prevValue) / prevValue) * 100;
                isIncrease = currentValue >= prevValue;
            }

            return new DashboardCardDto
            {
                Count = currentValue,
                PercentageChange = Math.Round(percentageChange, 2),
                IsIncrease = isIncrease
            };
        }*/

        /* public async Task<DashboardCardDto> TotalActiveUserAsync()
       {
           //return await _context.Users.CountAsync(u => u.EmailConfirmed);
           var now = DateTime.UtcNow;

           var thisMonth = await _context.Users
               .CountAsync(u => u.EmailConfirmed && u.Created.Month == now.Month);

           var lastMonth = await _context.Users
               .CountAsync(u => u.EmailConfirmed && u.Created.Month == now.AddMonths(-1).Month);

           var percentage = lastMonth == 0 ? 100 : ((double)(thisMonth - lastMonth) / lastMonth) * 100;

           return new DashboardCardDto
           {
               Count = thisMonth,
               PercentageChange = Math.Round(percentage, 2),// can be negative or positive// percentage
               IsIncrease = percentage >= 0
           };
       }
       public async Task<DashboardCardDto> TotalRegisterUserAsync()
       {
           // return await _context.Users.CountAsync(u => !u.EmailConfirmed);
           var today = DateTime.UtcNow.Date;

           var todayCount = await _context.Users
               .CountAsync(u => !u.EmailConfirmed && u.Created.Date == today);

           var weekCount = await _context.Users
               .CountAsync(u => !u.EmailConfirmed && u.Created >= today.AddDays(-7));

           // Compare today vs yesterday for percentage
           var yesterdayCount = await _context.Users
               .CountAsync(u => u.Created.Date == today.AddDays(-1));

           var percentage = yesterdayCount == 0 ? 100 : ((double)(todayCount - yesterdayCount) / yesterdayCount) * 100;

           return new DashboardCardDto
           {
               Count = todayCount,
               PercentageChange = Math.Round(percentage, 2), // can be negative or positive//percentage
               IsIncrease = percentage >= 0
           };
       }

       public async Task<DashboardCardDto> GetCompletedSwapsAsync()
       {
           var now = DateTime.UtcNow;

           var thisMonth = await _context.ListingItems
               .CountAsync(l => l.SwapListStatus == SwapListingStatus.Swapped.ToString() && l.Created.Month == now.Month);

           var lastMonth = await _context.ListingItems
               .CountAsync(l => l.SwapListStatus == SwapListingStatus.Swapped.ToString() && l.Created.Month == now.AddMonths(-1).Month);

           var percentage = lastMonth == 0 ? 100 : ((double)(thisMonth - lastMonth) / lastMonth) * 100;

           return new DashboardCardDto
           {
               Count = thisMonth,
               PercentageChange = Math.Round(percentage, 2),
               IsIncrease = percentage >= 0
           };
       }

       public async Task<DashboardCardDto> GetActiveSwapsAsync()
       {
           var now = DateTime.UtcNow;

           var thisMonth = await _context.ListingItems
               .CountAsync(l => (l.SwapListStatus == SwapListingStatus.Published.ToString() || l.SwapListStatus == SwapListingStatus.Negotiation.ToString())
                                && l.Created.Month == now.Month);

           var lastMonth = await _context.ListingItems
               .CountAsync(l => (l.SwapListStatus == SwapListingStatus.Published.ToString() || l.SwapListStatus == SwapListingStatus.Negotiation.ToString())
                                && l.Created.Month == now.AddMonths(-1).Month);

           var percentage = lastMonth == 0 ? 100 : ((double)(thisMonth - lastMonth) / lastMonth) * 100;

           return new DashboardCardDto
           {
               Count = thisMonth,
               PercentageChange = Math.Round(percentage, 2),
               IsIncrease = percentage >= 0
           };
       }*/

        /*public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
        {
            var now = DateTime.UtcNow;
            var today = now.Date;

            // --- Total Active Users (Email Confirmed)
            var activeUsersThisMonth = await _context.Users
                .CountAsync(u => u.EmailConfirmed && u.Created.Month == now.Month);

            var activeUsersLastMonth = await _context.Users
                .CountAsync(u => u.EmailConfirmed && u.Created.Month == now.AddMonths(-1).Month);

            double activeUserPercent = activeUsersLastMonth == 0
                ? 100
                : ((double)(activeUsersThisMonth - activeUsersLastMonth) / activeUsersLastMonth) * 100;

            var activeUsers = new DashboardCardDto
            {
                Count = activeUsersThisMonth,
                PercentageChange = Math.Round(activeUserPercent, 2),
                IsIncrease = activeUserPercent >= 0
            };

            // --- Total Registered Users (Not Confirmed)
            var todayRegistered = await _context.Users
                .CountAsync(u => !u.EmailConfirmed && u.Created.Date == today);

            var yesterdayRegistered = await _context.Users
                .CountAsync(u => u.Created.Date == today.AddDays(-1));

            double registerPercent = yesterdayRegistered == 0
                ? 100
                : ((double)(todayRegistered - yesterdayRegistered) / yesterdayRegistered) * 100;

            var registeredUsers = new DashboardCardDto
            {
                Count = todayRegistered,
                PercentageChange = Math.Round(registerPercent, 2),
                IsIncrease = registerPercent >= 0
            };

            // --- Completed Swaps
            var completedThisMonth = await _context.ListingItems
                .CountAsync(l => l.SwapListStatus == SwapListingStatus.Swapped.ToString()
                                 && l.Created.Month == now.Month);

            var completedLastMonth = await _context.ListingItems
                .CountAsync(l => l.SwapListStatus == SwapListingStatus.Swapped.ToString()
                                 && l.Created.Month == now.AddMonths(-1).Month);

            double completedPercent = completedLastMonth == 0
                ? 100
                : ((double)(completedThisMonth - completedLastMonth) / completedLastMonth) * 100;

            var completedSwaps = new DashboardCardDto
            {
                Count = completedThisMonth,
                PercentageChange = Math.Round(completedPercent, 2),
                IsIncrease = completedPercent >= 0
            };

            // --- Active Swaps
            var activeThisMonth = await _context.ListingItems
                .CountAsync(l => (l.SwapListStatus == SwapListingStatus.Published.ToString() ||
                                  l.SwapListStatus == SwapListingStatus.Negotiation.ToString())
                                 && l.Created.Month == now.Month);

            var activeLastMonth = await _context.ListingItems
                .CountAsync(l => (l.SwapListStatus == SwapListingStatus.Published.ToString() ||
                                  l.SwapListStatus == SwapListingStatus.Negotiation.ToString())
                                 && l.Created.Month == now.AddMonths(-1).Month);

            double activeSwapPercent = activeLastMonth == 0
                ? 100
                : ((double)(activeThisMonth - activeLastMonth) / activeLastMonth) * 100;

            var activeSwaps = new DashboardCardDto
            {
                Count = activeThisMonth,
                PercentageChange = Math.Round(activeSwapPercent, 2),
                IsIncrease = activeSwapPercent >= 0
            };

            // --- Return consolidated DTO
            return new DashboardSummaryDto
            {
                ActiveUsers = activeUsers,
                RegisteredUsers = registeredUsers,
                CompletedSwaps = completedSwaps,
                ActiveSwaps = activeSwaps
            };
        }*/

    }
}
