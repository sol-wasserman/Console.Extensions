using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions;

public static class ExtendsObject
{
    public static T Dump<T>(this T value, string? title = null)
    {
        AnsiConsole.Write(value.Render(title));

        return value;
    }
    
    public static IRenderable Render<T>(this T? value, string? title = null)
    {
        return value switch
        {
            Exception exception => exception.GetRenderable().Wrap(title),
            _ => TraversesObject.Traverse(value, title),
        };
    }
}
