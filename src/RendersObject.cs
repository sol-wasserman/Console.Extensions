using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions;

public static class RendersObject
{
    private static readonly JsonSerializer Serializer = JsonSerializer.Create(new()
    {
        Converters = { new StringEnumConverter() },
    });

    public static IRenderable Render<T>(this T? value, string? title = null)
    {
        return value switch
        {
            Exception exception => exception.GetRenderable(),
            _ => new Panel(MakeJToken(value).RenderAny(title)).NoBorder(),
        };
    }
    
    internal static JToken? MakeJToken(this object? value)
    {
        return value switch
        {
            JToken jToken => jToken,
            null => null,
            _ => JToken.FromObject(value, Serializer),
        };
    }

    internal static IRenderable RenderAny(this JToken? token, string? title = null)
    {
        return token switch
        {
            JArray array => array.RenderArray(title),
            JObject @object => @object.RenderObject(title),
            _ => token.RenderString(),
        };
    }

    internal static IRenderable RenderObject(this JObject value, string? title = null)
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

    internal static IRenderable RenderArray(this JArray list, string? title = null)
    {
        var tree = Theme.Tree(title);

        tree.AddNodes(list.Select(token => RenderAny(token)));

        return tree;
    }
}
