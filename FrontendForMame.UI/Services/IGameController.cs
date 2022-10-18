using System.Collections.Generic;

namespace FrontendForMame.UI.Services;

interface IGameController
{
    string? Name { get; }

    void Update();
    void Reset();

    bool JustHitRight();
    bool JustHitLeft();
    bool JustHitButton(int buttonId);

    IEnumerable<int> GetPressedButtons();
}
