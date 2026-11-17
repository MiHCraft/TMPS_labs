using System.Numerics;

namespace TowerDefense.Input;

public interface IGameInput
{
    Vector2 PointerPosition { get; }
    bool IsPressed(GameAction action);
    bool ShouldQuit();
}
