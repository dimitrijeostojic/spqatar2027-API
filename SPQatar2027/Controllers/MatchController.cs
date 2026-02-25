using Application.Match.Create;
using Application.Match.ForfeitMatch;
using Application.Match.GetMatches;
using Application.Match.RecordResult;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPQatar2027.Common;

namespace SPQatar2027.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MatchController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));


    [HttpPost]
    public async Task<IActionResult> CreateMatch(CreateMatchRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut("{publicId}/result")]
    public async Task<IActionResult> RecordResult(Guid publicId, RecordMatchResultRequest request)
    {
        request.MatchPublicId = publicId;
        var result = await _mediator.Send(request);
        return result.ToActionResult();
    }

    [HttpPut("{publicId}/forfeit")]
    public async Task<IActionResult> Forfeit(Guid publicId, ForfeitMatchRequest request)
    {
        request.MatchPublicId = publicId;
        var result = await _mediator.Send(request);
        return result.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetMatchesRequest request)
    {
        var result = await _mediator.Send(request);
        return result.ToActionResult();
    }
}
