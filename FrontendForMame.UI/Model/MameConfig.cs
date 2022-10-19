using System.Collections.Generic;

namespace FrontendForMame.UI.Model
{
    public record MameConfig(
        string RomListJsonSource,
        string RomLogoDirectory,
        string RomSnapDirectory,
        string RomPreviewDirectory,
        string MameDirectory,
        bool SkipGameInfo,
        bool UseJoystickProviderWinHybrid,
        bool UseWindowedMode,
        bool MaximizeWindow,
        List<string> AdditionnalArguments
    )
    {
        public MameConfig()
            : this(
                  "./mame-rom-list.json",
                  "./Logos",
                  "./Snaps",
                  "./Previews",
                  "./MAME",
                  true,
                  false,
                  false,
                  true,
                  new()
              )
        { }
    }
}
