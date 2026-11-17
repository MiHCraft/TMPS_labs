using System.Numerics;
using TowerDefense.Enemies;

namespace TowerDefense.Towers;

public static class TowerCombat
{
    public static void Update(this ITower tower, float deltaTime, IReadOnlyList<IEnemy> enemies)
    {
        tower.AdvanceTime(deltaTime);

        if (!tower.CanShoot)
            return;

        IEnemy? target = null;
        float nearestDistance = tower.Range;

        // Find the nearest living enemy within the effective tower range.
        foreach (IEnemy enemy in enemies)
        {
            if (enemy.IsDead || enemy.HasReachedEnd)
                continue;

            float distance = Vector2.Distance(tower.Position, enemy.Position);
            if (distance <= nearestDistance)
            {
                target = enemy;
                nearestDistance = distance;
            }
        }

        if (target == null)
            return;

        target.TakeDamage(tower.Damage);
        tower.RecordShot(target.Position, tower.AttackInterval);
    }
}
