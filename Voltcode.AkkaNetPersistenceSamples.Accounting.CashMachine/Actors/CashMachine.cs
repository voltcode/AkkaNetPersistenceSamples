using Akka.Actor;
using Akka.Persistence;
using Voltcode.AkkaNetPersistenceSamples.Accounting.Commands;
using Voltcode.AkkaNetPersistenceSamples.Accounting.Messages;

namespace Voltcode.AkkaNetPersistenceSamples.Accounting.CashMachine.Actors
{
    public class CashMachine : AtLeastOnceDeliveryReceiveActor
    {
        private readonly ActorPath bankerPath;

        private readonly IScreenLogger logger;

        public CashMachine(IScreenLogger logger)
        {
            this.logger = logger;

            this.bankerPath = ActorPath.Parse("akka.tcp://" + Keys.BankingSystemName + "@localhost:8123/user/" + Keys.BankerActorName);           

            Command<CreditAccount>(msg => Persist(new MessageQueued<CreditAccount>(msg), Handle));

            Command<DebitAccount>(msg => Persist(new MessageQueued<DebitAccount>(msg), Handle));

            Command<Confirm>(msg => Persist(msg, Handle));
            
            Recover<MessageQueued<CreditAccount>>(m => Handle(m));

            Recover<MessageQueued<DebitAccount>>(m => Handle(m));

            Recover<Confirm>(msg => Handle(msg));


            //misc. status commands
            Command<StatusRequest>(msg => Context.ActorSelection(this.bankerPath).Tell(msg));
            Command<StatusResponse>(msg=> Handle(msg));
        }

        private void Handle(StatusResponse statusResponse)
        {
            this.logger.PrintStatus($"Account: {statusResponse.AccountId}\r\nBalance: {statusResponse.Balance}");
        }

        private void Handle(MessageQueued<CreditAccount> wrapper)
        {
            Deliver(this.bankerPath, deliveryId => new Guaranteed<CreditAccount>(deliveryId, wrapper.Message));

            this.logger.PrintMessage($"Delivering CreditAccount, cmdid: {wrapper.Message.CommandId}");
        }

        private void Handle(MessageQueued<DebitAccount> wrapper)
        {
            Deliver(this.bankerPath, deliveryId => new Guaranteed<DebitAccount>(deliveryId, wrapper.Message));

            this.logger.PrintMessage($"Delivering DebitAccount, cmdid: {wrapper.Message.CommandId}");
        }

        private void Handle(Confirm msg)
        {
            ConfirmDelivery(msg.DeliveryId);

            this.logger.PrintMessage($"Delivery confirmed, id: {msg.DeliveryId}");
        }
        
        public override string PersistenceId => "Hole-in-the-wall-1";
    }
}
