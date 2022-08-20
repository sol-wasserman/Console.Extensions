using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions;

internal static class TraversesObject
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
    };

    internal static IRenderable Traverse<T>(T subject, string? title = null)
    {
        return subject.MakeNode().TraverseAny(title);
    }
    
    private static JsonNode? MakeNode(this object? subject)
    {
        return subject switch
        {
            JsonNode node => node,
            null => null,
            _ => JsonSerializer.SerializeToNode(subject, SerializerOptions),
        };
    }

    private static IRenderable TraverseAny(this JsonNode? subject, string? title = null)
    {
        return subject switch
        {
            JsonArray array => array.TraverseArray(title),
            JsonObject @object => @object.TraverseObject(title),
            _ => subject.Deserialize<JsonElement>(SerializerOptions).RenderAsString(title),
        };
    }

    private static IRenderable TraverseObject(this JsonObject subject, string? title = null)
    {
        return subject
            .Select(field =>
            {
                var (name, value) = field;

                return new KeyValuePair<string, IRenderable>(name, value.TraverseAny());
            })
            .RenderObject(title);
    }

    private static IRenderable TraverseArray(this JsonArray list, string? title = null)
    {
        return list
            .Select(token => TraverseAny(token))
            .RenderList(title);
    }
}
