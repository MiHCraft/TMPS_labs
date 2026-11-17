using System.Numerics;
using Raylib_cs;

namespace TowerDefense.Enemies;

public interface IEnemy
{
    Vector2 Position { get; }
    float Speed { get; }
    float Radius { get; }
    Color Color { get; }
    int MaxHealth { get; }
    int Health { get; }
    int Reward { get; }
    bool HasActiveShield { get; }
    bool IsDead { get; }
    bool HasReachedEnd { get; }

    void TakeDamage(int damage);
    void Update(float deltaTime);
}
