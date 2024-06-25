namespace CardTracker.Domain.Requests.Auth;

public record LoginRequest(string Email, string Password);