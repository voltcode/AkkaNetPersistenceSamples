namespace Voltcode.AkkaNetPersistenceSamples.Accounting.State
{
    public class AccountState
    {
        public static string SampleAccountId = "VIP #1";

        public AccountState(string accountId)
        {
            AccountId = accountId;
            CurrentAmount = 0;
        }

        public string AccountId { get;  }
        public decimal CurrentAmount { get; private set; }
        public bool IsOverdraft => CurrentAmount < 0;

        public void Credit(decimal amount)
        {
            this.CurrentAmount += amount;
        }

        public void Debit(decimal amount)
        {
            this.CurrentAmount -= amount;
        }
    }
}
