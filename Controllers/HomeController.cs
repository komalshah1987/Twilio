using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace TwilioService.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("index")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
       
        public IActionResult SendSMS()
        {
            string accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
            string authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: "This is a Test message",
                from: new Twilio.Types.PhoneNumber("+12183049089"),
                to: new Twilio.Types.PhoneNumber("+919869900564")
            );
            return View();
        }


        public async Task<IActionResult> SendMail()
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var plainTextContent = "SendGrid with C#";
            var htmlContent = "<strong>TEST using Send Grid</strong>";

            var messageEmail = new SendGridMessage()
            {
                From = new EmailAddress("komal.shah87@gmail.com", "Komal"),
                Subject = "Testing with SendGrid is Fun",
                PlainTextContent = plainTextContent,
                HtmlContent = htmlContent
            };
            messageEmail.AddTo(new EmailAddress("komal.shah87@gmail.com", "Komal"));
            var filePath = @"E:\Komal\index.txt";
            var bytes = System.IO.File.ReadAllBytes(filePath);
            var file = Convert.ToBase64String(bytes);
            messageEmail.AddAttachment("Voucher Details Report.txt", file);

            var response = await client.SendEmailAsync(messageEmail);
            ViewBag.SendGridStatusCode = response.StatusCode;
            ViewBag.SendGridHeaders = response.Headers.ToString();
            ViewBag.SendGridBody = response.Body;

            return await Task.Run(() => View());

        }
    }
}
