using System;
using Akka.Event;
using Akka.Persistence;
using Voltcode.AkkaNetPersistenceSamples.Accounting.Commands;
using Voltcode.AkkaNetPersistenceSamples.Accounting.Events;
using Voltcode.AkkaNetPersistenceSamples.Accounting.Messages;
using Voltcode.AkkaNetPersistenceSamples.Accounting.State;

namespace Voltcode.AkkaNetPersistenceSamples.Accounting.Bank.Actors
{
    public class Banker : PersistentActor
    {
        public override string PersistenceId => "Personal-banker-1";

        private AccountState accountState;

        private readonly ILoggingAdapter log = Logging.GetLogger(Context);

        public Banker()
        {
            this.accountState = new AccountState(AccountState.SampleAccountId);
        }

        protected override bool ReceiveRecover(object message)
        {
            switch (message)
            {
                case SnapshotOffer so:
                    log.Info($"Snapshot seq {so.Metadata.SequenceNr} found from (UTC): {so.Metadata.Timestamp}");
                    
                    // in real life, check for account Id :)
                    this.accountState = so.Snapshot as AccountState;
                    
                    return true;

                case RecoveryCompleted rc:
                    log.Info("Recovery completed!");
                    return true;

                case AccountDebited ad:
                    UpdateState(ad);
                    return true;

                case AccountCredited ac:
                    UpdateState(ac);
                    return true;
            }

            return false;
        }        

        protected override bool ReceiveCommand(object message)
        {
            switch (message)
            {
                case Guaranteed<CreditAccount> msg:
                    Persist(new AccountCredited(msg.Msg.AccountId, msg.Msg.Amount, DateTime.UtcNow), e =>
                    {                        
                        UpdateState(e);

                        if (LastSequenceNr % 10 == 0 && LastSequenceNr > 0)
                        {
                            SaveSnapshot(this.accountState);
                        }
                    });
                    Sender.Tell(new Confirm(msg.DeliveryId), Self);

                    return true;

                case Guaranteed<DebitAccount> msg:
                    Persist(new AccountDebited(msg.Msg.AccountId, msg.Msg.Amount, DateTime.UtcNow), e =>
                    {                        
                        UpdateState(e);

                        if (LastSequenceNr % 10 == 0 && LastSequenceNr > 0)
                        {
                            SaveSnapshot(this.accountState);
                        }
                    });
                    Sender.Tell(new Confirm(msg.DeliveryId), Self);

                    return true;

                case SaveSnapshotSuccess se:
                    log.Info("Snapshot saved successfully");
                    break;

                case SaveSnapshotFailure sf:
                    log.Warning("Snapshot save failed");
                    break;

                case StatusRequest sr:
                    Sender.Tell(new StatusResponse(this.accountState.AccountId, this.accountState.CurrentAmount), Self);
                    break;

                case null:
                    break;

                default:     
                    log.Warning($"Unrecognized command for banker, type {message.GetType().Name}");
                    break;
            }

            return false;
        }        

        private void UpdateState(AccountDebited ad)
        {
            this.accountState.Debit(ad.Amount);
            log.Info($"Account debited by {ad.AccountId}, current state: {this.accountState.CurrentAmount}");
        }

        private void UpdateState(AccountCredited ac)
        {
            this.accountState.Credit(ac.Amount);
            log.Info($"Account credited by {ac.AccountId}, current state: {this.accountState.CurrentAmount}");
        }
    }
}
