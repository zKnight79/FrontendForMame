using System.Diagnostics;

namespace FrontendForMame.UI.Helpers;

public static class ProcessHelper
{
    public static void ExecuteProcess(string fileName, params string[] args)
    {
        string arguments = string.Join(' ', args);
        ProcessStartInfo psi = new()
        {
            FileName = fileName,
            Arguments = arguments,
            CreateNoWindow = true,
            UseShellExecute = false
        };
        Process.Start(psi);
    }
}