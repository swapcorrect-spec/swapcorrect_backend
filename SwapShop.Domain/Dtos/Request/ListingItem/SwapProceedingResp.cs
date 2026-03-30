using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Dtos.Request.ListingItem
{
    public class   SwapProceedingResp
    {
        public string swapProceedId {  get; set; }
        public string swapperUserId {  get; set; }
        public string swapperName {  get; set; }
        public string swapperImage {  get; set; }
        public string visitorName {  get; set; }
        public string visitorUserId {  get; set; }
        public string visitorImage {  get; set; }
        public string ListedItem {  get; set; }
        public string SwapperRequestItem {  get; set; }
         public string Status {  get; set; }
         public string RoomName {  get; set; }
         public DateTime CreatedOn {  get; set; }
         public DateTime lastActivity {  get; set; }
    }
}
