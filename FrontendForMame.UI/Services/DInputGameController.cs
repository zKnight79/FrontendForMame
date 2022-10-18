using SharpDX.DirectInput;
using System.Collections.Generic;

namespace FrontendForMame.UI.Services;

class DInputGameController : IGameController
{
    private readonly Joystick _joystick;

    private JoystickState PreviousState { get; set; } = new();
    private JoystickState CurrentState { get; set; } = new();

    public DInputGameController(Joystick joystick)
    {
        _joystick = joystick;
        Reset();
    }

    public string? Name => _joystick.Information.InstanceName;

    public void Update()
    {
        _joystick.Poll();
        PreviousState = CurrentState;
        CurrentState = _joystick.GetCurrentState();
    }
    public void Reset()
    {
        CurrentState = PreviousState = _joystick.GetCurrentState();
    }

    public bool JustHitRight()
        => CurrentState.X == ushort.MaxValue && PreviousState.X < ushort.MaxValue;

    public bool JustHitLeft()
        => CurrentState.X == ushort.MinValue && PreviousState.X > ushort.MinValue;

    public bool JustHitButton(int buttonId)
        => CurrentState.Buttons[buttonId] && !PreviousState.Buttons[buttonId];

    public IEnumerable<int> GetPressedButtons()
    {
        for (int i=0;i<CurrentState.Buttons.Length; ++i)
        {
            if (CurrentState.Buttons[i])
            {
                yield return i;
            }
        }
    }
}
