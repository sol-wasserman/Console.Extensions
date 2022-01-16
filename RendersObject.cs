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

    public static IRenderable Render<T>(this T? value, string? header = null)
    {
        return value switch
        {
            Exception exception => exception.GetRenderable(),
            _ => new Panel(MakeJToken(value).RenderAny(header)).NoBorder(),
        };

        static JToken? MakeJToken(object? value)
        {
            return value switch
            {
                JToken jToken => jToken,
                null => null,
                _ => JToken.FromObject(value, Serializer)
            };
        }
    }

    private static IRenderable RenderAny(this JToken? token, string? header = null)
    {
        return token switch
        {
            JArray array => array.RenderArray(header),
            JObject @object => RenderObject(@object, header),
            _ => token.RenderString(),
        };
    }

    private static IRenderable RenderObject(this JObject value, string? header = null)
    {
        var grid = new Grid()
            .AddColumn()
            .AddColumn();

        foreach (var (name, token) in value)
        {
            grid.AddRow(Theme.Key(name), token.RenderAny());
        }

        var wrapper = Theme.Panel(grid);

        return string.IsNullOrEmpty(header)
            ? wrapper
            : wrapper.Header(header);
    }

    private static IRenderable RenderArray(this JArray list, string? title = null)
    {
        var tree = Theme.Tree(title);

        tree.AddNodes(list.Select(token => RenderAny(token)));

        return tree;
    }
}
