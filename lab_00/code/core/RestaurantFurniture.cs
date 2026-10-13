public class RestaurantFurniture
{
    private readonly List<Table> tables = new();
    private readonly List<Chair> chairs = new();

    public IReadOnlyList<Table> GetTables() => tables.AsReadOnly();
    public IReadOnlyList<Chair> GetChairs() => chairs.AsReadOnly();
    public IReadOnlyList<Order> GetOrders() => chairs
        .Select(c => c.GetOrder()).OfType<Order>().Distinct().ToList().AsReadOnly();

    public void AddTable(Table table)
    {
        if (tables.Any(t => t.Id == table.Id))
            throw new InvalidOperationException("A table with this ID already exists.");
        if (table.GetChairs().Count > 0)
            throw new InvalidOperationException("Add the table before attaching chairs.");
        tables.Add(table);
    }

    public void AddChair(Chair chair)
    {
        if (chairs.Any(c => c.Id == chair.Id))
            throw new InvalidOperationException("A chair with this ID already exists.");
        if (chair.Table != null && !tables.Contains(chair.Table))
            throw new InvalidOperationException("The chair's table is not in this restaurant.");
        chairs.Add(chair);
    }

    public void RemoveTable(Table table)
    {
        if (!tables.Remove(table)) return;
        foreach (Chair chair in table.GetChairs().ToList()) table.RemoveChair(chair);
    }

    public void RemoveChair(Chair chair)
    {
        if (!chairs.Remove(chair)) return;
        chair.Table?.RemoveChair(chair);
        chair.SetOrder(null);
    }

    public Order OrderChair(int chairId, string customerName, string customerPhone)
    {
        Chair chair = chairs.FirstOrDefault(c => c.Id == chairId)
            ?? throw new InvalidOperationException("Chair not found.");
        int id = GetOrders().Select(o => o.Id).DefaultIfEmpty(0).Max() + 1;
        var order = new Order(id, customerName, customerPhone);
        order.AddChair(chair);
        return order;
    }

    public void CancelOrder(int orderId)
    {
        Order order = GetOrders().FirstOrDefault(o => o.Id == orderId)
            ?? throw new InvalidOperationException("Order not found.");
        foreach (Chair chair in order.GetChairs().ToList()) order.RemoveChair(chair);
    }
}
