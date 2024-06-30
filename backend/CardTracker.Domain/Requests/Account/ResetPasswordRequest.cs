namespace CardTracker.Domain.Requests.Account;

public record ResetPasswordRequest(string ResetToken, string Password, string ConfirmPassword);