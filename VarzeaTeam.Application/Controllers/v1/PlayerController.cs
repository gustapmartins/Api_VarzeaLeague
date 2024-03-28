using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VarzeaTeam.Application.DTO.Team;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaTeam.Domain.Model.Player;
using VarzeaTeam.Application.DTO.Player;

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
    /// <param name="teamCreateDto">Objeto com os campos necessários para criação de um filme</param>
    ///     <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> PostTeam([FromBody] PlayerCreateDto teamCreateDto)
    {
        PlayerModel playerCreated = _mapper.Map<PlayerModel>(teamCreateDto);

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
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdatePlayer(string id, TeamUpdateDto team)
    {
        return Ok(id);
    }
}
