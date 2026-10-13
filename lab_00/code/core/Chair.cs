public class Chair
{
    private Order? order;

    public int Id { get; }
    public Table? Table { get; internal set; }

    public Chair(int id)
    {
        if (id <= 0) throw new ArgumentException("Chair ID must be positive.");
        Id = id;
    }

    public bool IsAvailable() => order == null;
    public Order? GetOrder() => order;

    public void SetOrder(Order? newOrder)
    {
        if (newOrder == order) return;
        if (newOrder == null) order?.RemoveChair(this);
        else newOrder.AddChair(this);
    }

    internal void AssignOrder(Order? newOrder) => order = newOrder;
}
