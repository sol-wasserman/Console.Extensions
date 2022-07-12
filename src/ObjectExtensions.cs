namespace Spectre.Console.Extensions;

public static class ObjectExtensions
{
    public static T Dump<T>(this T value, string? title = null)
    {
        AnsiConsole.Write(value.Render(title));

        return value;
    }
}
