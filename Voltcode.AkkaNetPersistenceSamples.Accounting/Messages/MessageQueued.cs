namespace Voltcode.AkkaNetPersistenceSamples.Accounting.Messages
{
    public class MessageQueued<T>
    {
        public MessageQueued(T message)
        {
            Message = message;
        }

        public T Message { get; }
    }
}
