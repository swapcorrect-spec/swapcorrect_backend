using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Enum
{
    public enum ListingReiviewStage
    {
        All,
        Pending,
        Approved,
        Rejected
    }
    public enum SwapListingStatus
    {
        Published,
        Negotiation,
        Swapped,
        All,
       
    }
    public enum SwapListingEnumStatus
    {
        Published,
        Negotiation,
        Swapped,
        All
    }
    public enum AnalyticsMetricFilter
    {
        All,
        ActiveUsers,
        ApprovedListings,
        ActiveSwaps,
        CompletedSwaps
    }
    public enum FlagContentType
    {
        Listing,
        Swap
    }
    public enum SwapProceedingStatus
    {
       
        Negotiation,
       Resolution,
       
        Swapped,
        Closed,
        AwaitingConfirmation,
        RequestAdvNegotiation,
        AdvanceChargePaymentCompleted,
        AwaitingVendorHoldingFee,
        AdvNegotiation,
        AdvNegotiationSwapped,
        SettlementRequest

    }
    public enum WithdrawalStatus
    {
        All,
        Pending,
        Treated
    }
    public enum TransactionDateFilter
    {
        All = 1,
        LastWeek,
        LastMonth
    }
   
    public enum ListingDateFilter
    {
        All=1,
        LastWeek,
        LastMonth
    }
    public enum ReportDateFilter
    {
        All=1,
        LastWeek,
        LastMonth
    }
    public enum ReportTypeEnum
    {
       User=1,
        Swap,
        Item
    }
    public enum ReportUserStatus
    {
        All=1,
        New,
        UnderReview,
        Resolved,
        Dismissed
    }
}
