using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Segfy.Schedule.Model.Pagination;
using Segfy.Schedule.Model.Entities;
using System.Linq;
using System;
using Segfy.Schedule.Model.Filters;
using Segfy.Schedule.Model.Dtos;
using System.Threading.Tasks;

namespace Segfy.Schedule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        /// <summary>
        /// Retorna todos os agendamentos
        /// </summary>
        /// <remarks>
        /// Exemplo de uma query:
        ///
        ///     GET /schedule?Filters[0].Field=Kind&amp;Filters[0].Operator=EqualsTo&amp;Filters[0].Value=WhatsApp
        ///
        /// 
        /// Exemplo de paginação:
        /// 
        ///     GET /schedule?Offset=2&amp;Limit=10
        ///     
        /// </remarks>
        /// <param name="filter"></param>
        /// <response code="200">Retorna a lista paginada de agendamentos</response>
        /// <response code="400">Algum erro relativo a validação</response>
        /// <response code="500">Algum erro interno não tratado no servidor</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DynamoDBPagedRequest<ScheduleItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] FilterData filter)
        {
            var items = Enumerable.Range(1, 5).Select(index => new ScheduleItemDto
            {
                Id = Guid.NewGuid(),
                Kind = "WhatsApp",
                Description = "Aniversário",
                Recurrence = Model.Enuns.Recurrence.Yearly,
                InsuredId = Guid.NewGuid(),
                AccountableId = Guid.NewGuid(),
                SubscriptionId = Guid.NewGuid(),
                Date = DateTime.Now.AddDays(index),
            })
            .ToArray();

            return Ok(new DynamoDBPagedRequest<ScheduleItemDto> { Items = items });
        }

        /// <summary>
        /// Retorna um agendamento pelo id
        /// </summary>
        /// <param name="id">Use um uuid guid</param>
        /// <response code="200">Retorna a lista paginada de agendamentos</response>
        /// <response code="400">Algum erro relativo a validação</response>
        /// <response code="500">Algum erro interno não tratado no servidor</response>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ScheduleItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOne(Guid id)
        {
            return Ok(new ScheduleItemDto
            {
                Id = Guid.NewGuid(),
                Kind = "WhatsApp",
                Description = "Aniversário",
                Recurrence = Model.Enuns.Recurrence.Yearly,
                InsuredId = Guid.NewGuid(),
                AccountableId = Guid.NewGuid(),
                SubscriptionId = Guid.NewGuid(),
                Date = DateTime.Now.AddDays(1),
            });
        }

        /// <summary>
        /// Salva e retorna o agendamento salvo
        /// </summary>
        /// <param name="item"></param>
        /// <response code="200">Retorna o agendamento salvo</response>
        /// <response code="400">Algum erro relativo a validação</response>
        /// <response code="500">Algum erro interno não tratado no servidor</response>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ScheduleItemDto item)
        {
            return Ok(item);
        }

        /// <summary>
        /// Atualiza um agendamento
        /// </summary>
        /// <param name="item"></param>
        /// <response code="204">Retorna o padrão "No content" para o update</response>
        /// <response code="400">Algum erro relativo a validação</response>
        /// <response code="500">Algum erro interno não tratado no servidor</response>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ScheduleItemDto item)
        {
            return NoContent();
        }

        /// <summary>
        /// Remove um agendamento
        /// </summary>
        /// <param name="id">Use um uuid guid</param>
        /// <response code="204">Retorna o padrão "No content" para o delete</response>
        /// <response code="400">Algum erro relativo a validação</response>
        /// <response code="500">Algum erro interno não tratado no servidor</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return NoContent();
        }
    }
}
