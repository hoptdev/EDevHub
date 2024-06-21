namespace ChatService.Models
{
    public class SendRequest
    {
        public int ToUserId { get; set; }

        public string Text { get; set; }
    }
}
