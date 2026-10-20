using Raylib_cs;



Raylib.InitWindow(1000, 700, "Tower Defense");
Raylib.SetTargetFPS(60);

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();

    Raylib.ClearBackground(Color.Black);
    Raylib.DrawText("Hello, Tower Defense!", 300, 300, 30, Color.White);

    Raylib.EndDrawing();
}

Raylib.CloseWindow();