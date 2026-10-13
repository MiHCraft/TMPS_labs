public class Order
{
    private readonly string customerName;
    private readonly string customerPhone;
    private readonly List<Chair> chairs = new();

    public int Id { get; }

    public Order(int id, string customerName, string customerPhone)
    {
        if (id <= 0) throw new ArgumentException("Order ID must be positive.");
        if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(customerPhone))
            throw new ArgumentException("Customer name and phone are required.");
        Id = id;
        this.customerName = customerName.Trim();
        this.customerPhone = customerPhone.Trim();
    }

    public void AddChair(Chair chair)
    {
        if (chairs.Contains(chair)) return;
        if (!chair.IsAvailable()) throw new InvalidOperationException("This chair is already ordered.");
        chairs.Add(chair);
        chair.AssignOrder(this);
    }

    public void RemoveChair(Chair chair)
    {
        if (chairs.Remove(chair)) chair.AssignOrder(null);
    }

    public IReadOnlyList<Chair> GetChairs() => chairs.AsReadOnly();
    public string GetCustomerName() => customerName;
    public string GetCustomerPhone() => customerPhone;
}
