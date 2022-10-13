using System.Threading;
using System.Threading.Tasks;
using Roblox.Api;

namespace Roblox.Economy
{
    /// <summary>
    /// A client for pulling economy transaction data.
    /// </summary>
    public interface IEconomyTransactionsClient
    {
        /// <summary>
        /// Gets economy transactions for the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="transactionType">The <seealso cref="TransactionType"/> to pull transactions for.</param>
        /// <param name="cursor">The cursor, used to page through transactions.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
        /// <returns>A <seealso cref="PagedResult{TData}"/> of <seealso cref="EconomyTransaction"/>.</returns>
        Task<PagedResult<EconomyTransaction>> GetUserTransactionsAsync(long userId, string transactionType, string cursor, CancellationToken cancellationToken);

        /// <summary>
        /// Gets economy transactions for the specified group.
        /// </summary>
        /// <param name="groupId">The ID of the group.</param>
        /// <param name="transactionType">The <seealso cref="TransactionType"/> to pull transactions for.</param>
        /// <param name="cursor">The cursor, used to page through transactions.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
        /// <returns>A <seealso cref="PagedResult{TData}"/> of <seealso cref="EconomyTransaction"/>.</returns>
        Task<PagedResult<EconomyTransaction>> GetGroupTransactionsAsync(long groupId, string transactionType, string cursor, CancellationToken cancellationToken);
    }
}
