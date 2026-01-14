using DumpTruckManagementSystem.Shared.Exceptions;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace DumpTruckManagementSystem.Api.Controllers
{
    public abstract class BaseApiController<T> : ControllerBase where T : BaseApiController<T>
    {
        // lazy–resolve Mediator on first access
        protected IMediator Mediator
            => HttpContext.RequestServices.GetRequiredService<IMediator>();

        protected string CurrentUserId
        {
            get
            {
                return User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue("sub")
                    ?? string.Empty;
            }
        }

        protected string CurrentUserName
            => User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

        public async Task<IActionResult> APIExecute<TResponse>(IRequest<TResponse> request)
        {
            try
            {
                var response = await Mediator.Send(request);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new Response<int>
                {
                    Data = 0,
                    Message = ex.Message,
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Succeeded = false
                });
            }
            catch (ForbiddenAccessException ex)
            {
                return StatusCode((int)HttpStatusCode.Forbidden, new Response<int>
                {
                    Data = 0,
                    Message = ex.Message,
                    HttpStatusCode = HttpStatusCode.Forbidden,
                    Succeeded = false
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<int>
                {
                    Data = 0,
                    Message = ex.Message,
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Succeeded = false
                });
            }
        }

    }
}
