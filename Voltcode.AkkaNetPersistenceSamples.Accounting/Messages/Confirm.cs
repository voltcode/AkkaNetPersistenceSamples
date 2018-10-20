namespace Voltcode.AkkaNetPersistenceSamples.Accounting.Messages
{
    public class Confirm
    {
        public Confirm(long deliveryId)
        {
            DeliveryId = deliveryId;
        }

        public long DeliveryId { get; }
    }
}
