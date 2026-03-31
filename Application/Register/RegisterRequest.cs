using Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Register;

public record RegisterRequest(string FirstName, string LastName, string UserName, string Email, string Password) : IRequest<Result<RegisterResponse>>;