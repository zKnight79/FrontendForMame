using SharpDX.DirectInput;
using System;
using System.Timers;

namespace FrontendForMame.UI.Services;

class ControllerManager : IControllerManager
{
    private readonly DirectInput _directInput = new();
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
        #region UPDATE CONTROLLERS
        foreach (IGameController gameController in _controllers)
        {
            if (gameController is not null)
            {
                gameController.Update();
            }
        }
        #endregion

        #region RIGHT EVENT
        foreach (IGameController gameController in _controllers)
        {
            if (gameController?.JustHitRight() ?? false)
            {
                OnRight?.Invoke();
                break;
            }
        }
        #endregion

        #region LEFT EVENT
        foreach (IGameController gameController in _controllers)
        {
            if (gameController?.JustHitLeft() ?? false)
            {
                OnLeft?.Invoke();
                break;
            }
        }
        #endregion

        #region LAUNCH EVENT
        foreach (IGameController gameController in _controllers)
        {
            if (gameController?.JustHitButton(0) ?? false)
            {
                OnLaunch?.Invoke();
                break;
            }
        }
        #endregion

        #region EXIT EVENT
        foreach (IGameController gameController in _controllers)
        {
            if (gameController?.JustHitButton(8) ?? false)
            {
                OnExit?.Invoke();
                break;
            }
        }
        #endregion

        #region SHUTDOWN EVENT
        foreach (IGameController gameController in _controllers)
        {
            if (gameController?.JustHitButton(9) ?? false)
            {
                OnShutdown?.Invoke();
                break;
            }
        }
        #endregion
    }
}
