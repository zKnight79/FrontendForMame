using FrontendForMame.UI.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace FrontendForMame.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public string Version { get; set; } = App.Version;
    public string? FrontendTitle { get; set; }
    public string? Controller1Name { get; set; }
    public string? Controller2Name { get; set; }
    public string? GameFullName { get; set; }
    public string? GameLogoPath { get; set; }
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
#if DEBUG
        MessageBox.Show(@"Shutdown /!\");
#else
        ProcessHelper.ExecuteProcess("shutdown", "/s", "/t 0");
#endif
    }

    private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Up:
                break;
            case Key.Down:
                break;
            case Key.Space:
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
}
