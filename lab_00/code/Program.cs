using System.Globalization;
using System.Text.Json;

public static class Program
{
    public static void Main()
    {
        var restaurant = new RestaurantFurniture();
        bool work = true;
        Console.WriteLine("Restaurant Ordering System");
        Console.WriteLine(
            "Demo: user actions are marked (user); all other actions are for the admin."
        );

        while (work)
        {
            DisplayMenu();
            try
            {
                int c = ReadInt("Choose an option: ");
                Console.Clear();
                switch (c)
                {
                    case 0:
                        work = false;
                        break;
                    case 1:
                        ShowChairs(restaurant.GetChairs().Where(c => c.IsAvailable()));
                        break;
                    case 2:
                        int chairId = ReadChairId(restaurant);
                        string name = ReadText("Your name: ");
                        string phone = ReadText("Your phone: ");
                        Order order = restaurant.OrderChair(chairId, name, phone);
                        Console.WriteLine(
                            $"Order {order.Id} created. Keep this ID to cancel your order."
                        );
                        break;
                    case 3:
                        restaurant.CancelOrder(ReadOrderId(restaurant));
                        Console.WriteLine("Order cancelled.");
                        break;
                    case 4:
                        restaurant = RestaurantStorage.Load(ReadText("File path: "));
                        Console.WriteLine("Restaurant loaded.");
                        break;
                    case 5:
                        RestaurantStorage.Save(restaurant, ReadText("File path: "));
                        Console.WriteLine("Restaurant saved.");
                        break;
                    case 6:
                        AddTable(restaurant);
                        break;
                    case 7:
                        restaurant.RemoveTable(FindTable(restaurant, ReadTableId(restaurant)));
                        Console.WriteLine("Table removed; its chairs are now unattached.");
                        break;
                    case 8:
                        AddChair(restaurant);
                        break;
                    case 9:
                        restaurant.RemoveChair(FindChair(restaurant, ReadChairId(restaurant)));
                        Console.WriteLine("Chair removed.");
                        break;
                    case 10:
                        Table table = FindTable(restaurant, ReadTableId(restaurant));
                        ShowTable(table);
                        ShowChairs(table.GetChairs());
                        break;
                    case 11:
                        ShowTables(restaurant.GetTables());
                        break;
                    case 12:
                        ShowChair(FindChair(restaurant, ReadChairId(restaurant)));
                        break;
                    case 13:
                        ShowChairs(restaurant.GetChairs());
                        break;
                    case 14:
                        int orderId = ReadOrderId(restaurant);
                        ShowOrder(
                            restaurant.GetOrders().FirstOrDefault(o => o.Id == orderId)
                                ?? throw new InvalidOperationException("Order not found.")
                        );
                        break;
                    case 15:
                        if (restaurant.GetOrders().Count == 0)
                            Console.WriteLine("No orders.");
                        foreach (Order item in restaurant.GetOrders())
                            ShowOrder(item);
                        break;
                    case 16:
                        Chair chair = FindChair(restaurant, ReadChairId(restaurant));
                        int tableId = ReadTableId(restaurant, "New table ID (0 for no table): ");
                        if (tableId == 0)
                            chair.Table?.RemoveChair(chair);
                        else
                            FindTable(restaurant, tableId).AddChair(chair);
                        Console.WriteLine("Chair moved.");
                        break;
                    case 17:
                        DemonstrateLsp();
                        break;
                    default:
                        Console.WriteLine("Incorrect option.");
                        break;
                }
            }
            catch (EndOfStreamException)
            {
                work = false;
            }
            catch (Exception error)
                when (error
                        is ArgumentException
                            or InvalidOperationException
                            or IOException
                            or UnauthorizedAccessException
                            or JsonException
                )
            {
                Console.WriteLine($"Error: {error.Message}");
            }
        }
    }

    private static void DisplayMenu()
    {
        Console.WriteLine(
            """

            1. (user) View available chairs
            2. (user) Order a chair
            3. (user) Cancel an order
            4. Load restaurant
            5. Save restaurant
            6. Add table
            7. Remove table
            8. Add chair
            9. Remove chair
            10. (user) View table
            11. (user) List tables
            12. (user) View chair
            13. (user) List chairs
            14. View order with customer details
            15. List orders with customer details
            16. Move or detach chair
            17. Demonstrate LSP
            0. Exit (save first to keep your changes)
            """
        );
    }

    private static int CountChairs(Table table)
    {
        return table.GetTheNumberOfAttachedChairs();
    }

    private static void DemonstrateLsp()
    {
        Table[] tables =
        {
            new RoundTable(1),
            new SquareTable(2),
            new RectangleTable(3),
        };

        Console.WriteLine("LSP demonstration: CountChairs(Table table) accepts every table subclass.");
        foreach (Table table in tables)
        {
            table.AddChair(new Chair(table.Id));
            Console.WriteLine($"{table.GetType().Name} passed as Table: {CountChairs(table)} chair(s)");
        }
        Console.WriteLine("Each subclass can replace Table while preserving the chair-counting behavior.");
    }

