using System.Numerics;
using Raylib_cs;

namespace TowerDefense.Towers;

public class Tower : ITower
{
    public const int Cost = 60;

    private float cooldown;
    private float shotTime;

    public Vector2 Position { get; }
    public float Range => 155;
    public int Damage => 25;
    public float AttackInterval => 0.65f;
    public Color BodyColor => Color.DarkBlue;
    public Color CoreColor => Color.SkyBlue;
    public TowerUpgrade Upgrades => TowerUpgrade.None;
    public bool CanShoot => cooldown == 0;
    public bool IsShooting => shotTime > 0;
    public Vector2 LastTargetPosition { get; private set; }

    public Tower(Vector2 position)
    {
        Position = position;
    }

    public void AdvanceTime(float deltaTime)
    {
        cooldown = MathF.Max(0, cooldown - deltaTime);
        shotTime = MathF.Max(0, shotTime - deltaTime);
    }

    public void RecordShot(Vector2 targetPosition, float attackInterval)
    {
        LastTargetPosition = targetPosition;
        shotTime = 0.1f;
        cooldown = attackInterval;
    }
}
