namespace Tomate.Handlers;

public interface IAsyncHandler<TArgs>
{
    Task<int> HandleAsync(TArgs args, CancellationToken cancellationToken = default); 
}