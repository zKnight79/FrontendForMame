using FrontendForMame.UI.Model;
using Microsoft.Extensions.Configuration;

namespace FrontendForMame.UI.Extensions;

public static class ConfigurationExtensions
{
    private const string LAUNCHFULLSCREEN_KEY = "LaunchFullscreen";
    private const string ALLOWSYSTEMSHUTDOWN_KEY = "AllowSystemShutdown";
    private const string CUSTOMIZEDTITLE_KEY = "CustomizedTitle";
    private const string USEVIDEOOPENEDHACK_KEY = "UseVideoOpenedHack";
    private const string CONTROLLERTESTMODE_KEY = "ControllerTestMode";

    public static bool GetLaunchFullscreen(this IConfiguration configuration)
        => configuration.GetValue<bool>(LAUNCHFULLSCREEN_KEY);

    public static bool GetAllowSystemShutdown(this IConfiguration configuration)
        => configuration.GetValue<bool>(ALLOWSYSTEMSHUTDOWN_KEY);

    public static string GetCustomizedTitle(this IConfiguration configuration)
        => configuration[CUSTOMIZEDTITLE_KEY];

    public static bool GetUseVideoOpenedHack(this IConfiguration configuration)
        => configuration.GetValue<bool>(USEVIDEOOPENEDHACK_KEY);

    public static bool GetControllerTestMode(this IConfiguration configuration)
        => configuration.GetValue<bool>(CONTROLLERTESTMODE_KEY);

    public static MameConfig GetMameConfig(this IConfiguration configuration)
    {
        IConfigurationSection section = configuration.GetSection(nameof(MameConfig));
        return section.Get<MameConfig>();
    }

    public static ControllerConfig GetControllerConfig(this IConfiguration configuration)
    {
        IConfigurationSection section = configuration.GetSection(nameof(ControllerConfig));
        return section.Get<ControllerConfig>();
    }
}
