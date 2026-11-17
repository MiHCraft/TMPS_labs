using System.Numerics;
using TowerDefense.Input;

namespace TowerDefense.Towers;

internal static class TowerShop
{
    public const int BuildCost = Tower.Cost;

    public static IReadOnlyList<TowerUpgradeOffer> UpgradeOffers { get; } =
        Array.AsReadOnly<TowerUpgradeOffer>([
            new(TowerUpgrade.Range, GameAction.BuyRangeUpgrade, "1 Range", 30,
                tower => new RangeTowerDecorator(tower)),
            new(TowerUpgrade.Damage, GameAction.BuyDamageUpgrade, "2 Damage", 40,
                tower => new DamageTowerDecorator(tower)),
            new(TowerUpgrade.Skin, GameAction.BuySkinUpgrade, "3 Skin", 15,
                tower => new SkinTowerDecorator(tower)),
        ]);

    public static ITower Build(Vector2 position) => new Tower(position);
}

internal sealed class TowerUpgradeOffer(
    TowerUpgrade upgrade, GameAction action, string label, int price, Func<ITower, ITower> decorate)
{
    public GameAction Action => action;

    public ITower Buy(ITower tower, GameSession session)
    {
        if (IsInstalled(tower) || !session.TrySpendGold(price))
            return tower;

        return decorate(tower);
    }

    public string GetHint(ITower tower) => IsInstalled(tower)
        ? $"{label} (owned)"
        : $"{label} ({price}g)";

    private bool IsInstalled(ITower tower) => (tower.Upgrades & upgrade) != 0;
}
