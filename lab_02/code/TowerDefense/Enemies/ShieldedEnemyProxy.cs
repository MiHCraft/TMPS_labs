using System.Numerics;
using Raylib_cs;

namespace TowerDefense.Enemies;

public sealed class ShieldedEnemyProxy : IEnemy
{
    private const int ShieldMaxHealth = 500;
    private const int ShieldDamageDivisor = 10;

    private readonly IEnemy enemy;
    private int shieldHealth = ShieldMaxHealth;

    public ShieldedEnemyProxy(IEnemy enemy)
    {
        this.enemy = enemy;
    }

    public Vector2 Position => enemy.Position;
    public float Speed => enemy.Speed;
    public float Radius => enemy.Radius;
    public Color Color => enemy.Color;
    public int MaxHealth => enemy.MaxHealth;
    public int Health => enemy.Health;
    public int Reward => enemy.Reward;
    public bool HasActiveShield => shieldHealth > 0;
    public bool IsDead => enemy.IsDead;
    public bool HasReachedEnd => enemy.HasReachedEnd;

    public void TakeDamage(int damage)
    {
        if (!HasActiveShield || damage <= 0)
        {
            enemy.TakeDamage(damage);
            return;
        }

        shieldHealth = Math.Max(0, shieldHealth - damage);
        enemy.TakeDamage(damage / ShieldDamageDivisor);
    }

    public void Update(float deltaTime) => enemy.Update(deltaTime);
}
