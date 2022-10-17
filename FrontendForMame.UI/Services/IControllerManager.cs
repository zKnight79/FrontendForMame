namespace FrontendForMame.UI.Services;


public delegate void ControllerManagerEventHandler();

public interface IControllerManager
{
    string? Controller1Name { get; }
    string? Controller2Name { get; }

    event ControllerManagerEventHandler? OnRight;
    event ControllerManagerEventHandler? OnLeft;
    event ControllerManagerEventHandler? OnLaunch;
    event ControllerManagerEventHandler? OnExit;
    event ControllerManagerEventHandler? OnShutdown;

    void Init();
    void Update();
}
