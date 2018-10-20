namespace Voltcode.AkkaNetPersistenceSamples.Accounting.CashMachine
{
    public interface IScreenLogger
    {
        void PrintMessage(string message);

        void PrintStatus(string message);
    }
}