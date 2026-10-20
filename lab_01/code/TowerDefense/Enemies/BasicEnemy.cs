using System.Numerics;
using Raylib_cs;

namespace TowerDefense.Enemies;

public sealed class BasicEnemy : Enemy
{
    public BasicEnemy(IReadOnlyList<Vector2> path)
        : base(path, speed: 90, radius: 16, color: Color.Yellow,
            health: 60, reward: 15)
    {
    }
}
