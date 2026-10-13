public class Table
{
    private readonly List<Chair> chairs = new();

    public int Id { get; }

    public Table(int id)
    {
        if (id <= 0) throw new ArgumentException("Table ID must be positive.");
        Id = id;
    }

    public virtual string GetShapeName() => "Table";
    public int GetTheNumberOfAttachedChairs() => chairs.Count;
    public IReadOnlyList<Chair> GetChairs() => chairs.AsReadOnly();

    public void AddChair(Chair chair)
    {
        if (chairs.Contains(chair)) return;
        if (chairs.Any(c => c.Id == chair.Id))
            throw new InvalidOperationException("A chair with this ID already exists at the table.");
        chair.Table?.RemoveChair(chair);
        chairs.Add(chair);
        chair.Table = this;
    }

    public void RemoveChair(Chair chair)
    {
        if (chairs.Remove(chair)) chair.Table = null;
    }
}
