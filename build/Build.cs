using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Tools.MinVer;

[GitHubActions("publish", GitHubActionsImage.UbuntuLatest,
    On = new[] { GitHubActionsTrigger.WorkflowDispatch, GitHubActionsTrigger.Push },
    ImportSecrets = new[] { nameof(NugetApiKey) },
    InvokedTargets = new[] { nameof(Push) })]
[GitHubActions("build", GitHubActionsImage.UbuntuLatest,
    On = new[] { GitHubActionsTrigger.WorkflowDispatch, GitHubActionsTrigger.PullRequest },
    InvokedTargets = new[] { nameof(Test) })]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter][Secret] readonly string NugetApiKey;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    AbsolutePath PackagesDirectory => RootDirectory / "nupkg";
    AbsolutePath ToolSourceDirectory => RootDirectory / "src" / "Tomate.Tool";
    AbsolutePath ExeSourceDirectory => RootDirectory / "src" / "Tomate.Exe";

    AbsolutePath LinuxOutputDirectory => ExeSourceDirectory / "bin" / Configuration / "net7.0" / "linux-x64" / "publish";
    AbsolutePath WinOutputDirectory => ExeSourceDirectory / "bin" / Configuration / "net7.0-windows10.0.17763.0" / "win-x64" / "publish";

    readonly string Version = "1.0.4";

    [Solution]
    readonly Solution Solution;

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            DotNetClean(s => s
                .SetProject(Solution)
                .SetConfiguration(Configuration));
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .EnableNoRestore());
        });

    Target Pack => _ => _
        .DependsOn(Test)
        .Produces(PackagesDirectory / "*.nupkg", LinuxOutputDirectory / "Tomate", WinOutputDirectory / "Tomate.exe")
        .Executes(() =>
        {
            Log.Information("Packing nuget package...");
            Log.Information("Package version: {0}", Version);
            DotNetPack(s => s
                .SetProject(ToolSourceDirectory)
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .EnableNoRestore()
                .SetVersion(Version));

            (string Runtime, string Framwork)[] configs = new[] { ("win-x64", "net7.0-windows10.0.17763.0"), ("linux-x64", "net7.0") };

            Log.Information($"Create executables...");
            configs.ForEach(config =>
                DotNetPublish(s => s
                    .SetProject(ExeSourceDirectory)
                    .SetConfiguration(Configuration)
                    .EnableSelfContained()
                    .EnablePublishSingleFile()
                    .SetVersion(Version)
                    .SetRuntime(config.Runtime)
                    .SetFramework(config.Framwork))
                );
        });

    Target Push => _ => _
        .DependsOn(Pack)
        .Requires(() => NugetApiKey)
        .Executes(() =>
        {
            GlobFiles(PackagesDirectory, "*.nupkg")
                .Where(x => !x.EndsWith(".symbols.nupkg"))
                .ForEach(x => DotNetNuGetPush(s => s
                    .SetTargetPath(x)
                    .SetApiKey(NugetApiKey)
                    .EnableSkipDuplicate()
                    .SetSource("https://api.nuget.org/v3/index.json")));
        });

}
