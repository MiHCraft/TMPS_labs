
using Raylib_cs;
using System.Numerics;

const int screenWidth = 1000;
const int screenHeight = 700;

const int rows = 8;
const int columns = 12;
const int cellSize = 60;

const int gridX = 140;
const int gridY = 100;

bool[,] occupied = new bool[rows, columns];

Raylib.InitWindow(screenWidth, screenHeight, "Tower Defense");
Raylib.SetTargetFPS(60);

while (!Raylib.WindowShouldClose())
{
    // INPUT
    Vector2 mouse = Raylib.GetMousePosition();

    int col = (int)(mouse.X - gridX) / cellSize;
    int row = (int)(mouse.Y - gridY) / cellSize;

    bool insideGrid =
        mouse.X >= gridX &&
        mouse.Y >= gridY &&
        col >= 0 && col < columns &&
        row >= 0 && row < rows;

    if (insideGrid && Raylib.IsMouseButtonPressed(MouseButton.Left))
    {
        occupied[row, col] = !occupied[row, col];
    }

    // DRAW
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.DarkGray);

    Raylib.DrawText("TOWER DEFENSE", 140, 35, 30, Color.White);

    for (int y = 0; y < rows; y++)
    {
        for (int x = 0; x < columns; x++)
        {
            int px = gridX + x * cellSize;
            int py = gridY + y * cellSize;

            Color color = occupied[y, x]
                ? Color.Blue
                : Color.DarkGreen;

            if (insideGrid && x == col && y == row)
                color = Color.LightGray;

            Raylib.DrawRectangle(
                px + 1, py + 1,
                cellSize - 2, cellSize - 2,
                color
            );
        }
    }

    Raylib.DrawText(
        insideGrid
            ? $"Cell: ({col}, {row})"
            : "Cell: outside",
        140, 610, 20, Color.White
    );

    Raylib.EndDrawing();
}

Raylib.CloseWindow();
