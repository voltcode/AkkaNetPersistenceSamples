namespace Voltcode.AkkaNetPersistenceSamples.Accounting.Messages
{
    public class Guaranteed<T>
    {
        public Guaranteed(long deliveryId, T msg)
        {
            DeliveryId = deliveryId;
            Msg = msg;
        }

        public long DeliveryId { get; }
        public T Msg { get; }
    }
}
