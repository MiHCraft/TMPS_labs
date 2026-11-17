using System.Numerics;
using Raylib_cs;

namespace TowerDefense.Enemies;

public sealed class FastEnemy : Enemy
{
    public FastEnemy(IReadOnlyList<Vector2> path)
        : base(path, speed: 150, radius: 12, color: Color.Orange,
            health: 40, reward: 12)
    {
    }
}
