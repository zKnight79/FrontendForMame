using System.Collections.Generic;
using System.Linq;

namespace FrontendForMame.UI.Model;

public record ControllerConfig(
    List<int> LaunchButton,
    List<int> ExitButton,
    List<int> ShutdownButton
)
{
    public ControllerConfig()
        : this(new(), new(), new())
    { }

    public int GetLaunchButton(int controllerId) => LaunchButton.ElementAtOrDefault(controllerId);
    public int GetExitButton(int controllerId) => ExitButton.ElementAtOrDefault(controllerId);
    public int GetShutdownButton(int controllerId) => ShutdownButton.ElementAtOrDefault(controllerId);
}
