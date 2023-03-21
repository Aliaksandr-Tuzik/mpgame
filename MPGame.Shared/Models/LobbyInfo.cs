namespace MPGame.Shared.Models;

public record LobbyInfo(string Title) {
    public string Uri => $"/lobby/{Title}";
}