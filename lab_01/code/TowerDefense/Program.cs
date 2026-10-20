using System.Numerics;
using Raylib_cs;
using TowerDefense;
using TowerDefense.Enemies;
using TowerDefense.Map;
using TowerDefense.Towers;

const int screenWidth = 1000;
const int screenHeight = 700;
const int cellSize = 60;
const int gridX = 140;
const int gridY = 100;

// Builder: define the size and road step by step, then build the map.
GameMap map = new MapBuilder()
    .SetSize(8, 12)
    .AddHorizontalRoad(2, 0, 4)
    .AddVerticalRoad(4, 2, 5)
    .AddHorizontalRoad(5, 4, 11)
    .Build();

var waypoints = new List<Vector2>();
foreach (var (row, col) in map.Path)
    waypoints.Add(CellCenter(row, col));

GameSession session = GameSession.Instance;
session.Reset();
var enemies = new List<Enemy>();
var towers = new List<Tower>();
EnemyWave? wave = null;
bool paused = false;

Raylib.InitWindow(screenWidth, screenHeight, "Tower Defense - Lab 1");
Raylib.SetTargetFPS(60);

while (!Raylib.WindowShouldClose())
{
    // 1. Input: restart, pause, start a wave, and build towers.
    if (Raylib.IsKeyPressed(KeyboardKey.R))
    {
        session.Reset();
        Array.Clear(map.Occupied);
        enemies.Clear();
        towers.Clear();
        wave = null;
        paused = false;
    }

    if (Raylib.IsKeyPressed(KeyboardKey.P) && !session.IsFinished)
        paused = !paused;

    if (Raylib.IsKeyPressed(KeyboardKey.Space) && !paused && session.BeginWave())
        wave = new EnemyWave(session.WaveNumber, waypoints);

    Vector2 mouse = Raylib.GetMousePosition();
    int col = (int)MathF.Floor((mouse.X - gridX) / cellSize);
    int row = (int)MathF.Floor((mouse.Y - gridY) / cellSize);
    bool insideGrid = row >= 0 && row < map.Rows && col >= 0 && col < map.Columns;

    if (!paused && !session.IsFinished && insideGrid
        && Raylib.IsMouseButtonPressed(MouseButton.Left)
        && map.CanBuild(row, col) && session.TrySpendGold(Tower.Cost))
    {
        map.BuildTower(row, col);
        towers.Add(new Tower(CellCenter(row, col)));
    }

    // 2. Update: move enemies, fire towers, and process the results.
    if (!paused && !session.IsFinished)
    {
        // Limit the frame time to prevent a large jump after moving the window.
        float deltaTime = MathF.Min(Raylib.GetFrameTime(), 0.1f);
        if (wave != null)
            wave.Update(deltaTime, enemies);

        foreach (Enemy enemy in enemies)
            enemy.Update(deltaTime);

        foreach (Tower tower in towers)
            tower.Update(deltaTime, enemies);

        // Iterate backwards so removal does not shift elements we have yet to check.
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            Enemy enemy = enemies[i];
            if (enemy.IsDead)
            {
                session.RewardEnemy(enemy.Reward);
                enemies.RemoveAt(i);
            }
            else if (enemy.HasReachedEnd)
            {
                session.LoseLife();
                enemies.RemoveAt(i);
            }
        }

        if (wave != null && wave.AllEnemiesSpawned && enemies.Count == 0)
        {
            session.CompleteWave();
            wave = null;
        }
    }

    // 3. Drawing: display the current data without changing the game logic.
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.DarkGray);
    Raylib.DrawText("TOWER DEFENSE", gridX, 20, 28, Color.White);
    Raylib.DrawText(
        $"Gold: {session.Gold}    Lives: {session.Lives}    "
        + $"Wave: {session.WaveNumber}/{GameSession.TotalWaves}    Kills: {session.DefeatedEnemies}",
        gridX, 62, 20, Color.White);

    for (int y = 0; y < map.Rows; y++)
    {
        for (int x = 0; x < map.Columns; x++)
        {
            int px = gridX + x * cellSize;
            int py = gridY + y * cellSize;
            Color color = Color.DarkGreen;

            if (map.Road[y, x])
                color = Color.Brown;
            if (map.Occupied[y, x])
                color = Color.Blue;

            if (insideGrid && x == col && y == row && !paused && !session.IsFinished)
            {
                bool canBuild = map.CanBuild(y, x) && session.Gold >= Tower.Cost;
                color = canBuild ? Color.LightGray : Color.Red;
            }

            Raylib.DrawRectangle(px + 1, py + 1, cellSize - 2, cellSize - 2, color);
        }
    }

    foreach (Tower tower in towers)
    {
        Raylib.DrawCircleV(tower.Position, 19, Color.DarkBlue);
        Raylib.DrawCircleV(tower.Position, 11, Color.SkyBlue);
        if (tower.IsShooting)
            Raylib.DrawLineEx(tower.Position, tower.LastTargetPosition, 3, Color.SkyBlue);

        if (insideGrid && tower.Position == CellCenter(row, col))
            Raylib.DrawCircleLines((int)tower.Position.X, (int)tower.Position.Y,
                tower.Range, Color.SkyBlue);
    }

    foreach (Enemy enemy in enemies)
    {
        Raylib.DrawCircleV(enemy.Position, enemy.Radius, enemy.Color);
        int barX = (int)enemy.Position.X - 20;
        int barY = (int)(enemy.Position.Y - enemy.Radius) - 9;
        Raylib.DrawRectangle(barX, barY, 40, 5, Color.Black);
        Raylib.DrawRectangle(barX, barY, 40 * enemy.Health / enemy.MaxHealth, 5, Color.Lime);
    }

    Raylib.DrawText("START", gridX - 65, gridY + 2 * cellSize + 22, 16, Color.White);
    Raylib.DrawText("BASE", gridX + map.Columns * cellSize + 10,
        gridY + 5 * cellSize + 22, 16, Color.White);
    Raylib.DrawText($"Click: build ({Tower.Cost})  |  SPACE: next wave  |  P: pause  |  R: restart",
        gridX, 610, 17, Color.White);

    string hint = "Defend the base! Hover over a tower to see its range.";
    if (!session.WaveInProgress && !session.IsFinished)
        hint = "Build towers near the road, then press SPACE.";
    if (insideGrid && !paused && !session.IsFinished)
    {
        if (map.Occupied[row, col])
            hint = "Tower range is shown by the blue circle.";
        else if (map.Road[row, col])
            hint = "You cannot build on the road.";
        else if (session.Gold < Tower.Cost)
            hint = "Not enough gold. Defeat enemies to earn more.";
    }
    Raylib.DrawText(hint, gridX, 645, 18, Color.White);

    if (paused || session.IsFinished)
    {
        Raylib.DrawRectangle(240, 240, 520, 190, new Color(20, 25, 35, 245));
        string title = paused ? "PAUSED" : "GAME OVER";
        if (session.HasWon)
            title = "VICTORY!";
        int textWidth = Raylib.MeasureText(title, 40);
        Raylib.DrawText(title, (screenWidth - textWidth) / 2, 275, 40, Color.White);
        string action = paused ? "Press P to continue" : "Press R to play again";
        textWidth = Raylib.MeasureText(action, 22);
        Raylib.DrawText(action, (screenWidth - textWidth) / 2, 350, 22, Color.White);
    }

    Raylib.EndDrawing();
}

Raylib.CloseWindow();

Vector2 CellCenter(int row, int col)
{
    return new Vector2(
        gridX + col * cellSize + cellSize / 2,
        gridY + row * cellSize + cellSize / 2);
}
