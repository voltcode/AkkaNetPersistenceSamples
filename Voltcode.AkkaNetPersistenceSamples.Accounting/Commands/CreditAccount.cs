using System;

namespace Voltcode.AkkaNetPersistenceSamples.Accounting.Commands
{
    public class CreditAccount : ICommand
    {
        public CreditAccount(decimal amount, string accountId)
        {
            Amount = amount;
            AccountId = accountId;
            CommandId = Guid.NewGuid();
        }

        public decimal Amount { get; }
        public string AccountId { get; }
        public Guid CommandId { get; }
    }
}
  