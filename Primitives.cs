using Newtonsoft.Json.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions;

public static class Primitives
{
    public static IRenderable Null()
    {
        return new Markup(text: "[dim]null[/]");
    }

    public static IRenderable String(this JToken? value)
    {
        if (value == null || value.Type == JTokenType.Null)
        {
            return Null();
        }

        return new Markup(
            text: value.ToString().EscapeMarkup(),
            style: new(foreground: value.Type.Color())
        );
    }

    private static Color? Color(this JTokenType value)
    {
        return value switch
        {
            JTokenType.Integer => Console.Color.Teal,
            JTokenType.Float => Console.Color.Teal,
            JTokenType.String => Console.Color.Yellow,
            JTokenType.Date => Console.Color.Yellow,
            JTokenType.Guid => Console.Color.Yellow,
            JTokenType.Uri => Console.Color.Yellow,
            JTokenType.TimeSpan => Console.Color.Yellow,
            JTokenType.Boolean => Console.Color.Red,
            _ => null,
        };
    }
}