namespace Spectre.Console.Extensions;

public static class Extensions
{
    /// <summary>
    /// Pretty print a complex object to the console.
    /// </summary>
    public static void Dump<T>(this IAnsiConsole console, T value)
    {
        console.Write(value.Render());
    }

    internal static T Tap<T>(this T value, Action<T> executor)
    {
        executor(value);

        return value;
    }
}
