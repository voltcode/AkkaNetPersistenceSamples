namespace Voltcode.AkkaNetPersistenceSamples.Accounting.Messages
{
    public class StatusResponse
    {
        public StatusResponse(string accountId, decimal balance)
        {
            AccountId = accountId;
            Balance = balance;
        }

        public string AccountId { get; }

        public decimal Balance { get; }
    }
}
