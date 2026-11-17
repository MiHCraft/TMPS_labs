using System.Numerics;
using Raylib_cs;

namespace TowerDefense.Towers;

public abstract class TowerDecorator : ITower
{
    protected TowerDecorator(ITower tower)
    {
        WrappedTower = tower;
    }

    protected ITower WrappedTower { get; }

    public Vector2 Position => WrappedTower.Position;
    public virtual float Range => WrappedTower.Range;
    public virtual int Damage => WrappedTower.Damage;
    public float AttackInterval => WrappedTower.AttackInterval;
    public virtual Color BodyColor => WrappedTower.BodyColor;
    public virtual Color CoreColor => WrappedTower.CoreColor;
    public virtual TowerUpgrade Upgrades => WrappedTower.Upgrades;
    public bool CanShoot => WrappedTower.CanShoot;
    public bool IsShooting => WrappedTower.IsShooting;
    public Vector2 LastTargetPosition => WrappedTower.LastTargetPosition;

    public void AdvanceTime(float deltaTime) => WrappedTower.AdvanceTime(deltaTime);

    public void RecordShot(Vector2 targetPosition, float attackInterval)
        => WrappedTower.RecordShot(targetPosition, attackInterval);
}
