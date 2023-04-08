using Tomate.Services;

namespace Tomate.Tests.Services;

public class SettingsServiceTests
{
    private readonly SettingsService cut;

    public SettingsServiceTests()
    {
        this.cut = new SettingsService();
    }
}