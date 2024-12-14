using Microsoft.AspNetCore.SignalR;

namespace ContabilidaMarket.SignalR
{
    public class NotificacionSingIr : Hub
    {
        // Diccionario para almacenar los identificadores de conexión
        private static Dictionary<string, string> _connections = new Dictionary<string, string>();
        public override Task OnConnectedAsync()
        {
            // Almacena el identificador de conexión del cliente
            _connections[Context.ConnectionId] = Context.ConnectionId;
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            // Elimina el identificador de conexión del cliente cuando se desconecta
            _connections.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
        public async Task SendMessage(string connectionId, string message)
        {
            // Envía el mensaje solo al cliente específico
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
        }
    }
}
