using System.Numerics;
using Raylib_cs;
using TowerDefense.Input;
using Xunit;

namespace TowerDefense.Checks;

public sealed class RaylibInputAdapterTests
{
    [Theory]
    [InlineData(GameAction.Restart, KeyboardKey.R, true)]
    [InlineData(GameAction.TogglePause, KeyboardKey.P, false)]
    [InlineData(GameAction.StartWave, KeyboardKey.Space, true)]
    [InlineData(GameAction.BuyRangeUpgrade, KeyboardKey.One, false)]
    [InlineData(GameAction.BuyDamageUpgrade, KeyboardKey.Two, true)]
    [InlineData(GameAction.BuySkinUpgrade, KeyboardKey.Three, false)]
    public void KeyboardActionQueriesTheMappedRaylibKey(
        GameAction action, KeyboardKey expectedKey, bool pressed)
    {
        var queriedKeys = new List<KeyboardKey>();
        IGameInput input = new RaylibInputAdapter(
            key =>
            {
                queriedKeys.Add(key);
                return pressed;
            },
            _ => throw new InvalidOperationException("A keyboard action queried the mouse."),
            () => Vector2.Zero,
            () => false
        );

        Assert.Equal(pressed, input.IsPressed(action));
        Assert.Equal(expectedKey, Assert.Single(queriedKeys));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void BuildActionQueriesTheLeftMouseButton(bool pressed)
    {
        var queriedButtons = new List<MouseButton>();
        IGameInput input = new RaylibInputAdapter(
            _ => throw new InvalidOperationException("The build action queried the keyboard."),
            button =>
            {
                queriedButtons.Add(button);
                return pressed;
            },
            () => Vector2.Zero,
            () => false
        );

        Assert.Equal(pressed, input.IsPressed(GameAction.BuildTower));
        Assert.Equal(MouseButton.Left, Assert.Single(queriedButtons));
    }

    [Fact]
    public void PointerAndQuitReflectTheCurrentRaylibValues()
    {
        var pointer = new Vector2(120, 25);
        bool shouldQuit = false;
        IGameInput input = new RaylibInputAdapter(_ => false, _ => false, () => pointer, () => shouldQuit);

        Assert.Equal(new Vector2(120, 25), input.PointerPosition);
        Assert.False(input.ShouldQuit());

        pointer = new Vector2(360, 160);
        shouldQuit = true;

        Assert.Equal(new Vector2(360, 160), input.PointerPosition);
        Assert.True(input.ShouldQuit());
    }
}
