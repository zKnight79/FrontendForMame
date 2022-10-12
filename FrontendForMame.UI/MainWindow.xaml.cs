using System.Diagnostics;
using System.Windows;

namespace FrontendForMame.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public string Version { get; set; } = App.Version;
    public string Controller1Name { get; set; } = "Controller 1";
    public string Controller2Name { get; set; } = "Controller 2";

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
