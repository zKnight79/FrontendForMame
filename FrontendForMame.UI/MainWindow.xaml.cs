using FrontendForMame.UI.Extensions;
using FrontendForMame.UI.Helpers;
using FrontendForMame.UI.Model;
using FrontendForMame.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace FrontendForMame.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    private readonly IConfiguration _configuration;
    private readonly IMameService _mameService;

    public MainWindow(IConfiguration configuration, IMameService mameService)
    {
        InitializeComponent();

        _configuration = configuration;
        _mameService = mameService;

        #region WINDOW CONFIGURATION
        if (_configuration.GetLaunchFullscreen())
        {
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;
        }
        else
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        #endregion

        #region CUSTOMIZED TITLE
        string customizedTitle = _configuration.GetCustomizedTitle();
        if (!string.IsNullOrWhiteSpace(customizedTitle))
        {
            Title = customizedTitle;
        }
        #endregion
    }

    public string Version { get; set; } = App.Version;

    public IEnumerable<MameRomDef>? MameRomDefs { get; set; }
    public int CurrentMameRomDefId { get; set; }
    public MameRomDef? CurrentMameRomDef => MameRomDefs?.ElementAt(CurrentMameRomDefId);
    public int MameRomDefCount => MameRomDefs?.Count() ?? 0;
    public string? CurrentMameRomLogoPath => _mameService?.GetRomLogoPath(CurrentMameRomDef);

    private void ChangeCurrentMameRomDef(int direction)
    {
        if (MameRomDefCount > 0)
        {
            CurrentMameRomDefId += direction;
            if (CurrentMameRomDefId < 0)
            {
                CurrentMameRomDefId %= MameRomDefCount;
                CurrentMameRomDefId += MameRomDefCount;
            }
            if (CurrentMameRomDefId >= MameRomDefCount)
            {
                CurrentMameRomDefId %= MameRomDefCount;
            }
            NotifyCurrentMameRomChanged();
        }
    }

    private void NotifyCurrentMameRomChanged()
    {
        OnPropertyChanged(nameof(CurrentMameRomDef));
        OnPropertyChanged(nameof(CurrentMameRomLogoPath));
    }

    public string? Controller1Name { get; set; }
    public string? Controller2Name { get; set; }
    public string? GameSnapPath { get; set; }
    public string? GamePreviewPath { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Shutdown_Click(object sender, RoutedEventArgs e)
    {
        if (_configuration.GetAllowSystemShutdown())
        {
            ProcessHelper.ExecuteProcess("shutdown", "/s", "/t 0");
        }
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Left:
                Left_Click(sender, e);
                break;
            case Key.Right:
                Right_Click(sender, e);
                break;
            case Key.Space:
                Launch_Click(sender, e);
                break;
            case Key.E:
                if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                {
                    Exit_Click(sender, e);
                }
                break;
            case Key.S:
                if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                {
                    Shutdown_Click(sender, e);
                }
                break;
        }
    }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        MameRomDefs = _mameService.GetRomDefinitions();
        NotifyCurrentMameRomChanged();
    }

    private void Left_Click(object sender, RoutedEventArgs e)
    {
        ChangeCurrentMameRomDef(-1);
    }

    private void Right_Click(object sender, RoutedEventArgs e)
    {
        ChangeCurrentMameRomDef(1);
    }

    private void Launch_Click(object sender, RoutedEventArgs e)
    {

    }
}
