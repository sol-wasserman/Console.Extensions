using System.Globalization;
using System.Text.Json;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions;

public static class Primitives
{
    public static readonly IRenderable Null = new Markup(text: "[dim]null[/]");

    public static IRenderable RenderString(this JsonElement? element)
    {
        return element.HasValue ? element.Value.RenderString() : Null;
    }
    
    public static IRenderable RenderString(this JsonElement element)
    {
        if (element.ValueKind is JsonValueKind.Null)
        {
            return Null;
        }

        return new Markup(
            text: ToString(element).EscapeMarkup(),
            style: new(foreground: element.ValueKind.Color())
        );
    }

    private static string ToString(JsonElement element) => element.ValueKind switch
    {
        JsonValueKind.String when element.TryGetDateTime(out var date) => date.ToString(CultureInfo.InvariantCulture),
        _ => element.ToString(),
    };
}
