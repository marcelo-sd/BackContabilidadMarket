using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ContabilidaMarket.SignalR;
using WorkerService0001;

namespace ContabilidaMarket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(401)]  // No autorizado
    [ProducesResponseType(403)] //no permitido
    public class TestController : ControllerBase
    {
        private readonly IColaDeTareasEnSEgundoPlano _taskQueue;
        private readonly CallBackTareasCompletadas _callback;
        private readonly IHubContext<NotificacionSingIr> _hubContext;

        public TestController(IColaDeTareasEnSEgundoPlano taskQueue, CallBackTareasCompletadas callback, IHubContext<NotificacionSingIr> hubContext)
        {
            _taskQueue = taskQueue;
            _callback = callback;
            _hubContext = hubContext;
        }

        [HttpPost("enqueue")]
        public async Task<IActionResult> Enqueue([FromQuery] string connectionId)
        {
            _taskQueue.ElementoDTrabajoEnSEgundoPlano(async token =>
            {
                // Simular una tarea larga
                await Task.Delay(5000, token);
                // Log para verificar que la tarea se ejecutó
                Console.WriteLine("Tarea encolada ejecutada.");
                // Llamar al callback y luego notificar al cliente
                _callback("La tarea se ejecuto exitosamente ");
                await NotificacionAClient(connectionId, "tarea completada che!");
            });
            return Ok(new { message = "Tarea encolada" });
        }

        private async Task NotificacionAClient(string connectionId, string message)
        {
            await _hubContext.Clients.Client(connectionId).
                SendAsync("ReceiveMessage", message);
        }
    }
}
