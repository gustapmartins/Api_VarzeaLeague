using VarzeaLeague.Domain.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using VarzeaLeague.Application.DTO.Team;
using VarzeaTeam.Application.DTO.Team;
using VarzeaLeague.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace VarzeaTeam.Application.Controllers.v1;

[Authorize]
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
    ///    Buscar todos os times cadastrados
    /// </summary>
    /// <param name="page">Objeto com os campos necessários para definir as paginas</param> 
    /// <param name="pageSize">Objeto com os campos necessários para os limites das paginas</param> 
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso exista informações cadastradas</response>
    /// <response code="404">Caso as informações sejam passadas erradas</response>
    [HttpGet("search-teams")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetTeams([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        TeamViewDto teamView = _mapper.Map<TeamViewDto>(await _teamService.GetAsync(page, pageSize));

        return Ok(teamView);
    }

    /// <summary>
    ///     Buscar o time pelo seu identificador
    /// </summary>
    /// <param name="id">Objeto com os campos necessários para buscar um time</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [HttpGet("search-team/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetIdTeams([FromRoute] string id)
    {
        TeamViewDto teamVieew = _mapper.Map<TeamViewDto>(await _teamService.GetIdAsync(id));

        return Ok(teamVieew);
    }

    /// <summary>
    ///     Adiciona um time ao banco de dados
    /// </summary>
    /// <param name="teamCreateDto">Objeto com os campos necessários para criação de um time</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [HttpPost("created-team")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CreateTeam([FromBody] TeamCreateDto teamCreateDto)
    {
        TeamModel teamCreated = _mapper.Map<TeamModel>(teamCreateDto);

        return CreatedAtAction(nameof(GetTeams), await _teamService.CreateAsync(teamCreated));
    }

    /// <summary>
    ///     Faz a remoção de um time no banco de dados 
    /// </summary>
    /// <param name="id"></param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [HttpDelete("delete-team/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteTeam([FromRoute] string id)
    {
        TeamViewDto teamView = _mapper.Map<TeamViewDto>(await _teamService.RemoveAsync(id));

        return Ok(teamView); 
    }

    /// <summary>
    ///     Consulta o time pelo identificador e atualiza 
    /// </summary>
    /// <param name="id">Objeto com os campos necessários para criação de um filme</param>
    /// <param name="team">TEAM</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [HttpPatch("update-team/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateTeam([FromRoute] string id, [FromBody] TeamModel team)
    {
        TeamModel teamUpdate = _mapper.Map<TeamModel>(team);

        TeamViewDto teamView = _mapper.Map<TeamViewDto>(await _teamService.UpdateAsync(id, teamUpdate));

        return Ok(teamView);
    }
}
