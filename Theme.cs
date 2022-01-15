using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions;

public static class Theme
{
    public static Tree Tree(string title)
    {
        return new Tree(title)
            .Guide(TreeGuide.Ascii)
            .Style(new(decoration: Decoration.Dim));
    }

    public static Panel Panel(IRenderable subject)
    {
        return new Panel(subject)
            .RoundedBorder()
            .BorderStyle(new(Color.Grey));
    }
}
