using FrontendForMame.UI.Extensions;
using FrontendForMame.UI.Helpers;
using FrontendForMame.UI.Model;
using FrontendForMame.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace FrontendForMame.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    private readonly IConfiguration _configuration;
    private readonly IMameService _mameService;
    private readonly IControllerManager _controllerManager;
    private readonly DispatcherTimer _controllerDispatcher = new()
    {
        Interval = TimeSpan.FromMilliseconds(25),
        IsEnabled = false
    };
    private DispatcherTimer? _scrollDispatcher;

    public MainWindow(IConfiguration configuration, IMameService mameService, IControllerManager controllerManager)
    {
        _configuration = configuration;
        _mameService = mameService;
        _controllerManager = controllerManager;

        InitializeComponent();

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

        #region CONTROLLER EVENTS
        _controllerManager.OnRight += Right_Click;
        _controllerManager.OnLeft += Left_Click;
        if (!_configuration.GetControllerTestMode())
        {
            _controllerManager.OnLaunch += Launch_Click;
            _controllerManager.OnExit += Exit_Click;
            _controllerManager.OnShutdown += Shutdown_Click;
        }
        #endregion
    }

    public static string Version => App.Version;

    public IEnumerable<MameRomDef>? MameRomDefs { get; set; }
    public int CurrentMameRomDefId { get; set; }
    public MameRomDef? CurrentMameRomDef => MameRomDefs?.ElementAt(CurrentMameRomDefId);
    public int MameRomDefCount => MameRomDefs?.Count() ?? 0;
    public string? CurrentMameRomLogoPath => _mameService?.GetRomLogoPath(CurrentMameRomDef);
    public string? CurrentMameRomSnapPath => _mameService?.GetRomSnapPath(CurrentMameRomDef);
    public string? CurrentMameRomPreviewPath => _mameService?.GetRomPreviewPath(CurrentMameRomDef);

    public int NextMameRomDefId => MameRomDefCount == 0 ? 0 : (CurrentMameRomDefId + 1) % MameRomDefCount;
    public int PreviousMameRomDefId => MameRomDefCount == 0 ? 0 : (MameRomDefCount + CurrentMameRomDefId - 1) % MameRomDefCount;
    public MameRomDef? NextMameRomDef => MameRomDefs?.ElementAt(NextMameRomDefId);
    public MameRomDef? PreviousMameRomDef => MameRomDefs?.ElementAt(PreviousMameRomDefId);
    public string? NextMameRomLogoPath => _mameService?.GetRomLogoPath(NextMameRomDef);
    public string? PreviousMameRomLogoPath => _mameService?.GetRomLogoPath(PreviousMameRomDef);

    private void ChangeCurrentMameRomDef(int direction)
    {
        _scrollDispatcher?.Stop();
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
        _scrollDispatcher?.Start();
    }
    private void NotifyCurrentMameRomChanged()
    {
        GamePreviewControl.Visibility = Visibility.Visible;
        GameSnapControl.Visibility = Visibility.Hidden;

        OnPropertyChanged(nameof(CurrentMameRomDef));
        OnPropertyChanged(nameof(CurrentMameRomLogoPath));
        OnPropertyChanged(nameof(CurrentMameRomSnapPath));
        OnPropertyChanged(nameof(CurrentMameRomPreviewPath));
        OnPropertyChanged(nameof(NextMameRomLogoPath));
        OnPropertyChanged(nameof(PreviousMameRomLogoPath));
    }

    public string? Controller1Name => _controllerManager.Controller1Name;
    public string? Controller2Name => _controllerManager.Controller2Name;
    public string ControllerTestHeight => _configuration.GetControllerTestMode() ? "*" : "0";
    public string Controller1Buttons => $"[{string.Join(", ", _controllerManager.GetPressedButtons().ElementAt(0))}]";
    public string Controller2Buttons => $"[{string.Join(", ", _controllerManager.GetPressedButtons().ElementAt(1))}]";

    public int AutoScrollGamesDelay => _configuration.GetAutoScrollGamesDelay();


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

        _controllerManager.Init();
        OnPropertyChanged(nameof(Controller1Name));
        OnPropertyChanged(nameof(Controller2Name));

        _controllerDispatcher.Tick += (sender, e) =>
        {
            _controllerManager.Update();
            OnPropertyChanged(nameof(Controller1Buttons));
            OnPropertyChanged(nameof(Controller2Buttons));
        };
        _controllerDispatcher.Start();

        if (AutoScrollGamesDelay > 0)
        {
            _scrollDispatcher = new()
            {
                Interval = TimeSpan.FromSeconds(AutoScrollGamesDelay)
            };
            _scrollDispatcher.Tick += (sender, e) => Right_Click(this, new());
            _scrollDispatcher.Start();
        }
    }

    private void Left_Click(object sender, RoutedEventArgs e)
    {
        ChangeCurrentMameRomDef(-1);
    }

    private void Right_Click(object sender, RoutedEventArgs e)
    {
        ChangeCurrentMameRomDef(1);
    }

    private async void Launch_Click(object sender, RoutedEventArgs e)
    {
        _scrollDispatcher?.Stop();
        _controllerDispatcher.Stop();
        GameSnapControl.Pause();
        await _mameService.LaunchGame(CurrentMameRomDef);
        GameSnapControl.Play();
        _controllerDispatcher.Start();
        _scrollDispatcher?.Start();
    }

    private void GameSnapControl_Play(object sender, RoutedEventArgs e)
    {
        GameSnapControl.Position = TimeSpan.Zero;
        GameSnapControl.Play();
    }

    private void GameSnapControl_MediaOpened(object sender, RoutedEventArgs e)
    {
        GamePreviewControl.Visibility = Visibility.Hidden;
        GameSnapControl.Visibility = Visibility.Visible;

        /*
            Hack because
            Windows Media Player opens video
            with black screen for a few second
            only on the first play
         */
        if (_configuration.GetUseVideoOpenedHack())
        {
            GameSnapControl.Position = GameSnapControl.NaturalDuration.TimeSpan.Subtract(TimeSpan.FromMilliseconds(1));
        }
    }
}
