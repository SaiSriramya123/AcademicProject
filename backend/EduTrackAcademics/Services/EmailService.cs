using System.Net;
using System.Net.Mail;
using EduTrackAcademics.Model;
using MailKit.Net.Smtp;
using MimeKit;

namespace EduTrackAcademics.Services
{
		public interface IEmailService { 
		Task SendEmailAsync(string to, string subject, string body);
	}

		public class EmailService : IEmailService
		{
			private readonly IConfiguration _config;
			public EmailService(IConfiguration config)
			{
				_config = config;
			}

			public async Task SendEmailAsync(string to, string subject, string body)
			{
				var email = new MimeMessage();
				email.From.Add(MailboxAddress.Parse(_config["EmailSettings:From"]));
				email.To.Add(MailboxAddress.Parse(to));
				email.Subject = subject;
				email.Body = new BodyBuilder { HtmlBody = body }.ToMessageBody();

				using var smtp = new MailKit.Net.Smtp.SmtpClient();
				await smtp.ConnectAsync(_config["EmailSettings:Host"], 587, MailKit.Security.SecureSocketOptions.StartTls);
			    await smtp.AuthenticateAsync(_config["EmailSettings:Username"], _config["EmailSettings:Password"]);
				await smtp.SendAsync(email);
				await smtp.DisconnectAsync(true);
			}
		}
}







	//		public void SendPassword(string toEmail, string password)
	//		{
	//			var mail = new MailMessage();
	//			mail.To.Add(toEmail);
	//			mail.Subject = "Coordinator Account Created";
	//			mail.Body =
	//$@"Your coordinator account has been created.

//Temporary Password: {password}

//Please login and change your password immediately.";

//			mail.From = new MailAddress("admin@edutrack.com");

//			var smtp = new SmtpClient("smtp.gmail.com", 587)
//			{
//				Credentials = new NetworkCredential("saisriramyasetti@gmail.com", "uync xqmp xvre guqw"),
//				EnableSsl = true
//			};

//			smtp.Send(mail);
//		}
