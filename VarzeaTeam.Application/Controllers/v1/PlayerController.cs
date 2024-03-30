using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaTeam.Domain.Model.Player;
using VarzeaTeam.Application.DTO.Player;
using VarzeaLeague.Application.DTO.Player;

namespace VarzeaLeague.Application.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class PlayerController: ControllerBase
{
    private readonly IPlayerService _playerService;
    private readonly IMapper _mapper;

    public PlayerController(IPlayerService playerService, IMapper mapper)
    {
        _playerService = playerService;
        _mapper = mapper;
    }

     /// <summary>
    ///     Adiciona um filme ao banco de dados
    /// </summary>
    ///     <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    [HttpGet("search-players")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetPlayer()
    {
        List<PlayerViewDto> playerView = _mapper.Map<List<PlayerViewDto>>(await _playerService.GetAsync());

        return Ok(playerView);
    }

    /// <summary>
    ///     Consultar categoria pelo id
    /// </summary>
    /// <param name="id">Objeto com os campos necessários para criação de um filme</param>
    ///     <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [HttpGet("search-player/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIdPlayer([FromRoute] string id)
    {
        PlayerViewDto playerView = _mapper.Map<PlayerViewDto>(await _playerService.GetIdAsync(id));

        return Ok(playerView);
    }

    /// <summary>
    ///     Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="playerCreatedDto">Objeto com os campos necessários para criação de um filme</param>
    ///     <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [HttpPost("created-player")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> CreatePlayer([FromBody] PlayerCreateDto playerCreatedDto)
    {
        PlayerModel playerCreated = _mapper.Map<PlayerModel>(playerCreatedDto);

        return CreatedAtAction(nameof(GetPlayer), await _playerService.CreateAsync(playerCreated));
    }

    /// <summary>
    ///     Faz 
    /// </summary>
    /// <param name="id"></param>
    ///     <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [HttpDelete("delete-player/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeletePlayer([FromRoute] string id)
    {
        PlayerViewDto playerView = _mapper.Map<PlayerViewDto>(await _playerService.RemoveAsync(id));

        return Ok(playerView);
    }

    /// <summary>
    ///     Consultar categoria pelo id
    /// </summary>
    /// <param name="id">Objeto com os campos necessários para criação de um filme</param>
    /// <param name="team">TEAM</param>
    ///     <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [HttpPatch("update-player/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdatePlayer([FromRoute] string id, [FromBody] PlayerUpdateDto team)
    {
        PlayerModel playerModel = _mapper.Map<PlayerModel>(team);

        PlayerViewDto playerView = _mapper.Map<PlayerViewDto>(await _playerService.UpdateAsync(id, playerModel));

        return Ok(playerView);
    }
}
