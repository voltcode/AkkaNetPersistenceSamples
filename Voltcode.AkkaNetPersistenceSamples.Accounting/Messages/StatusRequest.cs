namespace Voltcode.AkkaNetPersistenceSamples.Accounting.Messages
{
    public class StatusRequest
    {     
        public StatusRequest(string sampleAccountId)
        {
            this.AccountId = sampleAccountId;
        }

        public string AccountId { get; }
    }
}
