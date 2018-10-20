using System;
using Akka.Actor;
using Akka.Persistence.SqlServer;

namespace Voltcode.AkkaNetPersistenceSamples.Accounting.Bank
{
    public class Program
    {        
        static void Main(string[] args)
        {
            ActorSystem actorSystem = ActorSystem.Create(Keys.BankingSystemName);

            SqlServerPersistence.Get(actorSystem);

            IActorRef banker = actorSystem.ActorOf<Actors.Banker>(Keys.BankerActorName);        
            
            Console.WriteLine("Bank has opened");

            while(true)
            Console.ReadKey();
        }
    }
}
