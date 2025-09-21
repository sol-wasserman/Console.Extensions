using System.Text.Json;
using System.Text.Json.Serialization;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions;

/// <summary>
/// Configuration options for object dumping and rendering.
/// </summary>
public class DumpOptions
{
    /// <summary>
    /// Gets the default dump options with standard JSON serialization settings.
    /// </summary>
    public static DumpOptions Default { get; } = new()
    {
        JsonOptions = new()
        {
            Converters = { new JsonStringEnumConverter() },
        },
    };

    /// <summary>
    /// The JSON serializer options used for object serialization.
    /// </summary>
    public JsonSerializerOptions? JsonOptions { get; init; }

    /// <summary>
    /// The renderable used to represent null values.
    /// </summary>
    public IRenderable Null { get; init; } = new Markup(text: "[dim]null[/]");

    /// <summary>
    /// The function used to create tree structures for rendering arrays.
    /// </summary>
    public Func<string?, Tree> MakeTree { get; init; } = MakeDefaultTree;

    /// <summary>
    /// The function used to create labels for object properties.
    /// </summary>
    public Func<string, IRenderable> MakeLabel { get; init; } = MakeDefaultLabel;

    /// <summary>
    /// The function used to determine colors for different JSON value types.
    /// </summary>
    public Func<JsonValueKind, Color?> GetColor { get; init; } = GetDefaultColor;

    /// <summary>
    /// The function used to wrap content with panels.
    /// </summary>
    public Func<IRenderable, string?, IRenderable> WrapWithPanel { get; init; } = MakeDefaultPanel;

    /// <summary>
    /// The function used to render exceptions.
    /// </summary>
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
