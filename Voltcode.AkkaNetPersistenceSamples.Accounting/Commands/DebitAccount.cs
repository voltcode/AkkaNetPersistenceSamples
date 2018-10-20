using System;

namespace Voltcode.AkkaNetPersistenceSamples.Accounting.Commands
{
    public class DebitAccount : ICommand
    {
        public DebitAccount(decimal amount, string accountId)
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
