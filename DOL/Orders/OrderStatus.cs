using System;
using System.ComponentModel;

namespace BOL.Orders
{
    public enum OrderStatus
    {
        [Description("Waiting for Payment")]
        waitingForPayment,
        [Description("Payment is Accepted")]
        approved,
        [Description("Collecting")]
        collecting,
        [Description("Sent")]
        sent,
        [Description("Delivered")]
        delivered
    }

    public static class ExtensionClass
    {
        public static string GetDescription(this OrderStatus @enum)
        {

            Type genericEnumType = @enum.GetType();
            System.Reflection.MemberInfo[] memberInfo =
                        genericEnumType.GetMember(@enum.ToString());

            if ((memberInfo != null && memberInfo.Length > 0))
            {

                dynamic _Attribs = memberInfo[0].GetCustomAttributes
                      (typeof(DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Length > 0))
                {
                    return ((DescriptionAttribute)_Attribs[0]).Description;
                }
            }

            return @enum.ToString();
        }
    }

}
