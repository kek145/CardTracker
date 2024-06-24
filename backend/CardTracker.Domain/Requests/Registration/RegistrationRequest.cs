namespace CardTracker.Domain.Requests.Registration;

public record RegistrationRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword);