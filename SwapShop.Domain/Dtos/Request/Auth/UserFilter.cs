using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Dtos.Request.Auth
{
    public enum UserFilter
    {
        ALL,
        ACTIVE,
        UNVERIFIED,
        SUSPENDED

    }
}
