public class EliteChair : Chair
{
    private readonly decimal extraPrice;

    public EliteChair(int id, decimal extraPrice) : base(id)
    {
        if (extraPrice < 0) throw new ArgumentException("Extra price cannot be negative.");
        this.extraPrice = extraPrice;
    }

    public decimal GetExtraPrice() => extraPrice;
}
