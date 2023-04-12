using System.IO.Abstractions;
using System.Text.Json;
using Tomate.Models;
using Tomate.Services.Abstractions;

namespace Tomate.Services;

public class SettingsService : ISettingsService
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
        using var fileStream = CreateOrOpenFileStream();
        using var streamReader = new StreamReader(fileStream);

        try
        {
            var content = streamReader.ReadToEnd();
            var settings = JsonSerializer.Deserialize<Settings>(content);

            return settings;
        }
        catch (JsonException)
        {
            var newSettings = new Settings();
            
            WriteSettingsToFile(fileStream, newSettings);
            return newSettings;
        }
    }

    public void UpdateGlobalSettings(Settings settings)
    {
        using var stream = CreateOrOpenFileStream();

        WriteSettingsToFile(stream, settings);
    }

    private void WriteSettingsToFile(Stream stream, Settings newSettings)
    {
        using var streamWriter = new StreamWriter(stream);
        stream.SetLength(0);
        var newSettingsJson = JsonSerializer.Serialize(newSettings);
        streamWriter.Write(newSettingsJson);
    }

    private Settings CreateDefaultSettingsFile()
    {
        using var stream = CreateOrOpenFileStream();
        using var streamWriter = new StreamWriter(stream);

        var defaultSettings = new Settings();
        var defaultSettingsJson = JsonSerializer.Serialize(defaultSettings);

        streamWriter.Write(defaultSettingsJson);
        return defaultSettings;
    }

    private Stream CreateOrOpenFileStream()
    {
        var directory = _fileSystem.Path.GetDirectoryName(_settingsFilePath);
        if (directory == null) throw new Exception("Could not get directory from settings file path.");
        if (!_fileSystem.Directory.Exists(directory))
        {
            _fileSystem.Directory.CreateDirectory(directory);
        }

        return this._fileSystem.FileInfo.New(_settingsFilePath).Open(FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }
}