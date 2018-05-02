using BLL_API;
using BOL;
using BOL.Orders;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BLL
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _client = new HttpClient();

        private Task<HttpResponseMessage> Pay(Card card, int cost)
        {
            string json = ToJson(card, cost);

            _client.DefaultRequestHeaders
                   .Accept
                   .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders
                    .Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "dGVjaG5vbG9naW5lczpwbGF0Zm9ybW9z");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://mock-payment-processor.appspot.com/v1/payment")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            return _client.SendAsync(request)
                .ContinueWith(responseTask =>
                {
                    return responseTask.Result;   
                });

        }


        private string ToJson(Card card, int cost)
        {
            string json = "{"
                            + "\"amount\":" + cost + ","
                            + "\"number\":\"" + card.Number + "\","
                            + "\"holder\":\"" + card.Holder + "\","
                            + "\"exp_year\":" + card.ExpYear + ","
                            + "\"exp_month\":" + card.ExpMonth + ","
                            + "\"cvv\":\"" + card.CVV + "\""
                        + "}";
            return json;
        }

        public PaymentInfo Payment(Card card, int cost)
        {
            HttpResponseMessage paymentResult = Pay(card, cost).Result;

            var receiveStream = paymentResult.Content.ReadAsStreamAsync().Result;
            var readStream = new StreamReader(receiveStream, Encoding.UTF8);
            JObject responseContent = JObject.Parse(readStream.ReadToEnd());

            string paymentDetails = "";
            OrderStatus orderStatus;

            if (paymentResult.IsSuccessStatusCode)
            {
                orderStatus = OrderStatus.approved;
            }
            else
            {
                orderStatus = OrderStatus.waitingForPayment;

                switch (paymentResult.StatusCode)
                {
                    case System.Net.HttpStatusCode.BadRequest:
                        paymentDetails = "Invalid card number, ";
                        break;
                    case System.Net.HttpStatusCode.Unauthorized:
                        //401 nepavyko autentifikuoti API serviso vartotojo
                        //Exception
                        break;
                    case System.Net.HttpStatusCode.PaymentRequired:
                        string error = responseContent.Property("error").Value.ToString();
                        if (error == "OutOfFunds")
                        {
                            paymentDetails = "Insufficient balance in the card, ";
                        }
                        else if (error == "CardExpired")
                        {
                            paymentDetails = "Card is expired, ";
                        }
                        break;
                    case System.Net.HttpStatusCode.NotFound:
                        //404 operacija nerasta (Galima tik post)
                        //Exception
                        break;
                }
                paymentDetails += "please use another card for payment";
            }
            return new PaymentInfo() { OrderStatus = orderStatus, PaymentDetails = paymentDetails };
        }

    }
}
