using System.Text.Json;
using System.Text.Json.Serialization;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions;

public class DumpOptions
{
    public static DumpOptions Default { get; } = new()
    {
        JsonOptions = new()
        {
            Converters = { new JsonStringEnumConverter() },
        },
    };

    public JsonSerializerOptions? JsonOptions { get; init; }

    public IRenderable Null { get; init; } = new Markup(text: "[dim]null[/]");

    public Func<string?, Tree> MakeTree { get; init; } = MakeDefaultTree;
    public Func<string, IRenderable> MakeLabel { get; init; } = MakeDefaultLabel;
    public Func<JsonValueKind, Color?> GetColor { get; init; } = GetDefaultColor;
    public Func<IRenderable, string?, IRenderable> WrapWithPanel { get; init; } = MakeDefaultPanel;
    public Func<Exception, IRenderable> RenderException { get; init; } = exception => exception.GetRenderable();

    private static Tree MakeDefaultTree(string? title)
    {
        return new Tree(title ?? string.Empty)
            .Guide(TreeGuide.Ascii)
            .Style(new(decoration: Decoration.Dim));
    }

    private static Markup MakeDefaultLabel(string? label) => new(
        text: $"{label.EscapeMarkup()}:",
        style: new(Color.Green, decoration: Decoration.Bold)
    );

    private static Panel MakeDefaultPanel(IRenderable subject, string? label)
    {
        var wrapper = new Panel(subject);

        return string.IsNullOrEmpty(label)
            ? wrapper
            : wrapper.Header(label);
    }

    private static Color? GetDefaultColor(JsonValueKind subject)
    {
        return subject switch
        {
            JsonValueKind.Number => Color.Teal,
            JsonValueKind.String => Color.Yellow,
            JsonValueKind.True or JsonValueKind.False => Color.Red,
            _ => null,
        };
    }
}
