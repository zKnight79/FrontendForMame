using FrontendForMame.UI.Extensions;
using FrontendForMame.UI.Model;
using Microsoft.Extensions.Configuration;
using SharpDX.DirectInput;

namespace FrontendForMame.UI.Services;

class ControllerManager : IControllerManager
{
    private readonly DirectInput _directInput = new();
    private readonly IGameController?[] _controllers = new IGameController[2];
    private readonly ControllerConfig _controllerConfig;

    public ControllerManager(IConfiguration configuration)
    {
        _controllerConfig = configuration.GetControllerConfig();
    }

    public string? Controller1Name => _controllers[0]?.Name;
    public string? Controller2Name => _controllers[1]?.Name;

    public event ControllerManagerEventHandler? OnRight;
    public event ControllerManagerEventHandler? OnLeft;
    public event ControllerManagerEventHandler? OnLaunch;
    public event ControllerManagerEventHandler? OnExit;
    public event ControllerManagerEventHandler? OnShutdown;

    public void Init()
    {
        int idx = 0;
        foreach (DeviceType deviceType in new DeviceType[] { DeviceType.Gamepad, DeviceType.Joystick })
        {
            foreach (DeviceInstance deviceInstance in _directInput.GetDevices(deviceType, DeviceEnumerationFlags.AllDevices))
            {
                Joystick joystick = new(_directInput, deviceInstance.InstanceGuid);
                joystick.Properties.BufferSize = 128;
                joystick.Acquire();
                _controllers[idx] = new DInputGameController(joystick);
                idx++;
                if (idx >= _controllers.Length)
                {
                    return;
                }
            }
        }
    }

    public void Update()
    {
        for (int i = 0; i < _controllers.Length; ++i)
        {
            IGameController? gameController = _controllers[i];
            if (gameController is not null)
            {
                #region UPDATE CONTROLLERS
                gameController.Update();
                #endregion
                #region RIGHT EVENT
                if (gameController.JustHitRight())
                {
                    OnRight?.Invoke();
                    break;
                }
                #endregion
                #region LEFT EVENT
                if (gameController.JustHitLeft())
                {
                    OnLeft?.Invoke();
                    break;
                }
                #endregion
                #region LAUNCH EVENT
                if (gameController.JustHitButton(_controllerConfig.GetLaunchButton(i)))
                {
                    OnLaunch?.Invoke();
                    break;
                }
                #endregion
                #region EXIT EVENT
                if (gameController.JustHitButton(_controllerConfig.GetExitButton(i)))
                {
                    OnExit?.Invoke();
                    break;
                }
                #endregion
                #region SHUTDOWN EVENT
                if (gameController.JustHitButton(_controllerConfig.GetShutdownButton(i)))
                {
                    OnShutdown?.Invoke();
                    break;
                }
                #endregion
            }
        }
    }
}
