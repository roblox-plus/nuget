namespace Roblox.Economy;

/// <summary>
/// Transaction types.
/// </summary>
/// <seealso cref="IEconomyTransactionsClient.GetUserTransactionsAsync"/>
/// <seealso cref="IEconomyTransactionsClient.GetGroupTransactionsAsync"/>
public static class TransactionType
{
    /// <summary>
    /// Purchases
    /// </summary>
    public const string Purchase = "Purchase";

    /// <summary>
    /// Sales
    /// </summary>
    public const string Sale = "Sale";
}
