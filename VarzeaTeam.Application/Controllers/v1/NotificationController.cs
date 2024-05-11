using VarzeaLeague.Application.DTO.Notification;
using VarzeaLeague.Domain.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace VarzeaLeague.Application.Controllers.v1;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly IMapper _mapper;

    public NotificationController(INotificationService notificationService, IMapper mapper)
    {
        _notificationService = notificationService;
        _mapper = mapper;
    }

    /// <summary>
    ///     Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="page">Objeto com os campos necessários para definir as paginas</param> 
    /// <param name="pageSize">Objeto com os campos necessários para os limites das paginas</param> 
    ///     <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>
    [HttpGet("search-notifications")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetPlayer([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        List<NotificationViewDto> notificationView = _mapper.Map<List<NotificationViewDto>>(await _notificationService.GetNotificationAsync(page, pageSize));

        return Ok(notificationView);
    }
}
