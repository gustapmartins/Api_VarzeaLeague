using VarzeaLeague.Domain.Interface.Services;
using Swashbuckle.AspNetCore.Annotations;
using VarzeaLeague.Application.DTO.User;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using VarzeaLeague.Domain.Model.User;
using Microsoft.AspNetCore.Authorization;

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
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso a busca seja feita com sucesso</response>
    [HttpGet("search-users")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Get matches with optional pagination parameters")]
    public async Task<ActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        List<UserViewDto> authView = _mapper.Map<List<UserViewDto>>(await _authService.GetAsync(page, pageSize));

        return Ok(authView);
    }

    /// <summary>
    ///     Consultar todas as partidas criadas
    /// </summary>
    /// <param name="Id">Objeto com os campos necessários para definir as paginas</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso a busca seja feita com sucesso</response>
    [HttpGet("search-user/{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Get matches with optional pagination parameters")]
    public async Task<ActionResult> GetUserId([FromRoute] string Id)
    {
        UserViewDto authView = _mapper.Map<UserViewDto>(await _authService.GetIdAsync(Id));

        return Ok(authView);
    }

    /// <summary>
    ///     faz o login e retorna um token de acesso
    /// </summary>
    /// <param name="loginDto">Objeto com os campos necessários para logar um usuário</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    [AllowAnonymous]
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
    /// <remarks> Create User </remarks>
    /// <returns>IActionResult</returns>
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

    /// <summary>
    ///     Redefinir a senha de um usuario no banco de dados
    /// </summary>
    /// <param name="email">Objeto com os campos necessários para mudar a senha de um usuário</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    /// <response code="400">Caso a requisição esteja errada</response>
    [HttpPost("forget-password")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ForgetPassword([FromHeader] string email)
    {
        string token = await _authService.ForgetPassword(email);

        return Ok(new { token });
    }

    /// <summary>
    ///     Redefinir a senha de um usuario no banco de dados
    /// </summary>
    /// <param name="token">Objeto com os campos necessários para mudar a senha de um usuário</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    /// <response code="400">Caso a requisição esteja errada</response>
    [HttpPost("verification-password-OTP")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> VerificationPasswordOTP([FromHeader] string token)
    {
        string hashToken = await _authService.VerificationPasswordOTP(token);

        return Ok(new { token = hashToken });
    }

    /// <summary>
    ///     Redefinir a senha de um usuario no banco de dados
    /// </summary>
    /// <param name="passwordResetDto">Objeto com os campos necessários para mudar a senha de um usuário</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    /// <response code="400">Caso a requisição esteja errada</response>
    [Authorize]
    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ResetPassword([FromBody] PasswordResetDto passwordResetDto)
    {
        PasswordReset passwordReset = _mapper.Map<PasswordReset>(passwordResetDto);

        return Ok(await _authService.ResetPassword(passwordReset));
    }

    /// <summary>
    ///     Apagar um usuario no banco de dados
    /// </summary>
    /// <param name="Id">Objeto com os campos necessários para delete um     usuário</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    /// <response code="400">Caso a requisição esteja errada</response>
    [HttpDelete("delete-user/{Id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RemoveUser([FromQuery] string Id)
    {
        return Ok(await _authService.RemoveAsync(Id));
    }

    /// <summary>
    ///     Atualiza o usuario a partir do Id
    /// </summary>
    /// <param name="Id">Objeto com os campos necessários para criação de um filme</param>
    /// <param name="userDto">TEAM</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso inserção não seja feita com sucesso</response>
    [HttpPatch("update-user/{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateMatch(string Id, UserUpdateDto userDto)
    {
        UserModel userModel = _mapper.Map<UserModel>(userDto);

        UserViewDto userView = _mapper.Map<UserViewDto>(await _authService.UpdateAsync(Id, userModel));

        return Ok(userView);
    }
}

