using static GenericConst.Const;

namespace LogData
{
    public enum LogEventType
    {
        None = DefaultCode,
        MachineInActive, //자판기 비활성

        CurrencyEarned, //재화 획득

        ProductPurchased, //상품 구매
        ProductPurchaseFailedOutOfStock, //상품 구매 실패 (수량 부족)
        ProductPurchaseFailedInsufficientFunds, //상품 구매 실패 (잔액 부족)

        ProductConsumed, //상품 소비
    }

    public static class LogMessages
    {
        public static string Format(LogEventType eventType, string detail = "")
        {
            switch (eventType)
            {
                case LogEventType.MachineInActive:
                    return "Machine is inactive.";
                case LogEventType.CurrencyEarned:
                    return $"+{detail} won";
                case LogEventType.ProductPurchased:
                    return $"[Buy] {detail}";
                case LogEventType.ProductPurchaseFailedOutOfStock:
                    return $"[Buy Fail] {detail} - out of stock";
                case LogEventType.ProductPurchaseFailedInsufficientFunds:
                    return $"[Buy Fail] {detail} - not enough money";
                case LogEventType.ProductConsumed:
                    return $"[Use] {detail}";
                default:
                    return string.IsNullOrEmpty(detail) ? eventType.ToString() : detail;
            }
        }

        public static string FormatInactiveBlocked(LogEventType blockedEventType, string detail = "")
        {
            return $"{Format(LogEventType.MachineInActive)} {Format(blockedEventType, detail)}";
        }
    }
}
