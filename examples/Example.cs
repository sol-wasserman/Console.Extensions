using Spectre.Console.Extensions;

var order = new Order
{
    Date = new(2035, 11, 1, 12, 0, 0, DateTimeKind.Utc),
    IsWholesale = true,
    Products =
    [
        new(1, "Mug"),
        new(2, "Glass"),
        new(3),
    ],
    Tags = ["expedite", "fragile"],
};

order.Dump("Order");

public class Order
{
    public required DateTime Date { get; init; }
    public required bool IsWholesale { get; init; }
    public required List<Product> Products { get; init; }
    public required List<string> Tags { get; init; }

    public record Product(long Id, string? Name = null);
}
