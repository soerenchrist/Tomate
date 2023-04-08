using Tomate.Models.Cli;

namespace Tomate.Handlers;

public class ConfigHandler : IAsyncHandler<ConfigArgs>
{
    public Task<int> HandleAsync(ConfigArgs args, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}