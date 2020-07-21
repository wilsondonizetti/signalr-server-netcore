using Microsoft.AspNetCore.Mvc;
using SignalRServerNetCore31.SignalRHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServerNetCore31.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ServiceHub _serviceHub;

        public MessageController(ServiceHub serviceHub)
        {
            this._serviceHub = serviceHub;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]string msg)
        {            
            await _serviceHub.SendMessage(msg);

            return Ok("Mensagem enviada com sucesso!");
        }

        [HttpPost]
        [Route("sendobject")]
        public async Task<IActionResult> PostSendObject([FromBody] string msg)
        {
            await _serviceHub.SendObjectToUser(new { id = 1, descricao = "teste de envio de objeto",  mensagem = msg, usuario = "testUser" }, "testUser");

            return Ok("Mensagem enviada com sucesso!");
        }
    }
}
