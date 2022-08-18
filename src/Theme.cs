using System.Text.Json;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions;

using static JsonValueKind;

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

    public static Color? Color(this JsonValueKind value)
    {
        return value switch
        {
            Number => Console.Color.Teal,
            String => Console.Color.Yellow,
            True or False => Console.Color.Red,
            Object => Console.Color.Grey,
            Array => Console.Color.Blue,
            _ => null,
        };
    }
}
