using Newtonsoft.Json;

namespace ChatService.Models
{
    public class RabbitModel
    {
        public RabbitModel(string operation, Dictionary<string, string> data)
        {
            Operation = operation;
            Data = data;
        }

        public string Operation { get; set; }

        public Dictionary<string, string> Data { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
