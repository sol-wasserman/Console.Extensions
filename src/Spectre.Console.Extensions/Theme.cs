using System.Globalization;
using System.Text.Json;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions;

using static JsonValueKind;

internal static class Theme
{
    private static readonly IRenderable Null = new Markup(text: "[dim]null[/]");

    public static Tree RenderList(this IEnumerable<IRenderable> values, string? title = null)
    {
        var container = new Tree(title ?? string.Empty)
            .Guide(TreeGuide.Ascii)
            .Style(new(decoration: Decoration.Dim));
        
        container.AddNodes(values);

        return container;
    }

    public static IRenderable RenderObject(this IEnumerable<KeyValuePair<string, IRenderable>> fields, string? title = null)
    {
        var container = new Grid().AddColumns(count: 2);

        foreach (var (name, value) in fields)
        {
            container.AddRow(name.RenderLabel(), value);
        }

        return container.Wrap(title);
    }

    public static Panel Wrap(this IRenderable subject, string? title = null)
    {
        var wrapper = new Panel(subject);

        return string.IsNullOrEmpty(title)
            ? wrapper
            : wrapper.Header(title);
    }

    public static IRenderable RenderLabel(this string label)
    {
        return new Markup(
            text: $"{label.EscapeMarkup()}:",
            style: new(Console.Color.Green, decoration: Decoration.Bold)
        );
    }

    public static Color? Color(this JsonValueKind subject)
    {
        return subject switch
        {
            Number => Console.Color.Teal,
            String => Console.Color.Yellow,
            True or False => Console.Color.Red,
            _ => null,
        };
    }
    
    public static IRenderable RenderAsString(this JsonElement element, string? title = null)
    {
        if (element.ValueKind is JsonValueKind.Null)
        {
            return Null;
        }

        var value = new Markup(
            text: ToString().EscapeMarkup(),
            style: new(foreground: element.ValueKind.Color())
        );

        return title is not null
            ? value.Wrap(title)
            : value;
        
        string ToString() => element.ValueKind switch
        {
            String when element.TryGetDateTime(out var date) => date.ToString(CultureInfo.InvariantCulture),
            _ => element.ToString(),
        };
    }
}
