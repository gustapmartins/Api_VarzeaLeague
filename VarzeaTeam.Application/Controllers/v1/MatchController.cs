using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VarzeaLeague.Application.DTO.Match;
using VarzeaLeague.Domain.Enum;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Model;
using VarzeaTeam.Application.DTO.Match;

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
    /// <param name="Page">Objeto com os campos necessários para definir as paginas </param>
    /// <param name="PageSize">Objeto com os campos necessários para os limites das paginas </param>
    /// <param name="FilterType">Objeto com os campos necessários para os filtros da pesquisa </param>
    /// <param name="Date">Objeto com os campos necessários para os filtros da Data da partida </param>
    /// <param name="NameTeam">Objeto com os campos necessários para os buscar o nome do time </param>
    ///     <returns>IActionResult</returns>
    /// <response code="200">Caso a busca seja feita com sucesso</response>
    [HttpGet("search-matchs")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Get matches with optional pagination parameters")]
    public async Task<ActionResult> GetMatch([FromQuery] int Page = 1, [FromQuery] int PageSize = 10, [FromQuery] FilterTypeEnum? FilterType = null, [FromQuery] DateTime? Date = null, [FromQuery] string? NameTeam = null)
    {
        List<MatchViewDto> matchView = _mapper.Map<List<MatchViewDto>>(await _matchService.GetAsync(Page, PageSize, FilterType, NameTeam, Date));

        return Ok(matchView);
    }

    /// <summary>
    ///     Consultar partida a partir pelo id
    /// </summary>
    /// <param name="Id">Objeto com os campos necessários para criação de um filme</param>
    ///     <returns>IActionResult</returns>
    /// <response code="200">Caso a busca seja feita com sucesso</response>
    [HttpGet("search-match/{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIdMatch([FromRoute] string Id)
    {
        MatchViewDto matchView = _mapper.Map<MatchViewDto>(await _matchService.GetIdAsync(Id));

        return Ok(matchView);
    }

    /// <summary>
    ///     Adiciona uma partida no banco de dados
    /// </summary>
    /// <param name="matchCreateDto">Objeto com os campos necessários para criação de um filme</param>
    ///     <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [Authorize]
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
    /// <param name="Id">Campo id para buscar um objeto</param>
    ///     <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [Authorize]
    [HttpDelete("delete-match/{Id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteMatch([FromRoute] string Id)
    {
        MatchViewDto matchView = _mapper.Map<MatchViewDto>(await _matchService.RemoveAsync(Id));

        return Ok(matchView);
    }

    /// <summary>
    ///     Atualiza a partida a partir do id
    /// </summary>
    /// <param name="Id">Campo id para buscar um objeto</param>
    /// <param name="matchUpdateDto">MatchDto</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [Authorize]
    [HttpPatch("update-match/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateMatch([FromRoute] string Id, [FromBody] MatchUpdateDto matchUpdateDto)
    {
        MatchModel matchModel = _mapper.Map<MatchModel>(matchUpdateDto);

        MatchViewDto matchView = _mapper.Map<MatchViewDto>(await _matchService.UpdateAsync(Id, matchModel));

        return Ok(matchView);
    }
}
