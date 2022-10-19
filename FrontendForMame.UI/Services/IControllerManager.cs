using System.Collections.Generic;
using System.Windows;

namespace FrontendForMame.UI.Services;

public interface IControllerManager
{
    string? Controller1Name { get; }
    string? Controller2Name { get; }

    event RoutedEventHandler? OnRight;
    event RoutedEventHandler? OnLeft;
    event RoutedEventHandler? OnLaunch;
    event RoutedEventHandler? OnExit;
    event RoutedEventHandler? OnShutdown;

    void Init();
    void Update();

    IEnumerable<int>[] GetPressedButtons();
}
