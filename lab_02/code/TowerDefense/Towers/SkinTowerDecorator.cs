using Raylib_cs;

namespace TowerDefense.Towers;

public sealed class SkinTowerDecorator(ITower tower) : TowerDecorator(tower)
{
    public override Color BodyColor => Color.Maroon;
    public override Color CoreColor => Color.Gold;
    public override TowerUpgrade Upgrades => base.Upgrades | TowerUpgrade.Skin;
}
