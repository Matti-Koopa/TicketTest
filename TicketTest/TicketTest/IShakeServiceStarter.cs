namespace TicketTest
{
    public interface IShakeServiceStarter
    {
        void Start();

        void Stop();

        bool IsRunning { get; }
    }
}
