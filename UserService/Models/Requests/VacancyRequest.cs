using UserService.Models.Enums;

namespace UserService.Models.Requests
{
    public class VacancyRequest
    {
        public string Name { get; set; }

        public decimal Pay { get; set; }

        public string Description { get; set; }

        public Experience Experience { get; set; }
    }
}
