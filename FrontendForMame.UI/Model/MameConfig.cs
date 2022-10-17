﻿namespace FrontendForMame.UI.Model
{
    public record MameConfig(
        string RomListJsonSource,
        string RomLogoDirectory,
        string RomSnapDirectory,
        string RomPreviewDirectory,
        string MameDirectory
    )
    {
        public MameConfig()
            : this(
                  "./mame-rom-list.json",
                  "./Logos",
                  "./Snaps",
                  "./Previews",
                  "./MAME"
              )
        { }
    }
}
