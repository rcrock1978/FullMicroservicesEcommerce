namespace IdentityService.Domain.Exceptions;

public class UserNotActiveException : Exception
{
    public int UserId { get; }

    public UserNotActiveException(int userId) 
        : base($"User with ID '{userId}' is not active")
    {
        UserId = userId;
    }
}
