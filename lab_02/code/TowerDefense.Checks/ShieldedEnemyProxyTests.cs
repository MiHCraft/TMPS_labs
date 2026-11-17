using System.Numerics;
using TowerDefense.Enemies;
using Xunit;

namespace TowerDefense.Checks;

public sealed class ShieldedEnemyProxyTests
{
    [Fact]
    public void ActiveShieldForwardsOneTenthOfTheDamageEvenAfterTimePasses()
    {
        IEnemy tank = new TankEnemy([Vector2.Zero, new Vector2(1000, 0)]);
        IEnemy proxy = new ShieldedEnemyProxy(tank);

        proxy.Update(5);
        proxy.TakeDamage(100);

        Assert.True(proxy.HasActiveShield);
        Assert.Equal(new Vector2(275, 0), tank.Position);
        Assert.Equal(tank.Position, proxy.Position);
        Assert.Equal(170, tank.Health);
        Assert.Equal(tank.Health, proxy.Health);
    }

    [Fact]
    public void ShieldBreaksAfterFiveHundredDamageAndSubsequentHitsPassThrough()
    {
        IEnemy tank = new TankEnemy([Vector2.Zero, new Vector2(1000, 0)]);
        IEnemy proxy = new ShieldedEnemyProxy(tank);

        proxy.TakeDamage(499);
        Assert.True(proxy.HasActiveShield);
        Assert.Equal(131, tank.Health);

        proxy.TakeDamage(1);
        Assert.False(proxy.HasActiveShield);
        Assert.Equal(131, tank.Health);

        proxy.TakeDamage(25);
        Assert.Equal(106, tank.Health);
        Assert.Equal(tank.Health, proxy.Health);
    }

    [Fact]
    public void TheHitThatExceedsRemainingShieldHealthIsStillReduced()
    {
        IEnemy tank = new TankEnemy([Vector2.Zero, new Vector2(1000, 0)]);
        IEnemy proxy = new ShieldedEnemyProxy(tank);

        proxy.TakeDamage(450);
        proxy.TakeDamage(100);

        Assert.False(proxy.HasActiveShield);
        Assert.Equal(125, tank.Health);
    }
}
