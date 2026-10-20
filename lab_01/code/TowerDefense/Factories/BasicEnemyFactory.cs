using System.Numerics;
using TowerDefense.Enemies;

namespace TowerDefense.Factories;

public sealed class BasicEnemyFactory : EnemyFactory
{
    public override Enemy CreateEnemy(IReadOnlyList<Vector2> path)
    {
        return new BasicEnemy(path);
    }
}
