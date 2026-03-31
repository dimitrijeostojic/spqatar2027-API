using Application.Team.Create;
using Application.Team.Delete;
using Application.Team.GetAll;
using Application.Team.GetById;
using Application.Team.Update;
using Infrastracture.Attributes;
using Infrastracture.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPQatar2027.Common;

namespace SPQatar2027.Controllers;

[HasPermission(Permission.AccessTeam)]
[Route("api/[controller]")]
[ApiController]
public class TeamController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet]
    public async Task<IActionResult> GetAllTeams(CancellationToken cancellationToken)
    {
        var request = new GetAllTeamsRequest();
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }
    [HttpGet("{publicId:Guid}")]
    public async Task<IActionResult> GetTeamById([FromRoute] Guid publicId, CancellationToken cancellationToken)
    {
        var request = new GetTeamByIdRequest()
        {
            PublicId = publicId
        };
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [HasPermission(Permission.CreateTeam)]
    [HttpPost]
    public async Task<IActionResult> CreateTeam([FromBody] CreateTeamRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut("{publicId:Guid}")]
    public async Task<IActionResult> UpdateTeam([FromRoute] Guid publicId, [FromBody] UpdateTeamRequest request, CancellationToken cancellationToken)
    {
        request.TeamPublicId = publicId;
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [HttpDelete("{publicId:Guid}")]
    public async Task<IActionResult> DeleteTeam([FromRoute] Guid publicId, CancellationToken cancellationToken)
    {
        var request = new DeleteTeamRequest()
        {
            PublicId = publicId
        };
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }
}
