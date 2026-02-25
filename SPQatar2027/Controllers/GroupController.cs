using Application.Group.CreateGroup;
using Application.Group.DeleteGroup;
using Application.Group.GetAllGroups;
using Application.Group.GetGroupById;
using Application.Group.GetGroupStandings;
using Application.Group.UpdateGroup;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPQatar2027.Common;

namespace SPQatar2027.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class GroupController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet]
    public async Task<IActionResult> GetAllGroups(CancellationToken cancellationToken)
    {
        var request = new GetAllGroupsRequest();
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("{publicId:Guid}")]
    public async Task<IActionResult> GetGroupById([FromRoute] Guid publicId, CancellationToken cancellationToken)
    {
        var request = new GetGroupByIdRequest(publicId);
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("/group/standings/{groupPublicId:Guid}")]
    public async Task<IActionResult> GetGroupStandingByGroupPublicId([FromRoute] Guid groupPublicId, CancellationToken cancellationToken)
    {
        var request = new GetGroupStandingsRequest()
        {
            GroupPublicId = groupPublicId
        };
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest createGroupRequest, CancellationToken cancellationToken)
    {
        var request = new CreateGroupRequest(createGroupRequest.groupName);
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut("{publicId:Guid}")]
    public async Task<IActionResult> UpdateGroup([FromRoute] Guid publicId, [FromBody] UpdateGroupRequest updateGroupRequest, CancellationToken cancellationToken)
    {
        updateGroupRequest.PublicId = publicId;

        var result = await _mediator.Send(updateGroupRequest, cancellationToken);
        return result.ToActionResult();
    }

    [HttpDelete("{publicId:Guid}")]
    public async Task<IActionResult> DeleteGroup([FromRoute] Guid publicId, CancellationToken cancellationToken)
    {
        var request = new DeleteGroupRequest()
        {
            PublicId = publicId
        };
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToActionResult();
    }
}
