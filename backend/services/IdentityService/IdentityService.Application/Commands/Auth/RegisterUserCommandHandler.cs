using AutoMapper;
using IdentityService.Contracts.DTOs;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Events;
using IdentityService.Domain.Exceptions;
using IdentityService.Domain.Repositories;
using MediatR;
using Shared.Common.Application;
using Shared.Security.Hashing;

namespace IdentityService.Application.Commands.Auth;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;
    private readonly IPublisher _publisher;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordHasher passwordHasher,
        IMapper mapper,
        IPublisher publisher)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
        _publisher = publisher;
    }

    public async Task<Result<UserDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Check if email already exists
        if (await _userRepository.EmailExistsAsync(request.Email, cancellationToken))
        {
            return Result<UserDto>.Failure($"Email '{request.Email}' is already registered");
        }

        // Hash password
        var passwordHash = _passwordHasher.HashPassword(request.Password);

        // Create user
        var user = User.Create(
            request.FirstName,
            request.LastName,
            request.Email,
            passwordHash,
            request.PhoneNumber
        );

        // Assign default Customer role
        var customerRole = await _roleRepository.GetByNameAsync("Customer", cancellationToken);
        if (customerRole != null)
        {
            user.AddRole(customerRole);
        }

        // Save user
        await _userRepository.AddAsync(user, cancellationToken);

        // Publish domain event
        var domainEvent = new UserRegisteredEvent(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            DateTime.UtcNow
        );
        await _publisher.Publish(domainEvent, cancellationToken);

        var userDto = _mapper.Map<UserDto>(user);
        return Result<UserDto>.Success(userDto);
    }
}
