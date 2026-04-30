using InsureYouAI.Context;
using InsureYouAI.Entities;
using InsureYouAI.Services.OpenAIServices;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MimeKit;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace InsureYouAI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOpenAIService _aiService;
        private readonly InsureContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;


        public HomeController(IOpenAIService openAIService, InsureContext context, IConfiguration configuration, HttpClient client)
        {
            _aiService = openAIService;
            _context = context;
            _configuration = configuration;
            _client = client;
        }

        public IActionResult Index()
        {
            return View();
        }
        public PartialViewResult SendMessage()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(Message message)
        {
            message.SendDate=DateTime.Now;
            message.IsRead = false;
            _context.Messages.Add(message);
            _context.SaveChanges();

            TempData["Success"] = "Mailiniz sistemimize iletilmiştir.En kısa zamanda tarafınıza dönüş sağlanacaktır.";

            #region Claude_AI_Analiz

            var apiKey = _configuration["AntropicClaudeEmailKey:ApiKey"];

            string prompt = $@"Sen bir sigorta firmasının müşteri iletişim asistanısın.

            Kurumsal ama samimi, net ve anlaşılır bir dilde yaz.

            Yanıtlarını 2–3 paragrafla sınırla.

            Eksik bilgi (poliçe numarası, kimlik vb.) varsa kibarca talep et.

            Fiyat, ödeme, teminat gibi kritik konularda kesin rakam verme, müşteri temsilcisine yönlendir.

            Hasar ve sağlık gibi hassas durumlarda empatik ol.

            Cevaplarını teşekkür ve iyi dilekle bitir.
            
            Yanıtını aşağıdaki JSON formatında ver:
            {{
              ""subject"": ""Kısa ve profesyonel mail başlığı"",
              ""body"": ""Mail içeriği""
            }}

            Kullanıcının sana gönderdiği mesaj şu şekilde:
            '{message.MessageContent}.'";

            _client.BaseAddress = new Uri("https://api.anthropic.com/");
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
            _client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var requestBody = new
            {
                model = "claude-haiku-4-5",
                max_tokens = 1000,
                temperature = 0.5,
                messages = new[]
                {
        new
        {
            role = "user",
            content = prompt
        }
    }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("v1/messages", jsonContent);
            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine("CLAUDE STATUS: " + response.StatusCode);
            Console.WriteLine("CLAUDE RESPONSE: " + responseString);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Claude API Hatası: " + response.StatusCode + " - " + responseString);
            }

            var json = JsonNode.Parse(responseString);
            string? aiText = json?["content"]?[0]?["text"]?.ToString();
            if (!string.IsNullOrEmpty(aiText))
            {
                aiText = aiText.Replace("```json", "").Replace("```", "");
            }
            else
            {
                aiText = "{ \"subject\": \"Yanıt alınamadı\", \"body\": \"Lütfen tekrar deneyin.\" }";
            }

            var aiJson = JsonNode.Parse(aiText);

            string subject = aiJson?["subject"]?.ToString() ?? "InsureYouAI Yanıt";
            string body = aiJson?["body"]?.ToString() ?? "";
            ViewBag.v =subject;

            #endregion

            #region Email.Gönderme

            var email = _configuration["EmailSettings:Email"];
            var password = _configuration["EmailSettings:Password"];
            MimeMessage mimeMessage = new MimeMessage();
            MailboxAddress mailboxAddress = new MailboxAddress("InsureYouAI Admin", email);
            mimeMessage.From.Add(mailboxAddress);

            MailboxAddress mailboxAddressTo = new MailboxAddress("User", message.Email);
            mimeMessage.To.Add(mailboxAddressTo);

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = body ?? "Destek hattımız en kısa sürede tarafınıza dönüş sağlayacaktır. ";
            mimeMessage.Body = bodyBuilder.ToMessageBody();


            mimeMessage.Subject = subject;
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate(email, password);
                client.Send(mimeMessage);
                client.Disconnect(true);
            }

            #endregion


            return RedirectToAction("Index");
            
        }

        public PartialViewResult SubscribeEmail()
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult SubscribeEmail(string email)
        {
            return View();
        }
    }
}
