using Newtonsoft.Json.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions;

internal static class Theme
{
    public static Tree Tree(string? title = null)
    {
        return new Tree(title ?? string.Empty)
            .Guide(TreeGuide.Ascii)
            .Style(new(decoration: Decoration.Dim));
    }

    public static Panel Panel(IRenderable subject)
    {
        return new Panel(subject)
            .RoundedBorder()
            .BorderStyle(new(Console.Color.Grey));
    }

    public static IRenderable Key(string key)
    {
        return new Markup(
            text: $"{key.EscapeMarkup()}:",
            style: new(Console.Color.Green, decoration: Decoration.Bold)
        );
    }

    public static Color? Color(this JTokenType value)
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
