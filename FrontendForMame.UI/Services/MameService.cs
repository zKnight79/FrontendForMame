using FrontendForMame.UI.Extensions;
using FrontendForMame.UI.Helpers;
using FrontendForMame.UI.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FrontendForMame.UI.Services;

class MameService : IMameService
{
    private readonly ILogger<MameService> _logger;
    private readonly MameConfig _mameConfig;

    public MameService(IConfiguration configuration, ILogger<MameService> logger)
    {
        _logger = logger;
        _mameConfig = configuration.GetMameConfig();
    }

    public IEnumerable<MameRomDef>? GetRomDefinitions()
    {
        IEnumerable<MameRomDef>? mameRomDefs = null;

        try
        {
            string mameRomListJsonSource = _mameConfig.RomListJsonSource;
            using FileStream fs = File.OpenRead(mameRomListJsonSource);
            mameRomDefs = JsonSerializer.Deserialize<IEnumerable<MameRomDef>>(fs);
            if (mameRomDefs is null)
            {
                throw new Exception($"No MAME rom definitions in {mameRomListJsonSource}");
            }
            _logger.LogInformation("Found {mameRomDefCount} MAME rom definitions", mameRomDefs.Count());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Can't load Mame rom definitions");
        }
        
        return mameRomDefs;
    }

    private static string? GetRomAssetPath(MameRomDef? romDef, string assetBasePath, params string[] assetExtensions)
    {
        string? assetPath = null;

        if (romDef is not null)
        {
            foreach (string assetExtension in assetExtensions)
            {
                string path = Path.Combine(assetBasePath, $"{romDef.RomName}.{assetExtension}");
                if (File.Exists(path))
                {
                    assetPath = path;
                    break;
                }
            }
        }

        return assetPath;
    }

    public string? GetRomLogoPath(MameRomDef? romDef)
        => GetRomAssetPath(romDef, _mameConfig.RomLogoDirectory, "png", "jpg", "bmp");

    public string? GetRomSnapPath(MameRomDef? romDef)
        => GetRomAssetPath(romDef, _mameConfig.RomSnapDirectory, "wmv", "mp4", "avi");

    public string? GetRomPreviewPath(MameRomDef? romDef)
        => GetRomAssetPath(romDef, _mameConfig.RomPreviewDirectory, "png", "jpg", "bmp");

    public async Task LaunchGame(MameRomDef? romDef)
    {
        if (romDef is not null)
        {
            List<string> args = new()
            {
                romDef.RomName,
                $"-inipath \"{_mameConfig.MameDirectory}\""
            };
            if (_mameConfig.SkipGameInfo)
            {
                args.Add("-skip_gameinfo");
            }
            if (_mameConfig.UseJoystickProviderWinHybrid)
            {
                args.Add("-joystickprovider winhybrid");
            }
            if (_mameConfig.UseWindowedMode)
            {
                args.Add("-window");
                if (!_mameConfig.MaximizeWindow)
                {
                    args.Add("-nomaximize");
                }
            }
            foreach (string arg in _mameConfig.AdditionnalArguments)
            {
                args.Add(arg);
            }

            string mameExePath = Path.Combine(_mameConfig.MameDirectory, "mame.exe");
            Process? process = ProcessHelper.ExecuteProcess(
                mameExePath,
                args.ToArray()
            );
            if (process is not null)
            {
                await process.WaitForExitAsync();
            }
        }
    }
}
