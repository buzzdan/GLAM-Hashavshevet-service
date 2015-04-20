using Mvc.Mailer;
using GlamServer.Mailers.Models;

namespace GlamServer.Mailers
{ 
    public interface IPasswordResetMailer
    {
			MvcMailMessage PasswordReset(MailerModel model);
	}
}