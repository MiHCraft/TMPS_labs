using System.Numerics;
using TowerDefense.Enemies;

namespace TowerDefense.Factories;

public sealed class FastEnemyFactory : EnemyFactory
{
    public override IEnemy CreateEnemy(IReadOnlyList<Vector2> path)
    {
        return new FastEnemy(path);
    }
}
