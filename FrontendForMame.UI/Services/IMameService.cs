using FrontendForMame.UI.Model;
using System.Collections.Generic;

namespace FrontendForMame.UI.Services;

public interface IMameService
{
    IEnumerable<MameRomDef>? GetRomDefinitions();
    string? GetRomLogoPath(MameRomDef? romDef);
}
