using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;

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
    public string Controller1Name { get; set; } = "Controller 1";
    public string Controller2Name { get; set; } = "Controller 2";
    public string GameFullName { get; set; } = "Game Full Name";
    public string GameLogoPath{ get; set; } = "/Data/Missing.png";
    public string GameSnapPath { get; set; } = string.Empty;

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
        ProcessStartInfo psi = new()
        {
            FileName = "shutdown",
            Arguments = "/s /t 0",
            CreateNoWindow = true,
            UseShellExecute = false
        };
        Process.Start(psi);
    }
}
