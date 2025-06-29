namespace BEBase.Extension
{
    using BEBase.Entity;
    using BEBase.Repository;
    using Microsoft.AspNetCore.SignalR;

    public class ChatHub : Hub
    {
        private readonly IRepo<Message> _repo;

        public ChatHub(IRepo<Message> repo)
        {
            _repo = repo;
        }

        public async Task SendMessage( int senderId, int receiverId, string text)
        {
            var timestamp = DateTime.UtcNow;

            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Text = text,
                Timestamp = timestamp
            };

            await _repo.AddAsync(message);

            var payload = new
            {
                senderId,
                receiverId,
                text,
                timestamp
            };

            string group = GetGroupName(senderId, receiverId);
            await Clients.Group(group).SendAsync("ReceiveMessage", payload);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var senderIdStr = httpContext?.Request.Query["senderId"];
            var receiverIdStr = httpContext?.Request.Query["receiverId"];

            if (int.TryParse(senderIdStr, out int senderId) && int.TryParse(receiverIdStr, out int receiverId))
            {
                string group = GetGroupName(senderId, receiverId);
                await Groups.AddToGroupAsync(Context.ConnectionId, group);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var senderIdStr = httpContext?.Request.Query["senderId"];
            var receiverIdStr = httpContext?.Request.Query["receiverId"];

            if (int.TryParse(senderIdStr, out int senderId) && int.TryParse(receiverIdStr, out int receiverId))
            {
                string group = GetGroupName(senderId, receiverId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
            }

            await base.OnDisconnectedAsync(exception);
        }

        private string GetGroupName(int user1, int user2)
        {
            var ids = new[] { user1, user2 };
            Array.Sort(ids);
            return $"chat-{ids[0]}-{ids[1]}";
        }
    }
}
