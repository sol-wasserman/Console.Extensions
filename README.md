# Spectre.Console.Extensions

```c#
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
    Tags = new() {"super", "posh"}
};

AnsiConsole.Write(subject.Render());
```

Prints

![Example](images/example.png)
