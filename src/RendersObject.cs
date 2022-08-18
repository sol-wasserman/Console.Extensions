using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions;

public static class RendersObject
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
    };

    public static IRenderable Render<T>(this T? value, string? title = null)
    {
        return value switch
        {
            Exception exception => exception.GetRenderable(),
            _ => new Panel(MakeNode(value).RenderAny(title)).NoBorder(),
        };
    }
    
    internal static JsonNode? MakeNode(this object? value)
    {
        return value switch
        {
            JsonNode node => node,
            null => null,
            _ => JsonSerializer.SerializeToNode(value, SerializerOptions),
        };
    }

    internal static IRenderable RenderAny(this JsonNode? token, string? title = null)
    {
        return token switch
        {
            JsonArray array => array.RenderArray(title),
            JsonObject @object => @object.RenderObject(title),
            _ => token.Deserialize<JsonElement>(SerializerOptions).RenderString(),
        };
    }

    internal static IRenderable RenderObject(this JsonObject value, string? title = null)
    {
        var grid = new Grid()
            .AddColumn()
            .AddColumn();

        foreach (var pair in value)
        {
            var (name, token) = (pair.Key, pair.Value);
            
            grid.AddRow(Theme.Key(name), token.RenderAny());
        }

        var wrapper = Theme.Panel(grid);

        return string.IsNullOrEmpty(title)
            ? wrapper
            : wrapper.Header(title);
    }

    internal static IRenderable RenderArray(this JsonArray list, string? title = null)
    {
        var tree = Theme.Tree(title);

        tree.AddNodes(list.Select(token => RenderAny(token)));

        return tree;
    }
}
