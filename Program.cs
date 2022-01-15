using Spectre.Console;
using Spectre.Console.Extensions;

AnsiConsole.Console.Dump(new Transaction
{
    Id = 1001,
    Date = DateTime.Now,
    Amount = 15.07,
    Goods = new()
    {
        new(7804, "CryptoPunk 7804"),
        new(3100, "CryptoPunk 3100"),
    },
    Tags = new() { "super", "posh" }
});

#nullable disable
public class Transaction
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public double Amount { get; set; }
    public List<Line> Goods { get; set; }
    public List<string> Tags { get; set; }

    public record Line(long Id, string Title);
}
