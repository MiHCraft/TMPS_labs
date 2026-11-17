using System.Numerics;
using Raylib_cs;

namespace TowerDefense.Enemies;

public abstract class Enemy
{
    private readonly Vector2[] path;
    private int nextWaypoint = 1;

    public Vector2 Position { get; private set; }
    public float Speed { get; }
    public float Radius { get; }
    public Color Color { get; }
    public int MaxHealth { get; }
    public int Health { get; private set; }
    public int Reward { get; }
    public bool IsDead => Health == 0;
    public bool HasReachedEnd => nextWaypoint >= path.Length;

    protected Enemy(IReadOnlyList<Vector2> path, float speed, float radius,
        Color color, int health, int reward)
    {
        ArgumentNullException.ThrowIfNull(path);
        if (path.Count == 0)
            throw new ArgumentException("An enemy needs at least one waypoint.", nameof(path));
        if (!float.IsFinite(speed) || speed <= 0)
            throw new ArgumentOutOfRangeException(nameof(speed));

        this.path = path.ToArray();
        Position = this.path[0];
        Speed = speed;
        Radius = radius;
        Color = color;
        MaxHealth = health;
        Health = health;
        Reward = reward;
    }

    public void TakeDamage(int damage)
    {
        if (damage > 0)
            Health = Math.Max(0, Health - damage);
    }

    public void Update(float deltaTime)
    {
        if (!float.IsFinite(deltaTime) || deltaTime < 0)
            throw new ArgumentOutOfRangeException(nameof(deltaTime));

        if (IsDead)
            return;

        float remainingDistance = Speed * deltaTime;

        // An enemy can pass several waypoints in one frame, including a corner.
        while (!HasReachedEnd)
        {
            Vector2 offset = path[nextWaypoint] - Position;
            float distance = offset.Length();

            if (distance <= remainingDistance)
            {
                Position = path[nextWaypoint];
                nextWaypoint++;
                remainingDistance -= distance;
            }
            else
            {
                Vector2 direction = offset / distance;
                Position += direction * remainingDistance;
                break;
            }
        }
    }
}
