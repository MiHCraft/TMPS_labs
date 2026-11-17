using System.Numerics;
using TowerDefense.Enemies;

namespace TowerDefense.Factories;

public sealed class TankEnemyFactory : EnemyFactory
{
    public override IEnemy CreateEnemy(IReadOnlyList<Vector2> path)
    {
        return new ShieldedEnemyProxy(new TankEnemy(path));
    }
}
