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
    [HttpGet("buscar-partida")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetPlayer()
    {
        return Ok(await _playerService.GetAsync());
    }

    /// <summary>
    ///     Consultar categoria pelo id
    /// </summary>
    /// <param name="id">Objeto com os campos necessários para criação de um filme</param>
    ///     <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [HttpGet("buscar-partida/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIdPlayer(string id)
    {
        return Ok(await _playerService.GetIdAsync(id));
    }

    /// <summary>
    ///     Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="playerCreateDto">Objeto com os campos necessários para criação de um filme</param>
    ///     <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> PostTeam([FromBody] PlayerCreateDto playerCreateDto)
    {
        PlayerModel playerCreated = _mapper.Map<PlayerModel>(playerCreateDto);

        return CreatedAtAction(nameof(GetPlayer), await _playerService.CreateAsync(playerCreated));
    }

    /// <summary>
    ///     Faz 
    /// </summary>
    /// <param name="id"></param>
    ///     <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeletePlayer(string id)
    {
        PlayerViewDto teamView = _mapper.Map<PlayerViewDto>(await _playerService.RemoveAsync(id));

        return Ok(teamView);
    }

    /// <summary>
    ///     Consultar categoria pelo id
    /// </summary>
    /// <param name="id">Objeto com os campos necessários para criação de um filme</param>
    /// <param name="team">TEAM</param>
    ///     <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdatePlayer(string id, PlayerUpdateDto team)
    {
        PlayerModel teamUpdate = _mapper.Map<PlayerModel>(team);

        PlayerViewDto teamView = _mapper.Map<PlayerViewDto>(await _playerService.UpdateAsync(id, teamUpdate));

        return Ok(teamView);
    }
}
