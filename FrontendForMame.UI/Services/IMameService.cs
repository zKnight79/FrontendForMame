using FrontendForMame.UI.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FrontendForMame.UI.Services;

public interface IMameService
{
    IEnumerable<MameRomDef>? GetRomDefinitions();
    string? GetRomLogoPath(MameRomDef? romDef);
    string? GetRomSnapPath(MameRomDef? romDef);
    string? GetRomPreviewPath(MameRomDef? romDef);
    Task LaunchGame(MameRomDef? romDef);
}
