using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions;

public static class RendersObject
{
    [return: NotNullIfNotNull(nameof(value))]
    public static T? Dump<T>(this T value, string? title = null, DumpOptions? options = null)
    {
        AnsiConsole.Write(value.Render(title, options));

        return value;
    }

    public static IRenderable Render<T>(this T? value, string? title = null, DumpOptions? options = null)
    {
        options = options.Resolve();

        return value switch
        {
            Exception exception => options.WrapWithPanel(options.RenderException(exception), title),
            _ => SerializeToNode(title, options).RenderJsonNode(title, options),
        };
    }

    private static JsonNode? SerializeToNode<T>(T? subject, DumpOptions options)
    {
        return subject switch
        {
            JsonNode node => node,
            null => null,
            _ => JsonSerializer.SerializeToNode(subject, options.JsonOptions),
        };
    }

    private static IRenderable RenderJsonNode(this JsonNode? subject, string? title, DumpOptions options)
    {
        return subject switch
        {
            JsonArray value => value.RenderArray(title, options),
            JsonObject value => value.RenderObject(title, options),
            _ => subject.Deserialize<JsonElement>(options.JsonOptions).RenderAsString(title, options),
        };
    }

    private static IRenderable RenderObject(this JsonObject subject, string? title, DumpOptions options)
    {
        var container = new Grid().AddColumns(count: 2);
        
        foreach (var (key, value) in subject)
        {
            container.AddRow(
                options.MakeLabel(key), 
                value.RenderJsonNode(null, options)
            );
        }
        
        return options.WrapWithPanel(container, title);
    }

    private static Tree RenderArray(this JsonArray list, string? title, DumpOptions options)
    {
        var container = options.MakeTree(title);
        
        var items = list.Select(token => RenderJsonNode(token, null, options));

        container.AddNodes(items);

        return container;
    }
    
    private static IRenderable RenderAsString(this JsonElement element, string? title, DumpOptions options)
    {
        if (element.ValueKind is JsonValueKind.Null)
        {
            return options.Null;
        }

        var value = new Markup(
            text: ToString().EscapeMarkup(),
            style: new(foreground: options.GetColor(element.ValueKind))
        );

        return title is not null
            ? options.WrapWithPanel(value, title)
            : value;

        string ToString() => element.ValueKind switch
        {
            JsonValueKind.String when element.TryGetDateTime(out var date) => date.ToString(CultureInfo.InvariantCulture),
            _ => element.ToString(),
        };
    }
    
    private static DumpOptions Resolve(this DumpOptions? options)
    {
        return options ?? DumpOptions.Default;
    }
}
