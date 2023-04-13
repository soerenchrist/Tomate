using System.IO;
using System.Threading.Tasks;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Git;
using Nuke.Common.Tools.GitHub;
using Nuke.Common.Tools.MinVer;
using Octokit;

[GitHubActions("publish", GitHubActionsImage.UbuntuLatest,
    On = new[] { GitHubActionsTrigger.WorkflowDispatch, GitHubActionsTrigger.Push },
    ImportSecrets = new[] { nameof(NugetApiKey) },
    EnableGitHubToken = true,
    InvokedTargets = new[] { nameof(Push) })]
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

    [GitRepository]
    readonly GitRepository Repository;

    bool PublishRelease => Repository.IsOnMainBranch() && !IsLocalBuild;

    readonly string Version = "1.0.5";

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
        .OnlyWhenDynamic(() => PublishRelease)
        .DependsOn(Test)
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
        .OnlyWhenDynamic(() => PublishRelease)
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

            try
            {
                CreateRelease().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to create release");
            }
        });
        
        
    private async Task CreateRelease()
    {
        Log.Information("Creating release...");
        GitHubActions gitHubActions = GitHubActions.Instance;
        GitHubTasks.GitHubClient = new GitHubClient(new ProductHeaderValue("Tomate"))
        {
            Credentials = new Credentials(gitHubActions.Token)
        };

        var newRelease = new NewRelease(Version)
        {
            TargetCommitish = Repository.Commit,
            Draft = true,
            Name = "Tomate " + Version,
            Prerelease = false,
            Body = "Tomate " + Version
        };
        var owner = Repository.GetGitHubOwner();
        var name = Repository.GetGitHubName();
        var createdRelease = await GitHubTasks.GitHubClient.Repository.Release.Create(owner, name, newRelease);

        var linuxFile = GlobFiles(LinuxOutputDirectory, "Tomate").First();
        var winFile = GlobFiles(WinOutputDirectory, "Tomate.exe").First();

        var linuxName = $"tomate-linux-x64-{Version}";
        var winName = $"tomate-win-x64-{Version}.exe";

        await UploadReleaseAsset(createdRelease, linuxFile, linuxName);
        await UploadReleaseAsset(createdRelease, winFile, winName);

    }
    private async Task UploadReleaseAsset(Release release, string asset, string filename) {
        Log.Information($"Uploading {filename}...");
        await using var stream = File.OpenRead(asset);
        var assetUpload = new ReleaseAssetUpload
        {
            FileName = filename,
            ContentType = "application/octet-stream",
            RawData = stream
        };

        await GitHubTasks.GitHubClient.Repository.Release.UploadAsset(release, assetUpload);
    }

}
