using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOL.Orders
{
    public enum OrderStatus
    {
        waitingForPayment,
        approved,
        collecting,
        sent,
        delivered
    }
}
