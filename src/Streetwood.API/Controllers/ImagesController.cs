using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Streetwood.API.Bus;
using Streetwood.API.Controllers.Dto;
using Streetwood.Infrastructure.Commands.Models.Product;

namespace Streetwood.API.Controllers
{
    [Route("api/images")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ImagesController : ControllerBase
    {
        private readonly IBus bus;

        public ImagesController(IBus bus)
        {
            this.bus = bus;
        }

        [HttpPost()]
        public async Task<IActionResult> Post([FromForm]ImagePostDto input)
        {
            if (input.File == null)
            {
                return BadRequest("Missing file");
            }

            await bus.SendAsync(new AddProductImageCommandModel(input.Id, input.File, input.IsMain));
            return Accepted();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await bus.SendAsync(new DeleteImageCommandModel(id));
            return Accepted();
        }
    }
}