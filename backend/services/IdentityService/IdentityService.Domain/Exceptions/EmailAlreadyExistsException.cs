namespace IdentityService.Domain.Exceptions;

public class EmailAlreadyExistsException : Exception
{
    public string Email { get; }

    public EmailAlreadyExistsException(string email) 
        : base($"Email '{email}' is already registered")
    {
        Email = email;
    }
}
