using FrontendForMame.UI.Extensions;
using FrontendForMame.UI.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace FrontendForMame.UI.Services;

class MameService : IMameService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MameService> _logger;

    public MameService(IConfiguration configuration, ILogger<MameService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public IEnumerable<MameRomDef>? GetRomDefinitions()
    {
        IEnumerable<MameRomDef>? mameRomDefs = null;

        try
        {
            string mameRomListJsonSource = _configuration.GetMameRomListJsonSource();
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

    public string? GetRomLogoPath(MameRomDef? romDef)
    {
        string? logoPath = null;

        if (romDef is not null)
        {
            string path = Path.Combine(_configuration.GetMameRomLogoDirectory(), $"{romDef.RomName}.png");
            if (File.Exists(path))
            {
                logoPath = path;
            }
        }

        return logoPath;
    }
}
