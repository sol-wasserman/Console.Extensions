using Newtonsoft.Json.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions;

public static class Primitives
{
    public static readonly IRenderable Null = new Markup(text: "[dim]null[/]");

    public static IRenderable RenderString(this JToken? token)
    {
        if (token == null || token.Type == JTokenType.Null)
        {
            return Null;
        }

        return new Markup(
            text: token.ToString().EscapeMarkup(),
            style: new(foreground: token.Type.Color())
        );
    }
}
