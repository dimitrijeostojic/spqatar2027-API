using Application.Common;
using Core;
using Domain.Abstraction;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Register;

public sealed class RegisterRequestHandler(
    UserManager<User> userManager,
    IJwtTokenRepository jwtTokenRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RegisterRequest, Result<RegisterResponse>>
{
    private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    private readonly IJwtTokenRepository _jwtTokenRepository = jwtTokenRepository ?? throw new ArgumentNullException(nameof(jwtTokenRepository));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<Result<RegisterResponse>> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
           {

               var user = new User
               {
                   FirstName = request.FirstName,
                   LastName = request.LastName,
                   UserName = request.UserName,
                   Email = request.Email
               };

               var identityResult = await _userManager.CreateAsync(user, request.Password);

               if (!identityResult.Succeeded)
               {
                   throw new Exception(identityResult.Errors.First().Description);
               }

               var addToRolesResult = await _userManager.AddToRoleAsync(user, Roles.User);

               if (!addToRolesResult.Succeeded)
               {
                   throw new Exception(addToRolesResult.Errors.First().Description);
               }
           }, cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<RegisterResponse>.Failure(new Error("Register.Error", ex.Message));
        }

        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser is null)
        {
            return Result<RegisterResponse>.Failure(ApplicationErrors.NotFound);
        }
        var roles = await _userManager.GetRolesAsync(existingUser);
        var jwtToken = await _jwtTokenRepository.GenerateTokenAsync(existingUser, roles);

        var response = new RegisterResponse(jwtToken);

        return Result<RegisterResponse>.Success(response);
    }
}
