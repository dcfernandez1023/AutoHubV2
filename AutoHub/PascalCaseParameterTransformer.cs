using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;

public class CamelCaseParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value == null) { return null; }

        var valueString = value.ToString();
        if (string.IsNullOrEmpty(valueString))
        {
            return null;
        }

        var segments = Regex.Split(valueString, @"[^a-zA-Z0-9]+");
        for (int i = 1; i < segments.Length; i++)
        {
            segments[i] = char.ToUpper(segments[i][0]) + segments[i].Substring(1);
        }

        return char.ToLower(segments[0][0]) + segments[0].Substring(1) + string.Concat(segments[1..]);
    }
}
