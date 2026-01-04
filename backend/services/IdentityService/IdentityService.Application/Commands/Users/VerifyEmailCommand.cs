using MediatR;
using Shared.Common.Application;

namespace IdentityService.Application.Commands.Users;

public record VerifyEmailCommand(
    int UserId,
    string VerificationToken
) : IRequest<Result>;
