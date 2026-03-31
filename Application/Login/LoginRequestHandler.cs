using Application.Common;
using Core;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Login;

public sealed class LoginRequestHandler(
    UserManager<User> userManager,
    IJwtTokenRepository jwtTokenRepository)
    : IRequestHandler<LoginRequest, Result<LoginResponse>>
{
    private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    private readonly IJwtTokenRepository _jwtTokenRepository = jwtTokenRepository ?? throw new ArgumentNullException(nameof(jwtTokenRepository));

    public async Task<Result<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user is null 
            || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return Result<LoginResponse>.Failure(ApplicationErrors.InvalidCredentials);
        }

        var roles = await _userManager.GetRolesAsync(user);

        string token = await _jwtTokenRepository.GenerateTokenAsync(user, roles);

        var response = new LoginResponse(token);

        return Result<LoginResponse>.Success(response);
    }
}
