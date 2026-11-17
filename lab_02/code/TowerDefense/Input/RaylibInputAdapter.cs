using System.Numerics;
using Raylib_cs;

namespace TowerDefense.Input;

public sealed class RaylibInputAdapter : IGameInput
{
    public Vector2 PointerPosition => Raylib.GetMousePosition();

    public bool IsPressed(GameAction action)
    {
        return action switch
        {
            GameAction.Restart => Raylib.IsKeyPressed(KeyboardKey.R),
            GameAction.TogglePause => Raylib.IsKeyPressed(KeyboardKey.P),
            GameAction.StartWave => Raylib.IsKeyPressed(KeyboardKey.Space),
            GameAction.BuildTower => Raylib.IsMouseButtonPressed(MouseButton.Left),
            GameAction.BuyRangeUpgrade => Raylib.IsKeyPressed(KeyboardKey.One),
            GameAction.BuyDamageUpgrade => Raylib.IsKeyPressed(KeyboardKey.Two),
            GameAction.BuySkinUpgrade => Raylib.IsKeyPressed(KeyboardKey.Three),
            _ => throw new ArgumentOutOfRangeException(nameof(action), action, null),
        };
    }

    public bool ShouldQuit() => Raylib.WindowShouldClose();
}
