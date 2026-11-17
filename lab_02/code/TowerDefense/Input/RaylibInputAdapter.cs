using System.Numerics;
using Raylib_cs;

namespace TowerDefense.Input;

public sealed class RaylibInputAdapter : IGameInput
{
    private readonly Func<KeyboardKey, bool> isKeyPressed;
    private readonly Func<MouseButton, bool> isMouseButtonPressed;
    private readonly Func<Vector2> getMousePosition;
    private readonly Func<bool> windowShouldClose;

    public RaylibInputAdapter()
        : this(
            static key => Raylib.IsKeyPressed(key),
            static button => Raylib.IsMouseButtonPressed(button),
            Raylib.GetMousePosition,
            static () => Raylib.WindowShouldClose()
        )
    {
    }

    internal RaylibInputAdapter(
        Func<KeyboardKey, bool> isKeyPressed,
        Func<MouseButton, bool> isMouseButtonPressed,
        Func<Vector2> getMousePosition,
        Func<bool> windowShouldClose
    )
    {
        this.isKeyPressed = isKeyPressed;
        this.isMouseButtonPressed = isMouseButtonPressed;
        this.getMousePosition = getMousePosition;
        this.windowShouldClose = windowShouldClose;
    }

    public Vector2 PointerPosition => getMousePosition();

    public bool IsPressed(GameAction action)
    {
        return action switch
        {
            GameAction.Restart => isKeyPressed(KeyboardKey.R),
            GameAction.TogglePause => isKeyPressed(KeyboardKey.P),
            GameAction.StartWave => isKeyPressed(KeyboardKey.Space),
            GameAction.BuildTower => isMouseButtonPressed(MouseButton.Left),
            GameAction.BuyRangeUpgrade => isKeyPressed(KeyboardKey.One),
            GameAction.BuyDamageUpgrade => isKeyPressed(KeyboardKey.Two),
            GameAction.BuySkinUpgrade => isKeyPressed(KeyboardKey.Three),
            _ => throw new ArgumentOutOfRangeException(nameof(action), action, null),
        };
    }

    public bool ShouldQuit() => windowShouldClose();
}
