using System.IO.Abstractions;
using System.Text.Json;
using Tomate.Models;

namespace Tomate.Services;

public class SettingsService
{
    private readonly string _settingsFilePath;
    private readonly IFileSystem _fileSystem;

    public SettingsService(string settingsFilePath, IFileSystem fileSystem)
    {
        _settingsFilePath = settingsFilePath;
        _fileSystem = fileSystem;
    }

    public Settings ReadGlobalSettings()
    {
        if (!_fileSystem.File.Exists(_settingsFilePath))
        {
            return CreateDefaultSettingsFile();
        }

        var settingsJson = _fileSystem.File.ReadAllText(_settingsFilePath);
        try
        {
            var settings = JsonSerializer.Deserialize<Settings>(settingsJson);

            return settings;
        }
        catch (JsonException)
        {
            var newSettings = new Settings();
            WriteSettingsToFile(newSettings);
            return newSettings;
        }
    }

    public void UpdateSettings(Settings settings)
    {
        if (!_fileSystem.File.Exists(_settingsFilePath))
        {
            CreateFile();
        }

        WriteSettingsToFile(settings);
    }

    private void WriteSettingsToFile(Settings newSettings)
    {
        var newSettingsJson = JsonSerializer.Serialize(newSettings);
        _fileSystem.File.WriteAllText(_settingsFilePath, newSettingsJson);
    }

    private Settings CreateDefaultSettingsFile()
    {
        CreateFile();

        var defaultSettings = new Settings();
        var defaultSettingsJson = JsonSerializer.Serialize(defaultSettings);
        _fileSystem.File.WriteAllText(_settingsFilePath, defaultSettingsJson);
        return defaultSettings;
    }

    private void CreateFile()
    {
        var directory = _fileSystem.Path.GetDirectoryName(_settingsFilePath);
        if (directory == null) throw new Exception("Could not get directory from settings file path.");
        if (!_fileSystem.Directory.Exists(directory))
        {
            _fileSystem.Directory.CreateDirectory(directory);
        }

        _fileSystem.File.Create(_settingsFilePath);
    }
}