using Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Login;

public record LoginRequest(string Email, string Password) : IRequest<Result<LoginResponse>>;
