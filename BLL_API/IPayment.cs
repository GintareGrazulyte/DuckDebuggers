using DOL;
using DOL.Carts;
using System.Net.Http;
using System.Threading.Tasks;

namespace BLL_API
{
    public interface IPaymentService
    {
        //TODO: async
        Task<HttpResponseMessage> Pay(Card card, Cart cart);
    }
}
