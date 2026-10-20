using System.Numerics;
using Raylib_cs;

namespace TowerDefense.Enemies;

public sealed class TankEnemy : Enemy
{
    public TankEnemy(IReadOnlyList<Vector2> path)
        : base(path, speed: 55, radius: 22, color: Color.Purple,
            health: 180, reward: 30)
    {
    }
}
