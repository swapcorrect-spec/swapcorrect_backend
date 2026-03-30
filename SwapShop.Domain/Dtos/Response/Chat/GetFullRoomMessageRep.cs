using SwapShop.Domain.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Dtos.Response.Chat
{
    public class GetFullRoomMessageRep
    {
        public List<RoomMessageResp> roomMessages {  get; set; }
        public int ImageCount { get; set; }
        public int VideoCount { get; set; }
        public int FileCount { get; set; }
        public bool IsSwapper { get; set; }
        public ChatProceedRespDto? SwappingProceeding { get; set; }
    }
}
