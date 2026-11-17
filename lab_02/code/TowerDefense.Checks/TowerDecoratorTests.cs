using System.Numerics;
using Raylib_cs;
using TowerDefense.Enemies;
using TowerDefense.Towers;
using Xunit;

namespace TowerDefense.Checks;

public sealed class TowerDecoratorTests
{
    [Theory]
    [InlineData(TowerUpgrade.Range, TowerUpgrade.Damage, TowerUpgrade.Skin)]
    [InlineData(TowerUpgrade.Range, TowerUpgrade.Skin, TowerUpgrade.Damage)]
    [InlineData(TowerUpgrade.Damage, TowerUpgrade.Range, TowerUpgrade.Skin)]
    [InlineData(TowerUpgrade.Damage, TowerUpgrade.Skin, TowerUpgrade.Range)]
    [InlineData(TowerUpgrade.Skin, TowerUpgrade.Range, TowerUpgrade.Damage)]
    [InlineData(TowerUpgrade.Skin, TowerUpgrade.Damage, TowerUpgrade.Range)]
    public void AllUpgradeOrdersProduceTheSameTower(
        TowerUpgrade first,
        TowerUpgrade second,
        TowerUpgrade third
    )
    {
        ITower original = new Tower(new Vector2(40, 60));
        ITower decorated = Wrap(Wrap(Wrap(original, first), second), third);

        Assert.Equal(210f, decorated.Range);
        Assert.Equal(40, decorated.Damage);
        Assert.Equal(Color.Maroon, decorated.BodyColor);
        Assert.Equal(Color.Gold, decorated.CoreColor);
        Assert.Equal(original.Position, decorated.Position);
        Assert.Equal(original.AttackInterval, decorated.AttackInterval);
        Assert.Equal(
            TowerUpgrade.Range | TowerUpgrade.Damage | TowerUpgrade.Skin,
            decorated.Upgrades
        );
    }

    [Fact]
    public void UpgradedTowerHitsOneTargetBeyondItsOriginalRange()
    {
        ITower original = new Tower(Vector2.Zero);
        IEnemy nearest = new BasicEnemy([new Vector2(200, 0), new Vector2(300, 0)]);
        IEnemy farther = new BasicEnemy([new Vector2(205, 0), new Vector2(305, 0)]);
        IEnemy[] targets = [farther, nearest];

        original.Update(0, targets);
        Assert.Equal(60, nearest.Health);
        Assert.Equal(60, farther.Health);

        ITower upgraded = new DamageTowerDecorator(new RangeTowerDecorator(original));
        upgraded.Update(0, targets);

        Assert.Equal(20, nearest.Health);
        Assert.Equal(60, farther.Health);
        Assert.Equal(nearest.Position, upgraded.LastTargetPosition);
    }

    [Fact]
    public void SkinChangesColorsWithoutChangingCombatStats()
    {
        ITower original = new Tower(Vector2.Zero);
        ITower skinned = new SkinTowerDecorator(original);

        Assert.NotEqual(original.BodyColor, skinned.BodyColor);
        Assert.NotEqual(original.CoreColor, skinned.CoreColor);
        Assert.Equal(original.Range, skinned.Range);
        Assert.Equal(original.Damage, skinned.Damage);
        Assert.Equal(original.AttackInterval, skinned.AttackInterval);
    }

    private static ITower Wrap(ITower tower, TowerUpgrade upgrade) =>
        upgrade switch
        {
            TowerUpgrade.Range => new RangeTowerDecorator(tower),
            TowerUpgrade.Damage => new DamageTowerDecorator(tower),
            TowerUpgrade.Skin => new SkinTowerDecorator(tower),
            _ => throw new ArgumentOutOfRangeException(nameof(upgrade)),
        };
}
