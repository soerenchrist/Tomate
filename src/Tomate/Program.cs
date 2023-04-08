using System.IO.Abstractions;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Tomate.Handlers;
using Tomate.Models.Cli;
using Tomate.Services;
using Tomate.Services.Abstractions;

var services = new ServiceCollection();

RegisterHandlers(services);
RegisterServices(services);

var provider = services.BuildServiceProvider();

var result = Parser.Default.ParseArguments<StartArgs, ConfigArgs>(args)
    .MapResult(
        (StartArgs startArgs) => provider.GetRequiredService<IHandler<StartArgs>>().Handle(startArgs),
        (ConfigArgs configArgs) => provider.GetRequiredService<IHandler<ConfigArgs>>().Handle(configArgs),
        _ => 1
    );

return result;

void RegisterHandlers(IServiceCollection serviceCollection)
{
    // Register all handlers using reflection
    var handlerType = typeof(IHandler<>);
    var handlers = typeof(Program).Assembly.GetTypes()
        .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType))
        .ToList();

    foreach (var handler in handlers)
    {
        var handlerInterface = handler.GetInterfaces()
            .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType);
        serviceCollection.AddScoped(handlerInterface, handler);
    }
}

void RegisterServices(IServiceCollection serviceCollection)
{
    var baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    var configDirectory = Path.Combine(baseDirectory, "Tomate");
    var configFile = Path.Combine(configDirectory, "settings.json");

    var settingsService = new SettingsService(configFile, new FileSystem());
    serviceCollection.AddSingleton<ISettingsService>(settingsService);
}