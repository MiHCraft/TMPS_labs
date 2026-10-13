using System.Text.Json;

// Store IDs instead of object references to avoid cycles in the JSON file.
public static class RestaurantStorage
{
    public static void Save(RestaurantFurniture restaurant, string path)
    {
        var data = new RestaurantData
        {
            Tables = restaurant.GetTables().Select(t => new TableData
            {
                Id = t.Id, Shape = t.GetShapeName()
            }).ToList(),
            Chairs = restaurant.GetChairs().Select(c => new ChairData
            {
                Id = c.Id, TableId = c.Table?.Id,
                ExtraPrice = c is EliteChair elite ? elite.GetExtraPrice() : null
            }).ToList(),
            Orders = restaurant.GetOrders().Select(o => new OrderData
            {
                Id = o.Id, CustomerName = o.GetCustomerName(), CustomerPhone = o.GetCustomerPhone(),
                ChairIds = o.GetChairs().Select(c => c.Id).ToList()
            }).ToList()
        };
        File.WriteAllText(path, JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
    }

    public static RestaurantFurniture Load(string path)
    {
        var data = JsonSerializer.Deserialize<RestaurantData>(File.ReadAllText(path))
            ?? throw new InvalidOperationException("The file contains no restaurant.");
        if (data.Tables == null || data.Chairs == null || data.Orders == null)
            throw new InvalidOperationException("Invalid restaurant data.");

        var restaurant = new RestaurantFurniture();
        foreach (TableData item in data.Tables)
        {
            if (item == null) throw new InvalidOperationException("Invalid table data.");
            Table table = item.Shape switch
            {
                "Round" => new RoundTable(item.Id),
                "Square" => new SquareTable(item.Id),
                "Rectangle" => new RectangleTable(item.Id),
                "Table" => new Table(item.Id),
                _ => throw new InvalidOperationException("Unknown table shape.")
            };
            restaurant.AddTable(table);
        }

        foreach (ChairData item in data.Chairs)
        {
            if (item == null) throw new InvalidOperationException("Invalid chair data.");
            Chair chair = item.ExtraPrice.HasValue
                ? new EliteChair(item.Id, item.ExtraPrice.Value) : new Chair(item.Id);
            restaurant.AddChair(chair);
            if (item.TableId.HasValue)
            {
                Table table = restaurant.GetTables().FirstOrDefault(t => t.Id == item.TableId.Value)
                    ?? throw new InvalidOperationException("A chair refers to a missing table.");
                table.AddChair(chair);
            }
        }

        var orderIds = new HashSet<int>();
        foreach (OrderData item in data.Orders)
        {
            if (item == null || item.ChairIds == null || item.ChairIds.Count == 0 || !orderIds.Add(item.Id))
                throw new InvalidOperationException("Invalid or duplicate order data.");
            var order = new Order(item.Id, item.CustomerName, item.CustomerPhone);
            foreach (int chairId in item.ChairIds)
            {
                Chair chair = restaurant.GetChairs().FirstOrDefault(c => c.Id == chairId)
                    ?? throw new InvalidOperationException("An order refers to a missing chair.");
                order.AddChair(chair);
            }
        }
        return restaurant;
    }

    public class RestaurantData
    {
        public required List<TableData> Tables { get; set; }
        public required List<ChairData> Chairs { get; set; }
        public required List<OrderData> Orders { get; set; }
    }

    public class TableData
    {
        public int Id { get; set; }
        public string Shape { get; set; } = "";
    }

    public class ChairData
    {
        public int Id { get; set; }
        public int? TableId { get; set; }
        public decimal? ExtraPrice { get; set; }
    }

    public class OrderData
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = "";
        public string CustomerPhone { get; set; } = "";
        public List<int> ChairIds { get; set; } = new();
    }
}
