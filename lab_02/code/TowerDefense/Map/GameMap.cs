
namespace TowerDefense.Map;

public class GameMap
{
    public int Rows { get; }
    public int Columns { get; }

    public bool[,] Occupied { get; }
    public bool[,] Road { get; }
    public IReadOnlyList<(int row, int col)> Path { get; }

    public GameMap(int rows, int columns, bool[,] road)
        : this(rows, columns, road, Array.Empty<(int row, int col)>())
    {
    }

    public GameMap(int rows, int columns, bool[,] road,
        IEnumerable<(int row, int col)> path)
    {
        Rows = rows;
        Columns = columns;
        Road = road;
        Occupied = new bool[rows, columns];
        Path = Array.AsReadOnly(path.ToArray());
    }

    public bool CanBuild(int row, int col)
    {
        return row >= 0 && row < Rows
            && col >= 0 && col < Columns
            && !Road[row, col]
            && !Occupied[row, col];
    }

    public void BuildTower(int row, int col)
    {
        if (CanBuild(row, col))
            Occupied[row, col] = true;
    }
}
