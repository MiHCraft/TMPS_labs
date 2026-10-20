
namespace TowerDefense.Map;

public class MapBuilder
{
    private int rows;
    private int columns;
    private readonly List<(int row, int col)> path = new();

    public MapBuilder SetSize(int rows, int columns)
    {
        if (rows <= 0 || columns <= 0)
            throw new ArgumentException("Invalid map size.");

        this.rows = rows;
        this.columns = columns;
        return this;
    }

    public MapBuilder AddRoadCell(int row, int col)
    {
        path.Add((row, col));
        return this;
    }

    public MapBuilder AddHorizontalRoad(int row, int startCol, int endCol)
    {
        int step = startCol <= endCol ? 1 : -1;

        for (int col = startCol; col != endCol + step; col += step)
        {
            AddRoadCell(row, col);
        }

        return this;
    }

    public MapBuilder AddVerticalRoad(
        int col, int startRow, int endRow)
    {
        int step = startRow <= endRow ? 1 : -1;

        for (int row = startRow; row != endRow + step; row += step)
        {
            AddRoadCell(row, col);
        }

        return this;
    }

    public GameMap Build()
    {
        if (rows <= 0 || columns <= 0)
            throw new InvalidOperationException(
                "Map size must be configured.");

        bool[,] road = new bool[rows, columns];
        var orderedPath = new List<(int row, int col)>();

        foreach (var (row, col) in path)
        {
            if (row < 0 || row >= rows ||
                col < 0 || col >= columns)
                throw new InvalidOperationException(
                    "Road cell is outside the map.");

            road[row, col] = true;

            // Include a shared endpoint of two consecutive segments only once in the path.
            if (orderedPath.Count == 0 || orderedPath[orderedPath.Count - 1] != (row, col))
                orderedPath.Add((row, col));
        }

        return new GameMap(rows, columns, road, orderedPath);
    }
}
