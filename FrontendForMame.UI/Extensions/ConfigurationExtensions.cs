using Microsoft.Extensions.Configuration;

namespace FrontendForMame.UI.Extensions;

public static class ConfigurationExtensions
{
    private const string LAUNCHFULLSCREEN_KEY = "LaunchFullscreen";
    private const string ALLOWSYSTEMSHUTDOWN_KEY = "AllowSystemShutdown";
    private const string CUSTOMIZEDTITLE_KEY = "CustomizedTitle";
    private const string MAMEROMLISTJSONSOURCE_KEY = "MameRomListJsonSource";
    private const string MAMEROMLOGODIRECTORY_KEY = "MameRomLogoDirectory";
    private const string MAMEROMSNAPDIRECTORY_KEY = "MameRomSnapDirectory";

    public static bool GetLaunchFullscreen(this IConfiguration configuration)
        => configuration.GetValue<bool>(LAUNCHFULLSCREEN_KEY);

    public static bool GetAllowSystemShutdown(this IConfiguration configuration)
        => configuration.GetValue<bool>(ALLOWSYSTEMSHUTDOWN_KEY);

    public static string GetCustomizedTitle(this IConfiguration configuration)
        => configuration[CUSTOMIZEDTITLE_KEY];

    public static string GetMameRomListJsonSource(this IConfiguration configuration)
        => configuration[MAMEROMLISTJSONSOURCE_KEY];

    public static string GetMameRomLogoDirectory(this IConfiguration configuration)
        => configuration[MAMEROMLOGODIRECTORY_KEY];

    public static string GetMameRomSnapDirectory(this IConfiguration configuration)
        => configuration[MAMEROMSNAPDIRECTORY_KEY];
}
