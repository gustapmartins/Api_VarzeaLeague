using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VarzeaLeague.Application.DTO.Team;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaTeam.Application.Controllers.v1;
using VarzeaLeague.Domain.Model;
using AutoFixture;
using AutoMapper;
using Moq;
using MongoDB.Driver;
using VarzeaTeam.Infra.Data.Repository.EfCore;
using VarzeaLeague.Domain.Model.DatabaseSettings;
using Microsoft.Extensions.Options;

namespace VarzeaLeague.Test.Repository.EfCore;

public class TeamDaoEfCoreTest
{
    private readonly Fixture _fixture;
    private readonly Mock<IMongoCollection<TeamModel>> _teamCollectionMock;
    private readonly Mock<IOptions<VarzeaLeagueDatabaseSettings>> _varzeaLeagueSettingsMock;

    public TeamDaoEfCoreTest()
    {
        _fixture = new Fixture();
        _teamCollectionMock = new Mock<IMongoCollection<TeamModel>>();
        _varzeaLeagueSettingsMock = new Mock<IOptions<VarzeaLeagueDatabaseSettings>>();
    }

    [Fact]
    public async Task TeamExist_WhenCalled_ReturnsOkResultWithTeamExist()
    {
        //// Arrange
        //var teamToAdd = _fixture.Build<TeamModel>()
        //                       .With(x => x.NameTeam, "NameTeam")
        //                       .Create();

        //var mockCursor = new Mock<IAsyncCursor<TeamModel>>();
        //mockCursor.Setup(_ => _.Current).Returns(new List<TeamModel> { teamToAdd });
        //mockCursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);

        //// Configure settings mock
        //var settings = new VarzeaLeagueDatabaseSettings
        //{
        //    ConnectionString = "mongodb://localhost:27017",
        //    DatabaseName = "VarzeaLeagueDB"
        //};
        //_varzeaLeagueSettingsMock.Setup(s => s.Value).Returns(settings);

        //_teamCollectionMock
        //    .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<TeamModel>>(),
        //        It.IsAny<FindOptions<TeamModel, TeamModel>>(),
        //        It.IsAny<CancellationToken>()))
        //    .ReturnsAsync(mockCursor.Object);

        //var teamDao = new TeamDaoEfCore(_varzeaLeagueSettingsMock.Object);

        //// Act
        //var result = await teamDao.TeamExist(teamToAdd.NameTeam);

        //// Assert
        //Assert.NotNull(result);
        //Assert.Equal(teamToAdd.NameTeam, result.NameTeam);
    }
}
