namespace DevXpertHub.Api.Transformers;

public class LowerCaseRouteTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value == null) return null;
        return value.ToString()?.ToLowerInvariant();
    }
}