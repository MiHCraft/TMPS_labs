namespace TowerDefense.Towers;

public sealed class RangeTowerDecorator(ITower tower) : TowerDecorator(tower)
{
    public override float Range => base.Range + 55;
    public override TowerUpgrade Upgrades => base.Upgrades | TowerUpgrade.Range;
}
