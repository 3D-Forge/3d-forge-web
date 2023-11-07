using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Backend3DForge.Services.Email
{
	public class EmailService : IEmailService
	{
		private readonly EmailServiceConfigurationMetadata configuration;

		private string EmailTemplatesFoulder => Path.IsPathRooted(configuration.EmailTemplatesFoulder) ?
			configuration.EmailTemplatesFoulder :
			Path.Combine(Directory.GetCurrentDirectory(), configuration.EmailTemplatesFoulder);

		public EmailService(IOptions<EmailServiceConfigurationMetadata> configuration)
		{
			this.configuration = configuration.Value;
		}

		public async Task SendEmailAsync(string email, string subject, string message, bool isHtml = false)
		{
			using (SmtpClient client = Connect())
			{
				using (MailMessage mailMessage = new MailMessage(configuration.Email, email, subject, message))
				{
					mailMessage.IsBodyHtml = isHtml;
					await client.SendMailAsync(mailMessage);
				}
			}
		}

		public async Task SendEmailUseTemplateAsync(string email, string tepmlateName, Dictionary<string, string>? parameters = null, string? subject = null)
		{
			string templatePath = Path.Combine(EmailTemplatesFoulder, tepmlateName);
			if (!File.Exists(templatePath))
			{
				throw new FileNotFoundException($"Template {tepmlateName} not found");
			}

			string template = File.ReadAllText(templatePath);
			if (parameters != null)
			{
				foreach (var parameter in parameters)
				{
					template = template.Replace($@"{{{{{parameter.Key}}}}}", parameter.Value);
				}
			}

			if (subject == null)
			{
				subject = Regex.Match(template, @"<meta name=""subject"" content=""(.*)""").Groups[1].Value;
			}

			await SendEmailAsync(email, subject, template, true);
		}

		private SmtpClient Connect()
		{
			SmtpClient smtpClient = new SmtpClient();
			smtpClient.Host = configuration.Host;
			smtpClient.Port = configuration.Port;
			smtpClient.EnableSsl = configuration.UseSSL;
			smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
			smtpClient.UseDefaultCredentials = configuration.UseDefaultCredentials;
			smtpClient.Credentials = new NetworkCredential(configuration.Email, configuration.Password);
			return smtpClient;
		}
	}
}
