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
    public class HealthController
    {
        public HealthController()
        {
        }

        [HttpGet]
        [Route("isalive")]
        public IActionResult IsAlive()
        {
            return new OkResult();
        }
    }
}
