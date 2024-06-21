using ChatService.Helpers;
using ChatService.Hubs;
using ChatService.Models;
using ChatService.RabbitMQ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;
using UserService.Helpers.Attributes;
using UserService.Helpers.Interfaces;

namespace ChatService.Controllers
{
    [Route("/chat/messages")]
    public class ChatController : ControllerBase
    {
        public MessageHelper MessageHelper { get; set; }

        public ChatHelper ChatHelper { get; set; }

        public UserService.Helpers.UserHelper UserHelper { get; set; }

        public IHubContext<ChatHub> ChatHub { get; set; }

        public RabbitProducer Producer { get; set; }

        public ChatController(IHelper<Message> helper, IHelper<Chat> chatHelper, IUserHelper userHelper, IHubContext<ChatHub> hubContext)
        {
            MessageHelper = (MessageHelper)helper;
            ChatHelper = (ChatHelper)chatHelper;

            UserHelper = (UserService.Helpers.UserHelper)userHelper;
            ChatHub = hubContext;

            Producer = new RabbitProducer("websocket_messages");
        }

        [HttpGet("getChats")]
        [Authorize]
        [SwaggerHeader("Authorization", null)]
        public async Task<List<Chat>> GetByUserId()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var chats = await ChatHelper.GetChatAsync(userId);


            //eto problema
            chats.ForEach(async (x) =>
            {
                var a = UserHelper.GetByIdAsync(x.User1Id).Result;
                var b = UserHelper.GetByIdAsync(x.User2Id).Result;

                x.User1 = userId == x.User1Id ? a : b;
                x.User2 = userId != x.User1Id ? a : b;
            });


            return chats;
        }

        [HttpGet("getAll")]
        [Authorize]
        [SwaggerHeader("Authorization", null)]
        public async Task<ActionResult<List<Message>>> GetMessages(int chatId)
        {
            var mes = await MessageHelper.GetMessagesAsync(chatId);

            return Ok(mes);
        }

        [HttpPost("send")]
        [Authorize]
        [SwaggerHeader("Authorization", null)]
        public async Task<ActionResult<Message>> Update([FromForm] SendRequest req)
        {
            var userIds = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (string.IsNullOrEmpty(userIds)) return BadRequest();

            var userId = Convert.ToInt32(userIds);

            if (userId == req.ToUserId) return BadRequest();

            var chatId = await ChatHelper.TryCreate(userId, req.ToUserId);

            Message message = new Message(req.Text, userId, chatId);
            SendMessageInQueue(message);

            await ChatHub.Clients.User(req.ToUserId.ToString()).SendAsync("Receive", req.Text, userId, chatId, message.TimeSpan);
            return Ok(message);
        }

        private void SendMessageInQueue(Message message)
        {
            var msg = JsonConvert.SerializeObject(message);

            RabbitModel rabbit = new RabbitModel("send_message", new Dictionary<string, string>() { { "msgData", msg } });

            Producer.Publish(rabbit.ToString());
        }
    }
}
