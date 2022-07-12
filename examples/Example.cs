using Spectre.Console;
using Spectre.Console.Extensions;

var subject = new Transaction
{
    Date = DateTime.Now,
    IsPending = true,
    Goods = new()
    {
        new(7804, "CryptoPunk 7804"),
        new(3100, "CryptoPunk 3100"),
        new(2107, Title: null),
    },
    Tags = new() {"super", "posh"},
};

AnsiConsole.Write(subject.Render());

#nullable disable
public class Transaction
{
    public DateTime Date { get; init; }
    public bool IsPending { get; init; }
    public List<Line> Goods { get; init; }
    public List<string> Tags { get; init; }

    public record Line(long Id, string Title);
}

public enum Status { Pending }
