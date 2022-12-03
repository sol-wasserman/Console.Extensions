using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions.Tests;

public class TraversesObjectTests
{
    [Fact]
    public void String()
    {
        Write("Hello World").Should().Be("""
            Hello World
            """);

        Write("Hello World".Render("A title")).Should().Be("""
            ┌─A title─────┐
            │ Hello World │
            └─────────────┘

            """);
    }
    
    [Fact]
    public void Complex()
    {
        var subject = new Transaction
        {
            Date = new(1944, 6, 6), 
            IsPending = true,
            Goods = new()
            {
                new("Almond Cream Croissant", 20),
                new("Apple Tart", 10),
            },
            Tags = new() { "Food", "Bakery" },
        };
        
        Write(subject).Should().Be("""
            ┌──────────────────────────────────────────────────────────────────────────────┐
            │ Date:       06/06/1944 00:00:00                                              │
            │ IsPending:  True                                                             │
            │ Goods:      |-- ┌───────────────────────────────────┐                        │
            │             |   │ Title:     Almond Cream Croissant │                        │
            │             |   │ Quantity:  20                     │                        │
            │             |   └───────────────────────────────────┘                        │
            │             `-- ┌───────────────────────┐                                    │
            │                 │ Title:     Apple Tart │                                    │
            │                 │ Quantity:  10         │                                    │
            │                 └───────────────────────┘                                    │
            │ Tags:       |-- Food                                                         │
            │             `-- Bakery                                                       │
            └──────────────────────────────────────────────────────────────────────────────┘

            """);

        Write(subject.Render("A title")).Should().Be("""
            ┌─A title──────────────────────────────────────────────────────────────────────┐
            │ Date:       06/06/1944 00:00:00                                              │
            │ IsPending:  True                                                             │
            │ Goods:      |-- ┌───────────────────────────────────┐                        │
            │             |   │ Title:     Almond Cream Croissant │                        │
            │             |   │ Quantity:  20                     │                        │
            │             |   └───────────────────────────────────┘                        │
            │             `-- ┌───────────────────────┐                                    │
            │                 │ Title:     Apple Tart │                                    │
            │                 │ Quantity:  10         │                                    │
            │                 └───────────────────────┘                                    │
            │ Tags:       |-- Food                                                         │
            │             `-- Bakery                                                       │
            └──────────────────────────────────────────────────────────────────────────────┘

            """);
    }
    
    private static string Write(IRenderable value)
    {
        using var console = new TestConsole();
        
        console.Write(value);
        
        return console.Output;
    }

    private static string Write<T>(T value)
    {
        return Write(value.Render());
    }
    
    private record Transaction
    {
        public DateTime Date { get; init; }
        public bool IsPending { get; init; }
        public List<Line>? Goods { get; init; }
        public List<string>? Tags { get; init; }

        public record Line(string? Title, int Quantity);
    }
}
