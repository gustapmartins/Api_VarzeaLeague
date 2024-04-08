﻿using VarzeaLeague.Domain.Interface.Services;
using Swashbuckle.AspNetCore.Annotations;
using VarzeaLeague.Application.DTO.User;
using VarzeaLeague.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace VarzeaLeague.Application.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public AuthController(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    /// <summary>
    ///     Consultar todas as partidas criadas
    /// </summary>
    /// <param name="page">Objeto com os campos necessários para definir as paginas</param> 
    /// <param name="pageSize">Objeto com os campos necessários para os limites das paginas</param> 
    ///     <returns>IActionResult</returns>
    /// <response code="200">Caso a busca seja feita com sucesso</response>
    [HttpGet("search-auths")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Get matches with optional pagination parameters")]
    public async Task<ActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        List<UserViewDto> authView = _mapper.Map<List<UserViewDto>>(await _authService.GetAsync(page, pageSize));

        return Ok(authView);
    }

    /// <summary>
    ///     faz o login e retorna um token de acesso
    /// </summary>
    /// <param name="loginDto">Objeto com os campos necessários para logar um usuário</param>
    ///     <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> LoginAsync(LoginDto loginDto)
    {
        UserModel loginUser = _mapper.Map<UserModel>(loginDto);

        var token = await _authService.Login(loginUser);
        return Ok(new { token });
    }

    /// <summary>
    ///     Cria um novo usuario no banco de dados
    /// </summary>
    /// <param name="userCreateDto">Objeto com os campos necessários para criação de um usuário</param>
    ///     <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    /// <response code="400">Caso a requisição esteja errada</response>
    [HttpPost("created-user")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> PostTeam([FromBody] RegisterDto userCreateDto)
    {
        UserModel UserCreated = _mapper.Map<UserModel>(userCreateDto);

        return CreatedAtAction(nameof(GetUsers), await _authService.CreateAsync(UserCreated));
    }
}
