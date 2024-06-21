using System.Net.Mail;
using UserService.Helpers.Interfaces;

namespace UserService.Helpers
{
    public class EmailHelper : IEmailHelper
    {
        public bool IsEmailValid(string email)
        {
            bool isValid = false;
            try
            {
                MailAddress address = new MailAddress(email);
                isValid = address.Address == email;
            }
            catch (FormatException)
            {
                return false;
            }

            return isValid;
        }
    }
}
