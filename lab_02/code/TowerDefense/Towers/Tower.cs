using System.Numerics;
using TowerDefense.Enemies;

namespace TowerDefense.Towers;

public class Tower
{
    public const int Cost = 60;

    private float cooldown;
    private float shotTime;

    public Vector2 Position { get; }
    public float Range { get; } = 155;
    public int Damage { get; } = 25;
    public float AttackInterval { get; } = 0.65f;
    public Vector2 LastTargetPosition { get; private set; }
    public bool IsShooting => shotTime > 0;

    public Tower(Vector2 position)
    {
        Position = position;
    }

    public void Update(float deltaTime, List<Enemy> enemies)
    {
        cooldown = MathF.Max(0, cooldown - deltaTime);
        shotTime = MathF.Max(0, shotTime - deltaTime);

        if (cooldown > 0)
            return;

        Enemy? target = null;
        float nearestDistance = Range;

        // Find the nearest living enemy within the tower range.
        foreach (Enemy enemy in enemies)
        {
            if (enemy.IsDead || enemy.HasReachedEnd)
                continue;

            float distance = Vector2.Distance(Position, enemy.Position);
            if (distance <= nearestDistance)
            {
                target = enemy;
                nearestDistance = distance;
            }
        }

        if (target == null)
            return;

        target.TakeDamage(Damage);
        LastTargetPosition = target.Position;
        shotTime = 0.1f;
        cooldown = AttackInterval;
    }
}
