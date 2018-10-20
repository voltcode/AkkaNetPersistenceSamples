using System;

namespace Voltcode.AkkaNetPersistenceSamples.Accounting.Events
{
    public class AccountCredited
    {
        public AccountCredited(string accountId, decimal amount, DateTime dateTimeUtc)
        {
            AccountId = accountId;
            Amount = amount;
            DateTimeUtc = dateTimeUtc;
        }

        public string AccountId { get; }
        public decimal Amount { get; }
        public DateTime DateTimeUtc { get; }
    }
}
