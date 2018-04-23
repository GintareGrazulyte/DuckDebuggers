using DOL;
using DOL.Carts;
using System.Threading.Tasks;

namespace BLL_API
{
    public interface IPayment
    {
        Task<bool> Pay(Card card, Cart cart);
    }
}
