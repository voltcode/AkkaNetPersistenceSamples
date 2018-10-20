using System;
using System.Windows.Forms;
using Akka.Actor;
using Akka.Persistence.Sqlite;
using Voltcode.AkkaNetPersistenceSamples.Accounting.Commands;
using Voltcode.AkkaNetPersistenceSamples.Accounting.Messages;
using Voltcode.AkkaNetPersistenceSamples.Accounting.State;

namespace Voltcode.AkkaNetPersistenceSamples.Accounting.CashMachine
{
    public partial class CashMachineFrontend : Form, IScreenLogger
    {
        public static ActorSystem ActorSystem = ActorSystem.Create(Keys.CashMachineSystemName);

        private readonly IActorRef cashMachine;

        public CashMachineFrontend()
        {        
            SqlitePersistence.Get(ActorSystem);
            this.cashMachine = ActorSystem.ActorOf(Props.Create(() => new Actors.CashMachine(this)), Keys.CashMachineActorName);           

            InitializeComponent();
        }

        private void withdrawButton_Click(object sender, EventArgs e)
        {            
            this.cashMachine.Tell(new DebitAccount(GetAmount(), AccountState.SampleAccountId));
        }

        private void DepositButton_Click(object sender, EventArgs e)
        {
            this.cashMachine.Tell(new CreditAccount(GetAmount(), AccountState.SampleAccountId));
        }

        private decimal GetAmount()
        {
            decimal.TryParse(this.amountBox.Text, out decimal amount);
            return amount;
        }

        private void checkAccountButton_Click(object sender, EventArgs e)
        {
            this.cashMachine.Tell(new StatusRequest(AccountState.SampleAccountId));
        }

        public void PrintMessage(string message)
        {
            this.cashMachineLog.AppendText(message + Environment.NewLine);
        }

        public void PrintStatus(string message)
        {
            this.accountStatusLabel.Text = message;
        }
    }
}
