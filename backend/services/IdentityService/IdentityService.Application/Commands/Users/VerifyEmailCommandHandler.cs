using IdentityService.Domain.Events;
using IdentityService.Domain.Repositories;
using MediatR;
using Shared.Common.Application;

namespace IdentityService.Application.Commands.Users;

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IPublisher _publisher;

    public VerifyEmailCommandHandler(
        IUserRepository userRepository,
        IPublisher publisher)
    {
        _userRepository = userRepository;
        _publisher = publisher;
    }

    public async Task<Result> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        
        if (user == null)
        {
            return Result.Failure("User not found");
        }

        if (user.EmailVerified)
        {
            return Result.Failure("Email is already verified");
        }

        // TODO: Verify the verification token
        // For now, just mark as verified

        user.VerifyEmail();

        // Publish domain event
        var domainEvent = new UserEmailVerifiedEvent(user.Id, user.Email, DateTime.UtcNow);
        await _publisher.Publish(domainEvent, cancellationToken);

        return Result.Success();
    }
}
