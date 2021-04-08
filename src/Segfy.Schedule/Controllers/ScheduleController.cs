using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Segfy.Schedule.Model.Filters;
using Segfy.Schedule.Model.Dtos;
using System.Threading.Tasks;
using MediatR;
using AutoWrapper.Wrappers;
using Segfy.Schedule.Model.Schema;
using System.Collections.Generic;
using Segfy.Schedule.Infra.Mediators.ScheduleActions.Commands;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;

namespace Segfy.Schedule.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ScheduleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ScheduleController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        /// <summary>
        /// Retorna todos os agendamentos
        /// </summary>
        /// <remarks>
        /// Exemplo de uma query:
        ///
        ///     GET /schedule/{subscriptionId}/?Filters[0].Field=type&amp;Filters[0].Operator=EqualsTo&amp;Filters[0].Value=WhatsApp
        ///
        /// 
        /// Exemplo de paginação:
        /// 
        ///     GET /schedule/{subscriptionId}/?lastKey=ca4f01b5-4464-4eb2-8772-2f6b319916ed&amp;limit=15
        ///     
        /// </remarks>
        /// <param name="subscriptionId">ID da assinatura</param>
        /// <param name="filter"></param>
        /// <response code="200">Retorna a lista paginada de agendamentos</response>
        /// <response code="422">Algum erro relativo a validação</response>
        /// <response code="500">Algum erro interno não tratado no servidor</response>
        [HttpGet("{subscriptionId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseModelMultiple<ScheduleItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorSchema), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorSchema), StatusCodes.Status500InternalServerError)]
        public async Task<ResponseModel> Get([FromRoute] Guid subscriptionId, [FromQuery] FilterData filter)
        {
            var command = new GetAllSchedulesCommand { SubscriptionId = subscriptionId };
            if (filter.Limit.HasValue)
            {
                command.PerPage = filter.Limit.GetValueOrDefault();
            }
            if (filter.LastKey.HasValue)
            {
                command.NextKey = filter.LastKey.GetValueOrDefault();
            }

            var items = await _mediator.Send(command);

            var pagination = new Pagination();
            if (items.NextKey.GetValueOrDefault() != Guid.Empty)
            {
                pagination.NextKey = items.NextKey;
                pagination.NextApiPage = $"/schedule/{subscriptionId}/?lastKey={items.NextKey}&limit={filter.Limit}";
            }

            return ResponseModelMultiple<ScheduleItemDto>.Success(items.Items, pagination);
        }

        /// <summary>
        /// Retorna um agendamento pelo id
        /// </summary>
        /// <param name="subscriptionId">ID da assinatura </param>
        /// <param name="id">ID do registro de agendamento</param>
        /// <response code="200">Retorna a lista paginada de agendamentos</response>
        /// <response code="422">Algum erro relativo a validação</response>
        /// <response code="500">Algum erro interno não tratado no servidor</response>
        [HttpGet("{subscriptionId}/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseModelSingle<ScheduleItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorSchema), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorSchema), StatusCodes.Status500InternalServerError)]
        public async Task<ResponseModel> GetOne([FromRoute] Guid subscriptionId, [FromRoute] Guid id)
        {
            var item = await _mediator.Send(new GetScheduleCommand { SubscriptionId = subscriptionId, Id = id });
            return ResponseModelSingle<ScheduleItemDto>.Success(item);
        }

        /// <summary>
        /// Salva e retorna o agendamento salvo
        /// </summary>
        /// <param name="subscriptionId">ID da assinatura </param>
        /// <param name="schedule"></param>
        /// <response code="200">Retorna o agendamento salvo</response>
        /// <response code="422">Algum erro relativo a validação</response>
        /// <response code="500">Algum erro interno não tratado no servidor</response>
        [HttpPost("{subscriptionId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseModelSingle<ScheduleItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorSchema), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorSchema), StatusCodes.Status500InternalServerError)]
        public async Task<ResponseModel> Post([FromRoute] Guid subscriptionId, [FromBody] ScheduleCreationDto schedule)
        {
            var item = await _mediator.Send(new CreateScheduleCommand { SubscriptionId = subscriptionId, Schedule = schedule });
            return ResponseModelSingle<ScheduleItemDto>.Success(item);
        }

        /// <summary>
        /// Atualiza um agendamento
        /// </summary>
        /// <param name="subscriptionId">ID da assinatura </param>
        /// <param name="id">ID do registro de agendamento</param>
        /// <param name="schedule"></param>
        /// <response code="200">Retorna o agendamento atualizado</response>
        /// <response code="400">Algum erro relativo a validação</response>
        /// <response code="500">Algum erro interno não tratado no servidor</response>
        [HttpPut("{subscriptionId}/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseModelSingle<ScheduleItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorSchema), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorSchema), StatusCodes.Status500InternalServerError)]
        public async Task<ResponseModel> Put([FromRoute] Guid subscriptionId, [FromRoute] Guid id, [FromBody] ScheduleCreationDto schedule)
        {
            var item = await _mediator.Send(new UpdateScheduleCommand { SubscriptionId = subscriptionId, Id = id, Schedule = schedule });
            return ResponseModelSingle<ScheduleItemDto>.Success(item);
        }

        /// <summary>
        /// Remove um agendamento
        /// </summary>
        /// <param name="subscriptionId">ID da assinatura </param>
        /// <param name="id">ID do registro de agendamento</param>
        /// <response code="200">Retorna a confirmação de sucesso para a ação de apagar</response>
        /// <response code="400">Algum erro relativo a validação</response>
        /// <response code="500">Algum erro interno não tratado no servidor</response>
        [HttpDelete("{subscriptionId}/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorSchema), StatusCodes.Status500InternalServerError)]
        public async Task<ResponseModel> Delete([FromRoute] Guid subscriptionId, [FromRoute] Guid id)
        {
            var item = await _mediator.Send(new DeleteScheduleCommand { SubscriptionId = subscriptionId, Id = id });
            return ResponseModel.Success();
        }
    }
}
