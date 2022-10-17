namespace FrontendForMame.UI.Services;

interface IGameController
{
    string? Name { get; }

    void Update();
}
