using BLL_API;
using DOL;
using DOL.Carts;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _client = new HttpClient();

        public Task<HttpResponseMessage> Pay(Card card, Cart cart)
        {
            string json = ToJson(card, cart);

            _client.DefaultRequestHeaders
                   .Accept
                   .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders
                    .Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "dGVjaG5vbG9naW5lczpwbGF0Zm9ybW9z");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://mock-payment-processor.appspot.com/v1/payment");
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return _client.SendAsync(request)
                .ContinueWith(responseTask =>
                {
                    return responseTask.Result;   
                });

        }


        private string ToJson(Card card, Cart cart)
        {
            string json = "{"
                            + "\"amount\":" + cart.Cost + ","
                            + "\"number\":\"" + card.Number + "\","
                            + "\"holder\":\"" + card.Holder + "\","
                            + "\"exp_year\":" + card.ExpYear + ","
                            + "\"exp_month\":" + card.ExpMonth + ","
                            + "\"cvv\":\"" + card.CVV + "\""
                        + "}";
            return json;
        }
        
    }
}
