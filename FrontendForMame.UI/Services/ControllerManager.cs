namespace FrontendForMame.UI.Services;

class ControllerManager : IControllerManager
{
    private readonly IGameController[] _controllers = new IGameController[2];

    public string? Controller1Name => _controllers[0]?.Name;
    public string? Controller2Name => _controllers[1]?.Name;

    public event ControllerManagerEventHandler? OnRight;
    public event ControllerManagerEventHandler? OnLeft;
    public event ControllerManagerEventHandler? OnLaunch;
    public event ControllerManagerEventHandler? OnExit;
    public event ControllerManagerEventHandler? OnShutdown;

    public void Init()
    {
        
    }
}
