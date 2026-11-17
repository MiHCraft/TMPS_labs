using System.Numerics;
using TowerDefense.Enemies;

namespace TowerDefense.Factories;

public abstract class EnemyFactory
{
    // CREATIONAL PATTERN: FACTORY METHOD
    public abstract Enemy CreateEnemy(IReadOnlyList<Vector2> path);
}
