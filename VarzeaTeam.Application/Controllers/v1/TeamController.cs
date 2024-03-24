using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaTeam.Application.DTO.Team;
using VarzeaTeam.Domain.Model.Team;

namespace VarzeaTeam.Application.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;
    private readonly IMapper _mapper;

    public TeamController(ITeamService teamService, IMapper mapper)
    {
        _teamService = teamService;
        _mapper = mapper;
    }

    /// <summary>
    ///     Adiciona um filme ao banco de dados
    /// </summary>
    ///     <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    [HttpGet("consultar-time")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetTeams()
    {
        return Ok(await _teamService.GetAsync());
    }

    /// <summary>
    ///     Consultar categoria pelo id
    /// </summary>
    /// <param name="id">Objeto com os campos necessários para criação de um filme</param>
    ///     <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [HttpGet("consultar-time/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetIdTeams(string id)
    {
        return Ok(id);
    }

    /// <summary>
    ///     Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="team">Objeto com os campos necessários para criação de um filme</param>
    ///     <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [HttpPost("cadastrar-time")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> PostTeam([FromBody] TeamCreateDto teamCreateDto)
    {
        TeamModel teamCreated = _mapper.Map<TeamModel>(teamCreateDto);

        return CreatedAtAction(nameof(GetTeams), await _teamService.CreateAsync(teamCreated));
    }

    /// <summary>
    ///     Faz 
    /// </summary>
    /// <param name="id"></param>
    ///     <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [HttpDelete("deletar-time/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult DeleteTeam(string id)
    {
        return Ok(id);
    }

    /// <summary>
    ///     Consultar categoria pelo id
    /// </summary>
    /// <param name="id">Objeto com os campos necessários para criação de um filme</param>
    /// <param name="team">TEAM</param>
    ///     <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [HttpPatch("update-time/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult UpdateTeam(string id, TeamUpdateDto team)
    {
        return Ok(id);
    }

}
