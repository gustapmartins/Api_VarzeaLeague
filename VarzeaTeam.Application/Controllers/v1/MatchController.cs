using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VarzeaTeam.Domain.Model.Match;
using VarzeaTeam.Application.DTO.Team;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaTeam.Application.DTO.Match;
using VarzeaLeague.Application.DTO.Match;
using Swashbuckle.AspNetCore.Annotations;

namespace VarzeaTeam.Application.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class MatchController : ControllerBase
{
    private readonly IMatchService _matchService;
    private readonly IMapper _mapper;

    public MatchController(IMatchService matchService, IMapper mapper)
    {
        _matchService = matchService;
        _mapper = mapper;
    }

    /// <summary>
    ///     Consultar todas as partidas criadas
    /// </summary>
    /// <param name="page">Objeto com os campos necessários para definir as paginas</param> 
    /// <param name="pageSize">Objeto com os campos necessários para os limites das paginas</param> 
    ///     <returns>IActionResult</returns>
    /// <response code="200">Caso a busca seja feita com sucesso</response>
    [HttpGet("search-matchs")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Get matches with optional pagination parameters")]
    public async Task<ActionResult> GetMatch([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        List<MatchViewDto> matchView = _mapper.Map<List<MatchViewDto>>(await _matchService.GetAsync(page, pageSize));

        return Ok(matchView);
    }

    /// <summary>
    ///     Consultar partida a partir pelo id
    /// </summary>
    /// <param name="id">Objeto com os campos necessários para criação de um filme</param>
    ///     <returns>IActionResult</returns>
    /// <response code="200">Caso a busca seja feita com sucesso</response>
    [HttpGet("search-match/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIdMatch([FromRoute] string id)
    {
        MatchViewDto matchView = _mapper.Map<MatchViewDto>(await _matchService.GetIdAsync(id));

        return Ok(matchView);
    }

    /// <summary>
    ///     Adiciona uma partida no banco de dados
    /// </summary>
    /// <param name="matchCreateDto">Objeto com os campos necessários para criação de um filme</param>
    ///     <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [HttpPost("created-match")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> PostTeam([FromBody] MatchCreateDto matchCreateDto)
    {
        MatchModel matchCreated = _mapper.Map<MatchModel>(matchCreateDto);

        return CreatedAtAction(nameof(GetMatch), await _matchService.CreateAsync(matchCreated));
    }

    /// <summary>
    ///     Deleta uma partida a partir do id
    /// </summary>
    /// <param name="id"></param>
    ///     <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [HttpDelete("delete-match/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteMatch(string id)
    {
        MatchViewDto matchView = _mapper.Map<MatchViewDto>(await _matchService.RemoveAsync(id));

        return Ok(matchView);
    }

    /// <summary>
    ///     Atualiza a partida a partir do id
    /// </summary>
    /// <param name="id">Objeto com os campos necessários para criação de um filme</param>
    /// <param name="team">TEAM</param>
    ///     <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [HttpPatch("update-match/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateMatch(string id, TeamUpdateDto team)
    {
        MatchModel matchModel = _mapper.Map<MatchModel>(team);

        MatchViewDto matchView = _mapper.Map<MatchViewDto>(await _matchService.UpdateAsync(id, matchModel));

        return Ok(matchView);
    }
}
