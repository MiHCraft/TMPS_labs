using System.Numerics;
using TowerDefense.Enemies;

namespace TowerDefense.Factories;

public sealed class TankEnemyFactory : EnemyFactory
{
    public override Enemy CreateEnemy(IReadOnlyList<Vector2> path)
    {
        return new TankEnemy(path);
    }
}
