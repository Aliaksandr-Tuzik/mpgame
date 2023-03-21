namespace MPGame.Api.Models;

public record LobbyModel(string Id)
{
    public HashSet<string> Users { get; } = new();
}