namespace TowerDefense.Towers;

public sealed class DamageTowerDecorator(ITower tower) : TowerDecorator(tower)
{
    public override int Damage => base.Damage + 15;
    public override TowerUpgrade Upgrades => base.Upgrades | TowerUpgrade.Damage;
}