    private static void AddTable(RestaurantFurniture restaurant)
    {
        int id = ReadTableId(restaurant, "New table ID: ");
        Table table = ReadInt("Shape (1 round, 2 square, 3 rectangle): ") switch
        {
            1 => new RoundTable(id),
            2 => new SquareTable(id),
            3 => new RectangleTable(id),
            _ => throw new ArgumentException("Incorrect table shape."),
        };
        restaurant.AddTable(table);
        Console.WriteLine("Table added.");
    }

    private static void AddChair(RestaurantFurniture restaurant)
    {
        int id = ReadChairId(restaurant, "New chair ID: ");
        int tableId = ReadTableId(restaurant, "Table ID (0 for no table): ");
        Table? table = tableId == 0 ? null : FindTable(restaurant, tableId);
        Chair chair = ReadInt("Type (1 standard, 2 elite): ") switch
        {
            1 => new Chair(id),
            2 => new EliteChair(id, ReadPrice()),
            _ => throw new ArgumentException("Incorrect chair type."),
        };
        restaurant.AddChair(chair);
        table?.AddChair(chair);
        Console.WriteLine("Chair added.");
    }

    private static Table FindTable(RestaurantFurniture restaurant, int id) =>
        restaurant.GetTables().FirstOrDefault(t => t.Id == id)
        ?? throw new InvalidOperationException("Table not found.");

    private static Chair FindChair(RestaurantFurniture restaurant, int id) =>
        restaurant.GetChairs().FirstOrDefault(c => c.Id == id)
        ?? throw new InvalidOperationException("Chair not found.");

    private static int ReadTableId(RestaurantFurniture restaurant, string prompt = "Table ID: ")
    {
        Console.WriteLine("Tables:");
        ShowTables(restaurant.GetTables());
        return ReadInt(prompt);
    }

    private static int ReadChairId(RestaurantFurniture restaurant, string prompt = "Chair ID: ")
    {
        Console.WriteLine("Chairs:");
        ShowChairs(restaurant.GetChairs());
        return ReadInt(prompt);
    }

    private static int ReadOrderId(RestaurantFurniture restaurant)
    {
        Console.WriteLine("Orders:");
        var orders = restaurant.GetOrders();
        if (orders.Count == 0)
            Console.WriteLine("No orders.");
        foreach (Order order in orders)
            Console.WriteLine(
                $"Order {order.Id}: chairs: {string.Join(", ", order.GetChairs().Select(c => c.Id))}"
            );
        return ReadInt("Order ID: ");
    }

    private static void ShowTables(IReadOnlyList<Table> tables)
    {
        if (tables.Count == 0)
            Console.WriteLine("No tables.");
        foreach (Table table in tables)
            ShowTable(table);
    }

    private static void ShowTable(Table table) =>
        Console.WriteLine(
            $"Table {table.Id}: {table.GetShapeName()}, {table.GetTheNumberOfAttachedChairs()} chairs"
        );

    private static void ShowChair(Chair chair)
    {
        string table = chair.Table == null ? "unattached" : $"table {chair.Table.Id}";
        string type = chair is EliteChair elite
            ? $"elite, extra price {elite.GetExtraPrice().ToString("0.00", CultureInfo.InvariantCulture)}"
            : "standard";
        string status = chair.IsAvailable() ? "available" : "ordered";
        Console.WriteLine($"Chair {chair.Id}: {table}, {type}, {status}");
    }

    private static void ShowChairs(IEnumerable<Chair> chairs)
    {
        var items = chairs.ToList();
        if (items.Count == 0)
            Console.WriteLine("No chairs.");
        foreach (Chair chair in items)
            ShowChair(chair);
    }

    private static void ShowOrder(Order order) =>
        Console.WriteLine(
            $"Order {order.Id}: {order.GetCustomerName()}, phone {order.GetCustomerPhone()}, "
                + $"chairs: {string.Join(", ", order.GetChairs().Select(c => c.Id))}"
        );

    private static string ReadText(string prompt)
    {
        Console.Write(prompt);
        string input = Console.ReadLine() ?? throw new EndOfStreamException();
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Input cannot be empty.");
        return input.Trim();
    }

    private static int ReadInt(string prompt) =>
        int.TryParse(ReadText(prompt), out int value)
            ? value
            : throw new ArgumentException("Enter a whole number.");

    private static decimal ReadPrice() =>
        decimal.TryParse(
            ReadText("Extra price (e.g. 7.50): ").Replace(',', '.'),
            NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
            CultureInfo.InvariantCulture,
            out decimal value
        )
            ? value
            : throw new ArgumentException("Enter a valid price.");
}
