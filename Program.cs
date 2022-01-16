using Spectre.Console;
using Spectre.Console.Extensions;

AnsiConsole.Write(new Transaction
{
    Id = 1001,
    Date = DateTime.Now,
    Amount = 21.07,
    Goods = new()
    {
        new(7804, "CryptoPunk 7804"),
        new(3100, "CryptoPunk 3100"),
        new(Code: null, "Ocean Front"),
    },
    Tags = new() { "super", "posh" }
}.Render());

#nullable disable
public class Transaction
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public double Amount { get; set; }
    public List<Line> Goods { get; set; }
    public List<string> Tags { get; set; }

    public record Line(long? Code, string Title);
}
