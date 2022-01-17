namespace Core.Interfaces.Common
{
    public interface IWorkerService
    {
        Task DoWork(CancellationToken cancellationToken);
    }
}