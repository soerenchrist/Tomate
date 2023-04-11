using Tomate.Models;
using Tomate.Models.Cli;
using Tomate.Services.Abstractions;

namespace Tomate.Handlers;

public class ListConfigHandler : IAsyncHandler<ListConfigArgs>
{
    private readonly ISettingsService _settingsService;
    private readonly IOutput _output;

    public ListConfigHandler(ISettingsService settingsService,
        IOutput output)
    {
        _settingsService = settingsService;
        _output = output;
    }

    public Task<int> HandleAsync(ListConfigArgs args, CancellationToken cancellationToken = default)
    {
        var settings = _settingsService.ReadGlobalSettings();
        _output.WriteLine($"Focus time: {settings.FocusMinutes}");
        _output.WriteLine($"Short break: {settings.ShortBreakMinutes}");
        _output.WriteLine($"Long break: {settings.LongBreakMinutes}");
        _output.WriteLine($"Long break interval: {settings.LongBreakInterval}");
        if (settings.Cycles == Cycles.Infinite)
        {
            _output.WriteLine($"Cycles: Infinite");
        }
        else
        {
            _output.WriteLine($"Cycles: {settings.Cycles}");
        }

        return Task.FromResult(0);
    }
}