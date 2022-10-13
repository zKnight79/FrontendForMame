using Microsoft.Extensions.Configuration;

namespace FrontendForMame.UI.Extensions;

public static class ConfigurationExtensions
{
    private const string LAUNCHFULLSCREEN_KEY = "LaunchFullscreen";
    private const string ALLOWSYSTEMSHUTDOWN_KEY = "AllowSystemShutdown";
    private const string CUSTOMIZEDTITLE_KEY = "CustomizedTitle";

    public static bool GetLaunchFullscreen(this IConfiguration configuration)
        => configuration.GetValue<bool>(LAUNCHFULLSCREEN_KEY);

    public static bool GetAllowSystemShutdown(this IConfiguration configuration)
        => configuration.GetValue<bool>(ALLOWSYSTEMSHUTDOWN_KEY);

    public static string GetCustomizedTitle(this IConfiguration configuration)
        => configuration[CUSTOMIZEDTITLE_KEY];
}
