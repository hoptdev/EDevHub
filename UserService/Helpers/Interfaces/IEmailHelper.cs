using System.Net.Mail;

namespace UserService.Helpers.Interfaces
{
    public interface IEmailHelper
    {
        public bool IsEmailValid(string email);
    }
}