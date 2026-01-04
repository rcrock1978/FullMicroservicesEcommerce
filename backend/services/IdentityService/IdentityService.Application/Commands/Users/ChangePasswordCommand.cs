using MediatR;
using Shared.Common.Application;

namespace IdentityService.Application.Commands.Users;

public record ChangePasswordCommand(
    int UserId,
    string CurrentPassword,
    string NewPassword
) : IRequest<Result>;
