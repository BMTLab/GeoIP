namespace Shared.Models.Server;

public sealed record ServerInfo
{
    public string? Name { get; init; }
    public string? Version { get; init; }
    public string? Environment { get; init; }
    public string? Language { get; init; }
    public string? HelloMessage { get; init; }
}