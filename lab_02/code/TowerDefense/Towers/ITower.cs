using System.Numerics;
using Raylib_cs;

namespace TowerDefense.Towers;

public interface ITower
{
    Vector2 Position { get; }
    float Range { get; }
    int Damage { get; }
    float AttackInterval { get; }
    Color BodyColor { get; }
    Color CoreColor { get; }
    TowerUpgrade Upgrades { get; }
    bool CanShoot { get; }
    bool IsShooting { get; }
    Vector2 LastTargetPosition { get; }

    void AdvanceTime(float deltaTime);
    void RecordShot(Vector2 targetPosition, float attackInterval);
}
