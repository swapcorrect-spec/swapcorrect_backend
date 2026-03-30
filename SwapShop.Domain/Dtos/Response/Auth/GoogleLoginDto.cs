using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Domain.Dtos.Response.Auth
{
    public class GoogleLoginDto
    {
        public string IdToken { get; set; } = string.Empty;// Token from Google
    }
}
