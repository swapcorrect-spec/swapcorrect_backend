namespace SwapShop.Domain.Dtos.Response.Payment
{
    public class PaystackBankResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<PaystackBank> data { get; set; }
    }

    public class PaystackBank
    {
        public string name { get; set; }
        public string code { get; set; }
        public string slug { get; set; }
        public string currency { get; set; }
        public string type { get; set; }
    }

}
