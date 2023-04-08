using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Text.Json;
using NSubstitute;
using Tomate.Models;
using Tomate.Services;

namespace Tomate.Tests.Services;

public class SettingsServiceTests
{
    private const string SettingsFilePath = "~/.config/tomate/settings.json";

    [Fact]
    public void ReadGlobalSettings_WhenSettingsFileDoesNotExist_FileIsCreated()
    {
        var fileSystem = new MockFileSystem();
        var cut = new SettingsService(SettingsFilePath, fileSystem);

        cut.ReadGlobalSettings();

        fileSystem.File.Exists(SettingsFilePath).Should().BeTrue();
    }

    [Fact]
    public void ReadGlobalSettings_WhenSettingsFileDoesNotExist_FileContainsDefaultSettings()
    {
        var fileSystem = new MockFileSystem();
        var cut = new SettingsService(SettingsFilePath, fileSystem);

        cut.ReadGlobalSettings();

        var expectedJson = JsonSerializer.Serialize(new Settings());
        fileSystem.GetFile(SettingsFilePath).TextContents.Should().BeEquivalentTo(expectedJson);
    }

    [Fact]
    public void ReadGlobalSettings_WhenSettingsFileDoesNotExist_ShouldReturnDefaultSettings()
    {
        var fileSystem = new MockFileSystem();
        var cut = new SettingsService(SettingsFilePath, fileSystem);

        var settings = cut.ReadGlobalSettings();
        settings.Should().Be(new Settings());
    }

    [Fact]
    public void ReadGlobalSettings_WhenSettingsFileIsValid_ShouldReturnCorrespondingSettings()
    {
        var storedSettings = CreateValidSettings();
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { SettingsFilePath, JsonSerializer.Serialize(storedSettings) }
        });
        var cut = new SettingsService(SettingsFilePath, fileSystem);

        var settings = cut.ReadGlobalSettings();
        settings.Should().BeEquivalentTo(storedSettings);
    }

    [Fact]
    public void ReadGlobalSettings_WhenSettingsFileIsInvalid_ShouldReturnDefaultSettings()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { SettingsFilePath, "invalid json" }
        });
        var cut = new SettingsService(SettingsFilePath, fileSystem);

        var settings = cut.ReadGlobalSettings();
        settings.Should().Be(new Settings());
    }

    [Fact]
    public void ReadGlobalSettings_WhenSettingsFileIsInvalid_SettingsFileIsOverridenByDefaultSettings()
    {
        var defaultSettings = new Settings();
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { SettingsFilePath, "invalid json" }
        });
        var cut = new SettingsService(SettingsFilePath, fileSystem);

        cut.ReadGlobalSettings();
        fileSystem.GetFile(SettingsFilePath).TextContents.Should()
            .BeEquivalentTo(JsonSerializer.Serialize(defaultSettings));
    }

    [Fact]
    public void UpdateSettings_WhenFileDoesNotExist_FileShouldBeCreated()
    {
        var fileSystem = new MockFileSystem();
        var cut = new SettingsService(SettingsFilePath, fileSystem);
        var settings = CreateValidSettings();

        cut.UpdateSettings(settings);

        fileSystem.GetFile(SettingsFilePath).Should().NotBeNull();
    }

    [Fact]
    public void UpdateSettings_WhenFileDoesNotExist_FileShouldContainSettings()
    {
        var fileSystem = new MockFileSystem();
        var cut = new SettingsService(SettingsFilePath, fileSystem);
        var settings = CreateValidSettings();

        cut.UpdateSettings(settings);

        fileSystem.GetFile(SettingsFilePath).TextContents.Should().BeEquivalentTo(JsonSerializer.Serialize(settings));
    }

    [Fact]
    public void UpdateSettings_WhenFileExists_FileIsOverriden()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { SettingsFilePath, JsonSerializer.Serialize(new Settings()) }
        });
        var cut = new SettingsService(SettingsFilePath, fileSystem);
        var settings = CreateValidSettings();

        cut.UpdateSettings(settings);

        fileSystem.GetFile(SettingsFilePath).TextContents.Should().BeEquivalentTo(JsonSerializer.Serialize(settings));
    }

    private Settings CreateValidSettings()
    {
        return new Settings
        {
            FocusMinutes = 30,
            LongBreakInterval = 2,
            LongBreakMinutes = 25,
            ShortBreakMinutes = 4
        };
    }
}