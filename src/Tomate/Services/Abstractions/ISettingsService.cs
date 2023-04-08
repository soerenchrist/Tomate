using Tomate.Models;

namespace Tomate.Services.Abstractions;

public interface ISettingsService
{
    Settings ReadGlobalSettings();
    void UpdateGlobalSettings(Settings settings);
}