using Application.Common;
using Application.Group.CreateGroup;
using Application.Group.DeleteGroup;
using Application.Group.GetAllGroups;
using Application.Group.GetGroupById;
using Application.Group.GetGroupStandings;
using Application.Group.UpdateGroup;
using Core;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using SPQatar2027.Controllers;
using Xunit;

namespace UnitTests.Controllers;

public class GroupControllerTests
{
    [Fact]
    public async Task GetAllGroups_ReturnsOk_WhenSuccess()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();

        var dto = new GetAllGroupsDto { GroupName = "GroupA" };
        var responseObj = new GetAllGroupsResponse(new List<GetAllGroupsDto> { dto });
        var mediatorResult = Result<GetAllGroupsResponse>.Success(responseObj);

        mediator
            .Send(Arg.Any<GetAllGroupsRequest>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(mediatorResult));

        var controller = new GroupController(mediator);

        // Act
        var actionResult = await controller.GetAllGroups(CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<OkObjectResult>();
        var ok = (OkObjectResult)actionResult;
        ok.Value.Should().BeEquivalentTo(responseObj);
    }

    [Fact]
    public async Task GetGroupById_ReturnsOk_WhenFound()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();
        var publicId = Guid.NewGuid();

        var responseObj = new GetGroupByIdResponse { GroupName = "GroupX" };
        var mediatorResult = Result<GetGroupByIdResponse>.Success(responseObj);

        mediator
            .Send(Arg.Any<GetGroupByIdRequest>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(mediatorResult));

        var controller = new GroupController(mediator);

        // Act
        var actionResult = await controller.GetGroupById(publicId, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<OkObjectResult>();
        var ok = (OkObjectResult)actionResult;
        ok.Value.Should().BeEquivalentTo(responseObj);
    }

    [Fact]
    public async Task GetGroupById_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();
        var publicId = Guid.NewGuid();

        var mediatorResult = Result<GetGroupByIdResponse>.Failure(ApplicationErrors.NotFound);

        mediator
            .Send(Arg.Any<GetGroupByIdRequest>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(mediatorResult));

        var controller = new GroupController(mediator);

        // Act
        var actionResult = await controller.GetGroupById(publicId, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)actionResult;
        notFound.Value.Should().Be(ApplicationErrors.NotFound);
    }

    [Fact]
    public async Task GetGroupStandingByGroupPublicId_ReturnsOk_WhenSuccess()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();
        var groupPublicId = Guid.NewGuid();

        var dto = new GetGroupStandingsDto
        {
            TeamPublicId = Guid.NewGuid(),
            TeamName = "Team1",
            Played = 1,
            Wins = 1,
            Draws = 0,
            Losses = 0,
            PointsFor = 2,
            PointsAgainst = 0,
            StandingPoints = 3
        };

        var responseObj = new GetGroupStandingsResponse(new List<GetGroupStandingsDto> { dto });
        var mediatorResult = Result<GetGroupStandingsResponse>.Success(responseObj);

        mediator
            .Send(Arg.Any<GetGroupStandingsRequest>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(mediatorResult));

        var controller = new GroupController(mediator);

        // Act
        var actionResult = await controller.GetGroupStandingByGroupPublicId(groupPublicId, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<OkObjectResult>();
        var ok = (OkObjectResult)actionResult;
        ok.Value.Should().BeEquivalentTo(responseObj);
    }

    [Fact]
    public async Task CreateGroup_ReturnsOk_WhenSuccess()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();

        var createRequest = new CreateGroupRequest("MyGroup");
        var responseObj = new CreateGroupResponse { PublicId = Guid.NewGuid() };
        var mediatorResult = Result<CreateGroupResponse>.Success(responseObj);

        mediator
            .Send(Arg.Any<CreateGroupRequest>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(mediatorResult));

        var controller = new GroupController(mediator);

        // Act
        var actionResult = await controller.CreateGroup(createRequest, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<OkObjectResult>();
        var ok = (OkObjectResult)actionResult;
        ok.Value.Should().BeEquivalentTo(responseObj);

        await mediator.Received(1).Send(Arg.Is<CreateGroupRequest>(r => r.groupName == "MyGroup"), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateGroup_ReturnsOk_AndSendsPublicIdToRequest()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();
        var publicId = Guid.NewGuid();

        var updateRequest = new UpdateGroupRequest { Name = "UpdatedName", PublicId = Guid.Empty };
        var responseObj = new UpdateGroupResponse { PublicId = publicId, Name = "UpdatedName" };
        var mediatorResult = Result<UpdateGroupResponse>.Success(responseObj);

        mediator
            .Send(Arg.Any<UpdateGroupRequest>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(mediatorResult));

        var controller = new GroupController(mediator);

        // Act
        var actionResult = await controller.UpdateGroup(publicId, updateRequest, CancellationToken.None);

        // Assert response
        actionResult.Should().BeOfType<OkObjectResult>();
        var ok = (OkObjectResult)actionResult;
        ok.Value.Should().BeEquivalentTo(responseObj);

        // Verify mediator received the request with PublicId set by controller
        await mediator.Received(1).Send(
            Arg.Is<UpdateGroupRequest>(r => r.PublicId == publicId && r.Name == "UpdatedName"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteGroup_ReturnsOk_WhenSuccess()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();
        var publicId = Guid.NewGuid();

        var responseObj = new DeleteGroupResponse { PublicId = publicId, GroupName = "ToDelete" };
        var mediatorResult = Result<DeleteGroupResponse>.Success(responseObj);

        mediator
            .Send(Arg.Any<DeleteGroupRequest>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(mediatorResult));

        var controller = new GroupController(mediator);

        // Act
        var actionResult = await controller.DeleteGroup(publicId, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<OkObjectResult>();
        var ok = (OkObjectResult)actionResult;
        ok.Value.Should().BeEquivalentTo(responseObj);

        await mediator.Received(1).Send(Arg.Is<DeleteGroupRequest>(r => r.PublicId == publicId), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteGroup_ReturnsNotFound_WhenMediatorReturnsNotFound()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();
        var publicId = Guid.NewGuid();

        var mediatorResult = Result<DeleteGroupResponse>.Failure(ApplicationErrors.NotFound);

        mediator
            .Send(Arg.Any<DeleteGroupRequest>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(mediatorResult));

        var controller = new GroupController(mediator);

        // Act
        var actionResult = await controller.DeleteGroup(publicId, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)actionResult;
        notFound.Value.Should().Be(ApplicationErrors.NotFound);
    }
}
