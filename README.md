# Spectre.Console.Extensions

```c#
var subject = new Transaction
{
    Date = DateTime.Now,
    IsPending = true,
    Goods = new List<Line>
    {
        new(Id: 7804, Title: "CryptoPunk 7804"),
        new(Id: 3100, Title: "CryptoPunk 3100"),
        new(Id: 2107, Title: null),
    },
    Tags = new List<string> {"super", "posh"}
};

AnsiConsole.Write(subject.Render());
```

Prints

![Example](images/example.png)
