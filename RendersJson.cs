using Newtonsoft.Json.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions;

public static class RendersJson
{
    public static IRenderable Render<T>(this T? value)
    {
        return value switch
        {
            Exception exception => exception.GetRenderable(),
            _ => value.ToToken().RenderToken(),
        };
    }

    private static JToken? ToToken(this object? value)
    {
        return value switch
        {
            JToken jToken => jToken,
            null => null,
            _ => JToken.FromObject(value)
        };
    }

    private static IRenderable RenderToken(this JToken? token)
    {
        var rendered = token switch
        {
            JArray array => array.Array(),
            JObject @object => @object.Object(),
            _ => token.String(),
        };

        return new Panel(rendered).NoBorder();
    }

    private static IRenderable Object(this JObject @object)
    {
        var grid = new Grid()
            .AddColumn()
            .AddColumn();

        foreach (var (name, token) in @object)
        {
            WriteField(name, token);
        }

        return Theme.Panel(grid);

        void WriteField(string name, JToken? token)
        {
            var field = new Markup(
                text: $"{name.EscapeMarkup()}:",
                style: new(Console.Color.Green, decoration: Decoration.Bold)
            );

            var value = token switch
            {
                JArray array => array.Array(),
                JObject jObject => jObject.Object(),
                _ => token.String(),
            };

            grid.AddRow(field, value);
        }
    }

    private static IRenderable Array(this JArray list, string title = "")
    {
        var nodes = list.Select(RenderItem);

        return Theme
            .Tree(title)
            .Tap(tree => tree.AddNodes(nodes));

        IRenderable RenderItem(JToken item) => item switch
        {
            JObject @object => Object(@object),
            _ => item.String(),
        };
    }
}
