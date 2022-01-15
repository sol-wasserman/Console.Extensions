namespace Spectre.Console.Extensions;

public static class Extensions
{
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
