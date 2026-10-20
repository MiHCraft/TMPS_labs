using System.Numerics;
using TowerDefense.Factories;

namespace TowerDefense.Enemies;

public class EnemyWave
{
    private const float SpawnInterval = 0.9f;

    private readonly IReadOnlyList<Vector2> path;
    private readonly List<EnemyFactory> factories = new List<EnemyFactory>();
    private int nextEnemy;
    private float spawnTimer;

    public bool AllEnemiesSpawned => nextEnemy == factories.Count;

    public EnemyWave(int number, IReadOnlyList<Vector2> path)
    {
        this.path = path;
        int enemyCount = 4 + number * 2;

        for (int i = 0; i < enemyCount; i++)
        {
            if (number >= 3 && i % 4 == 3)
                factories.Add(new TankEnemyFactory());
            else if (number >= 2 && i % 3 == 2)
                factories.Add(new FastEnemyFactory());
            else
                factories.Add(new BasicEnemyFactory());
        }
    }

    public void Update(float deltaTime, List<Enemy> enemies)
    {
        spawnTimer -= deltaTime;

        while (!AllEnemiesSpawned && spawnTimer <= 0)
        {
            // Factory Method: the wave uses the base factory type;
            // the overridden method determines the concrete enemy class.
            EnemyFactory factory = factories[nextEnemy];
            enemies.Add(factory.CreateEnemy(path));
            nextEnemy++;
            spawnTimer += SpawnInterval;
        }
    }
}
